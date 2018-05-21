using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireData.Models;
using QuestionnairorBuilder.Services;

namespace QuestionnairorBuilder.Controllers
{
    public class ChoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromServices]IQuestionnaireService service, Guid questionnaireId, Guid questionId)
        {
            if (questionnaireId == null || questionnaireId == Guid.Empty || !service.ModelData.ContainsKey(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Add", questionnaireId, data = questionId });
            }
            Questionnaire modelData = service.ModelData[questionnaireId];
            Question question = modelData.Questions.Where<Question>(q => q.Id == questionId).FirstOrDefault();
            if (modelData.Questions == null || question == null || questionId == null || questionId == Guid.Empty)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Add", questionnaireId, data = questionId });
            }
            question.Choices.Add(new Choice(0));
            return RedirectToAction("Index", "Builder", new { questionnaireId });
        }

        [HttpPost]
        public IActionResult Remove([FromServices]IQuestionnaireService service, Guid questionnaireId, Guid questionId)
        {
            if (questionnaireId == null || questionnaireId == Guid.Empty || !service.ModelData.ContainsKey(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Remove", questionnaireId, data = questionId });
            }
            Questionnaire modelData = service.ModelData[questionnaireId];
            Question question = modelData.Questions.Where<Question>(q => q.Id == questionId).FirstOrDefault();
            if (modelData.Questions == null || question == null || questionId == null || questionId == Guid.Empty)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Remove", questionnaireId, data = questionId });
            }
            if (question.Choices.Count > 0)
                question.Choices.RemoveAt(question.Choices.Count - 1);
            else
            {
                return BadRequest(new { error = "No choices to remove", controller = "Choice", action = "Remove", questionnaireId, data = questionId });
            }
            return RedirectToAction("Index", "Builder", new { questionnaireId });
        }
    }
}