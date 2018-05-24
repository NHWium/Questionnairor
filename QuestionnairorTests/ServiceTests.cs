using Microsoft.AspNetCore.Mvc;
using Questionnairor.Areas.Builder.Controllers;
using Questionnairor.Models;
using Questionnairor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QuestionnairorUnitTests
{
    public class ServiceTests
    {
        private readonly ITestOutputHelper output;

        public ServiceTests(ITestOutputHelper output)
        {
            this.output = output;
        }

    }
}
