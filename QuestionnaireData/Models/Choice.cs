using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionnaireData.Models
{
    /// <summary>
    /// A possible choice in a multiple-choice question.
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Value of the choice. Used to identify the choice, so should be unique.
        /// </summary>
        public int Value { get; set; } = 0;
        /// <summary>
        /// Visual text of choice.
        /// </summary>
        public string Text { get; set; } = "";
        /// <summary>
        /// One or more responses, triggered by this choice.
        /// </summary>
        public List<Response> Responses { get; set; } = new List<Response>();
        /// <summary>
        /// This choice is the default question, selected at start.
        /// </summary>
        public bool IsDefault { get; set; } = false;
        
        /// <summary>
        /// A possible choice in a multiple-choice question.
        /// </summary>
        /// <param name="value">Value of the choice. Used to identify the choice, so should be unique.</param>
        public Choice(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Create a choice from provided json.
        /// </summary>
        /// <param name="json">The json data</param>
        /// <returns>The deserialized choice.</returns>
        public static Choice FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Choice>(json);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("### JSON ERROR ###:");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return new Choice(0)
                    .Text("Not Loaded")
                    .IsDefault(true);
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Choice c = (Choice)obj;
            return (Value == c.Value) && (Text.Equals(c.Text)) && (IsDefault == c.IsDefault) && (Responses.SequenceEqual(c.Responses));
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Text.GetHashCode() * Responses.GetHashCode()) ^ (Value + 1);
        }

    }
    /**
     * A extension class, allowing linq-like data building.
     */
    public static class ChoiceExtension
    {
        /// <param name="value">Visual text of choice.</param>
        public static Choice Text(this Choice o, string value)
        {
            o.Text = value;
            return o;
        }
        /// <param name="value">Value of the choice. Used to identify the choice, so should be unique.</param>
        public static Choice Value(this Choice o, int value)
        {
            o.Value = value;
            return o;
        }
        /// <param name="value">One or more responses, triggered by this choice.</param>
        public static Choice Responses(this Choice o, List<Response> value)
        {
            o.Responses = value;
            return o;
        }
        /// <param name="value">This choice is the default question, selected at start.</param>
        public static Choice IsDefault(this Choice o, bool value)
        {
            o.IsDefault = value;
            return o;
        }
    }
}
