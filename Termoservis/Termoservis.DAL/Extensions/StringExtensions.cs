using System;
using System.Collections.Generic;
using System.Linq;
using Fissoft.EntityFramework.Fts;

namespace Termoservis.DAL.Extensions
{
    /// <summary>
    /// The string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Transforms specified string to FTS string.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <returns>Returns the transformed FTS string.</returns>
        public static string AsFtsContainsString(this string keywords)
        {
            return $"({FullTextSearchModelUtil.FullTextContains}{ConvertWithAll(keywords)})";
        }

        /// <summary>
        /// Converts search keywords to keywords string seperated by AND keyword.
        /// </summary>
        /// <param name="search">The search.</param>
        /// <returns>Returns keywords string seperated by AND keyword.</returns>
        private static string ConvertWithAll(string search)
        {
            if (string.IsNullOrWhiteSpace(search) || !search.Contains(" ") || search.StartsWith("\"") && search.EndsWith("\""))
                return search;
            return string.Join(" and ", search.Split(' ', '　').Where(c => c != "and"));
        }
    }
}