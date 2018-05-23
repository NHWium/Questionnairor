using System.Collections.Generic;
using System.Linq;
using Questionnairor.Models;

namespace Questionnairor.Extensions
{
    public static class CollectiveExtensions
    {
        /// <summary>
        /// Extension to List<T>. Adds an object to the list only if unique.
        /// </summary>
        /// <param name="element">Element added if unique.</param>
        /// <returns>True if added.</returns>
        public static bool AddUnique<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
            {
                list.Add(element);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Extension to List<Choice>. Adds an object only if the Value is unique.
        /// </summary>
        /// <param name="element">Element added if unique.</param>
        /// <returns>True if added.</returns>
        public static bool AddUniqueChoice(this List<Choice> list, Choice element)
        {
            if (!list.Contains(element))
            {
                if (!list.Any(c => c.Value == element.Value))
                {
                    list.Add(element);
                    return true;
                }
            }
            return false;
        }
    }
}
