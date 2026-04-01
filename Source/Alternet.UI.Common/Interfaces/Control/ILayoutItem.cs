using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the contract for layout items, providing properties and methods to manage
    /// layout characteristics such as
    /// size, position, and alignment within a container.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are expected to handle layout-related properties
    /// and methods, allowing for flexible and dynamic UI arrangements. This interface is
    /// typically used in UI frameworks where layout management is a key concern.
    /// </remarks>
    public interface ILayoutItem
    {
        /// <summary>
        /// Gets or sets layout style of the child items.
        /// </summary>
        LayoutStyle? Layout { get; set; }

        /// <summary>
        /// Gets or sets whether layout rules are ignored for this item.
        /// </summary>
        bool IgnoreLayout { get; set; }

        /// <summary>
        /// Gets or sets the offset applied to the item's layout, in device-independent units.
        /// </summary>
        PointD LayoutOffset { get; set; }

        /// <summary>
        /// Gets or sets which item borders are docked to its parent item and determines
        /// how an item is resized with its parent.
        /// </summary>
        /// <returns>One of the <see cref="DockStyle" /> values.
        /// The default is <see cref="DockStyle.None" />.</returns>
        /// <remarks>
        /// Currently this property is used only when item is placed
        /// inside the <see cref="LayoutPanel"/>.
        /// </remarks>
        DockStyle Dock { get; set; }

        /// <summary>
        /// Gets or sets size of the client area, in device-independent units.
        /// </summary>
        SizeD ClientSize { get; }

        /// <summary>
        /// Gets whether layout is suspended.
        /// </summary>
        bool IsLayoutSuspended { get; }

        /// <summary>
        /// Gets whether layout is currently performed.
        /// </summary>
        bool IsLayoutPerform { get; }

        /// <summary>
        /// Gets the distance, in dips, between the right edge of the item and the left
        /// edge of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the right edge of the
        /// item and the left edge of its container's client area.</returns>
        Coord Right { get; set; }

        /// <summary>
        /// Gets the distance, in dips, between the bottom edge of the item and the top edge
        /// of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the bottom edge of
        /// the item and the top edge of its container's client area.</returns>
        Coord Bottom { get; set; }

        /// <summary>
        /// Gets whether there are any children.
        /// </summary>
        bool HasChildren { get; }

        /// <summary>
        /// Gets or sets the bounds relative to the parent, in device-independent units.
        /// </summary>
        RectD Bounds { get; set; }

        /// <summary>
        /// Gets or sets the distance between the left edge of the item
        /// and the left edge of its container's client area.
        /// </summary>
        Coord Left { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the item
        /// and the top edge of its container's client area.
        /// </summary>
        Coord Top { get; set; }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the item, in
        /// device-independent units.
        /// </summary>
        /// <value>The position of the item's upper-left corner, in device-independent units.</value>
        PointD Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item and all its
        /// child items are displayed.
        /// </summary>
        /// <value><c>true</c> if the item and all its child items are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the size of the item.
        /// </summary>
        /// <value>The size of the item, in device-independent units.
        /// The default value is <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the item.
        /// Set this property to <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>) to specify system-default sizing
        /// behavior when the item is first shown.
        /// </remarks>
        SizeD Size { get; set; }

        /// <summary>
        /// Gets or sets the width of the item.
        /// </summary>
        /// <value>The width of the item, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the width of the item.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the item is first shown.
        /// </remarks>
        Coord Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the item.
        /// </summary>
        /// <value>The height of the item, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the height of the item.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the item is first shown.
        /// </remarks>
        Coord Height { get; set; }

        /// <summary>
        /// Gets or sets the suggested size of the item.
        /// </summary>
        /// <value>The suggested size of the item, in device-independent units.
        /// The default value is <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the item. An actual
        /// size is calculated by the layout system.
        /// Set this property to <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>) to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        SizeD SuggestedSize { get; set; }

        /// <summary>
        /// Gets or sets the suggested width of the item.
        /// </summary>
        /// <value>The suggested width of the item, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the item. An
        /// actual width is calculated by the layout system.
        /// Set this property to <see cref="Coord.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        Coord SuggestedWidth { get; set; }

        /// <summary>
        /// Gets or sets the suggested height of the item.
        /// </summary>
        /// <value>The suggested height of the item, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the item. An
        /// actual height is calculated by the layout system.
        /// Set this property to <see cref="Coord.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        Coord SuggestedHeight { get; set; }

        /// <summary>
        /// Gets number of children items.
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// Gets or sets minimal value of the child's <see cref="Margin"/> property.
        /// </summary>
        Thickness? MinChildMargin { get; set; }

        /// <summary>
        /// Gets or sets the outer margin of an item.
        /// </summary>
        /// <value>Provides margin values for the item. The default value is a
        /// <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The margin is the space between this item and the adjacent item.
        /// Margin is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the margin can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Margin"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        Thickness Margin { get; set; }

        /// <summary>
        /// Gets or sets the padding inside an item.
        /// </summary>
        /// <value>Provides padding values for the item. The default value is
        /// a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The padding is the amount of space between the content of a
        /// item and its border.
        /// Padding is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the padding can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Padding"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the minimum size the item can be resized to.
        /// </summary>
        SizeD MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size the item can be resized to.
        /// </summary>
        SizeD MaximumSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum width the item can be resized to.
        /// </summary>
        Coord? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum height the item can be resized to.
        /// </summary>
        Coord? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum width the item can be resized to.
        /// </summary>
        Coord? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height the item can be resized to.
        /// </summary>
        Coord? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets column index which is used by the grid container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in the grid container.
        /// </remarks>
        int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets row index which is used by the grid container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in the grid container.
        /// </remarks>
        int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of columns
        /// this item's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in the grid container.
        /// </remarks>
        int ColumnSpan { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of rows
        /// this item's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in the grid container.
        /// </remarks>
        int RowSpan { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this item when it
        /// is positioned within a parent container.
        /// </summary>
        /// <value>A vertical alignment setting. The default is
        /// <c>null</c>.</value>
        VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this item when
        /// it is positioned within a parent container.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is
        /// <c>null</c>.</value>
        HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// item, in device-independent units.
        /// </summary>
        RectD ClientRectangle { get; }

        /// <summary>
        /// Gets or sets cached data for the layout engine.
        /// </summary>
        object? LayoutData { get; set; }

        /// <summary>
        /// Gets or sets additional properties which are layout specific.
        /// </summary>
        object? LayoutProps { get; set; }

        /// <summary>
        /// Gets or sets layout flags that specify the layout behavior of the item.
        /// </summary>
        LayoutFlags LayoutFlags { get; set; }

        /// <summary>
        /// Sets the specified bounds of the item to new location and size.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of
        /// the item.</param>
        /// <param name="y">The new <see cref="Top"/> property value
        /// of the item.</param>
        /// <param name="width">The new <see cref="Width"/> property value
        /// of the item.</param>
        /// <param name="height">The new <see cref="Height"/> property value
        /// of the item.</param>
        /// <param name="specified">Specifies which bounds to use when applying new
        /// location and size.</param>
        void SetBounds(
            Coord x,
            Coord y,
            Coord width,
            Coord height,
            BoundsSpecified specified);

        /// <summary>
        /// Resets <see cref="SuggestedHeight"/> property.
        /// </summary>
        void ResetSuggestedHeight();

        /// <summary>
        /// Resets <see cref="SuggestedWidth"/> property.
        /// </summary>
        void ResetSuggestedWidth();

        /// <summary>
        /// Resets <see cref="SuggestedSize"/> property.
        /// </summary>
        void ResetSuggestedSize();

        /// <summary>
        /// Gets a read-only list of all child layout items currently included in the layout.
        /// </summary>
        IReadOnlyList<ILayoutItem> AllChildrenInLayout { get; }

        /// <summary>
        /// Calculates the preferred size for the current object, constrained by any applicable size limits.
        /// </summary>
        /// <param name="context">The context information used to determine the preferred size, including layout constraints and preferences.</param>
        /// <returns>A SizeD structure representing the preferred size, adjusted to not exceed any defined size limitations.</returns>
        SizeD GetPreferredSizeLimited(PreferredSizeContext context);

        /// <summary>
        /// Applies <see cref="MinimumSize"/> and <see cref="MaximumSize"/> restrictions to the specified size.
        /// </summary>
        /// <param name="size">The size to be limited.</param>
        /// <returns>The size after applying the minimum and maximum restrictions.</returns>
        SizeD GetSizeLimited(SizeD size);

        /// <summary>
        /// Retrieves the size of a rectangular area into which an item can
        /// be fitted, in device-independent units.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> representing context
        /// for the layout.</param>
        /// <returns>A <see cref="SizeD"/> representing the width and height of
        /// a rectangle, in device-independent units.</returns>
        SizeD GetPreferredSize(PreferredSizeContext context);

        /// <summary>
        /// Calls <see cref="GetPreferredSize(PreferredSizeContext)"/>
        /// with <see cref="PreferredSizeContext.PositiveInfinity"/>
        /// as a parameter value.
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the preferred size.</returns>
        SizeD GetPreferredSize();
    }
}
