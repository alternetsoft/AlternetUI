using System;
using System.Collections.Generic;
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
    public class FontFamily : BaseObject
    {
        private static readonly string?[] GenericFamilyNames
            = new string?[(int)GenericFontFamily.Default + 1];

        private static FontFamily? genericSerif;
        private static FontFamily? genericDefault;
        private static FontFamily? genericSansSerif;
        private static FontFamily? genericMonospace;

        private static BaseDictionary<string, FontFamily>? items;
        private static List<string>? namesAscending;

        private string? name;
        private bool? isOk;
        private SKTypeface? typeface;
        private bool? isFixedPitch;

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the new <see cref="FontFamily"/>.</param>
        public FontFamily(string? name)
            : this(name, true)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="FontFamily"/> from the specified
        /// generic font family.
        /// </summary>
        /// <param name="genericFamily">The <see cref="GenericFontFamily"/>
        /// from which to create the new <see cref="FontFamily"/>.</param>
        public FontFamily(GenericFontFamily genericFamily)
        {
            if (genericFamily == GenericFontFamily.None)
                genericFamily = GenericFontFamily.Default;
            GenericFamily = genericFamily;
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
                    GenericFamily = GenericFontFamily.Default;
                    return;
                }

                if (!IsFamilyValid(name!))
                {
                    App.LogError(
                        $"'{name}' font family is not installed on this computer, using default font.");
                    GenericFamily = GenericFontFamily.Default;
                    return;
                }
            }

            this.name = name;
        }

        /// <summary>
        /// Gets a generic serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic serif font.</value>
        public static FontFamily GenericSerif
        {
            get => genericSerif ??= new FontFamily(GenericFontFamily.Serif);
        }

        /// <summary>
        /// Gets a generic default <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic default
        /// font.</value>
        public static FontFamily GenericDefault
        {
            get => genericDefault ??= new FontFamily(GenericFontFamily.Default);
        }

        /// <summary>
        /// Gets a generic sans serif <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic
        /// sans serif font.</value>
        public static FontFamily GenericSansSerif
        {
            get => genericSansSerif ??= new FontFamily(GenericFontFamily.SansSerif);
        }

        /// <summary>
        /// Gets a generic monospace <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A <see cref="FontFamily"/> that represents a generic
        /// monospace font.</value>
        public static FontFamily GenericMonospace
        {
            get => genericMonospace ??= new FontFamily(GenericFontFamily.Monospace);
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
                return isFixedPitch ??= IsFixedPitchFontFamily(Name);
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
                return name ??= GetName(GenericFamily);
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
                    FamiliesNames = FontFactory.Handler.GetFontFamiliesNames();

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
            return Items.ContainsKey(name);
        }

        /// <summary>
        /// Gets <see cref="FontFamily"/> for the specified <see cref="GenericFontFamily"/> enumeration.
        /// </summary>
        public static FontFamily GetFamily(GenericFontFamily? family)
        {
            family ??= GenericFontFamily.Default;
            switch (family.Value)
            {
                case GenericFontFamily.None:
                case GenericFontFamily.Default:
                default:
                    return GenericDefault;
                case GenericFontFamily.SansSerif:
                    return GenericSansSerif;
                case GenericFontFamily.Serif:
                    return GenericSerif;
                case GenericFontFamily.Monospace:
                    return GenericMonospace;
            }
        }

        /// <summary>
        /// Filters fonts and returns only compatible with SKiaSharp.
        /// </summary>
        /// <param name="fonts">Collection of the fonts.</param>
        /// <returns></returns>
        public static IEnumerable<string> RemoveNonSkiaFonts(IEnumerable<string> fonts)
        {
            var result = fonts.Where(x => SkiaHelper.IsFamilySkia(x));
            return result;
        }

        /// <summary>
        /// Gets name of the font family specified with <see cref="GenericFontFamily"/> enum.
        /// </summary>
        /// <param name="family">Font family.</param>
        /// <returns></returns>
        public static string GetName(GenericFontFamily? family)
        {
            if (family is null || family == GenericFontFamily.None)
                family = GenericFontFamily.Default;

            var savedResult = GenericFamilyNames[(int)family];
            if (savedResult is not null)
                return savedResult;

            var result = FontFactory.Handler.GetFontFamilyName(family.Value);

            GenericFamilyNames[(int)family] = result;

            return result;
        }

        /// <summary>
        /// Sets name of the font family specified with <see cref="GenericFontFamily"/> enum.
        /// </summary>
        /// <param name="genericFamily">Font family.</param>
        /// <param name="name">New name.</param>
        public static void SetFontFamilyName(GenericFontFamily genericFamily, string? name)
        {
            GenericFamilyNames[(int)genericFamily] = name;
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
                throw new NotImplementedException();
            }
        }
    }
}