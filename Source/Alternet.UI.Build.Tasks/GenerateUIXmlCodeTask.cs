using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using System.Xml;

namespace Alternet.UI.Build.Tasks
{
    public class GenerateUIXmlCodeTask : Task
    {
        internal static readonly string MyLogFilePath =
            Path.ChangeExtension(typeof(GenerateUIXmlCodeTask).Assembly.Location, ".log");

        private const string LogSubcategory = "Alternet.UI.UIXml.CodeGeneration";

#pragma warning disable
        private static readonly string[] StringSplitToArrayChars =
        {
            Environment.NewLine,
        };
#pragma warning restore

        public static void LogToFile(string s)
        {
            string[] result = s.Split(StringSplitToArrayChars, StringSplitOptions.None);

            string contents = string.Empty;

            foreach (string s2 in result)
                contents += $"{s2}{Environment.NewLine}";
            File.AppendAllText(@"e:/a.log", contents);
        }

        [Output]
        public bool HasErrors { get; set; }

        [Required]
        public ITaskItem[] InputFiles { get; set; } = default!;

        public override bool Execute()
        {
            // string xmlPath = WellKnownApiInfo.GetXmlPath();
            // bool xmlPathExists = File.Exists(xmlPath);
            // LogToFile($"===> XmlPath({xmlPathExists}): {xmlPath}");

            try
            {
                foreach (var inputFile in InputFiles)
                {
                    try
                    {
                        TranslateFile(inputFile);
                    }
                    catch (Exception ex)
                    {
                        if (ex is XmlException xmlException)
                        {
                            var lineNumber = xmlException.LineNumber;
                            var linePos = xmlException.LinePosition;
                            LogError(inputFile, $"Error generating file: {ex}", lineNumber, linePos);
                        }
                        else
                            LogError(inputFile, $"Error generating file: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
               LogError(null, $"Code generation error: {ex}");
            }

            HasErrors = Log.HasLoggedErrors;

            return !Log.HasLoggedErrors;
        }

        private static string GetValidTargetFilePath(ITaskItem inputFile)
        {
            var targetPath = inputFile.GetMetadata("CodeGeneratorTargetPath") ?? throw new InvalidOperationException("No target path specified");
            Directory.CreateDirectory(Path.GetDirectoryName(targetPath) ?? throw new InvalidOperationException("Invalid target directory"));
            return targetPath;
        }

        private void TranslateFile(ITaskItem inputFile)
        {
            GenerateCSharpOutput(inputFile);
        }

        private void GenerateCSharpOutput(ITaskItem inputFile)
        {
            var targetPath = GetValidTargetFilePath(inputFile);

            File.Delete(targetPath);

            using var stream = File.OpenRead(inputFile.ItemSpec);

            var metadata = inputFile.GetMetadata("ResourceName")
                ?? throw new InvalidOperationException("No resource name specified");

            var document = new UIXmlDocument(
                metadata,
                stream,
                WellKnownApiInfo.Provider);

            var output = CSharpUIXmlCodeGenerator.Generate(document, this, inputFile);

            File.WriteAllText(targetPath, output);

            LogDebug($"{inputFile.ItemSpec}: Generated code.");
        }

        public void LogDebug(string message)
        {
            Log.LogMessage(
                LogSubcategory,
                null,
                null,
                null,
                0,
                0,
                0,
                0,
                MessageImportance.Low,
                message, null);
        }

        public void LogDebugHigh(string message)
        {
            Log.LogMessage(
                LogSubcategory, null, null, null, 0, 0, 0, 0, MessageImportance.High, message, null);
        }

        public void LogError(
            ITaskItem? inputFile,
            string message,
            int lineNumber = 0,
            int columnNumber = 0)
        {
            Log.LogError(
                LogSubcategory,
                null,
                null,
                inputFile?.ItemSpec,
                lineNumber,
                columnNumber,
                lineNumber,
                columnNumber,
                message,
                null);
        }
    }
}