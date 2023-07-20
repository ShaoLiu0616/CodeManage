using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSDMAG.Models;
using PSDMAG.Services;

namespace PSDMAG.Controllers
{
    public class CodeMagController : Controller
    {
        private ICodeMagService _CodeMagService;
        private readonly AppSetting _appSettings;
        public CodeMagController(IOptions<AppSetting> options, ICodeMagService codeMagService)
        {
            _appSettings = options.Value;
            _CodeMagService = codeMagService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryJson(string uid)
        {
            var Result = _CodeMagService.Query(uid);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Insert(CodeMagActionRequest Request)
        {
            var Result = _CodeMagService.Insert(Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Update(CodeMagActionRequest Request)
        {

            var Result = _CodeMagService.Update(Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Delete(CodeMagActionRequest Request)
        {

            var Result = _CodeMagService.Delete(Request);
            return Content(Result, "application/json");
        }
    }
}
