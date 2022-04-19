using System.Reflection;

namespace SampleManagement.Common
{
    public static class ResourceLocator
    {
        public static string RepositoryRoot
        {
            get
            {
                var directory = Path.GetFullPath(
                    Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!,
                    string.Concat(Enumerable.Repeat("../", 8))));

                if (!Directory.Exists(directory))
                    throw new DirectoryNotFoundException("Repository root directory does not exist: " + directory);

                if (!Directory.Exists(Path.Combine(directory, ".git")))
                    throw new DirectoryNotFoundException("Repository root directory does not contain .git directory: " + directory);

                return directory;
            }
        }

        public static string SamplesDirectory
        {
            get
            {
                var directory = Path.Combine(RepositoryRoot, "Source/Samples");

                if (!Directory.Exists(directory))
                    throw new DirectoryNotFoundException("Samples directory does not exist: " + directory);

                return directory;
            }
        }

        public static string VSCodeDirectory
        {
            get
            {
                var directory = Path.Combine(RepositoryRoot, ".vscode");

                if (!Directory.Exists(directory))
                    throw new DirectoryNotFoundException(".vscode directory does not exist: " + directory);

                return directory;
            }
        }

        public static string VSCodeLaunchConfigPath
        {
            get
            {
                var fileName = Path.Combine(VSCodeDirectory, "launch.json");

                if (!File.Exists(fileName))
                    throw new FileNotFoundException("launch.json does not exist: " + fileName);

                return fileName;
            }
        }

        public static string VSCodeTasksConfigPath
        {
            get
            {
                var fileName = Path.Combine(VSCodeDirectory, "tasks.json");

                if (!File.Exists(fileName))
                    throw new FileNotFoundException("tasks.json does not exist: " + fileName);

                return fileName;
            }
        }
    }
}