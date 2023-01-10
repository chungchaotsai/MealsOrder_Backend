using System.Collections.Generic;
using System.Linq;

namespace MealsOrderAPI.Extensions
{
    /// <summary>
    /// Extension methods of string.
    /// </summary>
    public static class StringExtension
    {
        public static string FirstCharToUpper(this string str)
        {
            return !string.IsNullOrEmpty(str)
                ? $"{char.ToUpper(str.FirstOrDefault())}{str[1..]}"
                : str;
        }

        public static string AddPeriodAtEnd(this string body)
        {
            if (string.IsNullOrEmpty(body))
                return body;

            return body.LastOrDefault() != '.'
                ? $"{body}."
                : body;
        }

        public static string UseCommaJoinStringArray(params string[] body)
        {
            return $"{string.Join(", ", body)}";
        }

        public static string IEnumerableItemsToString(this IEnumerable<string> items)
        {
            string allFieldName = string.Empty;
            for (int i = 0; i < items.Count(); i++)
            {
                allFieldName += items.ElementAt(i).AddQuotationMarks();
                if (i == items.Count() - 2)
                {
                    allFieldName += " or ";
                    continue;
                }
                if (i == items.Count() - 1)
                    continue;
                allFieldName += ", ";
            }

            return allFieldName;
        }

        public static string AddQuotationMarks(this string s)
        {
            return $"'{s}'";
        }

    }
}
