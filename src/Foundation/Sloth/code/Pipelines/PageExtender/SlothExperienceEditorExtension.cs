using Sitecore.Configuration;
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
        public override void Process(RenderPageExtendersArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            var javascriptFilePath = Settings.GetSetting("Foundation.Sloth.SlothExperienceEditorExtension.JavascriptFilePath", Constants.FilePaths.SlothExperienceEditorExtensionJsPath);
            args.Writer.Write("<script src=\"{0}\"></script>", javascriptFilePath);
        }
    }
}