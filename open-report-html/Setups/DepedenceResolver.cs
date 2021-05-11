using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace open.report.html.Setups
{
    public static class DepedenceResolver
    {
        public static void ControllersDependences(this IServiceCollection services)
        {
            services.AddSingleton<GlobalOptions>();
        }
    }
}
