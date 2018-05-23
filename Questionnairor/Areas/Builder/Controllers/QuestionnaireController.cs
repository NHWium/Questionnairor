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
    public class QuestionnaireController : Controller
    {
        [HttpGet]
        public IActionResult Add([FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData = new Questionnaire();
            return View(modelData);
        }

        [HttpPost]
        public IActionResult AddEmpty([FromServices]IQuestionnaireService service)
        {
            return RedirectToAction("Add", "Questionnaire");
        }

        [HttpPost]
        public IActionResult Add(string title, string introduction, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Add", "Questionnaire");
            Questionnaire modelData;
            Guid id = Guid.NewGuid();
            modelData = new Questionnaire()
                .Id(id)
                .Title(title)
                .Introduction(introduction);
            if (id != null && id != Guid.Empty && !service.QuestionnaireData.ContainsKey(id))
                service.QuestionnaireData.Add(id, modelData);
            else if (modelData.Id != Guid.Empty && service.QuestionnaireData.ContainsKey(modelData.Id))
                service.QuestionnaireData[modelData.Id] = modelData;
            else
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Add", id, data = "" });
            }
            return RedirectToAction("Edit", "Questionnaire", new { id });
        }

        [HttpPost]
        public IActionResult Load(string jsonData, [FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData;
            modelData = Questionnaire.FromJson(jsonData);
            if (modelData.Id != Guid.Empty && !service.QuestionnaireData.ContainsKey(modelData.Id))
            {
                service.QuestionnaireData.Add(modelData.Id, modelData);
            }
            else if (modelData.Id != Guid.Empty && service.QuestionnaireData.ContainsKey(modelData.Id))
                service.QuestionnaireData[modelData.Id] = modelData;
            else
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Load", questionnaireId = modelData.Id, data = modelData.ToJson(Newtonsoft.Json.Formatting.None) });
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Edit(Guid id, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Edit/Get", id, data = "" });
            }
            Questionnaire modelData = service.QuestionnaireData[id];
            ViewData["Id"] = id;
            return View(modelData);
        }

        [HttpPost]
        public IActionResult Edit(Guid id, string title, string introduction, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Edit/Post", id, data = "" });
            }
            if (string.IsNullOrEmpty(title) || !ModelState.IsValid)
            {
                return RedirectToAction("Edit", "Questionnaire", new { id });
            }
            service.QuestionnaireData[id].Title(title).Introduction(introduction);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Delete(Guid id, [FromServices]IQuestionnaireService service)
        {
            if (!service.ValidId(id))
            {
                return BadRequest(new { error = "Illegal questionnaire identifier", controller = "Questionnaire", action = "Delete/Get", id, data = "" });
            }
            return View("Delete", service.QuestionnaireData[id]);
        }

        [HttpPost]
        [HttpDelete]
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