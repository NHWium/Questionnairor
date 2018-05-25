using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Questionnairor.Models
{
    /// <summary>
    /// A possible choice in a multiple-choice question.
    /// </summary>
    public class Choice : IEquatable<Choice>
    {
        /// <summary>
        /// Value of the choice. Used to identify the choice, so should be unique.
        /// </summary>
        [BindRequired]
        [Range(0,1000000)]
        public int Value { get; set; } = 0;
        /// <summary>
        /// Visual text of choice.
        /// </summary>
        [BindRequired]
        public string Text { get; set; } = "";
        /// <summary>
        /// This choice is the default question, selected at start.
        /// </summary>
        [BindRequired]
        public bool IsDefault { get; set; } = false;
        /// <summary>
        /// One or more responses, triggered by this choice.
        /// </summary>
        [BindRequired]
        public List<Response> Responses { get; set; } = new List<Response>();
        
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
                return new Choice().Value(0)
                    .Text("Not Loaded")
                    .IsDefault(true);
            }
        }

        /// <summary>
        /// Create a json string from the choice.
        /// </summary>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized choice.</returns>
        public string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public bool Equals(Choice c)
        {
            if (c is null) return false;
            if (Text == null && c.Text != null) return false;
            if (Text != null && c.Text == null) return false;
            if (Responses == null && c.Responses != null) return false;
            if (Responses != null && c.Responses == null) return false;
            return  (Value == c.Value) &&
                    (Text == null || Text.Equals(c.Text)) &&
                    (IsDefault == c.IsDefault) &&
                    (Responses == null || Responses.SequenceEqual(c.Responses));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(true, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals(obj as Choice);
        }

        // override object.GetHashCode - using some primes
        public override int GetHashCode()
        {
            int result = 151;
            if (Text != null) result ^= Text.GetHashCode();
            if (Responses != null) result *= Responses.GetHashCode();
            if (IsDefault) result ^= 29;
            if (Value != 0) result *= 2399;
            return result;
        }

        /// <summary>
        /// Get a specific response based on id.
        /// </summary>
        /// <param name="questionId">The id of the response to get.</param>
        /// <returns>The found response or null.</returns>
        public Response GetResponse(Guid responseId)
        {
            try
            {
                //Use First instead of FirstOrDefault in a try,catch - we do not want default value but null if not found.
                return Responses.First<Response>(r => r.Id == responseId);
            }
            catch (Exception)
            {
                return null;
            }
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
