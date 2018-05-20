using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireData.Models;
using QuestionnairorBuilder.Services;

namespace QuestionnairorBuilder.Controllers
{
    public class BuilderController : Controller
    {

        public IActionResult Index([FromServices]IQuestionnaireService service)
        {
            Questionnaire model = service.Model;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddChoice([FromServices]IQuestionnaireService service, Guid id)
        {
            Questionnaire model = service.Model;
            Question question = model.Questions.Where<Question>(q => q.Id == id).FirstOrDefault();
            if (question == null) question = new Question();
            question.Choices.Add(new Choice(question.Choices.Count));
            return RedirectToAction("Index", "Builder");
        }

        [HttpPost]
        public IActionResult RemoveChoice([FromServices]IQuestionnaireService service, Guid id)
        {
            Questionnaire model = service.Model;
            Question question = model.Questions.Where<Question>(q => q.Id == id).FirstOrDefault();
            if (question == null) question = new Question();
            question.Choices.RemoveAt(question.Choices.Count-1);
            return RedirectToAction("Index", "Builder");
        }

        [HttpPost]
        public IActionResult AddQuestion([FromServices]IQuestionnaireService service)
        {
            Questionnaire model = service.Model;
            model.Questions.Add(new Question());
            return RedirectToAction("Index", "Builder");
        }

    }
}