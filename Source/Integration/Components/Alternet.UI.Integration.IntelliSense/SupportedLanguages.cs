using System.Collections.Generic;

namespace Alternet.UI.Integration.IntelliSense
{
    public record Language(string Name, string FileExtension);

    public static class SupportedLanguages
    {
        public static readonly Language CSharp = new(nameof(CSharp), "cs");

        public static readonly IReadOnlyList<Language> All = new[] { CSharp };
    }
}