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
        private IMemberService _MemberService;
        public MemberController(IOptions<AppSetting> options, IMemberService memberService)
        {
            _appSettings = options.Value;
            _MemberService = memberService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult QueryJson()
        {
            var Result = _MemberService.Query();
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Insert(MemberActionRequest Request)
        {
            var Result = _MemberService.Insert(Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Update(MemberActionRequest Request)
        {
            var Result = _MemberService.Update(Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult Delete(MemberActionRequest Request)
        {
            var Result = _MemberService.Delete(Request);
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult GetMem()
        {
            var Result = _MemberService.GetMem();
            return Content(Result, "application/json");
        }
        [HttpPost]
        public IActionResult CheckMem(MemberActionRequest Request)
        {
            var Result = _MemberService.CheckMem(Request);
            return Content(Result, "application/json");
        }
    }
}
