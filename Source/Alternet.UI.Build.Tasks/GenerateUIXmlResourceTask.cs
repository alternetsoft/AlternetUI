using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;

namespace Alternet.UI.Build.Tasks
{
    public class GenerateUIXmlResourceTask : Task
    {
        private const string LogSubcategory = "Alternet.UI.UIXml.ResourceGeneration";

        [Required]
        public ITaskItem[] InputFiles { get; set; } = default!;

        public override bool Execute()
        {
            foreach (var inputFile in InputFiles)
            {
                try
                {
                    TranslateFile(inputFile);
                }
                catch (Exception ex)
                {
                    LogError(inputFile, $"Error generating file: {ex}");
                }
            }

            return !Log.HasLoggedErrors;
        }

        private static string GetValidTargetFilePath(ITaskItem inputFile)
        {
            var targetPath = inputFile.GetMetadata("ResourceGeneratorTargetPath") ?? throw new InvalidOperationException("No target path specified");
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath) ?? throw new InvalidOperationException("Invalid target directory"));
            return targetPath;
        }

        private void TranslateFile(ITaskItem inputFile)
        {
            //var defaultNamespace = inputFile.GetMetadata("CustomToolNamespace")?.Trim() ?? string.Empty;

            GenerateAssemblyOutput(inputFile);
        }

        private void GenerateAssemblyOutput(ITaskItem inputFile)
        {
            var targetPath = GetValidTargetFilePath(inputFile);

            using var stream = File.OpenRead(inputFile.ItemSpec);
            var document = new UIXmlDocument(
                inputFile.GetMetadata("ResourceName") ?? throw new InvalidOperationException("No resource name specified"),
                stream,
                WellKnownApiInfo.Provider);

            document.SanitizedDocument.Save(targetPath);

            LogDebug($"{inputFile.ItemSpec}: Generated resource.");
        }

        private void LogDebug(string message) =>
            Log.LogMessage(LogSubcategory, null, null, null, 0, 0, 0, 0, MessageImportance.Low, message, null);

        private void LogError(ITaskItem? inputFile, string message, int lineNumber = 0, int columnNumber = 0) =>
            Log.LogError(LogSubcategory, null, null, inputFile?.ItemSpec, lineNumber, columnNumber, 0, 0, message, null);
    }
}