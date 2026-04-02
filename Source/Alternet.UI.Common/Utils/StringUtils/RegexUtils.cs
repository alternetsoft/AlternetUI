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
        /// Splits each input text line into segments based on bold tag formatting and returns the results as an array
        /// of arrays.
        /// </summary>
        /// <remarks>Each segment in the result indicates whether it is bold or not, based on the presence
        /// of bold tags in the input lines. The order of the returned arrays matches the order of the input
        /// lines.</remarks>
        /// <param name="lines">An array of strings representing the lines of text to process. May be null or empty.</param>
        /// <returns>An array of arrays, where each inner array contains the segments of the corresponding input line with
        /// associated font style information. Returns an empty array if the input is null or contains no lines.</returns>
        public static TextAndFontStyle[][] GetTextLinesBoldTagSplitted(string[]? lines)
        {
            if (lines == null || lines.Length == 0)
                return Array.Empty<TextAndFontStyle[]>();

            TextAndFontStyle[][] result = new TextAndFontStyle[lines.Length][];

            for (int i = 0; i < lines.Length; i++)
            {
                result[i] = GetBoldTagSplitted(lines[i]);
            }

            return result;
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
            var pattern = "(<b>|</b>|<i>|</i>|<u>|</u>)";
            var inTagValue = string.Empty;
            var boldCounter = 0;
            var italicCounter = 0;
            var underlineCounter = 0;

            FontStyle GetFontStyle()
            {
                FontStyle fontStyle = FontStyle.Regular;
                if (boldCounter > 0)
                    fontStyle |= FontStyle.Bold;
                if (italicCounter > 0)
                    fontStyle |= FontStyle.Italic;
                if (underlineCounter > 0)
                    fontStyle |= FontStyle.Underline;
                return fontStyle;
            }

            bool ProcessTag(string subStr, string startTag, string endTag, ref int counter)
            {
                if (subStr.Equals(startTag))
                {
                    counter++;
                    return true;
                }
                else
                    if (subStr.Equals(endTag))
                    {
                        if (!string.IsNullOrEmpty(inTagValue))
                            list.Add(new(inTagValue, GetFontStyle()));
                        counter--;
                        return true;
                    }

                return false;
            }

            foreach (var subStr in Regex.Split(s.ToString(), pattern))
            {
                if (ProcessTag(subStr, StringUtils.BoldTagStart, StringUtils.BoldTagEnd, ref boldCounter))
                    continue;
                if (ProcessTag(subStr, StringUtils.ItalicTagStart, StringUtils.ItalicTagEnd, ref italicCounter))
                    continue;
                if (ProcessTag(subStr, StringUtils.UnderlineTagStart, StringUtils.UnderlineTagEnd, ref underlineCounter))
                    continue;

                if (GetFontStyle() != FontStyle.Regular)
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
