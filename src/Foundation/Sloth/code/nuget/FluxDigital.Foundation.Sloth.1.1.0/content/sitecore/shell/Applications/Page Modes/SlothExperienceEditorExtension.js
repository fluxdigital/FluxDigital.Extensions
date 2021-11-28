/**
 * Siteore Sloth Experience Editor Extension
 *
 * MMMMMMMMNyshNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
 * MMMMMMMMNyooshNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
 * MMMMMMMMMMmyooosydmNNMNdyyyhmMMMMNNNNNMMMMMMNNNNNMMMMNNmmNMM
 * Mhsyhhddmmmmysoooooosss-....:hddsoooooydmdhsoo+ooyyyssoooyMM
 * Myooooooooooooooooooooo-`-..-oooo+++++++ooo+++++++ooooooosMM
 * MNmddhhyyyysssssooooooo+:+://ooooo++++++oooo++++++ssssyyhmMM
 * MMMMMMMMMMNNNNmmmmddddhoooooosyyyys++++++yhh++++++dNNNMMMMMM
 * MMMMMMMMNmdhhyyyhhmmNMNo+++++hMMMMm++++++yMMs+++++dMMMMMMMMM
 * MMMMMNdyo+++++++++++oyh++++++sMMMMN+++++++NMh+++++hMMNNMMMMM
 * MMMMho:----:/++//:---:/++++++oMMMMy+++++++hMm+++++oNmoodMMMM
 * MMNy+.``````.-:.```````:+++++oNMMh++++++++oMm++++++oy++odMMM
 * MMh+/`/o++:``..```/ooo/.+++++oNNy++++++++++my+++++++++++oNMM
 * MMs+/./.`./.oyhy/./.`./-+++++oho+++++++++++o+++++++++++++hMM
 * MMs+/`````-`:oo+-..````-+++++++++++++++++++++++++++++++++hMM
 * MMy++:````/o+::/oo-```./+++++++++++++++++++++++++++++++++hMM
 * MMdo++/-.``..---.```.-/+++++++++++++++++++++++++++++++++omMM
 * MMMy++++/:--.....-://+++++++++++++++++++++++++++++++++++sMMM
 * MMMNs+++++++/////++++++++++++++++++++++++++++++++++++++omMMM
 * MMMMmso+++++++++++++++++++++++++++++++++++++++++++++++ohMMMM
 * MMMMMNho+++++++++++++++++++++++++++++++++++++++++++++ohMMMMM
 * MMMMMMMmyo+++++++++++++++++++++++++++++++++++++++++osdMMMMMM
 * MMMMMMMMMmhso++++++++++++++++++++++++++++++++++++oshNMMMMMMM
 * MMMMMMMMMMMNmysoo+++++++++++++++++++++++++++++oosdNMMMMMMMMM
 * MMMMMMMMMMMMMMNmhysoooo++++++++++++++++++ooosydmMMMMMMMMMMMM
 * MMMMMMMMMMMMMMMMMMNNmdhyysssooooooooossyhdmNNMMMMMMMMMMMMMMM
 * MMMMMMMMMMMMMMMMMMMMMMMMMMMMNNNNNNNMNMMMMMMMMMMMMMMMMMMMMMMM

 * Stops Sitecore taking user to the top of page in Experience Editor
 * when adding components and editing data sources or workflow
*/

if (typeof Sitecore !== typeof undefined) {
    Sitecore.PageModes.ChromeControls = Sitecore.PageModes.ChromeControls.extend({
        renderCommandTag: function (command, chrome, isMoreCommand) {
            var tag = this.base(command, chrome, isMoreCommand);
            if (command.click.indexOf("chrome:") == 0 || command.click.indexOf("webedit:setdatasource") > 0) {
                if (command.type == "common" || command.type == "datasourcesmenu" || command.type == "workflow") {
                    tag.click(function (e) {
                        e.stop();
                    });
                }
            }
            return tag;
        }
    },
    {
    commandRenderers: Sitecore.PageModes.ChromeControls.commandRenderers,
    eventNameSpace: Sitecore.PageModes.ChromeControls.eventNameSpace,
    registerCommandRenderer: Sitecore.PageModes.ChromeControls.registerCommandRenderer
    });
}