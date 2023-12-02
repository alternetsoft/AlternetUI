#nullable disable

using System;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    internal partial class SR
    {
        private static ResourceManager ResourceManager => SRID.ResourceManager;

        public static string GetString(string name, params object[] args)
        {
            string @string = GetString(name/*, Culture*/);

            if (args != null && args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string { Length: > 1024 } text)
                    {
#pragma warning disable
                        args[i] = text.Substring(0, 1021) + "...";
#pragma warning restore
                    }
                }

                return string.Format(CultureInfo.CurrentCulture, @string, args);
            }

            return @string;
        }

        public static string Get(string name)
        {
            return GetResourceString(name, null);
        }

        public static string Get(string name, params object[] args)
        {
            return Format(GetResourceString(name, null), args);
        }

        internal static string GetResourceString(string resourceKey, string defaultString)
        {
            string resourceString = null;
            try
            {
                resourceString = ResourceManager.GetString(resourceKey);
            }
            catch (MissingManifestResourceException)
            {
            }

            if (defaultString != null && resourceKey.Equals(resourceString, StringComparison.Ordinal))
            {
                return defaultString;
            }

            return resourceString;
        }

        internal static string Format(string resourceFormat, params object[] args)
        {
            if (args != null)
            {
                if (UsingResourceKeys())
                {
                    return resourceFormat + string.Join(", ", args);
                }

                return string.Format(resourceFormat, args);
            }

            return resourceFormat;
        }

        internal static string Format(string resourceFormat, object p1)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(resourceFormat, p1);
        }

        internal static string Format(string resourceFormat, object p1, object p2)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(resourceFormat, p1, p2);
        }

        internal static string Format(string resourceFormat, object p1, object p2, object p3)
        {
            if (UsingResourceKeys())
            {
                return string.Join(", ", resourceFormat, p1, p2, p3);
            }

            return string.Format(resourceFormat, p1, p2, p3);
        }

        // This method is used to decide if we need to append the exception message parameters
        // to the message when calling SR.Format.
        // by default it returns false.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool UsingResourceKeys()
        {
            return false;
        }
    }
}