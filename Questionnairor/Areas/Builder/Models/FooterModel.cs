using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Builder.Models
{
    public class FooterModel
    {
        public string QuestionId { get; set; } = "";
        public string Value { get; set; } = "";
        public FooterModel(object questionId, object value)
        {
            if (questionId != null) QuestionId = questionId.ToString();
            if (value != null) Value = value.ToString();
        }
    }
}
