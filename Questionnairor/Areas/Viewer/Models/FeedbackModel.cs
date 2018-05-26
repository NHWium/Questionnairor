using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Viewer.Models
{
    public class FeedbackModel
    {
        public Questionnaire Answers { get; set; }
        public List<Response> Responses { get; set; }
    }
}
