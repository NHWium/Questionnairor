using Microsoft.AspNetCore.Mvc;
using QuestionnairorBuilder.Controllers;
using QuestionnairorBuilder.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace QuestionnairorUnitTests
{
    public class ControllerTests
    {
        private readonly ITestOutputHelper output;

        public ControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task QuestionnaireIndexReturnsViewWithModel()
        {
            IQuestionnaireService service = new QuestionnaireService();
            QuestionnaireController controller = new QuestionnaireController();
            var result = controller.Index(service);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(service.QuestionnaireData, viewResult.Model);
            Assert.IsNotType<RedirectToActionResult>(result);
            //            Assert.Equal("Questionnaire", redirectToActionResult.ControllerName);
            //            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
