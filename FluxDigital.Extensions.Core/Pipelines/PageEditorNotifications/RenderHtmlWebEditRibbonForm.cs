//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using Sitecore;
//using Sitecore.Collections;
//using Sitecore.Data;
//using Sitecore.Data.Events;
//using Sitecore.Data.Items;
//using Sitecore.Diagnostics;
//using Sitecore.ExperienceEditor.Utils;
//using Sitecore.Globalization;
//using Sitecore.Mvc.Extensions;
//using Sitecore.Pipelines.GetPageEditorNotifications;
//using Sitecore.Pipelines.HasPresentation;
//using Sitecore.Resources;
//using Sitecore.SecurityModel;
//using Sitecore.Shell.Applications.WebEdit;
//using Sitecore.Shell.Framework;
//using Sitecore.Shell.Framework.CommandBuilders;
//using Sitecore.Shell.Framework.Commands;
//using Sitecore.Sites;
//using Sitecore.Web;
//using Sitecore.Web.UI.HtmlControls;
//using Sitecore.Web.UI.Sheer;
//using Sitecore.Web.UI.WebControls.Ribbons;

//namespace FluxDigital.Extensions.Core.Pipelines.PageEditorNotifications
//{
//    public class WebEditRibbonForm : Sitecore.Shell.Applications.WebEdit.WebEditRibbonForm
//    {
//        protected HtmlForm RibbonForm;
//        protected Border RibbonPane;
//        protected Border Treecrumb;
//        protected Border Notifications;
//        protected Border TreecrumbPane;
//        private bool currentItemDeleted;
//        private bool refreshHasBeenAsked;

//        public string ContextUri
//        {
//            get { return (this.ServerProperties[nameof(ContextUri)] ?? (object) this.CurrentItemUri) as string; }
//            set
//            {
//                Assert.ArgumentNotNullOrEmpty(value, nameof(value));
//                this.ServerProperties[nameof(ContextUri)] = (object) value;
//            }
//        }

//        public string CurrentItemUri
//        {
//            get { return this.ServerProperties[nameof(CurrentItemUri)] as string; }
//            set
//            {
//                Assert.ArgumentNotNullOrEmpty(value, nameof(value));
//                this.ServerProperties[nameof(CurrentItemUri)] = (object) value;
//            }
//        }

//        public override void HandleMessage(Message message)
//        {
//            Assert.ArgumentNotNull((object) message, nameof(message));
//            if (!this.VerifyWebeditLoaded())
//            {
//                SheerResponse.Alert("The Experience Editor is not yet available.");
//                message.CancelBubble = true;
//                message.CancelDispatch = true;
//            }
//            else
//            {
//                if (message.Name == "webedit:workflowwithdatasourceitems")
//                    Context.ClientPage.Modified = false;
//                if (message.Name == "item:save")
//                {
//                    SiteContext site = Context.Site;
//                    Assert.IsNotNull((object) site, "Site not found.");
//                    site.Notifications.Disabled = MainUtil.GetBool(message.Arguments["disableNotifications"], false);
//                    message = Message.Parse(message.Sender, "webedit:save");
//                }

//                if (message.Name == "ribbon:update")
//                {
//                    string contextUri = this.ContextUri;
//                    if (!string.IsNullOrEmpty(message.Arguments["id"]))
//                        contextUri = new ItemUri(new ID(message.Arguments["id"]),
//                            Language.Parse(message.Arguments["lang"]),
//                            new Sitecore.Data.Version(message.Arguments["ver"]), message.Arguments["db"]).ToString();
//                    this.Update(contextUri);
//                }
//                else if (message.Name == "item:refresh")
//                    this.Update(this.ContextUri);
//                else
//                    Dispatcher.Dispatch(message, this.GetCurrentItem(message));
//            }
//        }

//        protected void ConfirmAndReload(ClientPipelineArgs args)
//        {
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            if (this.currentItemDeleted)
//                return;
//            if (args.IsPostBack)
//            {
//                if (!args.HasResult || !(args.Result != "no"))
//                    return;
//                SheerResponse.Eval("window.parent.location.reload(true)");
//            }
//            else
//            {
//                if (this.refreshHasBeenAsked)
//                    return;
//                SheerResponse.Confirm("An item was deleted. Do you want to refresh the page?");
//                this.refreshHasBeenAsked = true;
//                args.WaitForPostBack();
//            }
//        }

