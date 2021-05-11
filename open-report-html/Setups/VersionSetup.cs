using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace open.report.html.Setups
{
    public static class VersionSetup
    {
        public static void ConfigureVersions(this IServiceCollection services)
        {
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = false;
                x.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = $"'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            });

        }
    }
}
