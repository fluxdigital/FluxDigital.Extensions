using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using SitecoreApiTestApp.Model;
using SitecoreApiTestConsole.Model;

namespace SitecoreApiTestConsole
{
    /// <summary>
    /// A test console app to provide simple testing of getting and creating items and images
    /// </summary>
    internal class Program
    {
        private const string BaseUrl = "https://sc93sc.dev.local/"; //change to your Sitecore url

        private static void Main(string[] args)
        {
            var cookies = Login(); //required for Ssc Api methods

            UploadImageSscApi(cookies); //test image upload with Ssc Api
            //CreateItemSscApi(cookies); //test create item Ssc Api
            //GetItemSscApi(cookies); //test get item with Ssc Api
            //UploadImageLegacyApi(); //test image upload with legacy Api

            Console.ReadLine();
        }

        /// <summary>
        /// Login method for Ssc - required by app Ssc Api calls
        /// </summary>
        /// <retu//rns></returns>
        private static CookieContainer Login()
        {
            Console.WriteLine($"Logging in via SSc Api...");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            
            var authUrl = $"{BaseUrl}sitecore/api/ssc/auth/login";
            var authData = new Authentication
            {
                Domain = "sitecore",
                Username = "admin",
                Password = "b"
            };

            var authRequest = (HttpWebRequest)WebRequest.Create(authUrl);

            authRequest.Method = "POST";
            authRequest.ContentType = "application/json";

            var requestAuthBody = JsonConvert.SerializeObject(authData);

            var authDatas = new UTF8Encoding().GetBytes(requestAuthBody);

            using (var dataStream = authRequest.GetRequestStream())
            {
                dataStream.Write(authDatas, 0, authDatas.Length);
            }

            var cookies = new CookieContainer();

            authRequest.CookieContainer = cookies;
            // System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            var authResponse = authRequest.GetResponse();

            Console.WriteLine($"Login Status:\n\r{((HttpWebResponse)authResponse).StatusDescription}");

            authResponse.Close();

            return cookies;
        }

        /// <summary>
        /// Test method which attempts to upload image via Ssc the Api. Doesn't work :-(
        /// </summary>
        /// <param name="cookies"></param>
        private static void UploadImageSscApi(CookieContainer cookies)
        {
            Console.WriteLine($"Uploading image via SSc Api...");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("https://doc.sitecore.com/SdnArchive/upload/sdn5/modules/sitecore%20item%20web%20api/sitecore_item_web_api_developer_guide_sc66-71-usletter.pdf");
                HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream mediaStream = httpWebReponse.GetResponseStream();

                var pdfName = "File Upload Test";
                var pdfExtension = ".pdf";

                var imageModel = new MediaItemModelModel
                {
                    ItemName = "pdftest",
                    TemplateId = "0603F166-35B8-469F-8123-E8D87BEDC171", // /sitecore/templates/System/Media/Unversioned/Pdf 
                    Description = "Pdf Test Desc",
                    Extension = pdfExtension,
                    //Blob = mediaStream
                };
                
                var path = "sitecore%2Fmedia%20library%2FFiles";

                var url = $"{BaseUrl}sitecore/api/ssc/item/{path}";

                var request = (HttpWebRequest) WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.CookieContainer = cookies;

                var requestBody = JsonConvert.SerializeObject(imageModel);

                var data = new UTF8Encoding().GetBytes(requestBody);

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                }

