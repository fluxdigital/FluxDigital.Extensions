# Sitecore Sauron

Sitecore Sauron is an open source Sitecore Module which allows you to display Messages, Instructions and Documentation in Experience Editor for certain page templates to assist Content Editors. Full HTML support is provided which means you can use links, bullets, underline, bold and so forth within your messages.
The idea for this module was born out creating a lot of new page templates and content editors being confused what the intention of the page templates is. Sauron solves this problem in a configurable and simple way.

![Sauron Logo](/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/sauron-icon-128x128.png)

## Installation

You can download one of the pre-build packages from the links below. 

[Sitecore 8x version](https://github.com/fluxdigital/FluxDigital.Extensions/blob/master/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/Sitecore%20Sauron-1.0.0.zip)

I am testing a package for Sitecore 9 and 10 so I should be able to release these soon.

You will also soon be able to use nuget to install Sauron like so:

```pm
Install-Package Sitecore-Sauron
```

## Usage
1. Insert a new Page Help message Folder here: /sitecore/system/Modules/Sauron/Sauron Message Configurations.
2. Within the new folder you have created add an new Page Help Message Config Item.
3. Set the Template you wish to show the Help Message for.
4. Add the Help Message Rich Text (you can include links here and other simple HTML elements if you wish).
![Sauron Edit](/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/sauron-example-page.png)
5. Ensure it's Enabled.
6. Save and view a Page which uses this template in Experience Editor. You should see your message at the top of the page similar to below:
![Sauron Message](/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/sauron-message.png)

## Performance & Caching
The module was tested with 100+ sub folders with a total of 379 page help message items. The first (uncached) load time for these is approximately 500 milliseconds, subsequent page loads are cached and take 0.02 - 0.03 milliseconds. With an handful of page help messages the inital load is a few milliseconds so there should be little or no performance hit from this module. The default cache timeout is 24hrs and set in the config: `<setting name="SauronCacheTimeMins" value="1440" />`. The cache is also cleared when new page help messages are created or are updated. 

## Configuration

The module configuration settings can be found in the following config file: xFluxDigital.Extensions.Sauron.config

You can disable the module be setting the value of this setting to false: `<setting name="SauronEnabled" value="true" />`

## Contributing

Pull requests are welcome. 

## License

[MIT](https://choosealicense.com/licenses/mit/)
