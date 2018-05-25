using Questionnairor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Questionnairor.Areas.Viewer.Models
{
    public class QuestionWithIndex : Question
    {
        public int QuestionIndex { get; set; }

        public QuestionWithIndex(int questionIndex, Question question)
        {
            QuestionIndex = questionIndex;
            Id = question.Id;
            Title = question.Title;
            Text = question.Text;
            Choices = question.Choices;
        }
    }
}
