using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Gets default used when <see cref="ListControlItem"/> is painted.
    /// </summary>
    public interface IListControlItemDefaults
    {
        /// <summary>
        /// Gets or sets a value indicating whether checkbox will
        /// allow three check states rather than two.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the checkbox is able to display
        /// three check states; otherwise, <see langword="false" />. The default value
        /// is <see langword="false"/>.
        /// </returns>
        bool CheckBoxThreeState { get; }

        /// <summary>
        /// Gets default size of the svg images.
        /// </summary>
        /// <remarks>
        /// Each item has <see cref="ListControlItem.SvgImageSize"/> property where
        /// this setting can be overriden. If <see cref="SvgImageSize"/> is not specified,
        /// default toolbar image size is used. Currently only rectangular svg images
        /// are supported.
        /// </remarks>
        SizeI? SvgImageSize { get; }

        /// <summary>
        /// Gets whether selected item has bold font.
        /// </summary>
        bool SelectedItemIsBold { get; }

        /// <summary>
        /// Gets minimal item height.
        /// </summary>
        Coord MinItemHeight { get; }

        /// <summary>
        /// Gets item text color.
        /// </summary>
        Color? ItemTextColor { get; }

        /// <summary>
        /// Gets default alignment of the items.
        /// </summary>
        /// <remarks>
        /// In order to set individual item alignment, item must be <see cref="ListControlItem"/>
        /// descendant, it has <see cref="ListControlItem.Alignment"/> property.
        /// </remarks>
        GenericAlignment ItemAlignment { get; }

        /// <summary>
        /// Gets selected item text color.
        /// </summary>
        Color? SelectedItemTextColor { get; }

        /// <summary>
        /// Gets disabled item text color.
        /// </summary>
        Color? DisabledItemTextColor { get; }

        /// <summary>
        /// Gets whether selection background is visible.
        /// </summary>
        bool SelectionVisible { get; }

        /// <summary>
        /// Gets whether to show checkboxes in the items.
        /// </summary>
        bool CheckBoxVisible { get; }
    }
}
