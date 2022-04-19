namespace SampleManagement.Common
{
    public static class TemplateUtility
    {
        public static void ReplacePlaceholdersInFile(string fileName, IEnumerable<(string Placeholder, string Replacement)> values)
        {
            void ReplaceInFile(string fileName, string searchString, string replacementString) =>
                File.WriteAllText(fileName, File.ReadAllText(fileName).Replace(searchString, replacementString));

            foreach (var value in values)
                ReplaceInFile(fileName, value.Placeholder, value.Replacement);
        }
    }
}