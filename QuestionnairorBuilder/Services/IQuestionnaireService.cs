using QuestionnaireData.Models;
using System;
using System.Collections.Generic;

namespace QuestionnairorBuilder.Services
{
    public interface IQuestionnaireService
    {
        Dictionary<Guid, Questionnaire> ModelData { get; set; }
    }
}