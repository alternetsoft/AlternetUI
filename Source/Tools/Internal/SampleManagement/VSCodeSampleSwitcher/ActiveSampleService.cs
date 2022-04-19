using SampleManagement.Common;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace VSCodeSampleSwitcher
{
    internal static class ActiveSampleService
    {
        private const string LaunchConfigFileName = "launch.json";

        private const string TasksConfigFileName = "tasks.json";

        public static void SetActiveSample(Sample sample)
        {
            void InstantiateTemplate(string fileName)
            {
                string targetFileName = ResourceLocator.GetVSCodeFilePath(fileName);
                string templateFileName = GetTemplateFilePath(fileName);

                if (File.Exists(targetFileName))
                    File.Delete(targetFileName);
                
                File.Copy(templateFileName, targetFileName);
                TemplateUtility.ReplacePlaceholdersInFile(targetFileName, new[] { ("##SampleName##", sample.Name) });
            }

            InstantiateTemplate(LaunchConfigFileName);
            InstantiateTemplate(TasksConfigFileName);
        }

        public static Sample? GetActiveSample()
        {
            var launchConfigFilePath = ResourceLocator.GetVSCodeFilePath(LaunchConfigFileName);
            if (!File.Exists(launchConfigFilePath))
                return null;

            var match = new Regex("\\\"preLaunchTask\\\": \\\"(?<Name>.*): Build\\\"").Match(
                File.ReadAllText(launchConfigFilePath));

            if (!match.Success)
                return null;

            return SamplesProvider.GetSampleByName(match.Groups["Name"].Value);
        }

        private static string GetTemplateDirectory()
        {
            var directory = Path.GetFullPath(
                Path.Combine(Path.GetDirectoryName(typeof(ActiveSampleService).Assembly.Location) ?? throw new Exception(),
                "../../../Template"));

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Template directory was not found: " + directory);

            return directory;
        }

        private static string GetTemplateFilePath(string fileName)
        {
            var filePath = Path.Combine(GetTemplateDirectory(), fileName);

            if (!File.Exists(filePath))
                throw new DirectoryNotFoundException("Template file was not found: " + filePath);

            return filePath;
        }
    }
}