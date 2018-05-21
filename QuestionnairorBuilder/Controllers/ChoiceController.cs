using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireData.Models;

namespace QuestionnairorBuilder.Controllers
{
    public class ChoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add([FromBody]Questionnaire questionnaireData, Question questionData)
        {
            TempData["model"] = null;
            if (questionnaireData == null || questionnaireData.Questions == null)
                return RedirectToAction("Index", "Builder");
            //Question question = modelData.Questions.Where<Question>(q => q.Id == questionId).FirstOrDefault();
            //if (question == null) question = new Question();
            questionData.Choices.Add(new Choice(0));
            TempData["model"] = questionnaireData.ToJson(Newtonsoft.Json.Formatting.None);
            return RedirectToAction("Index", "Builder");
        }

        [HttpPost]
        public IActionResult Remove([FromBody]Questionnaire questionnaireData, Question questionData)
        {
            TempData["model"] = null;
            if (questionnaireData == null || questionnaireData.Questions == null)
                return RedirectToAction("Index");
            //Question question = questionnaireData.Questions.Where<Question>(q => q.Id == questionId).FirstOrDefault();
            //if (question == null) question = new Question();
            if (questionData.Choices.Count > 0)
                questionData.Choices.RemoveAt(questionData.Choices.Count - 1);
            TempData["model"] = questionnaireData.ToJson(Newtonsoft.Json.Formatting.None);
            return RedirectToAction("Index", "Builder");
        }
    }
}