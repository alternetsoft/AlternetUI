namespace SampleManagement.Common
{
    public static class SamplesProvider
    {
        public static bool SampleExists(string name)
        {
            return AllSamples.Any(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<Sample> AllSamples => Directory.GetDirectories(ResourceLocator.SamplesDirectory, "*Sample").Select(x => new Sample(x));
    }
}