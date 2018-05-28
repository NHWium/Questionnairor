using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Questionnairor.Models;
using Questionnairor.Services;

namespace Questionnairor.Areas.API
{
    [Produces("application/json")]
    [Route("api/Questionnaire")]
    public class Questionnaire : Controller
    {
        /// <summary>
        /// Standard web api to submit the questionnaire.
        /// </summary>
        /// <param name="value">Fully valid json (don't forget \"quotes\")</param>
        /// <returns>Json data with the result/status, a text to possibly display, and the data after processing.</returns>
        // POST: api/Questionnaire
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            Models.Questionnaire data = Models.Questionnaire.FromJson(value);
            if (data == null || data.Id == null || data.Id == Guid.Empty || data.Title == "Not loaded")
                return BadRequest(new { status = "Error", text = "Badly formed json", data = value });
            if (!data.IsAllAnswered())
                return Ok(new { status = "Incomplete", text = "Each choice must be answered before the questionnaire can be submitted.", responses = "", data = data.ToJson(Formatting.None) });
            List<Response> responses = data.GetActiveResponses(data.GetAnswers());
            return Ok(new { status = "AllAnswered", text = "", responses, data = data.ToJson(Formatting.None) });
        }
    }
}
