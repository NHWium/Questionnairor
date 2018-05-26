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
        [HttpGet]
        public IActionResult Add(Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Add", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Add", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            int i = question.Choices.Count;
            while (question.ChoiceValueExists(i))
            {
                i++;
            }
            Choice choice = new Choice().Value(i);
            ViewData["QuestionId"] = questionId;
            return View(choice);
        }

        [HttpPost]
        public IActionResult Create(Guid questionId, Choice modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Create", id = questionId, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Create", id = questionId, data = modelData.ToJson(Formatting.None) });
            }
            if (question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            if (modelData.IsDefault && question.ChoiceDefaultSelected())
            {
                ModelState.AddModelError(nameof(modelData.IsDefault), "Only one choice in a question can be selected as default.");
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            Choice choice = new Choice().Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            question.Choices.Add(choice);
//            service.Set(HttpContext);
            return RedirectToAction("Edit", "Choice", new { questionId, value = modelData.Value });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Edit", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Edit", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Edit", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            ViewData["QuestionId"] = questionId;
            return View(choice);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid questionId, int oldValue, Choice modelData, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Update", id = oldValue, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Update", id = oldValue, data = modelData.ToJson(Formatting.None) });
            }
            if (oldValue != modelData.Value && question.ChoiceValueExists(modelData.Value))
            {
                ModelState.AddModelError(nameof(modelData.Value), "Each choice in a question must have an unique value.");
                ViewData["QuestionId"] = questionId;
                return View("Add", modelData);
            }
            if (!ModelState.IsValid)
            {
                ViewData["QuestionId"] = questionId;
                modelData.Value = oldValue;
                return View("Edit", modelData);
            }
            Choice choice = question.GetChoice(oldValue);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Update", id = oldValue, data = service.Data.ToJson(Formatting.None) });
            }
            choice.Value(modelData.Value).Text(modelData.Text).IsDefault(modelData.IsDefault);
            service.Set(HttpContext);
            return RedirectToAction("Edit", "Choice", new { questionId, value = modelData.Value });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Remove", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Remove", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Remove", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            ViewData["QuestionId"] = questionId;
            return View(choice);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionId, Choice modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["QuestionId"] = questionId;
                return View("Remove", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Delete", id = modelData.Value, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Delete", id = modelData.Value, data = modelData.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(modelData.Value);
            if (choice == null)
            {
                if (modelData == null) modelData = new Choice();
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Delete", id = modelData.Value, data = modelData.ToJson(Formatting.None) });
            }
            question.Choices.Remove(modelData);
            service.Set(HttpContext);
            return RedirectToAction("Edit", "Question", new { questionId });
        }

    }
}