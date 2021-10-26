using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Alternet.UI.Versioning
{
    public static class RegexPatcher
    {
        public static void PatchFile(string filePath, Regex regex, string groupName, string replacement)
        {
            var text = File.ReadAllText(filePath);

            var match = regex.Match(text);
            if (!match.Success)
                return;

            if (!match.Groups.TryGetValue(groupName, out var group))
                return;

            var newText = ReplaceNamedGroup(groupName, replacement, match);

            if (string.Equals(text, newText, StringComparison.Ordinal))
                return;

            File.WriteAllText(filePath, newText);
        }

        private static string ReplaceNamedGroup(string groupName, string replacement, Match m)
        {
            string capture = m.Value;
            capture = capture.Remove(m.Groups[groupName].Index - m.Index, m.Groups[groupName].Length);
            capture = capture.Insert(m.Groups[groupName].Index - m.Index, replacement);
            return capture;
        }
    }
}