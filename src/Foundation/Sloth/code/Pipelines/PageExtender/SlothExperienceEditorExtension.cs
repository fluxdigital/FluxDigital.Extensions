using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Pipelines.RenderPageExtenders;
using Sitecore.Mvc.Pipelines;

namespace FluxDigital.Foundation.Sloth.Pipelines.PageExtender
{
    /// <summary>
    /// injects SlothExperienceEditorExtension.js into experience editor to stop page scroll on save
    /// </summary>
    public class SlothExperienceEditorExtension : MvcPipelineProcessor<RenderPageExtendersArgs>
    {
        private readonly ID _slothRootItemId = new ID(Settings.GetSetting("Foundation.Sloth.RootItemId") ?? SlothConstants.Items.Sloth.ItemId);

        public override void Process(RenderPageExtendersArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");

            //check sloth is enabled firsts
            var slothRootItem = Context.Database.GetItem(_slothRootItemId);
            CheckboxField enabledField = slothRootItem?.Fields[SlothConstants.Templates.Sloth.Fields.Enabled];
            if(slothRootItem !=null && enabledField != null && enabledField.Checked){
                //inject the js file
                var javascriptFilePath = Settings.GetSetting("Foundation.Sloth.SlothExperienceEditorExtension.JavascriptFilePath", Constants.FilePaths.SlothExperienceEditorExtensionJsPath);
                args.Writer.Write("<script src=\"{0}\"></script>", javascriptFilePath);
            }
        }
    }
}