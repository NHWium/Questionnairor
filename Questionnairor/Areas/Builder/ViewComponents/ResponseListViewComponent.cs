using Microsoft.AspNetCore.Mvc;
using Questionnairor.Areas.Builder.Models;
using Questionnairor.Models;
using Questionnairor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Builder.ViewComponents
{
    public class ResponseListViewComponent : ViewComponent
    {
        private IQuestionnaireService service;
        public ResponseListViewComponent(IQuestionnaireService service)
        {
            this.service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid questionId)
        {
            await Task.Yield();
            if (service == null || !service.IsValid())
            {
                return Content("...");
                // return Content("");
            }
            Question question = service.Data.GetQuestion(questionId);
            if (question == null)
            {
                return Content("..");
                // return Content("");
            }
            List<Response> list = service.Data.GetAvailableResponses(question);
            if (list == null || list.Count == 0)
            {
                return Content(".");
                // return Content("");
            }
            ResponseList modelData = new ResponseList();
            modelData.ResponseId = null;
            modelData.Responses = list;
            return View(modelData);
        }
    }
}
