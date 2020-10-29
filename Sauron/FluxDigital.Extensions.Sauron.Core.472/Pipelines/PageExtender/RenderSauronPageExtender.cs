using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.Mvc.ExperienceEditor.Pipelines.RenderPageExtenders;
using Sitecore.Mvc.Pipelines;

namespace FluxDigital.Extensions.Sauron.Core._472.Pipelines.PageExtender
{
    public class RenderSauronPageExtender : MvcPipelineProcessor<RenderPageExtendersArgs>
    {
        public override void Process(RenderPageExtendersArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            //add sauron js and css to load in message and icon styles
            args.Writer.Write("<link href=\"/{0}\" rel=\"stylesheet\" />", "/sitecore modules/Sauron/styles/sauron-page-editor.css?id=2");
            args.Writer.Write("<script src=\"{0}\"></script>", "/sitecore modules/Sauron/scripts/sauron-page-editor.js?id=2");
        }
    }
}
