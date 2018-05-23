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
    public class ResponseController : Controller
    {
        [HttpGet, HttpPost]
        public IActionResult Add(Guid id, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Add/Get", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Add/Get", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Add/Get", id, data = value });
            }
            Response response = new Response();
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost]
        public IActionResult Create(Guid id, Guid questionId, int value, int? minimumChoices, string feedback, [FromServices] IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Add", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Add", id, data = "" });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Add", id, data = value });
            }
            if (string.IsNullOrEmpty(feedback) || !ModelState.IsValid)
            {
                return RedirectToAction("Add", "Response", new { id, questionId, value });
            }
            Response response = new Response()
                .Feedback(feedback);
            if (minimumChoices.HasValue) response.MinimumChoices(value);
            if (service.UnusedResponse(response.Id))
            {
                choice.Responses.Add(response);
                service.ResponseData.Add(response.Id, response);
            }
            return RedirectToAction("Edit", "Response", new { id, questionId, value, responseId = response.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Edit/Get", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Edit/Get", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Edit/Get", id, data = value });
            }
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Edit/Get", id, data = responseId });
            }
            Response response = service.GetResponse(id, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Edit/Get", id, data = responseId });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, Guid questionId, int value, Guid responseId, int minimumChoices, string feedback, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Edit", id, data = questionId });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Edit", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Edit", id, data = value });
            }
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Edit", id, data = responseId });
            }
            Response response = service.GetResponse(id, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Edit", id, data = responseId });
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Response", new { id, questionId, value, responseId });
            }
            response.MinimumChoices(minimumChoices).Feedback(feedback);
            service.ResponseData[responseId] = response;
            return RedirectToAction("Edit", "Choice", new { id, questionId, value });
        }

        [HttpGet]
        public IActionResult Delete(Guid id, Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Delete/Get", id, data = "" });
            }
            Question question = service.GetQuestion(id, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Delete/Get", id, data = questionId });
            }
            Choice choice = service.GetChoice(id, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Delete/Get", id, data = value });
            }
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Delete/Get", id, data = responseId });
            }
            Response response = service.GetResponse(id, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Delete/Get", id, data = responseId });
            }
            ViewData["Id"] = id;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost]
        [HttpDelete]
        public IActionResult Delete(Guid id, Guid questionId, int value, Guid responseId, bool confirm, [FromServices]IQuestionnaireService service)
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
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Delete/Get", id, data = responseId });
            }
            Response response = service.GetResponse(id, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Delete/Get", id, data = responseId });
            }
            choice.Responses.Remove(response);
            return RedirectToAction("Edit", "Choice", new { id, questionId });
        }

        [HttpPost]
        [HttpDelete]
        public IActionResult DeleteById(Guid responseId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "DeleteById", responseId, data = "" });
            }
            //TODO: global deleter
            return RedirectToAction("Index", "Questionnaire");
        }

    }
}