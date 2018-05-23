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
        [HttpGet, HttpPost]
        public IActionResult Add(Guid questionnaireId, Guid questionId, int value, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Add", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Add", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Add", questionnaireId, data = value });
            }
            Response response = new Response();
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost]
        public IActionResult Create(Guid questionnaireId, Guid questionId, int value, Response modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = value;
                return View("Add", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Create", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Create", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Create", questionnaireId, data = value });
            }
            Response response = new Response().Feedback(modelData.Feedback).MinimumChoices(modelData.MinimumChoices);
            choice.Responses.Add(response);
            if (service.UnusedResponse(response.Id))
            {
                service.ResponseData.Add(response.Id, response);
            }
            return RedirectToAction("Edit", "Response", new { questionnaireId, questionId, value, responseId = response.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionnaireId, Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Edit", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Edit", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Edit", questionnaireId, data = value });
            }
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Edit", questionnaireId, data = responseId });
            }
            Response response = service.GetResponse(questionnaireId, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Edit", questionnaireId, data = responseId });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Guid questionnaireId, Guid questionId, int value, Response modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = questionnaireId;
                ViewData["QuestionId"] = questionId;
                ViewData["ChoiceValue"] = value;
                return View("Edit", modelData);
            }
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Update", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Update", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Update", questionnaireId, data = value });
            }
            if (!service.ValidResponse(modelData.Id))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Update", questionnaireId, data = modelData.Id });
            }
            Response response = service.GetResponse(questionnaireId, questionId, value, modelData.Id);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Update", questionnaireId, data = modelData.ToJson(Formatting.None) });
            }
            response.MinimumChoices(modelData.MinimumChoices).Feedback(modelData.Feedback);
            service.ResponseData[modelData.Id] = response;
            return RedirectToAction("Edit", "Response", new { questionnaireId, questionId, value, responseId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionnaireId, Guid questionId, int value, Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Response", action = "Remove", questionnaireId, data = "" });
            }
            Question question = service.GetQuestion(questionnaireId, questionId);
            if (question == null)
            {
                return BadRequest(new { error = "Illegal question identifier", controller = "Response", action = "Remove", questionnaireId, data = questionId });
            }
            Choice choice = service.GetChoice(questionnaireId, questionId, value);
            if (choice == null)
            {
                return BadRequest(new { error = "Illegal choice", controller = "Response", action = "Delete/Get", questionnaireId, data = value });
            }
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Remove", questionnaireId, data = responseId });
            }
            Response response = service.GetResponse(questionnaireId, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Remove", questionnaireId, data = responseId });
            }
            ViewData["Id"] = questionnaireId;
            ViewData["QuestionId"] = questionId;
            ViewData["ChoiceValue"] = value;
            return View(response);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionnaireId, Guid questionId, int value, Guid responseId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Choice", action = "Delete", questionnaireId, data = questionId });
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
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "Delete", questionnaireId, data = responseId });
            }
            Response response = service.GetResponse(questionnaireId, questionId, value, responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "Delete", questionnaireId, data = response.ToJson(Formatting.None) });
            }
            //Note, will only remove from the current choice, not from general repository
            choice.Responses.Remove(response);
            return RedirectToAction("Edit", "Choice", new { questionnaireId, questionId });
        }

        [HttpPost, HttpDelete]
        public IActionResult DeleteById(Guid responseId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidResponse(responseId))
            {
                return BadRequest(new { error = "Illegal response identifier", controller = "Response", action = "DeleteById", responseId, data = "" });
            }
            Response response = service.GetResponse(responseId);
            if (response == null)
            {
                return BadRequest(new { error = "Illegal response", controller = "Response", action = "DeleteById", responseId, data = "" });
            }
            int amount = service.RemoveAllResponse(response);
            if (amount == 0)
            {
                return BadRequest(new { error = "Could not delete response", controller = "Response", action = "DeleteById", responseId, data = response.ToJson(Formatting.None) });
            }
            return RedirectToAction("Index", "Questionnaire");
        }

    }
}