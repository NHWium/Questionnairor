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
    public class ChoiceController : Controller
    {
        [HttpGet, HttpPost]
        public IActionResult Add(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Add", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Add", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, question.Choices.Count);
            if (choice == null)
            {
                choice = new Choice().Value(question.Choices.Count);
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            return View(choice);
        }

        [HttpPost]
        public IActionResult Create(Guid id, Guid questionId, Choice modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Create", id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Create", id, data = modelData.ToJson(Formatting.None) });
            }
            if (question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["Id"] = id;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            Choice choice = new Choice().Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            question.Choices.Add(choice);
            return RedirectToAction("Edit", "Choice", new { id, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Edit", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Edit", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Edit", id, data = value });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost]
        public IActionResult Update(Guid id, Guid questionId, int oldValue, Choice modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = id;
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = oldValue;
                return View("Edit", modelData);
            }
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Update", id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Update", id, data = modelData.ToJson(Formatting.None) });
            }
            if (oldValue != modelData.Value && question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["Id"] = id;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            Choice choice = service.GetChoice(id, questionId, oldValue);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Update", id, data = modelData.ToJson(Formatting.None) });
            }
            choice.Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            return RedirectToAction("Edit", "Choice", new { id, questionId, value = modelData.Value });
        }

        [HttpGet]
        public IActionResult Remove(Guid id, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Remove", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Remove", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Remove", id, data = value });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid id, Guid questionId, int value, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Delete", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Delete", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Delete", id, data = value });
            }
            question.Choices.Remove(choice);
            return RedirectToAction("Edit", "Question", new { id, questionId });
        }

    }
}