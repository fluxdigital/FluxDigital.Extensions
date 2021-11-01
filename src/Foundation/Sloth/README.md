# Sitecore Sloth

Sitecore Sloth is an open source Sitecore Module which prevents experience editor from scrolling the user up the page when adding new components, editing datasources and workflow etc. Having to scroll back down the page each time can be quite frustrating. The code is based on an [Blog Post from Kamruz Jaman](https://jammykam.wordpress.com/2017/09/08/inheriting-and-extending-sitecore-javascript/).

![Sloth Logo](/src/Foundation/Sloth/code/SitecorePackage/sitecore-sloth-logo-small.png)

Sitecore Sloth has been created as an Foundation Module following Helix conventions. Please note it will only work on MVC based Sitecore sites and not Webforms ones as it hooks into the MVC rendering pipeline.

Thanks to Kamruz for sharing the original code and ideas.

## Installation

[You can download a pre-built package here](https://github.com/fluxdigital/FluxDigital.Extensions/blob/master/src/Foundation/Sloth/code/SitecorePackage/Siteore%20Sloth-1.0.zip) and install it in the usual way through Sitecore Package Manager.

You will also soon be able to use nuget to install Sloth too

Once installed you should find that experience editor scrolls you back to where you were on the page instead of to the top of the page when saving changes to components. If it doesn't check that the module istalled correctly and that SlothExperienceEditorExtension.js is loaded as a js resource.

Sitecore Sloth has been tested with most versions of Sitecore from 9.1 up to Sitecore 10 update 2.

## Contributing

Pull requests are welcome.

## License

[MIT](https://choosealicense.com/licenses/mit/)
