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
        /// Gets or sets item margin.
        /// </summary>
        Thickness ItemMargin { get; set; }

        /// <summary>
        /// Gets or sets selected item back color.
        /// </summary>
        Color? SelectedItemBackColor { get;  set; }

        /// <summary>
        /// Gets or sets whether current item border is visible.
        /// </summary>
        bool CurrentItemBorderVisible { get; set; }

        /// <summary>
        /// Gets or sets current item border.
        /// </summary>
        BorderSettings? CurrentItemBorder { get; set; }

        /// <summary>
        /// Gets or sets current selection border.
        /// </summary>
        BorderSettings? SelectionBorder { get; set; }

        /// <summary>
        /// Gets or sets whether item text is displayed.
        /// </summary>
        public bool TextVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checkbox will
        /// allow three check states rather than two.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the checkbox is able to display
        /// three check states; otherwise, <see langword="false" />. The default value
        /// is <see langword="false"/>.
        /// </returns>
        bool CheckBoxThreeState { get; set; }

        /// <summary>
        /// Gets or sets default size of the svg images.
        /// </summary>
        /// <remarks>
        /// Each item has <see cref="ListControlItem.SvgImageSize"/> property where
        /// this setting can be overriden. If <see cref="SvgImageSize"/> is not specified,
        /// default toolbar image size is used. Currently only rectangular svg images
        /// are supported.
        /// </remarks>
        SizeI? SvgImageSize { get; set; }

        /// <summary>
        /// Gets or sets whether selected item has bold font.
        /// </summary>
        bool SelectedItemIsBold { get; set; }

        /// <summary>
        /// Gets or sets minimal item height.
        /// </summary>
        Coord MinItemHeight { get; set; }

        /// <summary>
        /// Gets or sets item text color.
        /// </summary>
        Color? ItemTextColor { get; set; }

        /// <summary>
        /// Gets or sets default alignment of the items.
        /// </summary>
        /// <remarks>
        /// In order to set individual item alignment, item must be <see cref="ListControlItem"/>
        /// descendant, it has <see cref="ListControlItem.Alignment"/> property.
        /// </remarks>
        HVAlignment ItemAlignment { get; set; }

        /// <summary>
        /// Gets or sets selected item text color.
        /// </summary>
        Color? SelectedItemTextColor { get; set; }

        /// <summary>
        /// Gets or sets disabled item text color.
        /// </summary>
        Color? DisabledItemTextColor { get; set; }

        /// <summary>
        /// Gets or sets whether selection background is visible.
        /// </summary>
        bool SelectionVisible { get; set; }

        /// <summary>
        /// Gets or sets whether to show checkboxes in the items.
        /// </summary>
        bool CheckBoxVisible { get; set; }

        /// <summary>
        /// Gets or sets whether to draw selection background under image.
        /// </summary>
        bool SelectionUnderImage { get; set; }
    }
}
