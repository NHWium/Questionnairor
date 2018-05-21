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
        [HttpPost]
        public IActionResult Add([FromServices] IQuestionnaireService service, Guid questionnaireId)
        {
            if (questionnaireId == null || questionnaireId == Guid.Empty || !service.ModelData.ContainsKey(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", questionnaireId, data = "" });
            }
            Questionnaire modelData = service.ModelData[questionnaireId];
            modelData.Questions.Add(new Question());
            return RedirectToAction("Index", "Builder", new { questionnaireId });
        }
    }
}