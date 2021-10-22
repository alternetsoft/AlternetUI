namespace Alternet.UI.Versioning
{
    public class ProductVersion
    {
        public ProductVersion(int major, int minor, VersionType type)
        {
            Major = major;
            Minor = minor;
            Type = type;
        }

        public int Major { get; }
        public int Minor { get; }
        public VersionType Type { get; }
    }
}