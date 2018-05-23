using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Questionnairor.Models;

namespace Questionnairor.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        public Dictionary<Guid, Questionnaire> QuestionnaireData { get; set; }
        public Dictionary<Guid, Response> ResponseData { get; set; }

        public QuestionnaireService()
        {
            QuestionnaireData = new Dictionary<Guid,Questionnaire>();
            ResponseData = new Dictionary<Guid, Response>();
        }

        public bool ValidId(Guid id)
        {
            if (id != null && id != Guid.Empty && QuestionnaireData.ContainsKey(id))
                return true;
            else
                return false;
        }

        public bool UnusedId(Guid id)
        {
            if (id != null && id != Guid.Empty && !QuestionnaireData.ContainsKey(id))
                return true;
            else
                return false;
        }

        public bool ValidResponse(Guid id)
        {
            if (id != null && id != Guid.Empty && ResponseData.ContainsKey(id))
                return true;
            else
                return false;
        }

        public bool UnusedResponse(Guid id)
        {
            if (id != null && id != Guid.Empty && !ResponseData.ContainsKey(id))
                return true;
            else
                return false;
        }

        public Questionnaire GetQuestionnaire(Guid id)
        {
            if (!ValidId(id)) return null;
            if (!QuestionnaireData.ContainsKey(id)) return null;
            return QuestionnaireData[id];
        }

        public Question GetQuestion(Guid id, Guid questionId)
        {
            if (!ValidId(id)) return null;
            Question question = QuestionnaireData[id].Questions.Where<Question>(q => q.Id == questionId).FirstOrDefault();
            if (question != null && QuestionnaireData[id].Questions != null && questionId != null && questionId != Guid.Empty)
                return question;
            else
                return null;
        }

        public Choice GetChoice(Guid id, Guid questionId, int value)
        {
            Question question = GetQuestion(id, questionId);
            if (question == null) return null;
            Choice choice = question.Choices.Where<Choice>(c => c.Value == value).FirstOrDefault();
            if (choice != null && question.Choices != null && choice.Value == value)
                return choice;
            else
                return null;
        }

        public Response GetResponse(Guid id, Guid questionId, int value, Guid responseId)
        {
            Question question = GetQuestion(id, questionId);
            if (question == null) return null;
            Choice choice = GetChoice(id, questionId, value);
            if (choice == null) return null;
            Response response = choice.Responses.Where<Response>(r => r.Id == responseId).FirstOrDefault();
            if (response != null && choice.Responses != null && response.Id == responseId)
                return response;
            else
                return null;
        }
    }
}
