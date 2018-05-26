using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnairor.Areas.Viewer.Models;
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
        public IActionResult Feedback(FeedbackModel modelData)
        {
            return View(modelData);
        }

        [HttpPost]
        public IActionResult Submit(Questionnaire modelData)
        {
            List<Response> responses = modelData.GetActiveResponses(modelData.GetAnswers());
            FeedbackModel feedbackData = new FeedbackModel();
            feedbackData.Answers = modelData;
            feedbackData.Responses = responses;
            return View("Feedback", feedbackData);
        }

        [HttpPost]
        public IActionResult Debug(Questionnaire modelData)
        {
            string result = modelData.ToJson(Formatting.Indented);
            List<Response> responses = modelData.GetActiveResponses(modelData.GetAnswers());
            result += "\n\nReponses:\n";
            foreach(Response response in responses)
            {
                result += "\n" + response.ToJson(Formatting.Indented);
            }

            return Ok(result);
        }
    }
}
