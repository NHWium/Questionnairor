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
        [HttpGet]
        public IActionResult Index([FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData = service.Get(HttpContext);
            return View(modelData);
        }

        [HttpGet]
        public IActionResult Add()
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
            service.Data = modelData;
            return RedirectToAction("Edit", "Questionnaire");
        }

        [HttpGet]
        public IActionResult Load()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Load(string jsonData, [FromServices]IQuestionnaireService service)
        {
            Questionnaire modelData = Questionnaire.FromJson(jsonData);
            if (!service.IsValid(modelData.Id) && modelData.Title == "Not Loaded")
            {
                return BadRequest(new { error = "Illegal json", controller = "Questionnaire", action = "Load", id = "", data = jsonData });
            }
            service.Data = modelData;
            return RedirectToAction("Edit", "Questionnaire");
        }

        [HttpGet]
        public IActionResult Edit([FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Questionnaire", action = "Edit", id = "", data = service.Data.ToJson(Formatting.None) });
            }
            return View(service.Data);
        }

        [HttpPost, HttpPatch]
        public IActionResult Update(Questionnaire modelData, [FromServices]IQuestionnaireService service)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", modelData);
            }
            if (!service.IsValid() && !service.IsValid(modelData.Id))
            {
                if (modelData == null) modelData = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Questionnaire", action = "Update", id = "", data = modelData.ToJson(Formatting.None) });
            }
            service.Data.Title(modelData.Title).Introduction(modelData.Introduction).Conclusion(modelData.Conclusion);
            return RedirectToAction("Edit", "Questionnaire");
        }

        [HttpGet]
        public IActionResult Remove([FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid())
            {
                if (service.Data == null) service.Data = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Questionnaire", action = "Remove", id = "", data = service.Data.ToJson(Formatting.None) });
            }
            return View("Remove", service.Data);
        }

        [HttpPost, HttpDelete]
        public IActionResult Delete(Questionnaire modelData, [FromServices]IQuestionnaireService service)
        {
            if (!service.IsValid() && !service.IsValid(modelData.Id))
            {
                if (modelData == null) modelData = new Questionnaire().Id(Guid.Empty);
                return BadRequest(new { error = "Illegal questionnaire", controller = "Questionnaire", action = "Delete", id = "", data = modelData.ToJson(Formatting.None) });
            }
            service.Data = new Questionnaire().Id(Guid.Empty);
            service.Clear(HttpContext);
            return RedirectToAction("Index", "Questionnaire");
        }
    }
}