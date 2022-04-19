using System;
using System.IO;

namespace SampleAdder
{
    internal static class SampleValidator
    {
        public static (bool Ok, string Message) ValidateSampleName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return (false, "The name cannot be empty or consist only of whitespace.");

            bool validFileName = name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
            if (!validFileName)
                return (false, "The name contains invalid file name characters.");

            if (SampleLocator.SampleExists(name))
                return (false, "Sample with this name already exists.");

            if (!name.EndsWith("Sample", StringComparison.OrdinalIgnoreCase))
                return (false, "The name must end with word \"Sample\".");

            if (name.Equals("Sample", StringComparison.OrdinalIgnoreCase))
                return (false, "The name must not me equal to \"Sample\".");

            if (!char.IsUpper(name[0]))
                return (false, "The name must start with an uppercase character.");

            return (true, "");
        }
    }
}