                using (var response = request.GetResponse())
                {
                    Console.WriteLine($"Item Status:\n\r{((HttpWebResponse) response).StatusDescription}");

                    var responsejson = (HttpWebResponse)request.GetResponse();
                    using (var sr = new StreamReader(responsejson.GetResponseStream()))
                    {
                        var text = sr.ReadToEnd();
                        Console.WriteLine($"Response:\n\r {text}");
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(
                    $"Error occurred. Message: {ex.Message}.\r\n StackTrace: {ex.StackTrace}.\r\n InnerException: {ex.InnerException}");
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream())
                    .ReadToEnd());
            }
        }

        /// <summary>
        /// Test method to create item using Ssc Api
        /// </summary>
        /// <param name="cookies"></param>
        private static void CreateItemSscApi(CookieContainer cookies)
        {
            Console.WriteLine($"Creating Item via SSc Api...");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                var itemModel = new SampleItemModel()
                {
                    ItemName = "Test Item",
                    TemplateId = "76036F5E-CBCE-46D1-AF0A-4143F9B557AA", //sample item template
                    Title = "Test Item",
                    Text = "test"
                };

                var path = "sitecore%2Fcontent%2Fhome";

                var url = $"{BaseUrl}sitecore/api/ssc/item/{path}";

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.CookieContainer = cookies;

                var requestBody = JsonConvert.SerializeObject(itemModel);

                Console.WriteLine($"Request:\n\r {requestBody}");

                var data = new UTF8Encoding().GetBytes(requestBody);

                using (var dataStream = request.GetRequestStream())
                {
                    dataStream.Write(data, 0, data.Length);
                }

                using (var response = request.GetResponse())
                {
                    Console.WriteLine($"Item Status:\n\r{((HttpWebResponse)response).StatusDescription}");

                    var responsejson = (HttpWebResponse)request.GetResponse();
                    using (var sr = new StreamReader(responsejson.GetResponseStream()))
                    {
                        var text = sr.ReadToEnd();
                        Console.WriteLine($"Response:\n\r {text}");
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(
                    $"Error occurred. Message: {ex.Message}.\r\n StackTrace: {ex.StackTrace}.\r\n InnerException: {ex.InnerException}");
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream())
                    .ReadToEnd());
                using (var sr = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var text = sr.ReadToEnd();
                    Console.WriteLine($"Response:\n\r {text}");
                }
            }
        }

        /// <summary>
        /// Test method to get Item via Ssc Api
        /// </summary>
        /// <param name="cookies"></param>
        private static void GetItemSscApi(CookieContainer cookies)
        {
            Console.WriteLine($"Get Item via SSc Api...");

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                var path = "110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9?includeStandardTemplateFields=true&database=master";

                var url = $"{BaseUrl}sitecore/api/ssc/item/{path}";

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookies;

                using (var response = request.GetResponse())
                {
                    Console.WriteLine($"Item Status:\n\r{((HttpWebResponse)response).StatusDescription}");

                    var responsejson = (HttpWebResponse)request.GetResponse();
                    using (var sr = new StreamReader(responsejson.GetResponseStream()))
                    {
                        var text = sr.ReadToEnd();
                        Console.WriteLine($"Response:\n\r {text}");
                    }
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(
                    $"Error occurred. Message: {ex.Message}.\r\n StackTrace: {ex.StackTrace}.\r\n InnerException: {ex.InnerException}");
                Console.WriteLine(new StreamReader(ex.Response.GetResponseStream())
                    .ReadToEnd());
            }
        }

        /// <summary>
        /// Test method for uploading an PDF via the Legacy Api. This works.
        /// </summary>
        public static void UploadImageLegacyApi()
        {
            Console.WriteLine($"Uploading image via Legacy Api...");

            string ParentID = "{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}";  //Must be media library folder/item. this is the root
            string mediaFileExt = ".pdf"; //you need to specify the extension. You can get that by the content-type returned or the way you find better.

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("https://doc.sitecore.com/SdnArchive/upload/sdn5/modules/sitecore%20item%20web%20api/sitecore_item_web_api_developer_guide_sc66-71-usletter.pdf");
            HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream mediaStream = httpWebReponse.GetResponseStream();


            //create a instance and inform the credentials as well the host
            MediaPost mp = new MediaPost(BaseUrl, @"sitecore\admin", "b");

            //post the media!
            //Important: It needs a Valid Item Name,
            //                    a Valid Parent ID under Media Library,
            //                    the database, the stream and the extension
            mp.PostMedia("test pdf 11", new Guid(ParentID), "master", mediaStream, mediaFileExt);

        }
    }
}