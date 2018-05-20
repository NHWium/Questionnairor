using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionnaireData.Models
{
    /// <summary>
    /// A multiple-choice question.
    /// </summary>
    public class Question
    {
        /// <summary>
        /// A global id to identify this question.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// The text to display.
        /// </summary>
        public string Text { get; set; } = "";
        /// <summary>
        /// The choices in the multiple-choice question.
        /// </summary>
        public List<Choice> Choices { get; set; } = new List<Choice>();

        /// <summary>
        /// A blank multiple-choice question.
        /// </summary>
        public Question()
        {
        }
        /// <summary>
        /// A multiple-choice question from minimum to maximum.
        /// </summary>
        /// <param name="text">The text to display.</param>
        /// <param name="minimumValue">Minimum value in the multiple-choice question.</param>
        /// <param name="maximumValue">Maximum value in the multiple-choice question.</param>
        /// <param name="defaultValue">Default value in the multiple-choice question.</param>
        public Question(string text, int minimumValue, int maximumValue, int defaultValue)
        {
            Text = text;
            Choices = new List<Choice>();
            for (int i = minimumValue; i < maximumValue; i++)
            {
                Choice choice = new Choice(i);
                if (i == defaultValue) choice.IsDefault = true;
                Choices.Add(choice);
            }
        }

        /// <summary>
        /// Create a question from provided json.
        /// </summary>
        /// <param name="json">The json data</param>
        /// <returns></returns>
        public static Question FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Question>(json);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("### JSON ERROR ###:");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return new Question()
                    .Text("Not Loaded");
            }
        }

        /// <summary>
        /// Create a json string from the question.
        /// </summary>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized question.</returns>
        public string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Question q = (Question)obj;
            return (Id.Equals(q.Id)) && (Text.Equals(q.Text)) && (Choices.SequenceEqual(q.Choices));
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode() * Text.GetHashCode() * Choices.GetHashCode();
        }
    }
    /**
     * A extension class, allowing linq-like data building.
     */
    public static class QuestionExtension
    {
        /// <param name="value">A global id to identify this question.</param>
        public static Question Id(this Question o, Guid value)
        {
            o.Id = value;
            return o;
        }
        /// <param name="value">The text to display.</param>
        public static Question Text(this Question o, string value)
        {
            o.Text = value;
            return o;
        }
        /// <param name="value">The choices in the multiple-choice question.</param>
        public static Question Choices(this Question o, List<Choice> value)
        {
            o.Choices = value;
            return o;
        }
    }
}