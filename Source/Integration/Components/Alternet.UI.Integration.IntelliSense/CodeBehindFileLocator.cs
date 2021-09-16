using System.IO;

namespace Alternet.UI.Integration.IntelliSense
{
    public static class CodeBehindFileLocator
    {
        public static string TryFindCodeBehindFile(string uixmlFileName)
        {
            foreach (var language in SupportedLanguages.All)
            {
                var codeBehindPath = uixmlFileName.Replace(".uixml", ".uixml." + language.FileExtension);
                if (File.Exists(codeBehindPath))
                    return codeBehindPath;
            }

            return null;
        }
    }
}