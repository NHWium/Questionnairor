using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnairor.Models;
using Questionnairor.Services;

namespace Questionnairor.Areas.Viewer.Controllers
{
    [Area("Viewer")]
    public class QuestionnaireController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
                return RedirectToAction("Index", "Questionnaire", new { Area = "Builder" });
            else
                return View(service.Data);
        }

        [HttpGet]
        public IActionResult Feedback(List<Response> modelData)
        {
            return View(modelData);
        }

        [HttpPost]
        public IActionResult Submit(Questionnaire modelData)
        {
            List<Response> responses = modelData.GetResponses(modelData.GetAnswers());
            return View("Feedback", responses);
        }

        [HttpPost]
        public IActionResult Debug(Questionnaire modelData)
        {
            string result = modelData.ToJson(Formatting.Indented);
            List<Response> responses = modelData.GetResponses(modelData.GetAnswers());
            result += "\n\nReponses:\n";
            foreach(Response response in responses)
            {
                result += "\n" + response.ToJson(Formatting.Indented);
            }

            return Ok(result);
        }
    }
}
