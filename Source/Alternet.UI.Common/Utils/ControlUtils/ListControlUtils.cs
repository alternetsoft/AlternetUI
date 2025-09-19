using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to list controls
    /// including <see cref="StdListBox"/> and <see cref="ComboBox"/>.
    /// </summary>
    public static class ListControlUtils
    {
        /// <summary>
        /// Gets or sets array of font sizes used for picker controls.
        /// </summary>
        public static FontScalar[] DefaultFontSizesForPicker
            = new FontScalar[] { 8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96 };

        /// <summary>
        /// Adds test child items to the specified <see cref="TreeViewItem"/>.
        /// </summary>
        /// <param name="tree">The tree control item to which test items will be added.</param>
        /// <param name="count">The number of test items to add.</param>
        /// <param name="initAction">The initialize action.</param>
        public static void AddTestItems(
            TreeViewItem tree,
            int count,
            Action<TreeViewItem>? initAction = null)
        {
            try
            {
                tree.Owner?.BeginUpdate();

                void Initialize(TreeViewItem item)
                {
                    item.SvgImage = KnownColorSvgImages.ImgLogo;
                    initAction?.Invoke(item);
                }

                for (int i = 0; i < count; i++)
                {
                    var item = new TreeViewItem();
                    item.Text = "Item " + LogUtils.GenNewId();
                    Initialize(item);
                    for (int j = 0; j < 10; j++)
                    {
                        var childItem = new TreeViewItem();
                        childItem.Text = "Item " + LogUtils.GenNewId();
                        Initialize(childItem);
                        item.Add(childItem);

                        if (i < 10)
                        {
                            for (int k = 0; k < 2; k++)
                            {
                                var childOfChildItem = new TreeViewItem();
                                childOfChildItem.Text = "Item " + LogUtils.GenNewId();
                                Initialize(childOfChildItem);
                                childItem.Add(childOfChildItem);
                            }
                        }
                    }

                    tree.Add(item);
                }
            }
            finally
            {
                tree.Owner?.EndUpdate();
            }
        }

        /// <summary>
        /// Initializes <see cref="ComboBox"/> with list of font names.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with font names.</param>
        /// <param name="defaultName">Select this font name in <see cref="ComboBox"/>.
        /// If its <c>null</c>, name of the default font is used.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        public static void AddFontNames(
            ComboBox control,
            bool select = true,
            string? defaultName = null)
        {
            var families = FontFamily.FamiliesNamesAscending;
            foreach (var family in families)
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
        /// Initializes <see cref="ComboBox"/> with list of known color names
        /// (Web and Standard colors are used).
        /// </summary>
        /// <param name="control">Control instance which items will be filled with color names.</param>
        /// <param name="defaultValue">Select this color name in <see cref="ComboBox"/>.</param>
        /// <param name="select">Specifies whether to select default item in the control.</param>
        /// <param name="onlyVisible">Whether to process only
        /// colors which are visible to the end-user. Optional. Default is True.</param>
        public static void AddColorNames(
            ComboBox control,
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
        /// Initializes <see cref="IListControl"/> with list of known colors.
        /// </summary>
        /// <param name="control">Control instance which items will be filled with colors.</param>
        /// <param name="defaultValue">Select this color in <see cref="IListControl"/>.</param>
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
        /// Adds font names to the specified collection.
        /// </summary>
        /// <param name="items"></param>
        public static void AddFontNames(BaseCollection<ListControlItem> items)
        {
            var families = FontFamily.FamiliesNamesAscending;
            ListControlItem.AddRangeOfValues(items, families);
        }

        /// <summary>
        /// Initializes collection of <see cref="ListControlItem"/> with list of font sizes.
        /// </summary>
        /// <param name="items">The collection of list control items
        /// to which font sizes will be added.</param>
        /// <param name="defaultSize">Optional. The additional size to add to the collection.</param>
        public static void AddFontSizes(
            BaseCollection<ListControlItem> items,
            Coord? defaultSize = null)
        {
            var fontSizes = new List<Coord>();
            fontSizes.AddRange(DefaultFontSizesForPicker);
            var fontSize = defaultSize ?? AbstractControl.DefaultFont.SizeInPoints;

            void AddAdditionalSize(Coord value)
            {
                if (!fontSizes!.Exists(x => x == value))
                {
                    fontSizes.Add(value);
                    fontSizes.Sort();
                }
            }

            ListControlItem.AddRangeOfValues(items, fontSizes);
            AddAdditionalSize(fontSize);
        }
    }
}