//        protected virtual void CopiedNotification(object sender, ItemCopiedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            Context.ClientPage.Start((object) this, "Reload");
//        }

//        protected virtual void CreatedNotification(object sender, ItemCreatedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            Context.ClientPage.Start((object) this, "Reload");
//        }

//        protected virtual void DeletedNotification(object sender, ItemDeletedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            Assert.IsNotNull((object) args.Item, "Deleted item in DeletedNotification args.");
//            ItemUri uri = ItemUri.Parse(this.CurrentItemUri);
//            Assert.IsNotNull((object) uri, "uri");
//            Item parent = args.Item.Database.GetItem(args.ParentID, uri.Language);
//            if (parent == null)
//                return;
//            if (uri.ItemID == args.Item.ID && uri.DatabaseName == args.Item.Database.Name)
//            {
//                this.currentItemDeleted = true;
//                this.Redirect(WebEditRibbonForm.GetTarget(parent));
//            }
//            else
//            {
//                if (Database.GetItem(uri) == null)
//                    return;
//                Context.ClientPage.Start((object) this, "ConfirmAndReload");
//            }
//        }

//        protected virtual Item GetCurrentItem(Message message)
//        {
//            Assert.ArgumentNotNull((object) message, nameof(message));
//            string path = message["id"];
//            if (string.IsNullOrEmpty(this.CurrentItemUri))
//                return (Item) null;
//            ItemUri uri = ItemUri.Parse(this.CurrentItemUri);
//            if (uri == (ItemUri) null)
//                return (Item) null;
//            Item obj = Database.GetItem(uri);
//            if (!string.IsNullOrEmpty(path) && obj != null)
//                return obj.Database.GetItem(path, obj.Language);
//            return obj;
//        }

//        protected virtual bool IsSimpleUser()
//        {
//            return false;
//        }

//        protected virtual void MovedNotification(object sender, ItemMovedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            string currentItemUri = this.CurrentItemUri;
//            if (string.IsNullOrEmpty(currentItemUri))
//                return;
//            ItemUri uri = ItemUri.Parse(currentItemUri);
//            if (uri == (ItemUri) null)
//                return;
//            if (args.Item.ID == uri.ItemID && args.Item.Database.Name == uri.DatabaseName)
//            {
//                Item obj = Database.GetItem(uri);
//                if (obj == null)
//                {
//                    Log.SingleError("Item not found after moving. Item uri:" + (object) uri, (object) this);
//                }
//                else
//                {
//                    this.Redirect(obj);
//                    WebEditRibbonForm.DisableOtherNotifications();
//                }
//            }
//            else
//                Context.ClientPage.Start((object) this, "Reload");
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            Assert.ArgumentNotNull((object) e, nameof(e));
//            //base.OnLoad(e);
//            this.refreshHasBeenAsked = false;
//            SiteContext site = Context.Site;
//            if (site != null)
//            {
//                site.Notifications.ItemDeleted += new ItemDeletedDelegate(this.DeletedNotification);
//                site.Notifications.ItemMoved += new ItemMovedDelegate(this.MovedNotification);
//                site.Notifications.ItemRenamed += new ItemRenamedDelegate(this.RenamedNotification);
//                site.Notifications.ItemCopied += new ItemCopiedDelegate(this.CopiedNotification);
//                site.Notifications.ItemCreated += new ItemCreatedDelegate(this.CreatedNotification);
//                site.Notifications.ItemSaved += new ItemSavedDelegate(this.SavedNotification);
//            }

