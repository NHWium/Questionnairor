using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Questionnairor.Models
{
    /// <summary>
    /// Feedback to a questionnaire. Once triggered a text can be shown.
    /// </summary>
    public class Response : IEquatable<Response>
    {
        /// <summary>
        /// A global id to identify this response.
        /// </summary>
        [Key]
        [BindRequired]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// The number of times this response must be chosen before it triggers. 
        /// </summary>
        [BindRequired]
        public int MinimumChoices { get; set; } = 1;
        /// <summary>
        /// The text to display as the responce once this triggers.
        /// </summary>
        [BindRequired]
        [Required, StringLength(1024, MinimumLength = 1)]
        public string Feedback { get; set; } = "";
        /// <summary>
        /// The text to display when building the responses.
        /// </summary>
        [BindRequired]
        [Required, StringLength(20, MinimumLength = 1)]
        public string Title { get; set; } = "";

        public Response()
        {
        }

        /// <summary>
        /// Create a response from provided json.
        /// </summary>
        /// <param name="json">The json data</param>
        /// <returns></returns>
        public static Response FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Response>(json);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("### JSON ERROR ###:");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return new Response()
                    .Title("Not Loaded")
                    .Id(Guid.Empty);
            }
        }

        /// <summary>
        /// Create a json string from the response.
        /// </summary>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized response.</returns>
        public string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public bool Equals(Response r)
        {
            if (r is null) return false;
            if (Id == null && r.Id != null) return false;
            if (Id != null && r.Id == null) return false;
            if (Feedback == null && r.Feedback != null) return false;
            if (Feedback != null && r.Feedback == null) return false;
            if (Title == null && r.Title != null) return false;
            if (Title != null && r.Title == null) return false;
            return  (Id == null || Id.Equals(r.Id)) &&
                    (MinimumChoices == r.MinimumChoices) &&
                    (Feedback == null || Feedback.Equals(r.Feedback)) &&
                    (Title == null || Title.Equals(r.Title));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(true, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals(obj as Response);
        }
        
        // override object.GetHashCode
        public override int GetHashCode()
        {
            int result = 29;
            if (Id != null) result *= Id.GetHashCode() + 229;
            if (Feedback != null) result *= Feedback.GetHashCode() * 409;
            if (Title != null) result *= Title.GetHashCode() * 659;
            result += MinimumChoices.GetHashCode() + 1013;
            return result;
        }
    }

    /// <summary>
    /// A extension class, allowing linq-like data building.
    /// </summary>
    public static class ResponseExtension
    {
        /// <param name="value">A global id to identify this response.</param>
        public static Response Id(this Response o, Guid value)
        {
            o.Id = value;
            return o;
        }
        /// <param name="value">The number of times this response must be chosen before it triggers.</param>
        public static Response MinimumChoices(this Response o, int value)
        {
            o.MinimumChoices = value;
            return o;
        }
        /// <param name="value">The text to display as the responce once this triggers.</param>
        public static Response Feedback(this Response o, string value)
        {
            o.Feedback = value;
            return o;
        }
        /// <param name="value">The text to display when building the responses.</param>
        public static Response Title(this Response o, string value)
        {
            o.Title = value;
            return o;
        }
    }
}
