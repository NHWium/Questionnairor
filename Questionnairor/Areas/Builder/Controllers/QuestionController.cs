using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Questionnairor.Models;
using Questionnairor.Services;

namespace Questionnairor.Areas.Builder.Controllers
{
    [Area("Builder")]
    public class QuestionController : Controller
    {
        [HttpGet]
        public IActionResult Add(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add/Get", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                question = new Question();
            }
            ViewData["Id"] = id;
            return View(question);
        }

        [HttpPost]
        public IActionResult AddEmpty(Guid id, [FromServices]IQuestionnaireService service)
        {
            return RedirectToAction("Add", "Question", new { id });
        }

        [HttpPost]
        public IActionResult Add(Guid id, string title, string text, [FromServices] IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", id, data = "" });
            }
            if (string.IsNullOrEmpty(title) || !ModelState.IsValid)
            {
                return RedirectToAction("Add", "Question", new { id });
            }
            Question question = new Question()
                .Title(title)
                .Text(text);
            service.QuestionnaireData[id].Questions.Add(question);
            return RedirectToAction("Edit", "Question", new { id, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Edit/Get", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Edit/Get", id, data = questionId });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            return View(question);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, Guid questionId, string title, string text, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Edit", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Edit", id, data = questionId });
            }
            if (string.IsNullOrEmpty(title) || !ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Question", new { id, questionId });
            }
            question.Title(title).Text(text);
            return RedirectToAction("Edit", "Questionnaire", new { id });
        }


        [HttpGet]
        public IActionResult Delete(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Delete", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Delete", id, data = questionId });
            }
            ViewData["Id"] = id;
            return View(question);
        }

        [HttpPost]
        [HttpDelete]
        public IActionResult Delete(Guid id, Guid questionId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Edit", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Edit", id, data = questionId });
            }
            service.QuestionnaireData[id].Questions.Remove(question);
            return RedirectToAction("Edit", "Questionnaire", new { id });
        }
    }
}