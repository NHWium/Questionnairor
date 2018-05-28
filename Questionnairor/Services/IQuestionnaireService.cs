using Microsoft.AspNetCore.Http;
using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Questionnairor.Services
{
    public interface IQuestionnaireService
    {
        Questionnaire Data { get; set; }
        Questionnaire Get(HttpContext httpContext);
        void Set(HttpContext httpContext);
        void Clear(HttpContext httpContext);
        bool IsValid();
        bool IsValid(Guid id);
    }
}