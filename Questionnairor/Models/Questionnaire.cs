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
        [Required, StringLength(30, MinimumLength = 1)]
        public string Title { get; set; } = "";
        /// <summary>
        /// General text at the top of page.
        /// </summary>
        [BindRequired]
        public string Introduction { get; set; } = "";
        /// <summary>
        /// Text at the top of the feedback page.
        /// </summary>
        [BindRequired]
        public string Conclusion { get; set; } = "";
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
                    .Title("Not Loaded")
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
            if (Conclusion == null && q.Conclusion != null) return false;
            if (Conclusion != null && q.Conclusion == null) return false;
            if (Questions == null && q.Questions != null) return false;
            if (Questions != null && q.Questions == null) return false;
            return (Id == null || Id.Equals(q.Id)) &&
                    (Title == null || Title.Equals(q.Title)) &&
                    (Introduction == null || Introduction.Equals(q.Introduction)) &&
                    (Conclusion == null || Conclusion.Equals(q.Conclusion)) &&
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
            int result = 3581;
            if (Id != null) result *= Id.GetHashCode() * 3023;
            if (Title != null) result *= Title.GetHashCode() * 71;
            if (Introduction != null) result += Introduction.GetHashCode() + 7829;
            if (Conclusion != null) result *= Conclusion.GetHashCode() + 3049;
            if (Questions != null) result += Questions.GetHashCode();
            return result;
        }

        /// <summary>
        /// Get a specific question based on id.
        /// </summary>
        /// <param name="questionId">The id of the question to get.</param>
        /// <returns>The found question or null.</returns>
        public Question GetQuestion(Guid questionId)
        {
            try
            {
                //Use First instead of FirstOrDefault in a try,catch - we do not want default value but null if not found.
                return Questions.First<Question>(question => question.Id == questionId);
            }
            catch (Exception)
            {
                return null;
            }
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
                return Questions.SelectMany(question => question.Choices
                    .SelectMany(choice => choice.Responses))
                    .First<Response>(response => response.Id.Equals(responseId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a specific response based on id.
        /// </summary>
        /// <param name="questionId">The id of the response to get.</param>
        /// <returns>The found response or null.</returns>
        public List<Choice> GetChoicesWithResponse(Response response)
        {
            return Questions.SelectMany(question => question.Choices)
                .Where(choice => choice.Responses.Contains(response))
                .ToList<Choice>();
        }

        /// <summary>
        /// Get all reponses not part of the given choice.
        /// </summary>
        /// <param name="currentChoice">The choice to excluse responses from.</param>
        /// <returns>A list of reponses.</returns>
        public List<Response> GetAvailableResponses(Choice currentChoice)
        {
            return Questions.SelectMany(question => question.Choices
                    .SelectMany(choice => choice.Responses))
                .Except(currentChoice.Responses)
                .ToList<Response>();
        }

        /// <summary>
        /// Get a list of all the answers chosen.
        /// </summary>
        /// <returns>All choices selected in all questions.</returns>
        public List<Choice> GetAnswers()
        {
            if (Questions == null) return null;
            return Questions
                .Where(question => question.Answer != null)
                .Select(question => question.GetChoice(question.Answer.Value))
                .Where(choice => choice != null)
                .ToList<Choice>();
        }

        /// <summary>
        /// If all questions are answered or not.
        /// </summary>
        /// <returns>True if all questions have answers.</returns>
        public bool IsAllAnswered()
        {
            if (Questions == null) return false;
            return !Questions
                .Any(question => question.Answer == null);
        }

        /// <summary>
        /// Get a list of all responses answered the minimum amount of times.
        /// </summary>
        /// <param name="answers">A list of answered choices.</param>
        /// <returns>A list of responses giving feedback.</returns>
        public List<Response> GetActiveResponses(List<Choice> answers)
        {
            if (answers == null) return null;
            return answers
                .Where(choice => choice.Responses != null)
                .SelectMany(choice => choice.Responses)
                .Where(response => 
                    response.MinimumChoices <= answers
                        .Count(choice =>
                            choice != null &&
                            choice.Responses.Contains(response)
                            )
                    )
                .Distinct()
                .ToList<Response>();
        }
    }

    /// <summary>
    /// A extension class, allowing linq-like data building.
    /// </summary>
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
        /// <param name="value">Text at the top of the feedback page.</param>
        public static Questionnaire Conclusion(this Questionnaire o, string value)
        {
            o.Conclusion = value;
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
