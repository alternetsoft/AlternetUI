using System.Collections.Generic;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace XRefMaps
{
    /// <summary>
    /// Represents a reference item on a <see cref="XrefMap"/>.
    /// </summary>
    public sealed partial class XrefMapReference
    {
        /// <summary>
        /// The online API documentation of Unity doesn't show some namespaces.
        /// </summary>
        private static readonly List<string> HrefNamespacesToTrim = new() { "UnityEditor", "UnityEngine" };

        [GeneratedRegex("`{2}\\d")]
        private static partial Regex GenericHrefRegex();

        [GeneratedRegex("\\*$")]
        private static partial Regex MethodHrefPointerRegex();

        [GeneratedRegex("\\(.*\\)")]
        private static partial Regex MethodHrefRegex();

        [GeneratedRegex("\\.([a-z].*)$")]
        private static partial Regex PropertyHrefRegex();

        public string uid { get; set; }

        public string name { get; set; }

        [YamlMember(Alias = "name.vb")]
        public string nameVb { get; set; }

        public string href { get; set; }

        public string commentId { get; set; }

        public string isSpec { get; set; }

        public string fullName { get; set; }

        [YamlMember(Alias = "fullName.vb")]
        public string fullNameVb { get; set; }

        public string nameWithType { get; set; }

        [YamlMember(Alias = "nameWithType.vb")]
        public string nameWithTypeVb { get; set; }

        /// <summary>
        /// Gets if this <see cref="XrefMapReference"/> is valid or not.
        /// </summary>
        public bool IsValid => !commentId.Contains("Overload:");

        /// <summary>
        /// Sets <see cref="href"/> to link to the online API documentation of Unity.
        /// </summary>
        /// <param name="apiUrl">The URL of the online API documentation of Unity.</param>
        public void FixHref(string apiUrl)
        {
            // Namespaces point to documentation index
            if (commentId.Contains("N:"))
            {
                href = "index";
            }
            else
            {
                href = uid;

                // Trim UnityEngine and UnityEditor namespaces from href
                foreach (var hrefNamespaceToTrim in HrefNamespacesToTrim)
                {
                    href = href.Replace(hrefNamespaceToTrim + ".", "");
                }

                // Fix href of constructors
                href = href.Replace(".#ctor", "-ctor");

                // Fix href of generics
                href = GenericHrefRegex().Replace(href, "");
                href = href.Replace("`", "_");

                // Fix href of methods
                href = MethodHrefPointerRegex().Replace(href, "");
                href = MethodHrefRegex().Replace(href, "");

                // Fix href of properties
                if (commentId.Contains("P:") || commentId.Contains("M:"))
                {
                    href = PropertyHrefRegex().Replace(href, "-$1");
                }
            }

            href = apiUrl + href + ".html";
        }
    }
}