using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using open.report.html.Dtos;
using open.report.html.Setups;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace open.report.html.Controllers.v1
{
    [EnableCors(Program.AllowSpecificOrigins)]
    [ApiController]
    [ApiVersion(SwaggerSetup.API_V_1_0, Deprecated = false)]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [Produces("application/json")]
    public class TemplateController : Controller
    {
        private readonly GlobalOptions _globalOptions;

        public TemplateController(GlobalOptions globalOptions)
        {
            this._globalOptions = globalOptions;
        }

        [HttpPut]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        public async ValueTask<IActionResult> UploadAsync(List<IFormFile> files)
        {
            if (!this.CheckLengthLimit(files))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"The max length of the files is {this._globalOptions.MaxFileLength}." });
            }

            foreach (var file in files) 
            {
                if (file == null || file.Length == 0)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"The file {file.FileName} is empty." });
                }

                if (!IsValidFile(file))
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"Invalid file format." });
                }

                string path = this.GetPathTo(file.FileName);
                var fileInfo = new FileInfo(path);

                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }

                if (fileInfo.Exists)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"File exists. Delete the file before." });
                }

                using var stream = new FileStream(path, FileMode.CreateNew);
                await file.CopyToAsync(stream);
                await stream.FlushAsync();
                stream.Close();
            }

            return Accepted(new { Message = "The file was saved." });
        }

        [HttpDelete]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        public async ValueTask<IActionResult> DeleteAsync(FileNameDto fileName)
        {
            string path = this.GetPathTo(fileName.FileName);
            var fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, new { Message = $"File not found." });
            }

            fileInfo.Delete();

            await ValueTask.CompletedTask;

            return Accepted(new { Message = "The file was deleted." });
        }

        [HttpPost]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async ValueTask<IActionResult> Render(RenderDto render)
        {
            Func<ViewResult> viewResult = () => {
                var viewName = this.GetViewName(render.FileName);

                var model = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(render.Model);
                
                ViewResult viewResult = this.View(viewName, model);
                return viewResult;
            };

            return await ValueTask.FromResult<ViewResult>(viewResult());
        }

        private string GetViewName(string fileName)
        {
            return $"{this._globalOptions.ViewTemplate}/{this.GetUser()}/{fileName}";
        }

        private static bool IsValidFile(IFormFile file)
        {
            if (!Path.GetExtension(file.FileName).ToLower(CultureInfo.CurrentCulture).Equals(".cshtml"))
            {
                return false;
            }

            return true;
        }

        private string GetPathTo(string fileName)
        {
            var root = this._globalOptions.ApplicationPath;
            var path = this._globalOptions.TemplatePath;
            var user = this.GetUser();

            var filePath = Path.Combine(root, path, user, fileName);

            return filePath;
        }

        private string GetUser()
        {
            return this._globalOptions.UserName;
        }

        private bool CheckLengthLimit(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length >= this._globalOptions.MaxFileLength)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