//            ItemUri queryString = ItemUri.ParseQueryString();
//            Assert.IsNotNull((object) queryString, typeof(ItemUri));
//            this.CurrentItemUri = queryString.ToString();
//            //if (Context.ClientPage.IsEvent)
//            //{
//            //    string currentItemUri = this.CurrentItemUri;
//            //    if (string.IsNullOrEmpty(currentItemUri))
//            //        return;
//            //    Item obj;
//            //    using (new SecurityDisabler())
//            //        obj = Database.GetItem(new ItemUri(currentItemUri));
//            //    if (obj == null)
//            //    {
//            //        SheerResponse.Eval("scShowItemDeletedNotification(\"" +
//            //                           Translate.Text(
//            //                               "The item does not exist. It may have been deleted by another user.") +
//            //                           "\")");
//            //    }
//            //    else
//            //    {
//            //        if (Database.GetItem(new ItemUri(currentItemUri)) != null)
//            //            return;
//            //        SheerResponse.Eval("scShowItemDeletedNotification(\"" +
//            //                           Translate.Text(
//            //                                   "The item could not be found.\n\nYou may not have read access or it may have been deleted by another user.")
//            //                               .Replace('\n', ' ') + "\")");
//            //    }
//            //}
//            //else
//            //{
//                Item obj = Database.GetItem(queryString);
//                if (obj == null)
//                {
//                    WebUtil.RedirectToErrorPage(Translate.Text(
//                        "The item could not be found.\n\nYou may not have read access or it may have been deleted by another user."));
//                }
//                else
//                {
//                    this.RenderRibbon(obj);
//                    this.RenderTreecrumb(obj);
//                    this.RenderNotifications(obj);
//                    this.RibbonForm.Attributes["class"] = UIUtil.GetBrowserClassString();
//                }
//            //}
//        }

//        protected void Redirect(ClientPipelineArgs args)
//        {
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            string parameter = args.Parameters["url"];
//            Assert.IsNotNullOrEmpty(parameter, "url");
//            SheerResponse.Eval(string.Format("window.parent.location.href='{0}'", (object) parameter));
//        }

//        protected void Reload(ClientPipelineArgs args)
//        {
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            SheerResponse.Eval("window.parent.location.reload(true)");
//        }

//        protected virtual void RenamedNotification(object sender, ItemRenamedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            ItemUri itemUri = ItemUri.Parse(this.CurrentItemUri);
//            Assert.IsNotNull((object) itemUri, "uri");
//            if (itemUri.ItemID == args.Item.ID && itemUri.DatabaseName == args.Item.Database.Name)
//            {
//                Item obj = args.Item.Database.GetItem(args.Item.ID, itemUri.Language);
//                if (obj != null)
//                {
//                    this.Redirect(obj);
//                    return;
//                }
//            }

//            Context.ClientPage.Start((object) this, "Reload");
//        }

//        protected virtual void RenderRibbon(Item item)
//        {
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            string queryString = WebUtil.GetQueryString("mode");
//            if (WebUtil.GetQueryString("sc_speakribbon") == "1")
//                return;
//            Ribbon ribbon1 = new Ribbon();
//            ribbon1.ID = "Ribbon";
//            ribbon1.ShowContextualTabs = false;
//            ribbon1.ActiveStrip = queryString == "preview"
//                ? "VersionStrip"
//                : WebUtil.GetCookieValue("sitecore_webedit_activestrip");
//            Ribbon ribbon2 = ribbon1;
//            string path;
//            switch (queryString)
//            {
//                case "preview":
//                    path = "/sitecore/content/Applications/WebEdit/Ribbons/Preview";
//                    break;
//                case "edit":
//                    path = this.IsSimpleUser()
//                        ? "/sitecore/content/Applications/WebEdit/Ribbons/Simple"
//                        : "/sitecore/content/Applications/WebEdit/Ribbons/WebEdit";
//                    break;
//                default:
//                    path = "/sitecore/content/Applications/WebEdit/Ribbons/Debug";
//                    break;
//            }

//            SiteRequest request = Context.Request;
//            Assert.IsNotNull((object) request, "Site request not found.");
//            CommandContext commandContext = new CommandContext(item);
//            commandContext.Parameters["sc_pagesite"] = request.QueryString["sc_pagesite"];
//            ribbon2.CommandContext = commandContext;
//            commandContext.RibbonSourceUri = new ItemUri(path, Context.Database);
//            if (this.RibbonPane == null)
//                return;
//            this.RibbonPane.InnerHtml = HtmlUtil.RenderControl((System.Web.UI.Control) ribbon2);
//        }

//        protected virtual void RenderTreecrumb(Item item)
//        {
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            if (this.IsSimpleUser() || WebUtil.GetQueryString("debug") == "1")
//            {
//                this.Treecrumb.Visible = false;
//            }
//            else
//            {
//                HtmlTextWriter output = new HtmlTextWriter((TextWriter) new StringWriter());
//                this.RenderTreecrumb(output, item);
//                this.RenderTreecrumbGo(output, item);
//                if (WebUtil.GetQueryString("mode") != "preview")
//                    this.RenderTreecrumbEdit(output, item);
//                if (this.Treecrumb == null)
//                    return;
//                this.Treecrumb.InnerHtml = output.InnerWriter.ToString();
//            }
//        }

