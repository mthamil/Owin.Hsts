Owin.Hsts
=========

According to [OWASP](https://www.owasp.org/index.php/HTTP_Strict_Transport_Security), HTTP Strict Transport Security (HSTS) protects users from a number of threats, in particular man-in-the-middle attacks by not only forcing encrypted sessions, but also stopping attackers who use invalid digital certificates.

>HTTP Strict Transport Security (HSTS) is an opt-in security enhancement that is specified by a web application through the use of a special response header. Once a supported browser receives this header that browser will prevent any communications from being sent over HTTP to the specified domain and will instead send all communications over HTTPS. It also prevents HTTPS click through prompts on browsers.

>The specification has been released and published end of 2012 as [RFC 6797](http://tools.ietf.org/html/rfc6797) (HTTP Strict Transport Security (HSTS)) by the IETF

\**[source](https://www.owasp.org/index.php/HTTP_Strict_Transport_Security)*

This package aims to provide developers the middleware required to effectively support this specification in a standardised way.

To install the package, search [NuGet](https://nuget.org) for `Owin.Hsts` or run the following command from the package console:

    Install-Package Owin.HSTS

Then open `Startup.cs` and add the following:

	using Owin.Hsts; // Add this at the top

	app.UseHttpStrictTransportSecurity();

Which will result in a similar configuration to the following:

    using Microsoft.Owin;
    using Owin;
    using Owin.Hsts;

    [assembly: OwinStartupAttribute(typeof(MyWebApp.Startup))]
    namespace MyWebApp
    {
        public partial class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseHttpStrictTransportSecurity();

                ConfigureAuth(app);
            }
        }
    }

The following options are set by default:

- All sub-domains are included (HSTS specification dictates an all or none approach)
- The policy expires after 28 days
- The middleware will not overwrite any header that is already set by the application

In order to override any of the above settings use the additional overload as demonstrated below:

    using System;
    using Microsoft.Owin;
    using Owin;
    using Owin.Hsts;

    [assembly: OwinStartupAttribute(typeof(MyWebApp.Startup))]
    namespace MyWebApp
    {
        public partial class Startup
        {
            public void Configuration(IAppBuilder app)
            {
                app.UseHttpStrictTransportSecurity(new HstsSettings
                                                   {
                                                       Duration = TimeSpan.FromDays(365d),
                                                       IncludeSubDomains = false,
                                                       OverwriteExisting = true
                                                   });

                ConfigureAuth(app);
            }
        }
    }

**For browser compatibility with HSTS, check [caniuse.com](http://caniuse.com/#feat=stricttransportsecurity)**