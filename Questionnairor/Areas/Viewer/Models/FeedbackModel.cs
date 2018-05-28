using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Viewer.Models
{
    public class FeedbackModel
    {
        public Questionnaire Answers { get; set; } = null;
        public List<Response> Responses { get; set; } = null;

        public FeedbackModel(Questionnaire data)
        {
            Answers = data;
            if (data.IsAllAnswered())
            {
                List<Response> responses = data.GetActiveResponses(data.GetAnswers());
                Responses = responses;
            }
        }
    }
}
