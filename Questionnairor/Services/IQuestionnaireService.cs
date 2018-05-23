using QuestionnaireData.Models;
using System;
using System.Collections.Generic;

namespace Questionnairor.Services
{
    public interface IQuestionnaireService
    {
        Dictionary<Guid, Questionnaire> QuestionnaireData { get; set; }
        Dictionary<Guid, Response> ResponseData { get; set; }

        Questionnaire GetQuestionnaire(Guid id);
        Question GetQuestion(Guid id, Guid questionId);
        Choice GetChoice(Guid id, Guid questionId, int value);
        Response GetResponse(Guid id, Guid questionId, int value, Guid responseId);
        bool ValidId(Guid id);
        bool UnusedId(Guid id);
        bool ValidResponse(Guid id);
        bool UnusedResponse(Guid id);
    }
}