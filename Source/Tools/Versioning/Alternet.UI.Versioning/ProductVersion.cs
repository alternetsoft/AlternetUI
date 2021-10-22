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

        public string GetInformationalVersion() =>
            $"{Major}.{Minor}.0 ({Major}.{Minor} {Type} build 0)";

        public string GetPackageVersion()
        {
            var v = $"{Major}.{Minor}.0";
            if (Type == VersionType.Release)
                return v;
            return $"{v}-{Type.ToString().ToLower()}";
        }

        public string GetSimpleVersion()
        {
            return $"{Major}.{Minor}";
        }

        public string GetAssemblyVersion() => $"{Major}.{Minor}.0.0";
    }
}