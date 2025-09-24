using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are related to the regular expressions.
    /// </summary>
    public static class RegexUtils
    {
        static RegexUtils()
        {
            Test();
        }

        /// <summary>
        /// Splits html text with bold tags into collection of strings with font style attribute.
        /// </summary>
        /// <param name="s">Html text to split.</param>
        /// <returns></returns>
        public static TextAndFontStyle[] GetBoldTagSplitted(ReadOnlySpan<char> s)
        {
            if (s.Length == 0)
                return Array.Empty<TextAndFontStyle>();

            var list = new List<TextAndFontStyle>();
            var pattern = "(<b>|</b>)";
            var isInTag = false;
            var inTagValue = string.Empty;

            foreach (var subStr in Regex.Split(s.ToString(), pattern))
            {
                if (subStr.Equals(StringUtils.BoldTagStart))
                {
                    isInTag = true;
                    continue;
                }
                else
                if (subStr.Equals(StringUtils.BoldTagEnd))
                {
                    isInTag = false;
                    if(!string.IsNullOrEmpty(inTagValue))
                        list.Add(new(inTagValue, FontStyle.Bold));
                    continue;
                }

                if (isInTag)
                {
                    inTagValue = subStr;
                    continue;
                }

                if (!string.IsNullOrEmpty(subStr))
                    list.Add(new(subStr, FontStyle.Regular));
            }

            return list.ToArray();
        }

        [Conditional("DEBUG")]
        internal static void TestGetBoldTagSplitted()
        {
            var s = "This is text with <b>bold</b> tag.";

            var result = GetBoldTagSplitted(s);
            App.LogSection(
                () =>
                {
                    App.Log($"<{s}>");
                    LogUtils.LogEnumerable(result);
                },
                nameof(TestGetBoldTagSplitted));
        }

        [Conditional("DEBUG")]
        private static void Test()
        {
        }
    }
}
