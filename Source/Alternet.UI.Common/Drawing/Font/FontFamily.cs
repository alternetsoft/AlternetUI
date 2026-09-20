using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Alternet.Skia;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a group of type faces having a similar basic design and
    /// certain variations in styles.
    /// </summary>
    public partial class FontFamily : BaseObject
    {
        private static readonly object syncRoot = new();
        private static BaseDictionary<string, FontFamily>? items;
        private static List<string>? namesAscending;

        private readonly string name;

        private bool? isOk;
        private SKTypeface? typeface;
        private bool? isFixedPitch;
        private static FontFamily? genericSansSerif;
        private static FontFamily? genericSerif;
        private static FontFamily? skiaDefault;

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the new <see cref="FontFamily"/>.</param>
        public FontFamily(string? name)
            : this(name, validate: true)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> with the specified <see cref="SKTypeface"/>.
        /// </summary>
        /// <param name="typeface">The <see cref="SKTypeface"/> for the new <see cref="FontFamily"/>.</param>
        public FontFamily(SKTypeface typeface)
        {
            name = typeface.FamilyName;
            SkiaTypeface = typeface;
            Items.Add(name, this);
        }

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the new <see cref="FontFamily"/>.</param>
        /// <param name="validate">Whether to perform validation of the specified
        /// <paramref name="name"/> parameter.</param>
        public FontFamily(string? name, bool validate)
        {
            if (validate)
            {
                if (string.IsNullOrEmpty(name))
                {
                    App.LogError("Font name cannot be empty, using default font.");
                    this.name = Font.Default.Name;
                    return;
                }

                if (!IsFamilyValid(name!))
                {
                    App.LogError(
                        $"'{name}' font family is not installed on this computer, using default font.");
                    this.name = Font.Default.Name;
                    return;
                }
            }

            this.name = name ?? Font.Default.Name;
        }

        /// <summary>
        /// Gets the default <see cref="FontFamily"/> used in the application.
        /// If the default font is not initialized, it returns the <see cref="SkiaDefault"/> font family.
        /// </summary>
        public static FontFamily Default
        {
            get
            {
                if (Font.IsDefaultFontInitialized)
                    return Font.Default.FontFamily;
                return SkiaDefault;
            }
        }

        /// <summary>
        /// Gets the <see cref="FontFamily"/> created from <see cref="SkiaHelper.DefaultTypeFace"/>.
        /// </summary>
        public static FontFamily SkiaDefault
        {
            get
            {
                return skiaDefault ??= new FontFamily(SkiaHelper.DefaultTypeFace);
            }
        }

        /// <summary>
        /// Returns an array that contains all the <see cref="FontFamily"/>
        /// objects currently available in the system.
        /// </summary>
        /// <value>
        /// An array of <see cref="FontFamily"/> objects currently available
        /// in the system.
        /// </value>
        public static IEnumerable<FontFamily> Families
        {
            get
            {
                return Items.Values;
            }
        }

        /// <summary>
        /// Returns a string array that contains all names of the
        /// <see cref="FontFamily"/>
        /// objects currently available in the system.
        /// </summary>
        /// <value>
        /// A string array of <see cref="FontFamily"/> names currently available
        /// in the system.
        /// </value>
        public static IEnumerable<string> FamiliesNames
        {
            get
            {
                return Items.Keys;
            }

            set
            {
                Reset();

                items = new();
                foreach (var name in value)
                {
                    if (name is null)
                        continue;
                    if (FontFactory.OnlySkiaFonts)
                    {
                        if (!SkiaHelper.IsFamilySkia(name))
                            continue;
                    }

                    items.TryAdd(name, new FontFamily(name, false));
                }
            }
        }

        /// <summary>
        /// Gets a generic serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic serif font.</value>
        public static FontFamily GenericSerif
        {
            get => genericSerif ??= MatchFamily("serif");
        }

        /// <summary>
        /// Gets a generic sans serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic
        /// sans serif font.</value>
        public static FontFamily GenericSansSerif
        {
            get => genericSansSerif ??= MatchFamily("sans-serif");
        }

        /// <summary>
        /// Gets a generic default <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic default
        /// font.</value>
        public static FontFamily GenericDefault
        {
            get => Font.Default.FontFamily;
        }

        /// <summary>
        /// Gets a generic monospace <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic
        /// monospace font.</value>
        public static FontFamily GenericMonospace
        {
            get => Font.DefaultMono.FontFamily;
        }

        /// <summary>
        /// Returns a string array that contains all names of the
        /// <see cref="FontFamily"/>
        /// objects currently available in the system. Names are returned in
        /// the ascending order.
        /// </summary>
        /// <value>
        /// A string array of <see cref="FontFamily"/> names currently available
        /// in the system in the ascending order.
        /// </value>
        public static IEnumerable<string> FamiliesNamesAscending
        {
            get
            {
                if (namesAscending is null)
                {
                    var names = FamiliesNames;
                    namesAscending = new();
                    namesAscending.AddRange(names);
                    namesAscending.Sort();
                }

                return namesAscending;
            }
        }

        /// <summary>
        /// Gets whether this font family has fixed pitch fonts.
        /// </summary>
        public virtual bool IsFixedPitch
        {
            get
            {
                return isFixedPitch ??= SkiaTypeface.IsFixedPitch;
            }
        }

        /// <summary>
        /// Gets <see cref="SKTypeface"/> for this object.
        /// </summary>
        public virtual SKTypeface SkiaTypeface
        {
            get
            {
                return typeface ??= SKFontManager.Default.MatchFamily(Name);
            }

            internal set
            {
                typeface = value;
            }
        }

        /// <summary>
        /// Gets the name of this <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A string that represents the name of this
        /// <see cref="FontFamily"/>.</value>
        public virtual string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// Gets whether this font family is valid.
        /// </summary>
        public virtual bool IsOk
        {
            get
            {
                return isOk ??= IsFamilyValid(Name);
            }
        }

        /// <summary>
        /// Gets generic font family type.
        /// </summary>
        public virtual GenericFontFamily? GenericFamily { get; }

        private static BaseDictionary<string, FontFamily> Items
        {
            get
            {
                if (items is null)
                {
                    lock (syncRoot)
                    {
                        if (items is null)
                        {
                            FamiliesNames = App.Handler.GetFontFamiliesNames();
                        }
                    }
                }

                return items!;
            }
        }

        /// <summary>
        /// Resets all loaded font families.
        /// </summary>
        public static void Reset()
        {
            items = null;
            namesAscending = null;
        }

        /// <summary>
        /// Gets whether font family is installed on this computer.
        /// </summary>
        public static bool IsFamilyValid(string name)
        {
            if (name == Font.DefaultFontName)
                return true;

            if (name == Font.DefaultMonoFontName)
                return true;

            var result = Items.ContainsKey(name);

            if (result)
                return true;

            return false;
        }

        /// <summary>
        /// Creates a new <see cref="FontFamily"/> from the specified <see cref="SKTypeface"/>.
        /// </summary>
        /// <param name="typeface">The SKTypeface to create the FontFamily from.</param>
        /// <returns>A new FontFamily instance.</returns>
        public static FontFamily FromSkia(SKTypeface typeface)
        {
            return new(typeface);
        }

        /// <summary>
        /// Searches for a <see cref="FontFamily"/> with the specified name or returns the default font family
        /// if the name is null or invalid.
        /// </summary>
        /// <param name="name">The name of the font family.</param>
        /// <returns>A new FontFamily instance or the default font family.</returns>
        public static FontFamily FromNameOrDefault(string? name)
        {
            if (name is null || name.Length == 0)
                return Default;
            var result = FromName(name);
            if (result is null)
                return Default;
            return result;
        }

        /// <summary>
        /// Searches for a <see cref="FontFamily"/> with the specified name or returns null
        /// if the name is null or invalid.
        /// </summary>
        /// <param name="name">The name of the font family.</param>
        /// <returns>A new FontFamily instance or null.</returns>
        public static FontFamily? FromName(string? name)
        {
            if (name is null)
                return null;
            if (Items.TryGetValue(name, out var result))
                return result;
            return null;
        }

        /// <summary>
        /// Creates a new <see cref="FontFamily"/> from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path to create the FontFamily from.</param>
        /// <returns>A new FontFamily instance.</returns>
        public static FontFamily FromFile(string filePath)
        {
            var typeface = SKTypeface.FromFile(filePath);
            return FromSkia(typeface);
        }

        /// <summary>
        /// Creates a new <see cref="FontFamily"/> from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to create the FontFamily from.</param>
        /// <returns>A new FontFamily instance.</returns>
        public static FontFamily FromStream(Stream stream)
        {
            var typeface = SKTypeface.FromStream(stream);
            return FromSkia(typeface);
        }

        /// <summary>
        /// Filters fonts and returns only compatible with SKiaSharp.
        /// </summary>
        /// <param name="fonts">Collection of the fonts.</param>
        /// <returns>A collection of font names that are compatible with SKiaSharp.</returns>
        public static IEnumerable<string> RemoveNonSkiaFonts(IEnumerable<string> fonts)
        {
            var result = fonts.Where(x => SkiaHelper.IsFamilySkia(x));
            return result;
        }

        /// <summary>
        /// Gets whether specified font family has only fixed pitch fonts.
        /// </summary>
        /// <param name="name">Font family name</param>
        /// <returns></returns>
        public static bool IsFixedPitchFontFamily(string name)
        {
            if (FontFactory.OnlySkiaFonts)
            {
                var family = SKFontManager.Default.MatchFamily(name);
                return family.IsFixedPitch;
            }
            else
            {
                throw new NotImplementedException("Non-Skia fonts are not supported.");
            }
        }

        /// <summary>
        /// Returns a <see cref="FontFamily"/> instance that matches the specified font family name.
        /// This method uses SkiaSharp's font matching capabilities to find the best match for the given name.
        /// If no match is found, it falls back to the default font family.
        /// </summary>
        /// <param name="name">The name of the font family to search for.</param>
        /// <returns>A <see cref="FontFamily"/> instance.</returns>
        public static FontFamily MatchFamily(string name)
        {
            var typeface = SKFontManager.Default.MatchFamily(name);

            if (typeface == null)
            {
                return Default;
            }

            return FromSkia(typeface);
        }
    }
}