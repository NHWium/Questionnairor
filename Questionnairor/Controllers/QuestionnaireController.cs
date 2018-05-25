using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Questionnairor.Controllers
{
    public class QuestionnaireController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Questionnaire", new { Area = "Builder" });
        }
    }
}