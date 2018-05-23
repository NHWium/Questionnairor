using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnairor.Extensions;
using Questionnairor.Models;
using Questionnairor.Services;

namespace Questionnairor.Areas.Builder.Controllers
{
    [Area("Builder")]
    public class QuestionController : Controller
    {
        [HttpGet, HttpPost]
        public IActionResult Add(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", id, data = questionId });
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
        public IActionResult Create(Guid id, Question modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                return View("Add", modelData);
            }
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = new Question().Id(modelData.Id).Title(modelData.Title).Text(modelData.Text);
            service.QuestionnaireData[id].Questions.Add(question);
            return RedirectToAction("Edit", "Question", new { id, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
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
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            return View(question);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid id, Question modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                ViewData["QuestionId"] = modelData.Id;
                return View("Edit", modelData);
            }
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Update", id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(id, modelData.Id);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Update", id, data = modelData.ToJson(Formatting.None) });
            }
            question.Title(modelData.Title).Text(modelData.Text);
            return RedirectToAction("Edit", "Question", new { id, questionId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Remove", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Remove", id, data = questionId });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            return View(question);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid id, Guid questionId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Delete", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Delete", id, data = questionId });
            }
            service.QuestionnaireData[id].Questions.Remove(question);
            return RedirectToAction("Edit", "Questionnaire", new { id });
        }
    }
}