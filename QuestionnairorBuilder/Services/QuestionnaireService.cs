using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireData.Models;

namespace QuestionnairorBuilder.Services
{
    public class QuestionnaireService: IQuestionnaireService
    {
        public Dictionary<Guid, Questionnaire> ModelData { get; set; }

        public QuestionnaireService()
        {
            ModelData = new Dictionary<Guid,Questionnaire>();
        }
    }
}
