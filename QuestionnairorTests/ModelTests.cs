using Newtonsoft.Json;
using Questionnairor.Extensions;
using Questionnairor.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace QuestionnairorUnitTests
{
    public class ModelTests
    {
        private readonly ITestOutputHelper output;

        public ModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [InlineData("{}", 1, "", "")]
        [InlineData("Illegal data", 1, "Not Loaded", "")]
        [InlineData("{\"MinimumChoices\":2,\"Title\":\"Test\"}", 2, "Test", "")]
        [InlineData("{\"MinimumChoices\":11}", 11, "", "")]
        [InlineData("{\"Title\":\"Test\",\"Feedback\":\"Test\"}", 1, "Test", "Test")]
        public void ResponseFromJsonWithoutId(string json, int minimumChoices, string title, string feedback)
        {
            Response expected = new Response()
                .Id(Guid.Empty)
                .MinimumChoices(minimumChoices)
                .Feedback(feedback)
                .Title(title);
            //Do not test Guid, as it is auto-generated
            output.WriteLine("#####\n" + expected.ToJson(Formatting.Indented));
            Assert.Equal<Response>(expected, Response.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":11,\"Feedback\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 11, "Test")]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":11}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 11, "")]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Feedback\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 1, "Test")]
        public void ResponseFromJsonWitId(string json, string id, int minimumChoices, string feedback)
        {
            Response expected = new Response()
                .Id(new Guid(id))
                .MinimumChoices(minimumChoices)
                .Feedback(feedback);
            Assert.Equal<Response>(expected, Response.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":11,\"Feedback\":\"Test\",\"Title\":\"\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 11, "Test", "")]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":11,\"Feedback\":\"Test\",\"Title\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 11, "Test", "Test")]
        public void ResponseToJson(string expected, string id, int minimumChoices, string feedback, string title)
        {
            Response json = new Response()
                .Id(new Guid(id))
                .MinimumChoices(minimumChoices)
                .Feedback(feedback)
                .Title(title);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData("{}", 0, "", false, null, null, null, null, null, null)]
        [InlineData("Illegal data", 0, "Not Loaded", true, null, null, null, null, null, null)]
        [InlineData("{\"Value\":0,\"Text\":\"Test\"}", 0, "Test", false, null, null, null, null, null, null)]
        [InlineData("{\"Value\":7,\"IsDefault\":true}", 7, "", true, null, null, null, null, null, null)]
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"Responses\":[]}", 0, "Test", false, null, null, null, null, null, null)]
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test\"}]}", 0, "Test", false, 2, "Test", null, null, null, null)]
        [InlineData("{\"Value\":5,\"Text\":\"Test\",\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":1,\"Feedback\":\"Test1\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test2\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":3,\"Feedback\":\"Test3\"}]}", 5, "Test", false, 1, "Test1", 2, "Test2", 3, "Test3")]
        public void ChoiceFromJson(string json, int value, string text, bool isDefault, int? minimumChoices1, string feedback1, int? minimumChoices2, string feedback2, int? minimumChoices3, string feedback3)
        {
            Choice expected = new Choice().Value(value)
                .Text(text)
                .IsDefault(isDefault);
            List<Response> l = new List<Response>() {};
            if (minimumChoices1 != null && feedback1 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices1)
                    .Feedback(feedback1);
                l.Add(r);
            }
            if (minimumChoices2 != null && feedback2 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices2)
                    .Feedback(feedback2);
                l.Add(r);
            }
            if (minimumChoices3 != null && feedback3 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices3)
                    .Feedback(feedback3);
                l.Add(r);
            }
            expected.Responses(l);
            Assert.Equal<Choice>(expected, Choice.FromJson(json));
        }

        [Fact]
        public void ChoiceFromJsonWithComplicatedResponses()
        {
            string json = "{\"Value\":0,\"Text\":\"Test\",\"Responses\":[{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":2,\"Feedback\":\"FeedbackTest\"}, {\"Id\":\"28301F4A-1B91-4C81-A4E8-56F370B3D30A\",\"MinimumChoices\":3}]}";
            Choice expected = new Choice().Value(0)
                .Text("Test")
                .IsDefault(false);
            Response r1 = new Response()
                .Id(new Guid("73129183-ce7b-48ef-820f-b96af9ab82c2"))
                .MinimumChoices(2)
                .Feedback("FeedbackTest");
            Response r2 = new Response()
                .Id(new Guid("28301f4a-1b91-4c81-a4e8-56f370b3d30a"))
                .MinimumChoices(3);
            List<Response> l = new List<Response>() { r1, r2 };
            expected.Responses(l);
            Assert.Equal<Choice>(expected, Choice.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[]}", 0, "Test", false, null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Value\":7,\"Text\":\"\",\"IsDefault\":true,\"Responses\":[]}", 7, "", true, null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test\",\"Title\":\"\"}]}", 0, "Test", false, 2, "Test", "", null, null, null, null, null, null)]
        [InlineData("{\"Value\":5,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":3,\"Feedback\":\"Test3\",\"Title\":\"Test3\"}]}", 5, "Test", false, 1, "Test1", "Test1", 2, "Test2", "", 3, "Test3", "Test3")]
        public void ChoiceToJson(string expected, int value, string text, bool isDefault, int? minimumChoices1, string feedback1, string title1, int? minimumChoices2, string feedback2, string title2, int? minimumChoices3, string feedback3, string title3)
        {
            Choice json = new Choice().Value(value)
                .Text(text)
                .IsDefault(isDefault);
            List<Response> l = new List<Response>() { };
            if (minimumChoices1 != null && feedback1 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices1)
                    .Feedback(feedback1)
                    .Title(title1);
                l.Add(r);
            }
            if (minimumChoices2 != null && feedback2 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices2)
                    .Feedback(feedback2)
                    .Title(title2);
                l.Add(r);
            }
            if (minimumChoices3 != null && feedback3 != null)
            {
                Response r = new Response()
                    .Id(Guid.Empty)
                    .MinimumChoices((int)minimumChoices3)
                    .Feedback(feedback3)
                    .Title(title3);
                l.Add(r);
            }
            json.Responses(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData(false, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Value\":0,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[]}")]
        [InlineData(true, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Value\":0,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"MinimumChoices\":2,\"Feedback\":\"Test\"}]}")]
        [InlineData(true, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Value\":5,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":1,\"Feedback\":\"Test1\"},{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"MinimumChoices\":2,\"Feedback\":\"Test2\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":3,\"Feedback\":\"Test3\"}]}")]
        public void ChoiceGetResponse(bool expectation, string responseId, string modelData)
        {
            Choice choice = Choice.FromJson(modelData);
            Assert.Equal(expectation, choice.GetResponse(new Guid(responseId)) != null);
        }

        [Theory]
        [InlineData("{}", "", "")]
        [InlineData("Illegal data", "Not Loaded", "Not Loaded")]
        [InlineData("{\"Text\":\"Test\"}", "", "Test")]
        public void QuestionFromJsonWithoutId(string json, string title, string text)
        {
            Question expected = new Question()
                .Id(Guid.Empty)
                .Title(title)
                .Text(text);
            Assert.Equal<Question>(expected, Question.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\",\"Text\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "Test", 7, "ChoiceText1", true, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true},{\"Value\":5,\"Text\":\"ChoiceText2\"},{\"Value\":3}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "TitleTest", "Test", 7, "ChoiceText1", true, 5, "ChoiceText2", false, 3, "", false)]
        public void QuestionFromJsonWithId(string json, string id, string title, string text, int? choiceValue1, string choiceText1, bool? choiceIsDefault1, int? choiceValue2, string choiceText2, bool? choiceIsDefault2, int? choiceValue3, string choiceText3, bool? choiceIsDefault3)
        {
            Question expected = new Question()
                .Id(new Guid(id))
                .Title(title)
                .Text(text);
            List<Choice> l = new List<Choice>() { };
            if (choiceValue1 != null && choiceText1 != null && choiceIsDefault1 != null)
            {
                Choice c = new Choice().Value((int)choiceValue1)
                    .Text(choiceText1)
                    .IsDefault((bool)choiceIsDefault1);
                l.Add(c);
            }
            if (choiceValue2 != null && choiceText2 != null && choiceIsDefault2 != null)
            {
                Choice c = new Choice().Value((int)choiceValue2)
                    .Text(choiceText2)
                    .IsDefault((bool)choiceIsDefault2);
                l.Add(c);
            }
            if (choiceValue3 != null && choiceText3 != null && choiceIsDefault3 != null)
            {
                Choice c = new Choice().Value((int)choiceValue3)
                    .Text(choiceText3)
                    .IsDefault((bool)choiceIsDefault3);
                l.Add(c);
            }
            expected.Choices(l);
            Assert.Equal<Question>(expected, Question.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Text\":\"\",\"Choices\":[],\"Answer\":null}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "", null, null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[],\"Answer\":null}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "TitleTest", "Test", null, null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]}],\"Answer\":null}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "Test", null, 7, "ChoiceText1", true, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]},{\"Value\":5,\"Text\":\"ChoiceText2\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":3,\"Text\":\"\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":13}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "TitleTest", "Test", 13, 7, "ChoiceText1", true, 5, "ChoiceText2", false, 3, "", false)]
        public void QuestionToJson(string expected, string id, string title, string text, int? answer, int? choiceValue1, string choiceText1, bool? choiceIsDefault1, int? choiceValue2, string choiceText2, bool? choiceIsDefault2, int? choiceValue3, string choiceText3, bool? choiceIsDefault3)
        {
            Question json = new Question()
                .Id(new Guid(id))
                .Title(title)
                .Answer(answer)
                .Text(text);
            List<Choice> l = new List<Choice>() { };
            if (choiceValue1 != null && choiceText1 != null && choiceIsDefault1 != null)
            {
                Choice c = new Choice().Value((int)choiceValue1)
                    .Text(choiceText1)
                    .IsDefault((bool)choiceIsDefault1);
                l.Add(c);
            }
            if (choiceValue2 != null && choiceText2 != null && choiceIsDefault2 != null)
            {
                Choice c = new Choice().Value((int)choiceValue2)
                    .Text(choiceText2)
                    .IsDefault((bool)choiceIsDefault2);
                l.Add(c);
            }
            if (choiceValue3 != null && choiceText3 != null && choiceIsDefault3 != null)
            {
                Choice c = new Choice().Value((int)choiceValue3)
                    .Text(choiceText3)
                    .IsDefault((bool)choiceIsDefault3);
                l.Add(c);
            }
            json.Choices(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData(false, 7, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[]}")]
        [InlineData(true, 7, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]}]}")]
        [InlineData(true, 5, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]},{\"Value\":5,\"Text\":\"ChoiceText2\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":3,\"Text\":\"\",\"IsDefault\":false,\"Responses\":[]}]}")]
        public void QuestionGetChoice(bool expectation, int value, string modelData)
        {
            Question question = Question.FromJson(modelData);
            Assert.Equal(expectation, question.GetChoice(value) != null);
        }

        [Theory]
        [InlineData(false, 7, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[]}")]
        [InlineData(false, 13, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]}]}")]
        [InlineData(true, 5, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]},{\"Value\":5,\"Text\":\"ChoiceText2\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":3,\"Text\":\"\",\"IsDefault\":false,\"Responses\":[]}]}")]
        public void QuestionChoiceValueExists(bool expectation, int value, string modelData)
        {
            Question question = Question.FromJson(modelData);
            Assert.Equal(expectation, question.ChoiceValueExists(value));
        }

        [Theory]
        [InlineData(false, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[]}")]
        [InlineData(true, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]}]}")]
        [InlineData(false, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":5,\"Text\":\"ChoiceText2\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":3,\"Text\":\"\",\"IsDefault\":false,\"Responses\":[]}]}")]
        [InlineData(true, "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"TitleTest\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true,\"Responses\":[]},{\"Value\":5,\"Text\":\"ChoiceText2\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":3,\"Text\":\"\",\"IsDefault\":false,\"Responses\":[]}]}")]
        public void QuestionChoiceDefaultSelected(bool expectation,string modelData)
        {
            Question question = Question.FromJson(modelData);
            Assert.Equal(expectation, question.ChoiceDefaultSelected());
        }

        [Theory]
        [InlineData("{}", "", "", "")]
        [InlineData("Illegal data", "Not Loaded", "", "")]
        [InlineData("{\"Introduction\":\"Test\"}", "", "Test", "")]
        public void QuestionnaireFromJsonWithoutId(string json, string title, string introduction, string conclusion)
        {
            Questionnaire expected = new Questionnaire()
                .Id(Guid.Empty)
                .Title(title)
                .Introduction(introduction)
                .Conclusion(conclusion);
            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Introduction\":\"Test\",\"Conclusion\":\"\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "Test", "", null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", "", "", null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[{\"Id\":\"28301F4A-1B91-4C81-A4E8-56F370B3D30A\",\"Text\":\"Test1\"},{\"Id\":\"DA14F817-9080-4E85-8715-14C6E734F02B\",\"Text\":\"Test2\"}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "", "", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\",\"Introduction\":\"Test\",\"Conclusion\":\"Test\",\"Questions\":[{\"Id\":\"28301F4A-1B91-4C81-A4E8-56F370B3D30A\",\"Text\":\"Test1\"},{\"Id\":\"DA14F817-9080-4E85-8715-14C6E734F02B\",\"Text\":\"Test2\"},{\"Id\":\"EDC7BA05-14A1-4397-AE73-77EE2DB3FD76\"}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", "Test", "Test", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", "EDC7BA05-14A1-4397-AE73-77EE2DB3FD76", "")]
        public void QuestionnaireFromJsonWithId(string json, string id, string title, string introduction, string conclusion, string questionId1, string questionText1, string questionId2, string questionText2, string questionId3, string questionText3)
        {
            Questionnaire expected = new Questionnaire()
                .Id(new Guid(id))
                .Title(title)
                .Introduction(introduction)
                .Conclusion(conclusion);
            List<Question> l = new List<Question>() { };
            if (questionId1 != null && questionText1 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId1))
                    .Text(questionText1);
                l.Add(q);
            }
            if (questionId2 != null && questionText2 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId2))
                    .Text(questionText2);
                l.Add(q);
            }
            if (questionId3 != null && questionText3 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId3))
                    .Text(questionText3);
                l.Add(q);
            }
            expected.Questions(l);
            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json));
        }

        [Fact]
        public void QuestionnaireFromJsonWithComplicatedQuestions()
        {
            string json = "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\", \"Title\":\"Test\", \"Introduction\":\"Test\", \"Conclusion\":\"Test\", \"Questions\":[" +
                          "   {\"Id\":\"177f9de3-3db4-4acd-9de7-66932a08a978\", \"Title\": \"Q1Test\", \"Text\":\"Q1Test\", \"Choices\":[" +
                          "       {\"Value\":3, \"Text\":\"C1Test\", \"IsDefault\":\"false\", \"Responses\":[" +
                          "           {\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":2, \"Feedback\":\"FeedbackTest1\"},{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\", \"MinimumChoices\":3}" +
                          "       ]}," +
                          "       {\"Value\":5, \"Text\":\"C2Test\", \"IsDefault\":\"true\", \"Responses\":[" +
                          "           {\"Id\":\"ef29ed60-7dfa-4ca6-8f03-ea382bd7bff2\", \"Feedback\":\"FeedbackTest2\"},{\"Id\":\"befce82d-422c-4960-bb8b-1fd5a504ee5a\"}" +
                          "       ]}" +
                          "   ]}," +  
                          "   {\"Id\":\"87a87a86-d91b-40e4-9c50-6b375e56a090\", \"Choices\":[" +
                          "       {\"Value\":7, \"Text\":\"C3Test\", \"Responses\":[" +
                          "           {\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":9, \"Feedback\":\"FeedbackTest3\"}" +
                          "       ]}" +
                          "   ]}," +
                          "   {\"Id\":\"9c7e9e9d-4ebd-4827-9d0c-608ba5afa2a7\", \"Text\":\"Q3Test\"}" +
                          "]}";
            Response r1 = new Response()
                .Id(new Guid("73129183-ce7b-48ef-820f-b96af9ab82c2"))
                .MinimumChoices(2)
                .Feedback("FeedbackTest1");
            Response r2 = new Response()
                .Id(new Guid("28301f4a-1b91-4c81-a4e8-56f370b3d30a"))
                .MinimumChoices(3);
            Response r3 = new Response()
                .Id(new Guid("ef29ed60-7dfa-4ca6-8f03-ea382bd7bff2"))
                .Feedback("FeedbackTest2");
            Response r4 = new Response()
                .Id(new Guid("befce82d-422c-4960-bb8b-1fd5a504ee5a"));
            Response r5 = new Response()
                .Id(new Guid("73129183-ce7b-48ef-820f-b96af9ab82c2"))
                .MinimumChoices(9)
                .Feedback("FeedbackTest3");
            Choice c1 = new Choice().Value(3)
                .Text("C1Test")
                .Responses(new List<Response>() { r1, r2 });
            Choice c2 = new Choice().Value(5)
                .Text("C2Test")
                .IsDefault(true)
                .Responses(new List<Response>() { r3, r4 });
            Choice c3 = new Choice().Value(7)
                .Text("C3Test")
                .Responses(new List<Response>() { r5 });
            Question q1 = new Question()
                .Id(new Guid("177f9de3-3db4-4acd-9de7-66932a08a978"))
                .Title("Q1Test")
                .Text("Q1Test")
                .Choices(new List<Choice>() { c1, c2 });
            Question q2 = new Question()
                .Id(new Guid("87a87a86-d91b-40e4-9c50-6b375e56a090"))
                .Choices(new List<Choice>() { c3 });
            Question q3 = new Question()
                .Id(new Guid("9c7e9e9d-4ebd-4827-9d0c-608ba5afa2a7"))
                .Text("Q3Test");
            Questionnaire expected = new Questionnaire()
                .Id(new Guid("73129183-ce7b-48ef-820f-b96af9ab82c2"))
                .Title("Test")
                .Introduction("Test")
                .Conclusion("Test")
                .Questions(new List<Question>() { q1, q2, q3 });

            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\",\"Introduction\":\"Test\",\"Conclusion\":\"\",\"Questions\":[]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", "Test", "", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Introduction\":\"\",\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[],\"Answer\":null},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[],\"Answer\":null}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "", "", "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "", "Test1", "da14f817-9080-4e85-8715-14c6e734f02b", "", "Test2", null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test3\",\"Introduction\":\"\",\"Conclusion\":\"Test4\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[],\"Answer\":null},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[],\"Answer\":null},{\"Id\":\"edc7ba05-14a1-4397-ae73-77ee2db3fd76\",\"Title\":\"Test3\",\"Text\":\"\",\"Choices\":[],\"Answer\":null}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test3", "", "Test4", "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "", "Test1", "da14f817-9080-4e85-8715-14c6e734f02b", "", "Test2", "edc7ba05-14a1-4397-ae73-77ee2db3fd76", "Test3", "")]
        public void QuestionnaireToJson(string expected, string id, string title, string introduction, string conclusion, string questionId1, string questionTitle1, string questionText1, string questionId2, string questionTitle2, string questionText2, string questionId3, string questionTitle3, string questionText3)
        {
            Questionnaire json = new Questionnaire()
                .Id(new Guid(id))
                .Title(title)
                .Introduction(introduction)
                .Conclusion(conclusion);
            List<Question> l = new List<Question>() { };
            if (questionId1 != null && questionText1 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId1))
                    .Title(questionTitle1)
                    .Text(questionText1);
                l.Add(q);
            }
            if (questionId2 != null && questionText2 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId2))
                    .Title(questionTitle2)
                    .Text(questionText2);
                l.Add(q);
            }
            if (questionId3 != null && questionText3 != null)
            {
                Question q = new Question()
                    .Id(new Guid(questionId3))
                    .Title(questionTitle3)
                    .Text(questionText3);
                l.Add(q);
            }
            json.Questions(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData(false, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\",\"Introduction\":\"Test\",\"Questions\":[]}")]
        [InlineData(true, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Introduction\":\"\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[]}]}")]
        [InlineData(true, "edc7ba05-14a1-4397-ae73-77ee2db3fd76", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test3\",\"Introduction\":\"\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[]},{\"Id\":\"edc7ba05-14a1-4397-ae73-77ee2db3fd76\",\"Title\":\"Test3\",\"Text\":\"\",\"Choices\":[]}]}")]
        public void QuestionnaireGetQuestion(bool expectation, string questionId, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            Assert.Equal(expectation, questionnaire.GetQuestion(new Guid(questionId)) != null);
        }

        [Theory]
        [InlineData(false, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"Test\",\"Introduction\":\"Test\",\"Questions\":[]}")]
        [InlineData(true, "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Introduction\":\"\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[{\"Value\":5,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"7a7cf86d-79b9-4c90-bde1-4c2349141033\",\"MinimumChoices\":1,\"Feedback\":\"Test1\"},{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"MinimumChoices\":2,\"Feedback\":\"Test2\"},{\"Id\":\"dd5d1d53-8b5d-4003-9c05-fdf17c83d422\",\"MinimumChoices\":3,\"Feedback\":\"Test3\"}]}]}]}")]
        [InlineData(false, "da039793-0af9-4103-85bf-49ef2223d9d2", "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Title\":\"\",\"Introduction\":\"\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Title\":\"\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Title\":\"\",\"Text\":\"Test2\",\"Choices\":[{\"Value\":5,\"Text\":\"Test\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"7a7cf86d-79b9-4c90-bde1-4c2349141033\",\"MinimumChoices\":1,\"Feedback\":\"Test1\"},{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"MinimumChoices\":2,\"Feedback\":\"Test2\"},{\"Id\":\"dd5d1d53-8b5d-4003-9c05-fdf17c83d422\",\"MinimumChoices\":3,\"Feedback\":\"Test3\"}]}]}]}")]
        public void QuestionnaireGetResponse(bool expectation, string responseId, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            Assert.Equal(expectation, questionnaire.GetResponse(new Guid(responseId)) != null);
        }

        [Theory]
        [InlineData(0, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        [InlineData(1, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        [InlineData(2, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        public void QuestionnaireGetChoicesWithResponse(int expectedAmount, string responseId, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            Response response = questionnaire.GetResponse(new Guid(responseId));
            Assert.Equal(expectedAmount, questionnaire.GetChoicesWithResponse(response).Count);
        }

        [Theory]
        [InlineData(false, "18727084-b180-464d-b036-84536dc864a5", 0, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        [InlineData(true, "18727084-b180-464d-b036-84536dc864a5", 1, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        [InlineData(true, "c6a6783b-58e2-479d-8154-600e2a141bc7", 0, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        [InlineData(false, "c6a6783b-58e2-479d-8154-600e2a141bc7", 1, "a9df1807-a893-4dfb-9b06-d085f502e651", "{\"Id\":\"54831f76-c6e5-482d-9ce2-28df85ecb1dc\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"18727084-b180-464d-b036-84536dc864a5\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"040600ea-ddb8-4674-8d45-ac0108911886\",\"MinimumChoices\":2,\"Feedback\":\"Test2\",\"Title\":\"Test2\"}]}],\"Answer\":null},{\"Id\":\"c6a6783b-58e2-479d-8154-600e2a141bc7\",\"Title\":\"Test1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":null,\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[{\"Id\":\"a9df1807-a893-4dfb-9b06-d085f502e651\",\"MinimumChoices\":1,\"Feedback\":\"Test1\",\"Title\":\"Test1\"}]}],\"Answer\":null}]}")]
        public void QuestionnaireGetAvailableResponses(bool expectation, string questionId, int value, string responseId, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            Choice choice = questionnaire.GetQuestion(new Guid(questionId)).GetChoice(value);
            Assert.Equal(expectation, questionnaire.GetAvailableResponses(choice).Contains(questionnaire.GetResponse(new Guid(responseId))));
        }

        [Theory]
        [InlineData(null, null, null, "{\"Id\":\"d65e2ff2-bcdc-4967-b7d8-584f40e60108\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"4f5564f4-faef-44e9-b8e9-027747ce97c2\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":3},{\"Id\":\"a04a6fae-3fec-49b0-a27b-9260e313b90d\",\"Title\":\"Q2\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"3\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"4\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":3},{\"Id\":\"43bd9cf1-b44f-45a8-bfc2-00803902ec42\",\"Title\":\"Q3\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"5\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[]}],\"Answer\":3}]}")]
        [InlineData("1", "4", null, "{\"Id\":\"d65e2ff2-bcdc-4967-b7d8-584f40e60108\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"4f5564f4-faef-44e9-b8e9-027747ce97c2\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":0},{\"Id\":\"a04a6fae-3fec-49b0-a27b-9260e313b90d\",\"Title\":\"Q2\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"3\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"4\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":1},{\"Id\":\"43bd9cf1-b44f-45a8-bfc2-00803902ec42\",\"Title\":\"Q3\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"5\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":null,\"IsDefault\":false,\"Responses\":[]}],\"Answer\":null}]}")]
        public void QuestionnaireGetAnswers(string choiceText1, string choiceText2, string choiceText3, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            Choice[] answers = questionnaire.GetAnswers().ToArray();
            if (choiceText1 != null)
                Assert.Equal(choiceText1, answers[0].Text);
            if (choiceText2 != null)
                Assert.Equal(choiceText2, answers[1].Text);
            if (choiceText3 != null)
                Assert.Equal(choiceText3, answers[2].Text);
            if (choiceText1 == null && choiceText2 == null && choiceText3 == null)
                Assert.Empty(answers);
        }

        [Theory]
        [InlineData(0, "{\"Id\":\"775e0117-73f0-4837-8415-68a7404dffda\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"b1454fd2-3fb1-4d20-8f09-dfce47bdfc77\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[]}],\"Answer\":null}]}")]
        [InlineData(0, "{\"Id\":\"775e0117-73f0-4837-8415-68a7404dffda\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"b1454fd2-3fb1-4d20-8f09-dfce47bdfc77\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":1,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":0}]}")]
        [InlineData(0, "{\"Id\":\"775e0117-73f0-4837-8415-68a7404dffda\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"b1454fd2-3fb1-4d20-8f09-dfce47bdfc77\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":2,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":1},{\"Id\":\"05525f30-f4ed-4fb7-a524-45d699e6b7ff\",\"Title\":\"Q2\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"3\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"4\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":2,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":0}]}")]
        [InlineData(1, "{\"Id\":\"775e0117-73f0-4837-8415-68a7404dffda\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"b1454fd2-3fb1-4d20-8f09-dfce47bdfc77\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":2,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":1},{\"Id\":\"05525f30-f4ed-4fb7-a524-45d699e6b7ff\",\"Title\":\"Q2\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"3\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"4\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":2,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":1}]}")]
        [InlineData(2, "{\"Id\":\"775e0117-73f0-4837-8415-68a7404dffda\",\"Title\":\"Test\",\"Introduction\":null,\"Conclusion\":\"\",\"Questions\":[{\"Id\":\"b1454fd2-3fb1-4d20-8f09-dfce47bdfc77\",\"Title\":\"Q1\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"1\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"2\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":1,\"Feedback\":\"R1\",\"Title\":\"R1\"}]}],\"Answer\":1},{\"Id\":\"05525f30-f4ed-4fb7-a524-45d699e6b7ff\",\"Title\":\"Q2\",\"Text\":null,\"Choices\":[{\"Value\":0,\"Text\":\"3\",\"IsDefault\":false,\"Responses\":[]},{\"Value\":1,\"Text\":\"4\",\"IsDefault\":false,\"Responses\":[{\"Id\":\"1b927d18-8763-4cff-b0ed-a71a17d8a496\",\"MinimumChoices\":1,\"Feedback\":\"R2\",\"Title\":\"R2\"}]}],\"Answer\":1}]}")]
        public void QuestionnaireGetActiveResponses(int expectedNumberOfActiveResponses, string modelData)
        {
            Questionnaire questionnaire = Questionnaire.FromJson(modelData);
            int numberOfActiveResponses = questionnaire.GetActiveResponses(questionnaire.GetAnswers()).Count;
            Assert.Equal(expectedNumberOfActiveResponses, numberOfActiveResponses);
        }

        [Fact]
        public void AddUniqueCannotAddSameElementTwice()
        {
            string element1 = "Test";
            string element2 = "AnotherString";
            List<string> list = new List<string>();
            Assert.True(list.AddUnique(element1));
            Assert.True(list.AddUnique(element2));
            Assert.False(list.AddUnique(element2));
            Assert.False(list.AddUnique(element1));
        }

        [Fact]
        public void AddUniqueChoiceCannotAddSameElementTwice()
        {
            Choice element1 = new Choice().Value(5).Text("Test1");
            Choice element2 = new Choice().Value(5).Text("Test2");
            Choice element3 = new Choice().Value(3).Text("Test1");
            List<Choice> list = new List<Choice>();
            Assert.True(list.AddUniqueChoice(element1));
            Assert.False(list.AddUniqueChoice(element2));
            Assert.True(list.AddUniqueChoice(element3));
            Assert.False(list.AddUniqueChoice(element3));
        }
    }
}
