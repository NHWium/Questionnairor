using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuestionnaireData.Models
{
    /// <summary>
    /// A questionnaire of multiple choice questions.
    /// </summary>
    public class Questionnaire
    {
        /// <summary>
        /// A global id to identify this questionnaire.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// List of questions on this page.
        /// </summary>
        public List<Question> Questions { get; set; } = new List<Question>();
        /// <summary>
        /// General text at the top of page.
        /// </summary>
        public string Introduction { get; set; } = "";

        /// <summary>
        /// Create a questionnaire from provided json.
        /// </summary>
        /// <param name="json">The json data</param>
        /// <returns></returns>
        public static Questionnaire FromJson(string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<Questionnaire>(json);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("### JSON ERROR ###:");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
                return new Questionnaire()
                    .Introduction("Not Loaded");
            }
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Questionnaire q = (Questionnaire)obj;
            return (Id.Equals(q.Id)) && (Introduction.Equals(q.Introduction)) && (Questions.SequenceEqual(q.Questions));
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode() * Introduction.GetHashCode() * Questions.GetHashCode();
        }
    }

    /**
     * A extension class, allowing linq-like data building.
     */
    public static class QuestionnaireExtension
    {
        /// <param name="value">A global id to identify this questionnaire.</param>
        public static Questionnaire Id(this Questionnaire o, Guid value)
        {
            o.Id = value;
            return o;
        }
        /// <param name="value">List of questions on this page.</param>
        public static Questionnaire Questions(this Questionnaire o, List<Question> value)
        {
            o.Questions = value;
            return o;
        }
        /// <param name="value">General text at the top of page.</param>
        public static Questionnaire Introduction(this Questionnaire o, string value)
        {
            o.Introduction = value;
            return o;
        }
    }

}