//        protected virtual void RenderTreecrumb(HtmlTextWriter output, Item item)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            Item parent = item.Parent;
//            if (parent != null && parent.ID != ItemIDs.RootID)
//                this.RenderTreecrumb(output, parent);
//            this.RenderTreecrumbLabel(output, item);
//            this.RenderTreecrumbGlyph(output, item);
//        }

//        protected virtual void RenderTreecrumbEdit(HtmlTextWriter output, Item item)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            bool flag = ItemUtility.CanEditItem(item, "webedit:open");
//            if (flag)
//            {
//                CommandBuilder commandBuilder = new CommandBuilder("webedit:open");
//                commandBuilder.Add("id", item.ID.ToString());
//                string clientEvent = Context.ClientPage.GetClientEvent(commandBuilder.ToString());
//                output.Write("<a href=\"javascript:void(0)\" onclick=\"{0}\" class=\"scTreecrumbGo\">",
//                    (object) clientEvent);
//            }
//            else
//                output.Write("<span class=\"scTreecrumbGo\">");

//            ImageBuilder imageBuilder = new ImageBuilder()
//            {
//                Src = "ApplicationsV2/16x16/edit.png",
//                Class = "scTreecrumbGoIcon",
//                Disabled = !flag
//            };
//            output.Write("{0} {1}{2}", (object) imageBuilder, (object) Translate.Text("Edit"),
//                flag ? (object) "</a>" : (object) "</span>");
//        }

//        protected virtual void RenderTreecrumbGlyph(HtmlTextWriter output, Item item)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            if (!item.HasChildren || Context.Device == null)
//                return;
//            ItemCollection children = new DataContext()
//            {
//                DataViewName = "Master"
//            }.GetChildren(item);
//            if (children == null || children.Count == 0)
//                return;
//            ShortID shortId = ID.NewID.ToShortID();
//            string str =
//                string.Format(
//                    "javascript:scContent.showOutOfFrameGallery(this, event, \"Gallery.ItemChildren\", {{height: 30, width: 30 }}, {{itemuri: \"{0}\" }});",
//                    (object) item.Uri);
//            ImageBuilder imageBuilder = new ImageBuilder()
//            {
//                Src = "/sitecore/shell/client/Speak/Assets/img/Speak/Common/16x16/dark_gray/separator.png",
//                Class = "scTreecrumbChevronGlyph"
//            };
//            output.Write("<a id=\"L{0}\" class=\"scTreecrumbChevron\" href=\"#\" onclick='{1}'>{2}</a>",
//                (object) shortId, (object) str, (object) imageBuilder);
//        }

//        protected virtual void RenderTreecrumbGo(HtmlTextWriter output, Item item)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            output.Write("<div class=\"scTreecrumbDivider\">{0}</div>", (object) Images.GetSpacer(1, 1));
//            bool flag = HasPresentationPipeline.Run(item);
//            if (flag)
//                output.Write("<a href=\"{0}\" class=\"scTreecrumbGo\" target=\"_parent\">",
//                    (object) Sitecore.Web.WebEditUtil.GetItemUrl(item));
//            else
//                output.Write("<span class=\"scTreecrumbGo\">");
//            ImageBuilder imageBuilder = new ImageBuilder()
//            {
//                Src = "ApplicationsV2/16x16/arrow_right_green.png",
//                Class = "scTreecrumbGoIcon",
//                Disabled = !flag
//            };
//            output.Write("{0} {1}{2}", (object) imageBuilder, (object) Translate.Text("Go"),
//                flag ? (object) "</a>" : (object) "</span>");
//        }

//        protected virtual void RenderTreecrumbLabel(HtmlTextWriter output, Item item)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            Item parent = item.Parent;
//            if (parent == null || parent.ID == ItemIDs.RootID)
//                return;
//            string str1 = string.Format("javascript:scForm.postRequest(\"\",\"\",\"\",{0})",
//                (object) StringUtil.EscapeJavascriptString(string.Format("Update(\"{0}\")", (object) item.Uri)));
//            output.Write("<a class=\"scTreecrumbNode\" href=\"#\" onclick='{0}'>", (object) str1);
//            string str2 = "scTreecrumbNodeLabel";
//            if (item.Uri.ToString() == this.CurrentItemUri)
//                str2 += " scTreecrumbNodeCurrentItem";
//            output.Write("<span class=\"{0}\">{1}</span></a>", (object) str2, (object) item.DisplayName);
//        }

