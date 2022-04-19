using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SampleAdder
{
    internal static class SampleLocator
    {
        public static string GetSamplesDirectory()
        {
            var directory = Path.GetFullPath(
                Path.Combine(Path.GetDirectoryName(typeof(SampleValidator).Assembly.Location) ?? throw new Exception(),
                "../../../../../../Samples"));

            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException("Samples directory was not found: " + directory);

            return directory;
        }

        public static bool SampleExists(string name)
        {
            return GetAllSamples().Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        private static IEnumerable<string> GetAllSamples()
        {
            return Directory.GetDirectories(GetSamplesDirectory(), "*Sample").Select(x => Path.GetFileName(x) ?? throw new Exception());
        }
    }
}