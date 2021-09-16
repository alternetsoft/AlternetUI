using System;
using System.IO;
using System.Linq;

namespace Alternet.UI.Integration.IntelliSense
{
    public static class LanguageDetector
    {
        public static Language DetectLanguageFromFileName(string fileName)
        {
            var language = SupportedLanguages.All.FirstOrDefault(
                x => ("." + x.FileExtension).Equals(Path.GetExtension(fileName), StringComparison.OrdinalIgnoreCase));

            if (language == null)
                throw new NotSupportedException();

            return language;
        }
    }
}