//        protected virtual void SavedNotification(object sender, ItemSavedEventArgs args)
//        {
//            Assert.ArgumentNotNull(sender, nameof(sender));
//            Assert.ArgumentNotNull((object) args, nameof(args));
//            if (Context.PageDesigner.IsDesigning || args.Changes.Renamed)
//                return;
//            Context.ClientPage.Start((object) this, "Reload");
//        }

//        protected void Update(string uri)
//        {
//            ItemUri uri1 = string.IsNullOrEmpty(uri) ? ItemUri.ParseQueryString() : ItemUri.Parse(uri);
//            if (uri1 == (ItemUri) null)
//                return;
//            this.ContextUri = uri1.ToString();
//            Item obj1 = Database.GetItem(uri1);
//            if (obj1 == null || this.CurrentItemUri == null)
//                return;
//            ItemUri uri2 = ItemUri.Parse(this.CurrentItemUri);
//            if (uri2 == (ItemUri) null)
//                return;
//            Item obj2 = Database.GetItem(uri2);
//            if (obj2 == null)
//                return;
//            this.RenderRibbon(obj2);
//            this.RenderTreecrumb(obj1);
//            SheerResponse.Eval("scAdjustPositioning()");
//        }

//        protected virtual bool VerifyWebeditLoaded()
//        {
//            return !string.IsNullOrEmpty(this.ContextUri);
//        }

//        private static void DisableOtherNotifications()
//        {
//            SiteContext site = Context.Site;
//            if (site == null)
//                return;
//            site.Notifications.Disabled = true;
//        }

//        private static string GetNotificationIcon(PageEditorNotificationType notificationType)
//        {
//            switch (notificationType)
//            {
//                case PageEditorNotificationType.Error:
//                    return "Custom/16x16/error.png";
//                case PageEditorNotificationType.Information:
//                    return "Custom/16x16/info.png";
//                default:
//                    return "Custom/16x16/warning.png";
//            }
//        }

//        private static Item GetTarget(Item parent)
//        {
//            Assert.ArgumentNotNull((object) parent, nameof(parent));
//            if (HasPresentationPipeline.Run(parent))
//                return parent;
//            SiteContext site = SiteContext.GetSite(Sitecore.Web.WebEditUtil.SiteName);
//            if (site == null)
//                return parent;
//            string contentStartPath = site.ContentStartPath;
//            return parent.Database.GetItem(contentStartPath, parent.Language) ?? parent;
//        }

//        private static void RenderNotification(
//            HtmlTextWriter output,
//            PageEditorNotification notification,
//            string marker)
//        {
//            Assert.ArgumentNotNull((object) output, nameof(output));
//            Assert.ArgumentNotNull((object) notification, nameof(notification));
//            Assert.ArgumentNotNull((object) marker, nameof(marker));
//            string str = Themes.MapTheme(notification.Icon ??
//                                         WebEditRibbonForm.GetNotificationIcon(notification.Type));
//            output.Write("<div class=\"scPageEditorNotification {0}{1}\">", (object) notification.Type,
//                (object) marker);
//            output.Write("<img class=\"Icon\" src=\"{0}\"/>", (object) str);
//            output.Write("<div class=\"Description\">{0}. eh?</div>", (object) notification.Description);
//            output.Write($"<div class=\"InfoText\"><b>some test stuff here</b></div>");
//            foreach (PageEditorNotificationOption option in notification.Options)
//                output.Write(
//                    "<a onclick=\"javascript: return scForm.postEvent(this, event, '{0}')\" href=\"#\" class=\"OptionTitle\">{1}</a>",
//                    (object) option.Command, (object) option.Title);
//            output.Write("<br style=\"clear: both\"/>");
//            output.Write("</div>");
//        }

