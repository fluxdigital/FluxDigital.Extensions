# Sitecore Sloth

Sitecore Sloth is an open source Sitecore Module which prevents experience editor from scrolling the user up the page when adding new components, editing datasources and workflow etc. Having to scroll back down the page each time can be quite frustrating. The code is based on an [Blog Post from Kamruz Jaman](https://jammykam.wordpress.com/2017/09/08/inheriting-and-extending-sitecore-javascript/). You can see some video demos below of the issue and Sloth.

![Sloth Logo](/src/Foundation/Sloth/code/SitecorePackage/sitecore-sloth-logo-small.png)

Sitecore Sloth has been created as an Foundation Module following Helix conventions. Please note it will only work on MVC based Sitecore sites and not Webforms ones as it hooks into the MVC rendering pipeline.

Thanks to Kamruz for sharing the original code and ideas.

## Experience Editor Issue & Sloth Demo

### Experience Editor Issue

In this example of a simple Bootstrap 5 Site, you can see when a Card components properties are edited in experience editor it scrolls the editor half way up the page when saving. Also you can see the same thing happens when a datasource is added to a Card. One thing to note is that I've seen that sometimes this happens for some components and sometimes it doesn't so the issue seems intermittent.

![Experience Editor Issue](/src/Foundation/Sloth/code/Videos/sloth-experience-editor-issue.gif)

### Sloth Demo

When performing the same tasks in as in the previous video (editing component properties & adding datasources) you can see that Experience Editor takes you back to where you were after saving.

![Sloth Demo](/src/Foundation/Sloth/code/Videos/sloth-demo.gif)

## Installation

[You can download a pre-built package here](https://github.com/fluxdigital/FluxDigital.Extensions/blob/master/src/Foundation/Sloth/code/SitecorePackage/Sitecore%20Sloth-1.0.zip) and install it in the usual way through Sitecore Package Manager. Below is a demo video of this:

![Install](/src/Foundation/Sloth/code/Videos/sloth-install.gif)

You will also soon be able to use nuget to install Sloth too

Once installed you should find that experience editor scrolls you back to where you were on the page instead of to the top of the page when saving changes to components. If it doesn't check that the module istalled correctly and that SlothExperienceEditorExtension.js is loaded as a js resource.

Sitecore Sloth has been tested with most versions of Sitecore from 9.1 up to 9.3. From initial testing Sitecore 10.1  update 2 seems to have resolved this issue so Sloth doesn't seem to be needed, but I'm not sure about other releases of Sitecore 10.x. If this is still an issue please let me know.

## Contributing

Pull requests are welcome.

## License

[MIT](https://choosealicense.com/licenses/mit/)
