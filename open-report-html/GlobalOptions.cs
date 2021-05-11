using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace open.report.html
{
    public class GlobalOptions
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public GlobalOptions(IConfiguration configuration, IWebHostEnvironment env)
        {
            this._configuration = configuration;
            this._env = env;
        }

        public int MaxFileLength { get => this._configuration.ReadConfig<int>("Program", nameof(this.MaxFileLength)); }
        public string TemplatePath { get => this._configuration.ReadConfig<string>("Program", nameof(TemplatePath)); }
        public string ApplicationPath { get => this._env.ContentRootPath; }
        public string UserName { get => "shared"; }
        public object ViewTemplate { get => this._configuration.ReadConfig<string>("Program", nameof(ViewTemplate)); }

    }
}
