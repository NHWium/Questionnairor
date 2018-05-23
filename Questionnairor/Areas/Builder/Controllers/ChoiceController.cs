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
        public IActionResult Add(Guid questionnaireId, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Add", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Add", questionnaireId, data = questionId });
            }
            int i = question.Choices.Count;
            while (question.ChoiceValueExists(i))
            {
                i++;
            }
            Choice choice = new Choice().Value(i);
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            return View(choice);
        }

        [HttpPost]
        public IActionResult Create(Guid questionnaireId, Guid questionId, Choice modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Create", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Create", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            if (question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            Choice choice = new Choice().Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            question.Choices.Add(choice);
            return RedirectToAction("Edit", "Choice", new { questionnaireId, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionnaireId, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Edit", questionnaireId, data = questionId });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Edit", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Edit", questionnaireId, data = value });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid questionnaireId, Guid questionId, int oldValue, Choice modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = oldValue;
                return View("Edit", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            if (oldValue != modelData.Value && question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, oldValue);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            choice.Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            return RedirectToAction("Edit", "Choice", new { questionnaireId, questionId, value = modelData.Value });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionnaireId, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Remove", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Remove", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Remove", questionnaireId, data = value });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionnaireId, Guid questionId, int value, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Delete", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Delete", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Delete", questionnaireId, data = value });
            }
            question.Choices.Remove(choice);
            return RedirectToAction("Edit", "Question", new { questionnaireId, questionId });
        }

    }
}