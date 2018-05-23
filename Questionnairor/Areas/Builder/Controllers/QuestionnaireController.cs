using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Questionnairor.Models;
using Questionnairor.Services;
using Newtonsoft.Json;

namespace Questionnairor.Areas.Builder.Controllers
{
    [Area("Builder")]
    public class QuestionnaireController : Controller
    {

        [HttpGet, HttpPost]
        public IActionResult Add([FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData = new Questionnaire();
            return View("Add", modelData);
        }

        [HttpPost]
        public IActionResult Create(Questionnaire modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", modelData);
            }
            if (service.UnusedId(modelData.Id))
                service.QuestionnaireData.Add(modelData.Id, modelData);
            else if (service.ValidId(modelData.Id))
                service.QuestionnaireData[modelData.Id] = modelData;
            else
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Add", modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            return RedirectToAction("Edit", "Questionnaire", new { questionnaireId = modelData.Id });
        }

        [HttpPost]
        public IActionResult Load(string jsonData, [FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData = Questionnaire.FromJson(jsonData);
            if (service.UnusedId(modelData.Id))
                service.QuestionnaireData.Add(modelData.Id, modelData);
            else if (service.ValidId(modelData.Id))
                service.QuestionnaireData[modelData.Id] = modelData;
            else
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Load", questionnaireId = modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            return RedirectToAction("Edit", "Questionnaire", new { questionnaireId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid questionnaireId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Edit", questionnaireId, data = "" });
            }
            Questionnaire modelData = service.QuestionnaireData[questionnaireId];
            ViewData["Id"] = questionnaireId;
            return View(modelData);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Questionnaire modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Id"] = modelData.Id;
                return View("Edit", modelData);
            }
            if (!service.ValidId(modelData.Id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Update", modelData.Id, data = modelData.ToJson(Formatting.None) });
            }
            service.QuestionnaireData[modelData.Id].Title(modelData.Title).Introduction(modelData.Introduction);
            return RedirectToAction("Edit", "Questionnaire", new { questionnaireId = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid questionnaireId, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Remove", questionnaireId, data = "" });
            }
            ViewData["Id"] = questionnaireId;
            return View("Remove", service.QuestionnaireData[questionnaireId]);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid questionnaireId, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(questionnaireId))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Delete", questionnaireId, data = "" });
            }
            service.QuestionnaireData.Remove(questionnaireId);
            return RedirectToAction("Index", "Home");
        }
    }
}