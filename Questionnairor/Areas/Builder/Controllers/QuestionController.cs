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
        public IActionResult Add(Guid questionnaireId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", questionnaireId, data = "" });
            }
            Question question = new Question();
            ViewData["Id"] = questionnaireId;
            return View(question);
        }

        [HttpPost]
        public IActionResult Create(Guid questionnaireId, Question modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                return View("Add", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Add", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            Question question = new Question().Id(modelData.Id).Title(modelData.Title).Text(modelData.Text);
            service.QuestionnaireData[questionnaireId].Questions.Add(question);
            return RedirectToAction("Edit", "Question", new { questionnaireId, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionnaireId, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Edit", questionnaireId, data = questionId });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Edit", questionnaireId, data = questionId });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            return View(question);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid questionnaireId, Question modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = modelData.Id;
                return View("Edit", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(questionnaireId, modelData.Id);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            question.Title(modelData.Title).Text(modelData.Text);
            return RedirectToAction("Edit", "Question", new { questionnaireId, questionId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionnaireId, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Remove", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Remove", questionnaireId, data = questionId });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            return View(question);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionnaireId, Guid questionId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Question", action = "Delete", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Delete", questionnaireId, data = questionId });
            }
            service.QuestionnaireData[questionnaireId].Questions.Remove(question);
            return RedirectToAction("Edit", "Questionnaire", new { questionnaireId });
        }
    }
}