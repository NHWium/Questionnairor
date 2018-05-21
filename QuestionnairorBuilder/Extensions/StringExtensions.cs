using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionnairorBuilder.Extensions
{
    public static class StringExtensions
    {
        private const string NULL_STRING = "__@NULL@__";

        // Thanks to Shaul Behr from
        // https://stackoverflow.com/questions/16522620/blanks-converts-to-null-in-razor-view-c-sharp
        public static string StringifyIfNull(this string s)
        {
            return s == null ? NULL_STRING : s;
        }

        public static string DestringifyIfNull(this string s)
        {
            return NULL_STRING == s ? null : s;
        }
    }
}
