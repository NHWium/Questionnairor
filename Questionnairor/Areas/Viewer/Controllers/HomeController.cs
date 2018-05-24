using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Questionnairor.Services;

namespace Questionnairor.Areas.Viewer.Controllers
{
    [Area("Viewer")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices]IQuestionnaireService service)
        {
            return View(service.Data);
        }
    }
}