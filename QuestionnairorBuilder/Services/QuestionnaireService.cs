using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuestionnaireData.Models;

namespace QuestionnairorBuilder.Services
{
    public class QuestionnaireService: IQuestionnaireService
    {
        public Questionnaire Model { get; set; }

        public QuestionnaireService()
        {
            Model = new Questionnaire();
        }
    }
}
