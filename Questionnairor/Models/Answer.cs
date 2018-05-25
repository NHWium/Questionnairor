using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnairor.Models
{
    public class Answer
    {
        /// <summary>
        /// A global id to identify the question answered.
        /// </summary>
        public Guid QuestionId { get; set; } = Guid.Empty;
        /// <summary>
        /// Value of the answer.
        /// </summary>
        public int? SelectedChoiceValue { get; set; } = null; 
    }
    /**
     * A extension class, allowing linq-like data building.
     */
    public static class AnswerExtension
    {
        /// <param name="value">A global id to identify the question answered.</param>
        public static Answer QuestionId(this Answer o, Guid value)
        {
            o.QuestionId = value;
            return o;
        }
        /// <param name="value">Value of the answer.</param>
        public static Answer SelectedChoiceValue(this Answer o, int? value)
        {
            o.SelectedChoiceValue = value;
            return o;
        }
    }
}
