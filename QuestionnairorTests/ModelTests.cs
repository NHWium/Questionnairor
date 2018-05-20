using QuestionnaireData.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace QuestionnairorTests
{
    public class ModelTests
    {
        [Theory]
        [InlineData("{}", 1, "")]
        [InlineData("Illegal data", 1, "Not Loaded")]
        [InlineData("{\"MinimumChoices\":2,\"Feedback\":\"Test\"}", 2, "Test")]
        [InlineData("{\"MinimumChoices\":11}", 11, "")]
        [InlineData("{\"Feedback\":\"Test\"}", 1, "Test")]
        public void ResponseFromJsonWithoutId(string json, int minimumChoices, string feedback)
        {
            Response expected = new Response()
                .Id(Guid.Empty)
                .MinimumChoices(minimumChoices)
                .Feedback(feedback);
            //Do not test Guid, as it is auto-generated
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
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"MinimumChoices\":11,\"Feedback\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", 11, "Test")]
        public void ResponseToJson(string expected, string id, int minimumChoices, string feedback)
        {
            Response json = new Response()
                .Id(new Guid(id))
                .MinimumChoices(minimumChoices)
                .Feedback(feedback);
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
            Choice expected = new Choice(value)
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
            Choice expected = new Choice(0)
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
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"Responses\":[],\"IsDefault\":false}", 0, "Test", false, null, null, null, null, null, null)]
        [InlineData("{\"Value\":7,\"Text\":\"\",\"Responses\":[],\"IsDefault\":true}", 7, "", true, null, null, null, null, null, null)]
        [InlineData("{\"Value\":0,\"Text\":\"Test\",\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test\"}],\"IsDefault\":false}", 0, "Test", false, 2, "Test", null, null, null, null)]
        [InlineData("{\"Value\":5,\"Text\":\"Test\",\"Responses\":[{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":1,\"Feedback\":\"Test1\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":2,\"Feedback\":\"Test2\"},{\"Id\":\"00000000-0000-0000-0000-000000000000\",\"MinimumChoices\":3,\"Feedback\":\"Test3\"}],\"IsDefault\":false}", 5, "Test", false, 1, "Test1", 2, "Test2", 3, "Test3")]
        public void ChoiceToJson(string expected, int value, string text, bool isDefault, int? minimumChoices1, string feedback1, int? minimumChoices2, string feedback2, int? minimumChoices3, string feedback3)
        {
            Choice json = new Choice(value)
                .Text(text)
                .IsDefault(isDefault);
            List<Response> l = new List<Response>() { };
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
            json.Responses(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData("{}", "")]
        [InlineData("Illegal data", "Not Loaded")]
        [InlineData("{\"Text\":\"Test\"}", "Test")]
        public void QuestionFromJsonWithoutId(string json, string text)
        {
            Question expected = new Question()
                .Id(Guid.Empty)
                .Text(text);
            Assert.Equal<Question>(expected, Question.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", 7, "ChoiceText1", true, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"IsDefault\":true},{\"Value\":5,\"Text\":\"ChoiceText2\"},{\"Value\":3}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", 7, "ChoiceText1", true, 5, "ChoiceText2", false, 3, "", false)]
        public void QuestionFromJsonWithId(string json, string id, string text, int? choiceValue1, string choiceText1, bool? choiceIsDefault1, int? choiceValue2, string choiceText2, bool? choiceIsDefault2, int? choiceValue3, string choiceText3, bool? choiceIsDefault3)
        {
            Question expected = new Question()
                .Id(new Guid(id))
                .Text(text);
            List<Choice> l = new List<Choice>() { };
            if (choiceValue1 != null && choiceText1 != null && choiceIsDefault1 != null)
            {
                Choice c = new Choice((int)choiceValue1)
                    .Text(choiceText1)
                    .IsDefault((bool)choiceIsDefault1);
                l.Add(c);
            }
            if (choiceValue2 != null && choiceText2 != null && choiceIsDefault2 != null)
            {
                Choice c = new Choice((int)choiceValue2)
                    .Text(choiceText2)
                    .IsDefault((bool)choiceIsDefault2);
                l.Add(c);
            }
            if (choiceValue3 != null && choiceText3 != null && choiceIsDefault3 != null)
            {
                Choice c = new Choice((int)choiceValue3)
                    .Text(choiceText3)
                    .IsDefault((bool)choiceIsDefault3);
                l.Add(c);
            }
            expected.Choices(l);
            Assert.Equal<Question>(expected, Question.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"\",\"Choices\":[]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"Responses\":[],\"IsDefault\":true}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", 7, "ChoiceText1", true, null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Text\":\"Test\",\"Choices\":[{\"Value\":7,\"Text\":\"ChoiceText1\",\"Responses\":[],\"IsDefault\":true},{\"Value\":5,\"Text\":\"ChoiceText2\",\"Responses\":[],\"IsDefault\":false},{\"Value\":3,\"Text\":\"\",\"Responses\":[],\"IsDefault\":false}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", 7, "ChoiceText1", true, 5, "ChoiceText2", false, 3, "", false)]
        public void QuestionToJson(string expected, string id, string text, int? choiceValue1, string choiceText1, bool? choiceIsDefault1, int? choiceValue2, string choiceText2, bool? choiceIsDefault2, int? choiceValue3, string choiceText3, bool? choiceIsDefault3)
        {
            Question json = new Question()
                .Id(new Guid(id))
                .Text(text);
            List<Choice> l = new List<Choice>() { };
            if (choiceValue1 != null && choiceText1 != null && choiceIsDefault1 != null)
            {
                Choice c = new Choice((int)choiceValue1)
                    .Text(choiceText1)
                    .IsDefault((bool)choiceIsDefault1);
                l.Add(c);
            }
            if (choiceValue2 != null && choiceText2 != null && choiceIsDefault2 != null)
            {
                Choice c = new Choice((int)choiceValue2)
                    .Text(choiceText2)
                    .IsDefault((bool)choiceIsDefault2);
                l.Add(c);
            }
            if (choiceValue3 != null && choiceText3 != null && choiceIsDefault3 != null)
            {
                Choice c = new Choice((int)choiceValue3)
                    .Text(choiceText3)
                    .IsDefault((bool)choiceIsDefault3);
                l.Add(c);
            }
            json.Choices(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }

        [Theory]
        [InlineData("{}", "")]
        [InlineData("Illegal data", "Not Loaded")]
        [InlineData("{\"Introduction\":\"Test\"}", "Test")]
        public void QuestionnaireFromJsonWithoutId(string json, string introduction)
        {
            Questionnaire expected = new Questionnaire()
                .Id(Guid.Empty)
                .Introduction(introduction);
            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Introduction\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[{\"Id\":\"28301F4A-1B91-4C81-A4E8-56F370B3D30A\",\"Text\":\"Test1\"},{\"Id\":\"DA14F817-9080-4E85-8715-14C6E734F02B\",\"Text\":\"Test2\"}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[{\"Id\":\"28301F4A-1B91-4C81-A4E8-56F370B3D30A\",\"Text\":\"Test1\"},{\"Id\":\"DA14F817-9080-4E85-8715-14C6E734F02B\",\"Text\":\"Test2\"},{\"Id\":\"EDC7BA05-14A1-4397-AE73-77EE2DB3FD76\"}]}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", "EDC7BA05-14A1-4397-AE73-77EE2DB3FD76", "")]
        public void QuestionnaireFromJsonWithId(string json, string id, string introduction, string questionId1, string questionText1, string questionId2, string questionText2, string questionId3, string questionText3)
        {
            Questionnaire expected = new Questionnaire()
                .Id(new Guid(id))
                .Introduction(introduction);
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
            string json = "{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\", \"Introduction\":\"Test\", \"Questions\":[" +
                          "   {\"Id\":\"177f9de3-3db4-4acd-9de7-66932a08a978\", \"Text\":\"Q1Test\", \"Choices\":[" +
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
            Choice c1 = new Choice(3)
                .Text("C1Test")
                .Responses(new List<Response>() { r1, r2 });
            Choice c2 = new Choice(5)
                .Text("C2Test")
                .IsDefault(true)
                .Responses(new List<Response>() { r3, r4 });
            Choice c3 = new Choice(7)
                .Text("C3Test")
                .Responses(new List<Response>() { r5 });
            Question q1 = new Question()
                .Id(new Guid("177f9de3-3db4-4acd-9de7-66932a08a978"))
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
                .Introduction("Test")
                .Questions(new List<Question>() { q1, q2, q3 });

            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json));
        }

        [Theory]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[],\"Introduction\":\"Test\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "Test", null, null, null, null, null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Text\":\"Test2\",\"Choices\":[]}],\"Introduction\":\"\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "Test1", "da14f817-9080-4e85-8715-14c6e734f02b", "Test2", null, null)]
        [InlineData("{\"Id\":\"73129183-ce7b-48ef-820f-b96af9ab82c2\",\"Questions\":[{\"Id\":\"28301f4a-1b91-4c81-a4e8-56f370b3d30a\",\"Text\":\"Test1\",\"Choices\":[]},{\"Id\":\"da14f817-9080-4e85-8715-14c6e734f02b\",\"Text\":\"Test2\",\"Choices\":[]},{\"Id\":\"edc7ba05-14a1-4397-ae73-77ee2db3fd76\",\"Text\":\"\",\"Choices\":[]}],\"Introduction\":\"\"}", "73129183-ce7b-48ef-820f-b96af9ab82c2", "", "28301f4a-1b91-4c81-a4e8-56f370b3d30a", "Test1", "da14f817-9080-4e85-8715-14c6e734f02b", "Test2", "edc7ba05-14a1-4397-ae73-77ee2db3fd76", "")]
        public void QuestionnaireToJson(string expected, string id, string introduction, string questionId1, string questionText1, string questionId2, string questionText2, string questionId3, string questionText3)
        {
            Questionnaire json = new Questionnaire()
                .Id(new Guid(id))
                .Introduction(introduction);
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
            json.Questions(l);
            Assert.Equal(expected, json.ToJson(Newtonsoft.Json.Formatting.None));
        }
    }
}
