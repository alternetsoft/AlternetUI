namespace SampleManagement.Common
{
    public record Sample(string Directory)
    {
        public string Name => Path.GetFileName(Directory);

        public override string ToString()
        {
            return Name;
        }
    }
}