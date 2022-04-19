namespace SampleManagement.Common
{
    public record Sample
    {
        public string Directory { get; }

        public Sample(string directory)
        {
            Directory = Path.GetFullPath(directory).Replace('\\', '/');
        }

        public string Name => Path.GetFileName(Directory);

        public override string ToString()
        {
            return Name;
        }
    }
}