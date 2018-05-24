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
        [HttpGet]
        public IActionResult Add([FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Add", id = "", data = service.Data.ToJson(Formatting.None) });
            }
            Question question = new Question();
            return View(question);
        }

        [HttpPost]
        public IActionResult Create(Question modelData, [FromServices] IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Question().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Create", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = new Question().Id(modelData.Id).Title(modelData.Title).Text(modelData.Text);
            service.Data.Questions.Add(question);
            return RedirectToAction("Edit", "Question", new { questionId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Edit", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Question", action = "Edit", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            return View(question);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Question modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", modelData);
            }
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Question().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(modelData.Id);
            if (question == null)
            {
                if (modelData == null) modelData = new Question().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question identifier", controller = "Question", action = "Update", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            question.Title(modelData.Title).Text(modelData.Text);
            return RedirectToAction("Edit", "Question", new { questionId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionId, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Remove", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Question", action = "Remove", id = questionId, data = service.Data.ToJson(Formatting.None) });
            }
            return View(question);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Question modelData, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (modelData == null) modelData = new Question().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Question", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            Question question = service.Data.GetQuestion(modelData.Id);
            if (question == null)
            {
                if (modelData == null) modelData = new Question().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal question", controller = "Question", action = "Delete", id = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            service.Data.Questions.Remove(modelData);
            return RedirectToAction("Edit", "Questionnaire");
        }
    }
}