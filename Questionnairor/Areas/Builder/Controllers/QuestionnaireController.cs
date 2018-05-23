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
            return RedirectToAction("Edit", "Questionnaire", new { modelData.Id });
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
            return RedirectToAction("Edit", "Questionnaire", new { modelData.Id });
        }

        [HttpGet]
        public IActionResult Edit(Guid id, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Edit", id, data = "" });
            }
            Questionnaire modelData = service.QuestionnaireData[id];
            ViewData["Id"] = id;
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
            return RedirectToAction("Edit", "Questionnaire", new { id = modelData.Id });
        }

        [HttpGet]
        public IActionResult Remove(Guid id, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Remove", id, data = "" });
            }
            ViewData["Id"] = id;
            return View("Remove", service.QuestionnaireData[id]);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Guid id, bool confirm, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Delete", id, data = "" });
            }
            service.QuestionnaireData.Remove(id);
            return RedirectToAction("Index", "Home");
        }
    }
}