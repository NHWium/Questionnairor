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
    public class QuestionnaireController : Controller
    {
        [HttpPost]
        public IActionResult Add([FromServices] IQuestionnaireService service)
        {
            Questionnaire modelData;
            Guid questionnaireId = Guid.NewGuid();
            modelData = new Questionnaire()
                .Id(questionnaireId);
            if (questionnaireId != null && questionnaireId != Guid.Empty && !service.ModelData.ContainsKey(questionnaireId))
                service.ModelData.Add(questionnaireId, modelData);
            else
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Add", questionnaireId, data = "" });
            }
            return RedirectToAction("Index", "Builder", new { questionnaireId });
        }
    }
}