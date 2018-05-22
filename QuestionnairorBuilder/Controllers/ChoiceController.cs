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
        [HttpGet]
        public IActionResult Add(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Add/Get", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Add/Get", id, data = questionId });
            }
            Choice modelData = new Choice(question.Choices.Count);
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            return View(modelData);
        }

        [HttpPost]
        public IActionResult AddEmpty(Guid id, Guid questionId, [FromServices]IQuestionnaireService service)
        {
            return RedirectToAction("Add", "Choice", new { id, questionId });
        }

        [HttpPost]
        public IActionResult Add(Guid id, Guid questionId, int? value, string text, bool? isDefault, [FromServices] IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Add", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Add", id, data = "" });
            }
            if (!value.HasValue || !ModelState.IsValid)
            {
                return RedirectToAction("Add", "Choice", new { id, questionId = question.Id });
            }
            Choice choice = new Choice((int)value).Text(text);
            if (isDefault.HasValue) choice.IsDefault((bool)isDefault);
            question.Choices.Add(choice);
            return RedirectToAction("Edit", "Choice", new { id, questionId = question.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Edit/Get", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Edit/Get", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Edit/Get", id, data = value });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, Guid questionId, int value, int? changedValue, string text, bool isDefault, [FromServices]IQuestionnaireService service)
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
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Edit/Get", id, data = value });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Choice", new { id, questionId, value });
            }
            if (changedValue == 5) //TODO: check for unique
            {
                return RedirectToAction("Edit", "Choice", new { id, questionId, value });
            }
            choice.Value(value).Text(text).IsDefault(isDefault);
            return RedirectToAction("Edit", "Question", new { id, questionId });
        }

        [HttpGet]
        public IActionResult Delete(Guid id, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Delete/Get", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Choice", action = "Delete/Get", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Delete/Get", id, data = value });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(choice);
        }

        [HttpPost]
        [HttpDelete]
        public IActionResult Delete(Guid id, Guid questionId, int value, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Delete", id, data = questionId });
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