﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Questionnairor.Models
{
    /// <summary>
    /// A multiple-choice question.
    /// </summary>
    public class Question : IEquatable<Question>
    {
        /// <summary>
        /// A global id to identify this question.
        /// </summary>
        [BindRequired]
        public Guid Id { get; set; } = Guid.NewGuid();
        /// <summary>
        /// A title text.
        /// </summary>
        [BindRequired]
        [Required, StringLength(60, MinimumLength = 5)]
        public string Title { get; set; } = "";
        /// <summary>
        /// The text to display.
        /// </summary>
        [BindRequired]
        public string Text { get; set; } = "";
        /// <summary>
        /// The choices in the multiple-choice question.
        /// </summary>
        [BindRequired]
        public List<Choice> Choices { get; set; } = new List<Choice>();

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
                    .Title("Not Loaded")
                    .Text("Not Loaded")
                    .Id(Guid.Empty);
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

        public bool Equals(Question q)
        {
            if (q is null) return false;
            if (Id == null && q.Id != null) return false;
            if (Id != null && q.Id == null) return false;
            if (Title == null && q.Title != null) return false;
            if (Title != null && q.Title == null) return false;
            if (Text == null && q.Text != null) return false;
            if (Text != null && q.Text == null) return false;
            if (Choices == null && q.Choices != null) return false;
            if (Choices != null && q.Choices == null) return false;
            return  (Id == null || Id.Equals(q.Id)) && 
                    (Title == null || Title.Equals(q.Title)) && 
                    (Text == null || Text.Equals(q.Text)) && 
                    (Choices == null || Choices.SequenceEqual(q.Choices));
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(true, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals(obj as Question);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return (Id.GetHashCode() * 13 + Title.GetHashCode() * 2089 + Text.GetHashCode() * 2617 + Choices.GetHashCode() * 6163) ^ 13;
        }

        /// <summary>
        /// Does the value exist in the list of choices.
        /// </summary>
        /// <param name="value">The value to test again.</param>
        /// <returns>True if value exists.</returns>
        public bool ChoiceValueExists(int value)
        {
            return Choices.Any(c => c.Value == value);
        }

        /// <summary>
        /// Get a specific choice based on value.
        /// </summary>
        /// <param name="questionId">The id of the choice to get.</param>
        /// <returns>The found choice or null.</returns>
        public Choice GetChoice(int value)
        {
            try
            {
                //Use First instead of FirstOrDefault in a try,catch - we do not want default value but null if not found.
                return Choices.First<Choice>(c => c.Value == value);
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
    public static class QuestionExtension
    {
        /// <param name="value">A global id to identify this question.</param>
        public static Question Id(this Question o, Guid value)
        {
            o.Id = value;
            return o;
        }
        /// <param name="value">A title text.</param>
        public static Question Title(this Question o, string value)
        {
            o.Title = value;
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