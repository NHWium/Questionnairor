using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class Questionnaire : IEquatable<Questionnaire>
    {
        /// <summary>
        /// A global id to identify this questionnaire.
        /// </summary>
        [BindRequired]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// A title text.
        /// </summary>
        [BindRequired]
        public string Title { get; set; } = "";
        /// <summary>
        /// General text at the top of page.
        /// </summary>
        [BindRequired]
        public string Introduction { get; set; } = "";
        /// <summary>
        /// List of questions on this page.
        /// </summary>
        [BindRequired]
        public List<Question> Questions { get; set; } = new List<Question>();

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
                    .Introduction("Not Loaded")
                    .Id(Guid.Empty);
            }
        }

        /// <summary>
        /// Create a json string from the questionnaire.
        /// </summary>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>The serialized questionnaire.</returns>
        public string ToJson(Formatting formatting)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

        public bool Equals(Questionnaire q)
        {
            if (q is null) return false;
            if (Id == null && q.Id != null) return false;
            if (Id != null && q.Id == null) return false;
            if (Title == null && q.Title != null) return false;
            if (Title != null && q.Title == null) return false;
            if (Introduction == null && q.Introduction != null) return false;
            if (Introduction != null && q.Introduction == null) return false;
            if (Questions == null && q.Questions != null) return false;
            if (Questions != null && q.Questions == null) return false;
            return (Id == null || Id.Equals(q.Id)) &&
                    (Title == null || Title.Equals(q.Title)) &&
                    (Introduction == null || Introduction.Equals(q.Introduction)) &&
                    (Questions == null || Questions.SequenceEqual(q.Questions));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(true, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals(obj as Questionnaire);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Id.GetHashCode() ^ 3 + Title.GetHashCode() ^ 5 + Introduction.GetHashCode() ^ 7 + Questions.GetHashCode() + 6277) * 2287;
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
        /// <param name="value">A title text.</param>
        public static Questionnaire Title(this Questionnaire o, string value)
        {
            o.Title = value;
            return o;
        }
        /// <param name="value">General text at the top of page.</param>
        public static Questionnaire Introduction(this Questionnaire o, string value)
        {
            o.Introduction = value;
            return o;
        }
        /// <param name="value">List of questions on this page.</param>
        public static Questionnaire Questions(this Questionnaire o, List<Question> value)
        {
            o.Questions = value;
            return o;
        }
    }

}
