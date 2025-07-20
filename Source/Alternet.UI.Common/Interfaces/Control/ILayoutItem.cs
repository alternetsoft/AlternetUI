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
        /// Gets or sets layout style of the child items, elements or controls.
        /// </summary>
        LayoutStyle? Layout { get; set; }

        /// <summary>
        /// Gets or sets whether layout rules are ignored for this control.
        /// </summary>
        bool IgnoreLayout { get; set; }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent control and determines
        /// how a control is resized with its parent.
        /// </summary>
        /// <returns>One of the <see cref="DockStyle" /> values.
        /// The default is <see cref="DockStyle.None" />.</returns>
        /// <remarks>
        /// Currently this property is used only when control is placed
        /// inside the <see cref="LayoutPanel"/>.
        /// </remarks>
        DockStyle Dock { get; set; }

        /// <summary>
        /// Gets or sets cached data for the layout engine.
        /// </summary>
        object? LayoutData { get; set; }

        /// <summary>
        /// Gets or sets additional properties which are layout specific.
        /// </summary>
        object? LayoutProps { get; set; }

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
        /// Gets the distance, in dips, between the right edge of the control and the left
        /// edge of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the right edge of the
        /// control and the left edge of its container's client area.</returns>
        Coord Right { get; set; }

        /// <summary>
        /// Gets the distance, in dips, between the bottom edge of the control and the top edge
        /// of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the bottom edge of
        /// the control and the top edge of its container's client area.</returns>
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
        /// Gets or sets the distance between the left edge of the control
        /// and the left edge of its container's client area.
        /// </summary>
        Coord Left { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the control
        /// and the top edge of its container's client area.
        /// </summary>
        Coord Top { get; set; }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the control, in
        /// device-independent units.
        /// </summary>
        /// <value>The position of the control's upper-left corner, in device-independent units.</value>
        PointD Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its
        /// child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>The size of the control, in device-independent units.
        /// The default value is <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the control.
        /// Set this property to <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>) to specify system-default sizing
        /// behavior when the control is first shown.
        /// </remarks>
        SizeD Size { get; set; }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <value>The width of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the width of the control.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        Coord Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        /// <value>The height of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the height of the control.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        Coord Height { get; set; }

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent units.
        /// The default value is <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual
        /// size is calculated by the layout system.
        /// Set this property to <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>) to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        SizeD SuggestedSize { get; set; }

        /// <summary>
        /// Gets or sets the suggested width of the control.
        /// </summary>
        /// <value>The suggested width of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the control. An
        /// actual width is calculated by the layout system.
        /// Set this property to <see cref="Coord.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        Coord SuggestedWidth { get; set; }

        /// <summary>
        /// Gets or sets the suggested height of the control.
        /// </summary>
        /// <value>The suggested height of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the control. An
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
        /// Gets or sets the outer margin of a control.
        /// </summary>
        /// <value>Provides margin values for the control. The default value is a
        /// <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The margin is the space between this control and the adjacent control.
        /// Margin is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the margin can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Margin"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        Thickness Margin { get; set; }

        /// <summary>
        /// Gets or sets the padding inside a control.
        /// </summary>
        /// <value>Provides padding values for the control. The default value is
        /// a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The padding is the amount of space between the content of a
        /// object and its border.
        /// Padding is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the padding can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Padding"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        Thickness Padding { get; set; }

        /// <summary>
        /// Gets or sets the minimum size the window can be resized to.
        /// </summary>
        SizeD MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size the window can be resized to.
        /// </summary>
        SizeD MaximumSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum width the window can be resized to.
        /// </summary>
        Coord? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum height the window can be resized to.
        /// </summary>
        Coord? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum width the window can be resized to.
        /// </summary>
        Coord? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height the window can be resized to.
        /// </summary>
        Coord? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets column index which is used by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets row index which is used by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of columns
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int ColumnSpan { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of rows
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int RowSpan { get; set; }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it
        /// is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is
        /// <c>null</c>.</value>
        VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when
        /// it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is
        /// <c>null</c>.</value>
        HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// control, in device-independent units.
        /// </summary>
        RectD ClientRectangle { get; }

        /// <summary>
        /// Sets the specified bounds of the control to new location and size.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of
        /// the control.</param>
        /// <param name="y">The new <see cref="Top"/> property value
        /// of the control.</param>
        /// <param name="width">The new <see cref="Width"/> property value
        /// of the control.</param>
        /// <param name="height">The new <see cref="Height"/> property value
        /// of the control.</param>
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
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units.
        /// </summary>
        /// <param name="availableSize">The available space that a parent element
        /// can allocate a child control.</param>
        /// <returns>A <see cref="SizeD"/> representing the width and height of
        /// a rectangle, in device-independent units.</returns>
        SizeD GetPreferredSize(SizeD availableSize);

        /// <summary>
        /// Calls <see cref="GetPreferredSize(SizeD)"/> with <see cref="SizeD.PositiveInfinity"/>
        /// as a parameter value.
        /// </summary>
        /// <returns>A <see cref="SizeD"/> representing the preferred size.</returns>
        SizeD GetPreferredSize();
    }
}
