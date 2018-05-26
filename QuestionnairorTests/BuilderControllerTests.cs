using Microsoft.AspNetCore.Http;
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
        public void QuestionnaireIndexReturnsViewWithModel()
        {
            HttpContext httpContext = new DefaultHttpContext();
            IQuestionnaireService service = new QuestionnaireService();
            QuestionnaireController controller = new QuestionnaireController();
            var result = controller.Index(service);
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(service.Data, viewResult.Model);
            Assert.IsNotType<RedirectToActionResult>(result);
        }
    }
}
