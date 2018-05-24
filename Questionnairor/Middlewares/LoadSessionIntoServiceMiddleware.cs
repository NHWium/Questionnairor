using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questionnairor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Middlewares
{
    public class LoadSessionIntoServiceMiddleware : IMiddleware
    {
        private IQuestionnaireService service;

        public LoadSessionIntoServiceMiddleware(IQuestionnaireService service)
        {
            this.service = service;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            service.Get(httpContext);
            await next(httpContext);
            service.Set(httpContext);
        }
    }
}
