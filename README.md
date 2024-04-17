# ![](https://realityripple.com/Software/Applications/Satellite-Restriction-Tracker/favicon-32x32.png) Satellite Restriction Tracker
ViaSat Satellite Network usage monitoring utility with history graphing.

#### Version 1.9
> Author: Andrew Sachen  
> Created: December 30, 2010  
> Updated: April 16, 2024  

Language: Visual Basic.NET  
Compiler: Visual Studio 2019  
Framework: Version 4.0+

##### Involved Technologies:
* HTTPS
  * HTTP Proxy Support
* HTML Parsing
  * Single Sign-on
  * SAML
  * OAuth2
  * AJAX
  * graphql
* Proprietary Remote Usage Service protocol
  * Based on [SRP](https://en.wikipedia.org/wiki/Secure_Remote_Password_protocol) using HMAC-SHA-512
* Windows Services

## Building
This application can be compiled using Visual Studio 2010 or newer, however an Authenticode-based Digital Signature check is integrated into the code to prevent incorrectly-signed or unsigned copies from running. Comment out lines 4-19 of `/RestrictionTracker/ApplicationEvents.vb` and lines 3-12 of `/RestrictionController/modRC.vb` to disable this signature checks before compiling if you wish to build your own copy.

This application is *not* designed to support Mono/Xamarin compilation and will not work on Linux or OS X systems. A Linux and OS X compatible release is available at [RealityRipple/Satellite-Restriction-Tracker-for-MONO](https://github.com/RealityRipple/Satellite-Restriction-Tracker-for-MONO), which uses the `/RestrictionLibrary/` DLL from this project.

## Documentation
Satellite Restriction Tracker grabs usage data off a ViaSat compatible service website via HTTPS and stores the data for graphed display, providing a historical account of usage for satellite Internet users.  
The configuration window provides access to all the application's features and options, and also facilitates the creation of a Portable version of the application.

Preferences are stored in the Application Data folder in a file called `user.config` of XML format, and passwords are encrypted using a simple RC4 methodology to prevent snooping.  
If the Windows Service feature is used, preferences are also stored in the ProgramData folder as well.  
The portable version stores its `user.config` file locally, of course.

The History window provides the option to import and export usage data in one of three formats: XML, CSV, or `.WB`, a proprietary binary database format. The location of the database can be customized through the Preferences window, as well.

Additional features include a quick-link to a network speed test or modem information page, system tray icon preferences, a custom notification system with customizable themes, and graph color selection options.  
Finally, a Remote Usage Service feature allows users to synchronize their data to a webserver, which gathers usage information every fifteen minutes, providing a full picture of Internet usage, even when this application is not running or the user's Internet access is down, for a small yearly maintenance fee.

## Download
You can grab the latest release from the [Official Web Site](https://realityripple.com/Software/Applications/Satellite-Restriction-Tracker/).

## License
This is free and unencumbered software released into the public domain, supported by donations, not advertisements. If you find this software useful, [please support it](https://realityripple.com/donate.php?itm=Satellite+Restriction+Tracker)!
