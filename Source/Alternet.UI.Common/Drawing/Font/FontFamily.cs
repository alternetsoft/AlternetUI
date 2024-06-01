using System;
using System.Collections.Generic;
using System.Linq;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Defines a group of type faces having a similar basic design and
    /// certain variations in styles.
    /// </summary>
    public class FontFamily : BaseObject
    {
        private static FontFamily? genericSerif;
        private static FontFamily? genericDefault;
        private static FontFamily? genericSansSerif;
        private static FontFamily? genericMonospace;

        private static AdvDictionary<string, FontFamily>? items;
        private static List<string>? namesAscending;

        private string? name;

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
                    BaseApplication.LogError("Font name cannot be empty, using default font.");
                    GenericFamily = GenericFontFamily.Default;
                    return;
                }

                if (!IsFamilyValid(name))
                {
                    BaseApplication.LogError(
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
                if(namesAscending is null)
                {
                    namesAscending = new();
                    namesAscending.AddRange(FamiliesNames);
                    namesAscending.Sort();
                }

                return namesAscending;
            }
        }

        private static AdvDictionary<string, FontFamily> Items
        {
            get
            {
                if (items is null)
                {
                    items = new();
                    var names = FontFactory.Handler.GetFontFamiliesNames();
                    FamiliesNames = names;
                }

                return items;
            }
        }

        /// <summary>
        /// Gets the name of this <see cref="FontFamily"/>.
        /// </summary>
        /// <value>A string that represents the name of this
        /// <see cref="FontFamily"/>.</value>
        public string Name
        {
            get
            {
                return name ??= GetName(GenericFamily);
            }
        }

        /// <summary>
        /// Gets generic font family type.
        /// </summary>
        public GenericFontFamily? GenericFamily { get; }

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
        /// Gets name of the font family specified with <see cref="GenericFontFamily"/> enum.
        /// </summary>
        /// <param name="family"></param>
        /// <returns></returns>
        public static string GetName(GenericFontFamily? family)
        {
            family ??= GenericFontFamily.Default;
            if (family == GenericFontFamily.None)
                family = GenericFontFamily.Default;
            var result = FontFactory.Handler.GetFontFamilyName(family.Value);
            return result;
        }
    }
}