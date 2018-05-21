using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireData.Models;
using QuestionnairorBuilder.Extensions;
using QuestionnairorBuilder.Services;

namespace QuestionnairorBuilder.Controllers
{
    public class QuestionController : Controller
    {
        [HttpPost("[controller]/[action]/{questionnaireId}")]
        public IActionResult Add([FromServices] IQuestionnaireService service, Guid questionnaireId)
        {
            if (questionnaireId == null || questionnaireId == Guid.Empty || !service.ModelData.ContainsKey(questionnaireId))
            {
                return RedirectToAction("Index", "Builder");
            }
            Questionnaire modelData = service.ModelData[questionnaireId];
            modelData.Questions.Add(new Question());
            ViewData["QuestionnaireId"] = questionnaireId;
            return RedirectToAction("Index", "Builder", new { questionnaireId });
        }
    }
}