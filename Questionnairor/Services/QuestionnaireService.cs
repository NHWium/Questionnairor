using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questionnairor.Models;
using Questionnairor.Extensions;
using Microsoft.EntityFrameworkCore;
using Questionnairor.Data;

namespace Questionnairor.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        public Questionnaire Data { get; set; }


        public QuestionnaireService()
        {
            Data = new Questionnaire();
        }

        public Questionnaire Get(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext != null && httpContext.Session.IsAvailable)
                Data = httpContext.Session.Get("QuestionnairorQuestionnaire");
            else
                Data = new Questionnaire();
            return Data;
        }

        public void Set(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (IsValid() && httpContext != null)
                httpContext.Session.Set("QuestionnairorQuestionnaire", Data);
        }

        public void Clear(Microsoft.AspNetCore.Http.HttpContext httpContext)
        {
            if (httpContext != null && httpContext.Session.IsAvailable)
                httpContext.Session.Clear();
        }

        public bool IsValid()
        {
            return (Data != null && Data.Id != null && Data.Id != Guid.Empty);
        }
        public bool IsValid(Guid id)
        {
            return IsValid() && id != null && id != Guid.Empty && id == Data.Id;
        }
    }
}
