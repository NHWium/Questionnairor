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
        [InlineData("{'MinimumChoices': 2, 'Feedback': 'Test'}", 2, "Test")]
        [InlineData("{'MinimumChoices': 11}", 11, "")]
        [InlineData("{'Feedback': 'Test'}", 1, "Test")]
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
        [InlineData("{'Id':'73129183-CE7B-48EF-820F-B96AF9AB82C2','MinimumChoices': 11,'Feedback': 'Test'}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", 11, "Test")]
        [InlineData("{'Id':'73129183-CE7B-48EF-820F-B96AF9AB82C2','MinimumChoices': 11}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", 11, "")]
        [InlineData("{'Id':'73129183-CE7B-48EF-820F-B96AF9AB82C2','Feedback': 'Test'}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", 1, "Test")]
        public void ResponseFromJsonWitId(string json, string id, int minimumChoices, string feedback)
        {
            Response expected = new Response()
                .Id(new Guid(id))
                .MinimumChoices(minimumChoices)
                .Feedback(feedback);
            Assert.Equal<Response>(expected, Response.FromJson(json));
        }


        [Theory]
        [InlineData("{}", 0, "", false, null, null, null, null, null, null)]
        [InlineData("Illegal data", 0, "Not Loaded", true, null, null, null, null, null, null)]
        [InlineData("{'Value': 0, 'Text': 'Test'}", 0, "Test", false, null, null, null, null, null, null)]
        [InlineData("{'Value': 7, 'IsDefault': 'true'}", 7, "", true, null, null, null, null, null, null)]
        [InlineData("{'Value': 0, 'Text': 'Test', Responses: []}", 0, "Test", false, null, null, null, null, null, null)]
        [InlineData("{'Value': 0, 'Text': 'Test', Responses: [{'Id':'00000000-0000-0000-0000-000000000000','MinimumChoices': 2, 'Feedback': 'Test'}]}", 0, "Test", false, 2, "Test", null, null, null, null)]
        [InlineData("{'Value': 5, 'Text': 'Test', Responses: [{'Id':'00000000-0000-0000-0000-000000000000','MinimumChoices': 1, 'Feedback': 'Test1'},{'Id':'00000000-0000-0000-0000-000000000000','MinimumChoices': 2, 'Feedback': 'Test2'},{'Id':'00000000-0000-0000-0000-000000000000','MinimumChoices': 3, 'Feedback': 'Test3'}]}", 5, "Test", false, 1, "Test1", 2, "Test2", 3, "Test3")]
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
            string json = "{'Value': 0, 'Text': 'Test', 'Responses': [{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2','MinimumChoices': 2, 'Feedback': 'FeedbackTest'}, {'Id': '28301F4A-1B91-4C81-A4E8-56F370B3D30A', 'MinimumChoices': 3}]}";
            Choice expected = new Choice(0)
                .Text("Test")
                .IsDefault(false);
            Response r1 = new Response()
                .Id(new Guid("73129183-CE7B-48EF-820F-B96AF9AB82C2"))
                .MinimumChoices(2)
                .Feedback("FeedbackTest");
            Response r2 = new Response()
                .Id(new Guid("28301F4A-1B91-4C81-A4E8-56F370B3D30A"))
                .MinimumChoices(3);
            List<Response> l = new List<Response>() { r1, r2 };
            expected.Responses(l);
            Assert.Equal<Choice>(expected, Choice.FromJson(json));
        }

        [Theory]
        [InlineData("{}", "")]
        [InlineData("Illegal data", "Not Loaded")]
        [InlineData("{'Text': 'Test'}", "Test")]
        public void QuestionFromJsonWithoutId(string json, string text)
        {
            Question expected = new Question()
                .Id(Guid.Empty)
                .Text(text);
            Assert.Equal<Question>(expected, Question.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2'}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "", null, null, null, null, null, null, null, null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Text': 'Test'}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Text': 'Test', 'Choices': []}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "Test", null, null, null, null, null, null, null, null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Text': 'Test', 'Choices': [{'Value': 7, 'Text': 'ChoiceText1', 'IsDefault': 'true'}]}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "Test", 7, "ChoiceText1", true, null, null, null, null, null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Text': 'Test', 'Choices': [{'Value': 7, 'Text': 'ChoiceText1', 'IsDefault': 'true'}, {'Value': 5, 'Text': 'ChoiceText2'}, {'Value': 3}]}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "Test", 7, "ChoiceText1", true, 5, "ChoiceText2", false, 3, "", false)]
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
        [InlineData("{}", "")]
        [InlineData("Illegal data", "Not Loaded")]
        [InlineData("{'Introduction': 'Test'}", "Test")]
        public void QuestionnaireFromJsonWithoutId(string json, string introduction)
        {
            Questionnaire expected = new Questionnaire()
                .Id(Guid.Empty)
                .Introduction(introduction);
            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json).Id(Guid.Empty));
        }

        [Theory]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Introduction': 'Test'}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "Test", null, null, null, null, null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Questions': [{'Id': '28301F4A-1B91-4C81-A4E8-56F370B3D30A', 'Text': 'Test1'}, {'Id': 'DA14F817-9080-4E85-8715-14C6E734F02B', 'Text': 'Test2'}]}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", null, null)]
        [InlineData("{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Questions': [{'Id': '28301F4A-1B91-4C81-A4E8-56F370B3D30A', 'Text': 'Test1'}, {'Id': 'DA14F817-9080-4E85-8715-14C6E734F02B', 'Text': 'Test2'}, {'Id': 'EDC7BA05-14A1-4397-AE73-77EE2DB3FD76'}]}", "73129183-CE7B-48EF-820F-B96AF9AB82C2", "", "28301F4A-1B91-4C81-A4E8-56F370B3D30A", "Test1", "DA14F817-9080-4E85-8715-14C6E734F02B", "Test2", "EDC7BA05-14A1-4397-AE73-77EE2DB3FD76", "")]
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
            string json = "{'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2', 'Introduction': 'Test', 'Questions': [" +
                            "   {'Id': '177F9DE3-3DB4-4ACD-9DE7-66932A08A978', 'Text': 'Q1Test', 'Choices': [" +
                            "       {'Value': 3, 'Text': 'C1Test', 'IsDefault': 'false', 'Responses': [" +
                            "           {'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2','MinimumChoices': 2, 'Feedback': 'FeedbackTest1'}, {'Id': '28301F4A-1B91-4C81-A4E8-56F370B3D30A', 'MinimumChoices': 3}" +
                            "       ]}," +
                            "       {'Value': 5, 'Text': 'C2Test', 'IsDefault': 'true', 'Responses': [" +
                            "           {'Id': 'EF29ED60-7DFA-4CA6-8F03-EA382BD7BFF2', 'Feedback': 'FeedbackTest2'}, {'Id': 'BEFCE82D-422C-4960-BB8B-1FD5A504EE5A'}" +
                            "       ]}" +
                            "   ]}," +  
                            "   {'Id': '87A87A86-D91B-40E4-9C50-6B375E56A090', 'Choices': [" +
                            "       {'Value': 7, 'Text': 'C3Test', 'Responses': [" +
                            "           {'Id': '73129183-CE7B-48EF-820F-B96AF9AB82C2','MinimumChoices': 9, 'Feedback': 'FeedbackTest3'}" +
                            "       ]}" +
                            "   ]}," +
                            "   {'Id': '9C7E9E9D-4EBD-4827-9D0C-608BA5AFA2A7', 'Text': 'Q3Test'}" +
                            "]}";
            Response r1 = new Response()
                .Id(new Guid("73129183-CE7B-48EF-820F-B96AF9AB82C2"))
                .MinimumChoices(2)
                .Feedback("FeedbackTest1");
            Response r2 = new Response()
                .Id(new Guid("28301F4A-1B91-4C81-A4E8-56F370B3D30A"))
                .MinimumChoices(3);
            Response r3 = new Response()
                .Id(new Guid("EF29ED60-7DFA-4CA6-8F03-EA382BD7BFF2"))
                .Feedback("FeedbackTest2");
            Response r4 = new Response()
                .Id(new Guid("BEFCE82D-422C-4960-BB8B-1FD5A504EE5A"));
            Response r5 = new Response()
                .Id(new Guid("73129183-CE7B-48EF-820F-B96AF9AB82C2"))
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
                .Id(new Guid("177F9DE3-3DB4-4ACD-9DE7-66932A08A978"))
                .Text("Q1Test")
                .Choices(new List<Choice>() { c1, c2 });
            Question q2 = new Question()
                .Id(new Guid("87A87A86-D91B-40E4-9C50-6B375E56A090"))
                .Choices(new List<Choice>() { c3 });
            Question q3 = new Question()
                .Id(new Guid("9C7E9E9D-4EBD-4827-9D0C-608BA5AFA2A7"))
                .Text("Q3Test");
            Questionnaire expected = new Questionnaire()
                .Id(new Guid("73129183-CE7B-48EF-820F-B96AF9AB82C2"))
                .Introduction("Test")
                .Questions(new List<Question>() { q1, q2, q3 });

            Assert.Equal<Questionnaire>(expected, Questionnaire.FromJson(json));
        }
    }
}
