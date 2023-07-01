using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSDMAG.Models;
using PSDMAG.Services;

namespace PSDMAG.Controllers
{
    public class CodeMagController : Controller
    {
        private readonly AppSetting _appSettings;
        public CodeMagController(IOptions<AppSetting> options)
        {
            _appSettings = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryJson(string uid) {

            var CodeMagService = new CodeMagService();
            var Result = CodeMagService.Query(_appSettings.LocalDB,uid);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Insert(CodeMagActionRequest Request)
        {

            var CodeMagService = new CodeMagService();
            var Result = CodeMagService.Insert(_appSettings.LocalDB, Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Update(CodeMagActionRequest Request)
        {

            var CodeMagService = new CodeMagService();
            var Result = CodeMagService.Update(_appSettings.LocalDB, Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Delete(CodeMagActionRequest Request)
        {

            var CodeMagService = new CodeMagService();
            var Result = CodeMagService.Delete(_appSettings.LocalDB, Request);
            return Content(Result, "application/json");
        }
    }
}
