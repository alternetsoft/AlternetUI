namespace SampleManagement.Common
{
    public static class SamplesProvider
    {
        public static bool SampleExists(string name)
        {
            return TryGetSampleByName(name) != null;
        }

        public static Sample GetSampleByName(string name)
        {
            return TryGetSampleByName(name) ?? throw new ArgumentException("Sample was not found: " + name, nameof(name));
        }

        static Sample? TryGetSampleByName(string name)
        {
            return AllSamples.FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Sample> AllSamples => Directory.GetDirectories(ResourceLocator.SamplesDirectory, "*Sample").Select(x => new Sample(x)).OrderBy(x => x.Name);
    }
}