using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to <see cref="ListControl"/> and its descendants
    /// like <see cref="ListBox"/> and <see cref="ComboBox"/>.
    /// </summary>
    public static class ListControlUtils
    {
        /// <summary>
        /// Initializes <see cref="ListControl"/> with list of font names.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with font names.</param>
        /// <param name="defaultName">Select this font name in <see cref="ListControl"/>.
        /// If its <c>null</c>, name of the default font is used.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        public static void AddFontNames(
            IListControl control,
            bool select = true,
            string? defaultName = null)
        {
            var families = FontFamily.FamiliesNamesAscending;
            foreach(var family in families)
                control.Add(new ListControlItem(family));
            defaultName ??= AbstractControl.DefaultFont.Name;
            if (select)
            {
                var found = control.FindStringExact(defaultName);
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Initializes <see cref="ListControl"/> with list of known color names
        /// (Web and Standard colors are used).
        /// </summary>
        /// <param name="control">Control instance which items will be filled with color names.</param>
        /// <param name="defaultValue">Select this color name in <see cref="ListControl"/>.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        /// <param name="onlyVisible">Whether to process only
        /// colors which are visible to the end-user. Optional. Default is True.</param>
        public static void AddColorNames(
            ListControl control,
            bool select = true,
            Color? defaultValue = null,
            bool onlyVisible = true)
        {
            var knownColors =
                Color.GetKnownColors(
                    [KnownColorCategory.Standard, KnownColorCategory.Web],
                    onlyVisible);

            var colorsNames = new List<string>();
            colorsNames.AddRange(knownColors.Select(x => x.Name));

            var name = defaultValue?.Name;

            if (name is not null && !colorsNames.Exists(x => x == name))
            {
                colorsNames.Add(name);
                colorsNames.Sort();
            }

            control.Items.AddRange(colorsNames);
            if (select && name is not null)
            {
                var found = control.FindStringExact(name);
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Initializes <see cref="ListControl"/> with list of known colors.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with colors.</param>
        /// <param name="defaultValue">Select this color in <see cref="ListControl"/>.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        /// <param name="cats">Array of categories to add colors from. Optional. If not specified,
        /// standard and web colors will be added.</param>
        /// <param name="onlyVisible">Whether to process only
        /// colors which are visible to the end-user. Optional. Default is True.</param>
        public static void AddColors(
            IListControl control,
            bool select = true,
            Color? defaultValue = null,
            KnownColorCategory[]? cats = null,
            bool onlyVisible = true)
        {
            cats ??= [KnownColorCategory.Standard, KnownColorCategory.Web];

            var knownColors = Color.GetKnownColors(cats, onlyVisible);

            foreach (var item in knownColors)
            {
                AddColor(item);
            }

            if (defaultValue is not null && !knownColors.Contains(defaultValue))
                AddColor(defaultValue);

            void AddColor(Color c)
            {
                ListControlItem controlItem = new(c.NameLocalized, c);
                control.Add(controlItem);
            }

            if (select && defaultValue is not null)
            {
                var found = control.FindStringExact(defaultValue.NameLocalized);
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Initializes <see cref="ListControl"/> with list of font sizes.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with font sizes.</param>
        /// <param name="defaultSize">Select this font size in <see cref="ListControl"/>.
        /// If its <c>null</c>, size of the default font is used.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        public static void AddFontSizes(
            ListControl control,
            bool select = true,
            Coord? defaultSize = null)
        {
            var fontSizes = new List<Coord>();
            fontSizes.AddRange(
                new double[] { 8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96 });
            var fontSize = defaultSize ?? AbstractControl.DefaultFont.SizeInPoints;

            void AddAdditionalSize(Coord value)
            {
                if (!fontSizes!.Exists(x => x == value))
                {
                    fontSizes.Add(value);
                    fontSizes.Sort();
                }
            }

            AddAdditionalSize(fontSize);
            control.Items.AddRange(fontSizes.Cast<object>());
            if (select)
            {
                var found = control.FindStringExact(fontSize.ToString());
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }
    }
}
