using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods related to list controls.
    /// </summary>
    public static class ListControlUtils
    {
        /// <summary>
        /// Gets or sets array of font sizes used for picker controls.
        /// </summary>
        public static FontScalar[] DefaultFontSizesForPicker
            = new FontScalar[] { 8, 9, 10, 11, 12, 14, 18, 24, 30, 36, 48, 60, 72, 96 };

        /// <summary>
        /// Adds a specified number of test items to the provided tree view and initializes two columns, 'Name' and
        /// 'Data', for each item.
        /// </summary>
        /// <remarks>This method clears any existing columns in the tree view and creates two new columns:
        /// 'Name' and 'Data'. Each test item is assigned a unique identifier and may include child items. Updates to
        /// the tree view are batched for improved performance.</remarks>
        /// <param name="owner">The tree view control that manages the columns and items to which test items will be added.</param>
        /// <param name="count">The number of test items to create and add to the tree view.</param>
        public static void SetTestItemsWithColumns(StdTreeView owner, int count)
        {
            TreeViewRootItem root = new ();
            owner.Header.Visible = true;

            try
            {
                owner.BeginUpdate();

                owner.Clear();
                owner.Columns.Clear();
                var nameColumn = owner.AddColumn("Name", 200);
                var dataColumn = owner.AddColumn("Data", 100);
                var infoColumn = owner.AddColumn("Info", 150);

                owner.Header.Visible = true;

                void Initialize(TreeViewItem item)
                {
                    var textCell = item.SafeCell(nameColumn);
                    textCell.Text = item.Text;
                    textCell.SvgImage = KnownColorSvgImages.ImgLogo;
                    var dataCell = item.SafeCell(dataColumn);
                    dataCell.Text = "Data " + LogUtils.GenNewId();
                    dataCell.HorizontalAlignment = HorizontalAlignment.Right;
                    var infoCell = item.SafeCell(infoColumn);
                    infoCell.Text = "Info " + LogUtils.GenNewId();
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

                    root.Add(item);
                }

                owner.RootItem = root;
            }
            finally
            {
                owner.EndUpdate();
            }
        }

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
                ListControlItem controlItem = new(c.ToDisplayString(), c);
                control.Add(controlItem);
            }

            if (select && defaultValue is not null)
            {
                var found = control.FindStringExact(defaultValue.ToDisplayString());
                if (found != null)
                    control.SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Adds font names to the specified collection.
        /// </summary>
        /// <param name="items"></param>
        public static void AddFontNames(IListSource<ListControlItem> items)
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
            IListSource<ListControlItem> items,
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
