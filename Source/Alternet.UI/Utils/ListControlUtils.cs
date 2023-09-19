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
            ListControl control,
            bool select = true,
            string? defaultName = null)
        {
            control.Items.AddRange(FontFamily.FamiliesNamesAscending);
            defaultName ??= Font.Default.Name;
            if (select)
            {
                var found = control.FindStringExact(defaultName);
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Initializes <see cref="ListControl"/> with list of known color names.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with color names.</param>
        /// <param name="defaultValue">Select this color name in <see cref="ListControl"/>.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        public static void AddColorNames(
            ListControl control,
            bool select = true,
            Color? defaultValue = null)
        {
            var knownColors = Color.GetKnownColors();
            var colorsNames = knownColors.Select(x => x.Name).ToArray();

            control.Items.AddRange(colorsNames);
            if (select && defaultValue is not null)
            {
                var found = control.FindStringExact(defaultValue.Value.Name);
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
            double? defaultSize = null)
        {
            var fontSizes = new List<double>();
            fontSizes.AddRange(
                new double[] { 8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96 });
            var fontSize = defaultSize ?? Font.Default.SizeInPoints;

            void AddAdditionalSize(double value)
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
