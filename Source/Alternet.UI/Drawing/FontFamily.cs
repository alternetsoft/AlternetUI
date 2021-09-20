using System;
using System.Linq;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a group of type faces having a similar basic design and certain variations in styles.
    /// </summary>
    public sealed class FontFamily
    {
        private string? name;

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the new <see cref="FontFamily"/>.</param>
        /// <exception cref="ArgumentException"><c>name</c> is an empty string ("").</exception>
        /// <exception cref="ArgumentException"><c>name</c> specifies a font that is not installed on the computer running the application.</exception>
        public FontFamily(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            if (name == string.Empty)
                throw new ArgumentException($"'{nameof(name)}' cannot be empty.", nameof(name));

            if (!UI.Native.Font.IsFamilyValid(name))
                throw new ArgumentException($"'{name}' font family is not installed on this computer.", nameof(name));

            this.name = name;
        }

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> from the specified generic font family.
        /// </summary>
        /// <param name="genericFamily">The <see cref="GenericFontFamily"/> from which to create the new <see cref="FontFamily"/>.</param>
        public FontFamily(GenericFontFamily genericFamily)
        {
            GenericFamily = genericFamily;
        }

        /// <summary>
        /// Gets a generic serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic serif font.</value>
        public static FontFamily GenericSerif { get; } = new FontFamily(GenericFontFamily.Serif);

        /// <summary>
        /// Gets a generic sans serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic sans serif font.</value>
        public static FontFamily GenericSansSerif { get; } = new FontFamily(GenericFontFamily.SansSerif);

        /// <summary>
        /// Gets a generic monospace <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic monospace font.</value>
        public static FontFamily GenericMonospace { get; } = new FontFamily(GenericFontFamily.Monospace);

        /// <summary>
        /// Returns an array that contains all the <see cref="FontFamily"/> objects currently available in the system.
        /// </summary>
        /// <value>
        /// An array of <see cref="FontFamily"/> objects currently available in the system.
        /// </value>
        public static FontFamily[] Families => UI.Native.Font.Families.Select(x => new FontFamily(x)).ToArray();

        /// <summary>
        /// Gets the name of this <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A string that represents the name of this <see cref="FontFamily"/>.</value>
        public string Name => name ??= UI.Native.Font.GetGenericFamilyName((UI.Native.GenericFontFamily)(GenericFamily ?? throw new Exception()));

        internal GenericFontFamily? GenericFamily { get; }
    }
}