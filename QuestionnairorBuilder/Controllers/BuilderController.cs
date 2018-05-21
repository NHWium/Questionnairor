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
    public class BuilderController : Controller
    {
        [HttpGet]
        public IActionResult Index([FromServices] IQuestionnaireService service, Guid questionnaireId)
        {
            Questionnaire modelData;
            if (questionnaireId == null || questionnaireId == Guid.Empty)
            {
                questionnaireId = Guid.NewGuid();
                modelData = new Questionnaire()
                    .Id(questionnaireId);
                service.ModelData.Add(questionnaireId, modelData);
            }
            else if (questionnaireId != null && questionnaireId != Guid.Empty && service.ModelData.ContainsKey(questionnaireId))
                modelData = service.ModelData[questionnaireId];
            else 
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Builder", action = "Index", questionnaireId, data = "" });
            }
            return View(modelData);
        }
    }
}