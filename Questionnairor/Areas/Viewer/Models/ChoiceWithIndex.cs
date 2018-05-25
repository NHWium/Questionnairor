using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Viewer.Models
{
    public class ChoiceWithIndex : Choice
    {
        public int QuestionIndex { get; set; }
        public int ChoiceIndex { get; set; }

        public ChoiceWithIndex (int questionIndex, int choiceIndex, Choice choice)
        {
            QuestionIndex = questionIndex;
            ChoiceIndex = choiceIndex;
            this.Value = choice.Value;
            this.Text = choice.Text;
            this.IsDefault = choice.IsDefault;
            this.Responses = choice.Responses;
        }
    }
}
