using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnairor.Models;
using Questionnairor.Services;

namespace Questionnairor.Areas.Builder.Controllers
{
    [Area("Builder")]
    public class ResponseController : Controller
    {
        [HttpGet]
        public IActionResult Add(Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Response", action = "Add", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Response", action = "Add", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Add", id = value, data = service.Data.ToJson(Formatting.None) });
            }
            Response response = new Response();
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost]
        public IActionResult Create(Guid questionId, int value, Response modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = value;
                return View("Add", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Response", action = "Create", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Response", action = "Create", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Create", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Response response = new Response().Title(modelData.Title).Feedback(modelData.Feedback).MinimumChoices(modelData.MinimumChoices);
            choice.Responses.Add(response);
            service.Set(HttpContext);
            return RedirectToAction("Edit", "Response", new { questionId, value, responseId = response.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Response", action = "Edit", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Response", action = "Edit", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Edit", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Response response = choice.GetResponse(responseId);
            if (response == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Edit", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid questionId, int value, Response modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = value;
                return View("Edit", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Response", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Response", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Response response = choice.GetResponse(modelData.Id);
            if (response == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            response.Title(modelData.Title).MinimumChoices(modelData.MinimumChoices).Feedback(modelData.Feedback);
            return RedirectToAction("Edit", "Response", new { questionId, value, responseId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Response", action = "Remove", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Response", action = "Remove", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Delete/Get", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Response response = choice.GetResponse(responseId);
            if (response == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Remove", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionId, int value, Response modelData, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Response response = choice.GetResponse(modelData.Id);
            if (response == null)
            {
                if (modelData == null) modelData = new Response().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            choice.Responses.Remove(response);
            return RedirectToAction("Edit", "Choice", new { questionId, oldValue = value });
        }

        [HttpPost]
        public IActionResult AddShared(Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Choice", action = "AddShared", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Choice", action = "AddShared", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Choice choice = question.GetChoice(value);
            if (choice == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal choice", controller = "Choice", action = "AddShared", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            Response response = service.Data.GetResponse(responseId);
            if (response == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "AddShared", id = responseId, data = service.Data.ToJson(Formatting.None) });
            }
            choice.Responses.Add(response);
            return RedirectToAction("Edit", "Response", new { questionId, value, responseId });
        }
    }
}