//        private void Redirect(Item item)
//        {
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            Context.ClientPage.Start((object) this, nameof(Redirect), new NameValueCollection()
//            {
//                {
//                    "url",
//                    Sitecore.Web.WebEditUtil.GetItemUrl(item)
//                }
//            });
//        }

//        private void RenderNotifications(Item item)
//        {
//            Assert.ArgumentNotNull((object) item, nameof(item));
//            if (WebUtil.GetQueryString("mode") != "edit")
//                return;
//            List<PageEditorNotification> editorNotifications = ItemUtility.GetPageEditorNotifications(item);
//            if (editorNotifications.Count == 0)
//            {
//                this.Notifications.Visible = false;
//            }
//            else
//            {
//                HtmlTextWriter output = new HtmlTextWriter((TextWriter) new StringWriter());
//                int count = editorNotifications.Count;
//                for (int index = 0; index < count; ++index)
//                {
//                    PageEditorNotification notification = editorNotifications[index];
//                    string empty = string.Empty;
//                    if (index == 0)
//                        empty += " First";
//                    if (index == count - 1)
//                        empty += " Last";
//                    WebEditRibbonForm.RenderNotification(output, notification, empty);
//                }

//                this.Notifications.InnerHtml = output.InnerWriter.ToString();
//            }
//        }

//        //protected override void OnLoad(EventArgs e)
//        //{
//        //    //base.OnLoad(e);
//        //    ItemUri queryString = ItemUri.ParseQueryString();
//        //    if (!Context.ClientPage.IsEvent)
//        //    {
//        //        Item item = Database.GetItem(queryString);
//        //        if (item != null)
//        //        {
//        //            RenderNotifications(item);
//        //        }
//        //    }
//        //}

//        //private static void RenderNotification(StringBuilder output, PageEditorNotification notification, string marker)
//        //{
//        //    Assert.ArgumentNotNull((object)output, nameof(output));
//        //    Assert.ArgumentNotNull((object)notification, nameof(notification));
//        //    Assert.ArgumentNotNull((object)marker, nameof(marker));
//        //    string str = Themes.MapTheme(notification.Icon ?? GetNotificationIcon(notification.Type));
//        //    output.Append($"<div class=\"scPageEditorNotification {(object)notification.Type}{(object)marker}\">");
//        //    output.Append($"<img class=\"Icon\" src=\"{(object)str}\"/>");
//        //    output.Append($"<div class=\"Description\">{(object)notification.Description}</div>");
//        //    output.Append($"<div class=\"InfoText\"><b>some test stuff here</b></div>");
//        //    output.Append($"<p>and things on a new line</p>");
//        //    foreach (PageEditorNotificationOption option in notification.Options)
//        //    {
//        //        output.Append($"<p>and things on a new line {option.Title}</p>");
//        //        //output.Append($"<a onclick=\"javascript: return scForm.postEvent(this, event, '{(object) option.Command}')\" href=\"#\" class=\"OptionTitle\">{(object) option.Title}</a>");
//        //    }
//        //    output.Append($"<p>more stuff afterwards as well</p>");
//        //    output.Append("<br style=\"clear: both\"/>");
//        //    output.Append("</div>");
//        //}

//        //private void RenderNotifications(Item item)
//        //{
//        //    Assert.ArgumentNotNull((object) item, nameof(item));
//        //    if (WebUtil.GetQueryString("mode") != "edit")
//        //        return;
//        //    List<PageEditorNotification> editorNotifications = ItemUtility.GetPageEditorNotifications(item);
//        //    if (editorNotifications.Count == 0)
//        //    {
//        //        Notifications.Visible = false;
//        //    }
//        //    else
//        //    {
//        //        StringBuilder output = new StringBuilder();
//        //        int count = editorNotifications.Count;
//        //        for (int index = 0; index < count; ++index)
//        //        {
//        //            PageEditorNotification notification = editorNotifications[index];
//        //            string empty = string.Empty;
//        //            if (index == 0)
//        //                empty += " First";
//        //            if (index == count - 1)
//        //                empty += " Last";
//        //            RenderHtmlRenderHtmlWebEditRibbonForm.RenderNotification(output, notification, empty);
//        //        }

//        //        this.Notifications.InnerHtml = "<div class=\"scPageEditorNotification\">test</div>"; //output.ToString();
//        //    }
//        //}

//        //private static string GetNotificationIcon(PageEditorNotificationType notificationType)
//        //{
//        //    switch (notificationType)
//        //    {
//        //        case PageEditorNotificationType.Error:
//        //            return "Custom/16x16/error.png";
//        //        case PageEditorNotificationType.Information:
//        //            return "Custom/16x16/info.png";
//        //        default:
//        //            return "Custom/16x16/warning.png";
//        //    }
//        //}
//    }
//}