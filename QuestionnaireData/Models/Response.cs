using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuestionnaireData.Models
{
    /// <summary>
    /// Feedback to a questionnaire. Once triggered a text can be shown.
    /// </summary>
    public class Response
    {
        /// <summary>
        /// A global id to identify this response.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// The number of times this response must be chosen before it triggers. 
        /// </summary>
        public int MinimumChoices { get; set; } = 1;
        /// <summary>
        /// The text to display as the responce once this triggers.
        /// </summary>
        public string Feedback { get; set; } = "";

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
                    .Feedback("Not Loaded")
                    .Id(Guid.Empty);
            }
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Response r = (Response)obj;
            return (Id.Equals(r.Id)) && (MinimumChoices == r.MinimumChoices) && (Feedback.Equals(r.Feedback));
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Id.GetHashCode() * Feedback.GetHashCode()) ^ (MinimumChoices + 1);
        }
    }
    /**
     * A extension class, allowing linq-like data building.
     */
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
    }
}
