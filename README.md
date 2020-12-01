# Sitecore Sauron

Sitecore Sauron is an open source Sitecore Module which allows you to display Messages, Instructions and Documentation in Experience Editor for specific page templates to assit Content Editors. 

![Sauron Logo](/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/sauron-icon-128x128.png)

## Installation

You can download one of the pre-build packages from the links below. T

[Sitecore 8x version](https://github.com/fluxdigital/FluxDigital.Extensions/blob/master/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/Sitecore%20Sauron-1.0.0.zip)

I am working on a version for Sitecore 9 and 10 but they are not complete yet.

You will be able to use nuget install Sauron soon also like so.

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
6. Save and view the Page in Experience Editor. You should see your message at the top of the page similar to below:
![Sauron Message](/Sauron/FluxDigital.Extensions.Sauron.Web/SitecorePackage/sauron-message.png)

## Contributing

Pull requests are welcome. 

## License

[MIT](https://choosealicense.com/licenses/mit/)
