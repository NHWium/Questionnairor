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
            if (!service.Data.IsAllAnswered())
            {
                ModelState.AddModelError("Questions", "Each choice must be answered before the questionnaire can be submitted.");
            }
            return View(service.Data);
        }

        [HttpPost]
        public IActionResult Submit(Questionnaire modelData)
        {
            if (modelData == null || !modelData.IsAllAnswered())
                return RedirectToAction("Index", "Questionnaire");
            List<Response> responses = modelData.GetActiveResponses(modelData.GetAnswers());
            FeedbackModel feedbackData = new FeedbackModel();
            feedbackData.Answers = modelData;
            feedbackData.Responses = responses;
            return View("Feedback", feedbackData);
        }
    }
}
