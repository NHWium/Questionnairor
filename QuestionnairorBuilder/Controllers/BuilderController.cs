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
        [HttpGet("[controller]/[action]/{questionnaireId}")]
        public IActionResult Index([FromServices] IQuestionnaireService service, Guid questionnaireId)
        {
            Questionnaire modelData;
            if (questionnaireId == null || questionnaireId == Guid.Empty)
                Guid.TryParse((string)ViewData["QuestionnaireId"], out questionnaireId);
            if (questionnaireId != null && questionnaireId != Guid.Empty && service.ModelData.ContainsKey(questionnaireId))
                modelData = service.ModelData[questionnaireId];
            else 
            {
                if (questionnaireId == null || questionnaireId == Guid.Empty)
                    questionnaireId = Guid.NewGuid();
                modelData = new Questionnaire()
                    .Id(questionnaireId);
                service.ModelData.Add(questionnaireId, modelData);
            }
            return View(modelData);
        }
        [HttpGet("")]
        [HttpGet("/Builder/Index")]
        public IActionResult Index([FromServices] IQuestionnaireService service)
        {
            Questionnaire modelData;
            Guid questionnaireId = Guid.Empty;
            if (questionnaireId == null || questionnaireId == Guid.Empty)
                Guid.TryParse((string)ViewData["QuestionnaireId"], out questionnaireId);
            if (questionnaireId != null && questionnaireId != Guid.Empty && service.ModelData.ContainsKey(questionnaireId))
                modelData = service.ModelData[questionnaireId];
            else
            {
                if (questionnaireId == null || questionnaireId == Guid.Empty)
                    questionnaireId = Guid.NewGuid();
                modelData = new Questionnaire()
                    .Id(questionnaireId);
                service.ModelData.Add(questionnaireId, modelData);
            }
            return View(modelData);
        }

    }
}