using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SitecoreApiTestConsole
{
    public class MediaPost
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public MediaPost(string host, string username, string password)
        {
            this.Host = host;
            this.Username = username;
            this.Password = password;
        }
        public void PostMedia(string itemName, Guid parentId, string databaseName, Stream fileStream, string fileExtension)
        {

            try
            {

                //create the url
                string url = String.Format("{0}/-/item/v1/?", this.Host.TrimEnd('/'));
                //append parameters
                url += String.Format("name={0}&sc_itemid={1}&sc_database={2}&payload=content"
                    , HttpUtility.UrlEncode(itemName)
                    , HttpUtility.UrlEncode(parentId.ToString())
                    , HttpUtility.UrlEncode(databaseName));

                Console.WriteLine(url);

                //create request instance
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                //headers
                request.Method = "POST";
                request.KeepAlive = false;
                request.Headers.Add("X-Scitemwebapi-Username", this.Username);
                request.Headers.Add("X-Scitemwebapi-Password", this.Password);
                //apply content type and boundary
                string boundary = "---------------------------" + DateTime.Now.ToString("yyyyMMddHHmmssfff",
                    System.Globalization.CultureInfo.InvariantCulture);
                byte[] boundaryBytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                using (Stream stream = request.GetRequestStream())
                {
                    //boundary
                    stream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    //file header
                    string header = "Content-Disposition: form-data; name=\"file\"; filename=\"" + itemName +
                                    fileExtension + "\"\r\nContent-Type: multipart/form-data\r\n\r\n";
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(header);
                    stream.Write(bytes, 0, bytes.Length);
                    //file bytes
                    byte[] buffer = new byte[32768];
                    int bytesRead;
                    if (stream != null)
                    {
                        // upload from a given stream
                        while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                        }
                    }

                    byte[] end = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                    stream.Write(end, 0, end.Length);
                    stream.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    var text = sr.ReadToEnd();
                    Console.WriteLine($"Response:\n\r {text}");
                }
            }
            catch (WebException ex)
            {
                //send post
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream())
                    .ReadToEnd());
            }
        }
    }
}
