using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PSDMAG.Models;
using PSDMAG.Services;

namespace PSDMAG.Controllers
{
    public class MemberController : Controller
    {
        private readonly AppSetting _appSettings;
        public MemberController(IOptions<AppSetting> options)
        {
            _appSettings = options.Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryJson() {

            var MemberService = new MemberService();
            var Result = MemberService.Query(_appSettings.LocalDB);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Insert(MemberActionRequest Request)
        {

            var MemberService = new MemberService();
            var Result = MemberService.Insert(_appSettings.LocalDB, Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Update(MemberActionRequest Request)
        {

            var MemberService = new MemberService();
            var Result = MemberService.Insert(_appSettings.LocalDB, Request);
            return Content(Result, "application/json");
        }
    }
}
