namespace SampleManagement.Common
{
    public record ProjectConfiguration(string Name)
    {
        public override string ToString()
        {
            return Name;
        }

        public static ProjectConfiguration ClrDebug { get; } = new ProjectConfiguration("CLR Debug");
        public static ProjectConfiguration NativeDebug { get; } = new ProjectConfiguration("Native Debug");

        public static ProjectConfiguration[] All { get; } = new[] { ClrDebug, NativeDebug };
    }
}