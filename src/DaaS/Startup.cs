using System;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Globalization;

[assembly: OwinStartup(typeof(DaaS.Startup))]

namespace DaaS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                var queryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : string.Empty;
                var queryParams = queryString.Split('&');

                var format = string.Empty;
                var cultureCode = string.Empty;

                foreach (var queryValues in queryParams.Select(queryParam => queryParam.Split('=')).Where(queryValues => queryValues.Length == 2))
                {
                    switch (queryValues[0])
                    {
                        case "format":
                            format = queryValues[1];
                            break;
                        case "culture":
                            cultureCode = queryValues[1];
                            break;
                    }
                }

                var culture = CultureInfo.InvariantCulture;
                try
                {
                    culture = CultureInfo.GetCultureInfo(cultureCode);
                }
                catch (CultureNotFoundException)
                {
                }

                string response = !string.IsNullOrEmpty(format) ? DateTime.Now.ToString(format, culture) : DateTime.Now.ToString(culture);

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync(response);
            });
        }
    }
}
