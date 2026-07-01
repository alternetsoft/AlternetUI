using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a picker control for selecting font names.
    /// </summary>
    /// <remarks>The <see cref="FontNamePicker"/> class provides functionality
    /// to display a list of available
    /// font names  and allows the user to select one.
    /// By default, the control initializes with a predefined set of font names.</remarks>
    public partial class FontNamePicker : ListPicker
    {
        /// <summary>
        /// Gets or sets a value indicating whether the default font should be visible in the list.
        /// This property allows you to control the visibility of the default font option in the font name picker.
        /// After changing this property, all new instances of the
        /// <see cref="FontNamePicker"/> control will reflect the updated visibility
        /// setting for the default font.
        /// </summary>
        public static bool IsDefaultFontNameVisible = true;

        /// <summary>
        /// Gets or sets a value indicating whether the default monospaced font should be visible in the list.
        /// This property allows you to control the visibility of the default monospaced font
        /// option in the font name picker. After changing this property, all new instances of the
        /// <see cref="FontNamePicker"/> control will reflect the updated visibility
        /// setting for the default monospaced font.
        /// </summary>
        public static bool IsDefaultMonoFontNameVisible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontNamePicker"/> class.
        /// </summary>
        public FontNamePicker()
        {
            PopupWindowTitle = CommonStrings.Default.WindowTitleSelectFontName;
        }

        /// <summary>
        /// Sets value of the control to the specified font name.
        /// </summary>
        /// <param name="font">The font to set as the value of the control.</param>
        public virtual FontNamePicker SetValue(Font? font)
        {
            if (font is null)
            {
                Value = null;
                return this;
            }

            if (font.IsDefaultFont)
            {
                var item = FindItemWithAttr("FontOrigin", FontOriginKind.Default);
                if (item != null)
                {
                    Value = item.Value;
                }
            }

            if (font.IsDefaultMonoFont)
            {
                var item = FindItemWithAttr("FontOrigin", FontOriginKind.DefaultMono);
                if (item != null)
                {
                    Value = item.Value;
                }
            }

            Value = font.Name;

            return this;
        }

        /// <summary>
        /// Sets visibility of the item with the default font name in the list.
        /// This is an additional item with that is added to the list of font names
        /// which are received from the system.
        /// </summary>
        /// <param name="visible">A value indicating whether the default font item should be visible.</param>
        public virtual FontNamePicker SetDefaultFontVisible(bool visible = true)
        {
            var item = FindItemWithAttr("FontOrigin", FontOriginKind.Default);
            if (item != null)
            {
                item.IsVisible = visible;
                Invalidate();
            }

            return this;
        }

        /// <summary>
        /// Sets visibility of the item with the default monospaced font name in the list.
        /// This is an additional item with that is added to the list of font names
        /// which are received from the system.
        /// </summary>
        /// <param name="visible">A value indicating whether the default monospaced
        /// font item should be visible.</param>
        public virtual FontNamePicker SetDefaultMonoFontVisible(bool visible = true)
        {
            var item = FindItemWithAttr("FontOrigin", FontOriginKind.DefaultMono);
            if (item != null)
            {
                item.IsVisible = visible;
                Invalidate();
            }

            return this;
        }

        /// <inheritdoc/>
        protected override IListSource<ListControlItem> CreateItems()
        {
            var result = base.CreateItems();

            var item = new ListControlItem();
            item.DisplayText = Font.GetEffectiveFontDisplayName(Font.Default);
            item.Value = Font.Default.Name;
            item.CustomAttr["FontOrigin"] = FontOriginKind.Default;
            item.IsVisible = IsDefaultFontNameVisible;
            result.Add(item);

            item = new ListControlItem();
            item.DisplayText = Font.GetEffectiveFontDisplayName(Font.DefaultMono);
            item.Value = Font.DefaultMono.Name;
            item.CustomAttr["FontOrigin"] = FontOriginKind.DefaultMono;
            item.IsVisible = IsDefaultMonoFontNameVisible;
            result.Add(item);

            ListControlUtils.AddFontNames(result);
            return result;
        }
    }
}
