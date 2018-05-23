using System;
using System.Collections.Generic;
using System.Text;

namespace Questionnairor.Extensions
{
    public static class CollectiveExtensions
    {
        /// <summary>
        /// Extension to List<T>. Adds an object to the list only if unique.
        /// </summary>
        /// <param name="element">Element added if unique.</param>
        public static void AddUnique<T>(this List<T> list, T element)
        {
            if (!list.Contains(element))
                list.Add(element);
        }
    }
}
