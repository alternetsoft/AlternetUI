using System;
using System.Diagnostics.CodeAnalysis;

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

        public string GetMajorMinorAndBuild(int buildNumber = 0)
        {
            var v = $"{Major}.{Minor}.{buildNumber}.0";
            return v;
        }

        public string GetPackageVersion(int buildNumber = 0)
        {
            var v = $"{Major}.{Minor}.{buildNumber}";
            if (Type == VersionType.Release)
                return v;
            return $"{v}-{Type.ToString().ToLower()}";
        }

        public string GetPackageFloatingReferenceVersion()
        {
            var v = $"{Major}.{Minor}.*";
            if (Type == VersionType.Release)
                return v;
            return $"{v}-{Type.ToString().ToLower()}*";
        }

        public string GetSimpleVersion()
        {
            return $"{Major}.{Minor}";
        }

        public string GetAssemblyVersion() => $"{Major}.{Minor}.0.0";

        public static bool TryParseSimpleVersion(string value, [NotNullWhen(true)] out ProductVersion? version)
        {
            version = null;

            var parts = value.Split(".");
            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0], out var major))
                return false;
            if (!int.TryParse(parts[1], out var minor))
                return false;

            version = new ProductVersion(major, minor, VersionType.Release);
            return true;
        }

        public Version GetAssemblyVersion(int buildNumber)
        {
            var version = Version.Parse(GetAssemblyVersion());
            return new Version(version.Major, version.Minor, buildNumber, 0);
        }

        public ProductVersion WithType(VersionType value) => new(Major, Minor, value);
    }
}