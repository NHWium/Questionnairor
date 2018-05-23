using Microsoft.AspNetCore.Mvc;
using Questionnairor.Areas.Builder.Controllers;
using Questionnairor.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QuestionnairorUnitTests
{
    public class BuilderControllerTests
    {
        private readonly ITestOutputHelper output;

        public BuilderControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task HomeIndexReturnsViewWithModel()
        {
            IQuestionnaireService service = new QuestionnaireService();
            HomeController controller = new HomeController();
            var result = controller.Index(service);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(service.QuestionnaireData, viewResult.Model);
            Assert.IsNotType<RedirectToActionResult>(result);
            //            Assert.Equal("Questionnaire", redirectToActionResult.ControllerName);
            //            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
