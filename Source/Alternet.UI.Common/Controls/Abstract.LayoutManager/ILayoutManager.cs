using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines an interface for managing the layout of controls and items within a container. The <see cref="ILayoutManager"/>
    /// interface provides methods for performing layout operations, such as arranging child controls based on specified layout
    /// styles and calculating preferred sizes for layout items. Implementations of this interface can be used to create
    /// custom layout behaviors for controls and containers in a user interface framework.
    /// </summary>
    public interface ILayoutManager
    {
        /// <summary>
        /// Calculates the preferred size for a container when arranging its child layout items in a stack, either
        /// vertically or horizontally.
        /// </summary>
        /// <remarks>If the container specifies a suggested size, that value is used. Otherwise, the
        /// preferred size is determined by aggregating the preferred sizes of the relevant child items, accounting for
        /// their margins and the container's padding. Docked children can be excluded from the calculation by setting
        /// ignoreDocked to true.</remarks>
        /// <param name="container">The container layout item for which to calculate the preferred size.</param>
        /// <param name="context">The context providing available size constraints and other layout information.</param>
        /// <param name="isVert">true to stack child items vertically; false to stack them horizontally.</param>
        /// <param name="children">An optional list of child layout items to consider. If null, all children in the container's layout are
        /// used.</param>
        /// <param name="ignoreDocked">true to ignore child items that are docked; false to include all children in the calculation.</param>
        /// <returns>A SizeD structure representing the preferred size of the container based on the stacking arrangement of its
        /// children. Returns SizeD.Empty if the available size is empty or negative.</returns>
        SizeD GetPreferredSizeWhenStack(
            ILayoutItem container,
            PreferredSizeContext context,
            bool isVert,
            IReadOnlyList<ILayoutItem>? children = null,
            bool ignoreDocked = true);

        /// <summary>
        /// Determines whether the specified child layout item should be excluded from layout calculations.
        /// </summary>
        /// <param name="control">The child layout item to evaluate. Cannot be null.</param>
        /// <returns>true if the child is either not visible or marked to ignore layout; otherwise, false.</returns>
        bool ChildIgnoresLayout(ILayoutItem control);

        /// <summary>
        /// Arranges the specified child layout item within the given bounds according to the provided dock style,
        /// updating the bounds to reflect the remaining available space.
        /// </summary>
        /// <remarks>If the dock style specifies an auto-size variant, the child's preferred size is used
        /// for layout. When the UseMarginsWhenDock flag is set, the child's margins are applied during arrangement.
        /// This method is typically used by container layouts to sequentially arrange multiple children.</remarks>
        /// <param name="bounds">A reference to the bounding rectangle in which to arrange the child. Updated to represent the remaining
        /// space after layout.</param>
        /// <param name="child">The layout item to be positioned within the bounds according to the specified dock style.</param>
        /// <param name="value">The dock style that determines how the child is positioned and sized within the bounds.</param>
        /// <param name="containerFlags">Flags that influence layout behavior, such as whether to apply margins when docking.</param>
        void LayoutWhenDocked(ref RectD bounds, ILayoutItem child, DockStyle value, LayoutFlags containerFlags);

        /// <summary>
        /// Called when the control or item should reposition its child controls.
        /// This method checks the layout style and calls the appropriate method to perform the layout of child controls.
        /// </summary>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.OnLayout"/>.
        /// </remarks>
        /// <param name="container">Container control which children will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="getBounds">Returns rectangle in which layout is performed.</param>
        /// <param name="items">List of controls to layout.</param>
        void OnLayout(
            ILayoutItem container,
            LayoutStyle layout,
            Func<RectD> getBounds,
            IReadOnlyList<ILayoutItem> items);

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control or item can
        /// be fitted, in device-independent units. This method checks the layout style and
        /// calls the appropriate method to calculate the preferred size.
        /// </summary>
        /// <param name="context">The <see cref="PreferredSizeContext"/> providing
        /// the available size and other layout information.</param>
        /// <returns>A <see cref="ILayoutItem.SuggestedSize"/> representing the width and height of
        /// a rectangle, in device-independent units.</returns>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.GetPreferredSize(PreferredSizeContext)"/>.
        /// </remarks>
        /// <param name="container">Container control which children will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        SizeD OnGetPreferredSize(
            ILayoutItem container,
            PreferredSizeContext context,
            LayoutStyle layout);

        /// <summary>
        /// Calculates horizontal position using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="childControl">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        AxisIntervalD AlignHorizontal(
                    RectD layoutBounds,
                    ILayoutItem childControl,
                    SizeD childPreferredSize,
                    HorizontalAlignment alignment);

        /// <summary>
        /// Calculates vertical position using align parameters.
        /// </summary>
        /// <param name="layoutBounds">Rectangle in which alignment is performed.</param>
        /// <param name="control">Control to align.</param>
        /// <param name="childPreferredSize">Preferred size.</param>
        /// <param name="alignment">Alignment of the control.</param>
        /// <returns></returns>
        AxisIntervalD AlignVertical(
            RectD layoutBounds,
            ILayoutItem control,
            SizeD childPreferredSize,
            VerticalAlignment alignment);
    }
}
