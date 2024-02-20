using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the individual <see cref="ISizer"/> item.
    /// </summary>
    internal interface ISizerItem : IDisposableObject
    {
        /// <summary>
        /// Gets whether item is shown.
        /// </summary>
        /// <remarks>
        /// Returns <c>true</c> if this item is a control or a spacer and it is shown or if this
        /// item is a sizer and not all of its elements are hidden.
        /// In other words, for sizer items, all of the child elements must
        /// be hidden for the sizer itself to be considered hidden.
        /// </remarks>
        /// <remarks>
        /// This function behaves obviously for the controls and spacers but for the
        /// sizers it returns <c>true</c> if any sizer element is shown and only returns
        /// <c>false</c> if all of them are hidden. Also, it always returns true if
        /// <see cref="SizerFlag.ReserveSpaceEvenIfHidden"/> was used.
        /// </remarks>
        bool IsShown { get; }

        /// <summary>
        /// Gets the current size of the item, as set in the last Layout.
        /// </summary>
        SizeI Size { get; }

        /// <summary>
        /// Gets the minimum size to be allocated for this item including border size.
        /// </summary>
        SizeI MinSizeWithBorder { get; }

        /// <summary>
        /// Gets the maximum size to be allocated for this item.
        /// </summary>
        SizeI MaxSize { get; }

        /// <summary>
        /// Gets the maximum size to be allocated for this item including border size.
        /// </summary>
        SizeI MaxSizeWithBorder { get; }

        /// <summary>
        /// Gets or sets the minimum size to be allocated for this item.
        /// </summary>
        /// <remarks>
        /// If this item is a control, the size is also passed to <see cref="Control.MinimumSize"/>.
        /// </remarks>
        SizeI MinSize { get; set; }

        /// <summary>
        /// Gets or sets the numeric id of the item. Returns (-3) if the id has not been set.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Is this item a control?
        /// </summary>
        bool IsControl { get; }

        /// <summary>
        /// Is this item a sizer?
        /// </summary>
        bool IsSizer { get; }

        /// <summary>
        /// Is this item a spacer?
        /// </summary>
        bool IsSpacer { get; }

        /// <summary>
        /// Gets or sets the proportion item attribute.
        /// </summary>
        int Proportion { get; set; }

        /// <summary>
        /// Gets or sets the flag attribute.
        /// </summary>
        SizerFlag Flag { get; set; }

        /// <summary>
        /// Gets or sets the border attribute.
        /// </summary>
        int Border { get; set; }

        /// <summary>
        /// Gets or sets the ration item attribute.
        /// </summary>
        float Ratio { get; set; }

        /// <summary>
        /// Enables deleting the SizerItem without destroying the contained sizer.
        /// </summary>
        void DetachSizer();

        /// <summary>
        /// Enables deleting the SizerItem without resetting the sizer in the
        /// contained control.
        /// </summary>
        void DetachControl();

        /// <summary>
        /// Calculates the minimum desired size for the item, including any space needed by borders.
        /// </summary>
        SizeI CalcMin();

        /// <summary>
        /// Set the position and size of the space allocated to the sizer, and
        /// adjust the position and size of the item to be within that space taking
        /// alignment and borders into account.
        /// </summary>
        /// <param name="pos">Item position.</param>
        /// <param name="size">Item size.</param>
        void SetDimension(PointI pos, SizeI size);

        /// <summary>
        /// Sets the initial size.
        /// </summary>
        /// <param name="w">Width.</param>
        /// <param name="h">Height.</param>
        void SetInitSize(int w, int h);

        /// <summary>
        /// Sets <see cref="Ratio"/> item attribute.
        /// </summary>
        /// <param name="width">Width value.</param>
        /// <param name="height">Height value.</param>
        /// <remarks>
        /// If either of dimensions is zero, ratio is assumed to be 1
        /// to avoid "divide by zero" errors
        /// </remarks>
        void SetRatio(int width, int height);

        /// <summary>
        /// Gets the rectangle of the item on the parent window, excluding borders.
        /// </summary>
        RectI GetRect();

        /// <summary>
        /// If this item is tracking a sizer, return it; <c>null</c> otherwise.
        /// </summary>
        ISizer GetSizer();

        /// <summary>
        /// If this item is tracking a spacer, return its size.
        /// </summary>
        SizeI GetSpacer();

        /// <summary>
        /// Set the show item attribute, which sizers use to determine if the item is
        /// to be made part of the layout or not. If the item is tracking a control then
        /// it is shown or hidden as needed.
        /// </summary>
        /// <param name="show">Show or hide the item.</param>
        void Show(bool show = true);

        /// <summary>
        /// Gets the current position of the item, as set in the last Layout.
        /// </summary>
        PointI GetPosition();

        /// <summary>
        /// Set the control to be tracked by this item. The old control isn't deleted.
        /// </summary>
        /// <param name="control">Control.</param>
        void SetControl(Control control);

        /// <summary>
        /// Set the sizer tracked by this item. Old sizer, if any, is deleted.
        /// </summary>
        /// <param name="sizer">Sizer.</param>
        /// <remarks>
        /// This function deletes the current contents of the item.
        /// </remarks>
        void SetSizer(ISizer sizer);

        /// <summary>
        /// Set the size of the spacer tracked by this item. Old spacer, if any, is deleted.
        /// </summary>
        /// <param name="w">Width.</param>
        /// <param name="h">Height</param>
        /// <remarks>
        /// This function deletes the current contents of the item.
        /// </remarks>
        void SetSpacer(int w, int h);
    }
}