using Microsoft.VisualBasic.FileIO;
using System;
using System.IO;

namespace SampleAdder
{
    internal static class TemplateService
    {
        static string GetTemplateDirectory()
        {
            var directory = Path.GetFullPath(
                Path.Combine(Path.GetDirectoryName(typeof(SampleValidator).Assembly.Location) ?? throw new Exception(),
                "../../../Template/Project"));

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Sample template directory was not found: " + directory);

            return directory;
        }

        public static void InstantiateTemplate(string sampleName, string targetDirectory)
        {
            var templateDirectory = GetTemplateDirectory();
            FileSystem.CopyDirectory(templateDirectory, targetDirectory);

            RenameFiles(sampleName, targetDirectory);
            ReplacePlaceholders(sampleName, targetDirectory);
        }

        private static void ReplacePlaceholders(string sampleName, string targetDirectory)
        {
            void ReplaceInFile(string fileName, string searchString, string replacementString) =>
                File.WriteAllText(fileName, File.ReadAllText(fileName).Replace(searchString, replacementString));

            foreach (var fileName in Directory.GetFiles(targetDirectory, "*.*", System.IO.SearchOption.AllDirectories))
                ReplaceInFile(fileName, "##SampleName##", sampleName);
        }

        private static void RenameFiles(string sampleName, string targetDirectory)
        {
            File.Move(
                Path.Combine(targetDirectory, "SampleTemplate.csproj"),
                Path.Combine(targetDirectory, sampleName + ".csproj"));
        }
    }
}