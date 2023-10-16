using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://docs.wxwidgets.org/3.2/classwx_sizer.html

    /// <summary>
    /// Provides methods and properties used for laying out sub-controls in a control.
    /// </summary>
    public interface ISizer : IDisposableObject
    {
        /// <summary>
        /// Does the actual calculation of children's minimal sizes.
        /// </summary>
        Int32Size CalcMin();

        /// <summary>
        /// Appends a child to the sizer.
        /// </summary>
        /// <param name="control">
        /// The control to be added to the sizer. Its initial size (either set explicitly by
        /// the user or calculated internally when using default size) is interpreted as the
        /// minimal and in many cases also the initial size.
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Add(Control control, int proportion = 0, SizerFlag flag = 0, int border = 0);

        /// <summary>
        /// Appends a child to the sizer.
        /// </summary>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> and similar sizers to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Add(ISizer sizer, int proportion = 0, SizerFlag flag = 0, int border = 0);

        /// <summary>
        /// Appends a spacer child to the sizer.
        /// </summary>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        /// <remarks>
        /// <paramref name="width"/> and <paramref name="height"/> specify the dimension of
        /// a spacer to be added to the sizer.
        /// Adding spacers to sizers gives more flexibility in the design of dialogs;
        /// imagine for example a horizontal box with two buttons at the bottom of a dialog:
        /// you might want to insert a space between the two buttons and make that
        /// space stretchable using the proportion flag and the result will be that
        /// the left button will be aligned with the left side of the dialog and the
        /// right button with the right side - the space in between will shrink and
        /// grow with the dialog.
        /// </remarks>
        ISizerItem Add(int width, int height, int proportion = 0, SizerFlag flag = 0, int border = 0);

        /// <summary>
        /// Adds non-stretchable space to the sizer.
        /// </summary>
        /// <param name="size">Width and height value.</param>
        /// <remarks>
        /// More readable way of calling: "Add(size, size, 0)"
        /// </remarks>
        ISizerItem AddSpacer(int size);

        /// <summary>
        /// Adds stretchable space to the sizer.
        /// </summary>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> and similar sizers to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <remarks>
        /// More readable way of calling: "Add(0, 0, proportion)".
        /// </remarks>
        ISizerItem AddStretchSpacer(int proportion = 1);

        /// <summary>
        /// Insert a child into the sizer before any existing item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="control">
        /// The control to be added to the sizer. Its initial size (either set explicitly by
        /// the user or calculated internally when using default size) is interpreted as the
        /// minimal and in many cases also the initial size.
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Insert(
            int index,
            Control control,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Insert a child into the sizer before any existing item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Insert(
            int index,
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Insert a child into the sizer before any existing item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Insert(
            int index,
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Inserts non-stretchable space to the sizer.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="size">Item width and height.</param>
        /// <remarks>
        /// More readable way of calling "Insert(index, size, size)".
        /// </remarks>
        ISizerItem InsertSpacer(int index, int size);

        /// <summary>
        /// Inserts stretchable space to the sizer.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> and similar sizers to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <remarks>
        /// More readable way of calling: "Insert(0, 0, proportion)".
        /// </remarks>
        ISizerItem InsertStretchSpacer(int index, int proportion = 1);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
        /// </summary>
        /// <param name="control">
        /// The control to be added to the sizer. Its initial size (either set explicitly by
        /// the user or calculated internally when using default size) is interpreted as the
        /// minimal and in many cases also the initial size.
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Prepend(
            Control control,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
        /// </summary>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Prepend(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
        /// </summary>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        /// <param name="flag">Item flags affecting sizer's behaviour..</param>
        /// <param name="border">Item border. Determines the border width, if the flag parameter
        /// is set to include any border flag.</param>
        ISizerItem Prepend(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Prepends non-stretchable space to the sizer.
        /// </summary>
        /// <param name="size">Width and height of the item.</param>
        /// <remarks>
        /// More readable way of calling: "Prepend(size, size, 0)".
        /// </remarks>
        ISizerItem PrependSpacer(int size);

        /// <summary>
        /// Prepends stretchable space to the sizer.
        /// </summary>
        /// <remarks>
        /// More readable way of calling: "Prepend(0, 0, proportion)".
        /// </remarks>
        /// <param name="proportion">
        /// Used in <see cref="IBoxSizer"/> and similar sizers to indicate if a child
        /// of a sizer can change its size in the main orientation of the sizer - where 0
        /// stands for not changeable and a value of more than zero is interpreted relative
        /// to the value of other children of the same sizer. For example, you might have
        /// a horizontal sizer with three children, two of which are supposed to change
        /// their size with the sizer. Then the two stretchable windows would get a value
        /// of 1 each to make them grow and shrink equally with the sizer's horizontal
        /// dimension.
        /// </param>
        ISizerItem PrependStretchSpacer(int proportion = 1);

        /// <summary>
        /// Removes a sizer child from the sizer and destroys it.
        /// </summary>
        /// <param name="sizer">The (child-)sizer to be removed.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Remove(ISizer sizer);

        /// <summary>
        /// Removes a child from the sizer and destroys it if it is a sizer or a spacer,
        /// but not if it is a control (because controls are owned by their parent control,
        /// not the sizer).
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Remove(int index);

        /// <summary>
        /// Detaches the child control from the sizer without destroying it.
        /// </summary>
        /// <param name="control"></param>
        /// <returns><c>true</c> if the child item was found and detached, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Detach(Control control);

        /// <summary>
        /// Detaches the child sizer from the sizer without destroying it.
        /// </summary>
        /// <param name="sizer">The (child-)sizer to be detached.</param>
        /// <returns><c>true</c> if the child item was found and detached, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Detach(ISizer sizer);

        /// <summary>
        /// Detaches an item at the specified position from the sizer without destroying it.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if the child item was found and detached, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Detach(int index);

        /// <summary>
        /// Detaches the given <paramref name="oldControl"/> from the sizer and replaces it
        /// with the given <paramref name="newControl"/>.
        /// </summary>
        /// <param name="oldControl">Old control.</param>
        /// <param name="newControl">New control.</param>
        /// <param name="recursive">Whether to to search the given element recursively in subsizers</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// The detached child control is not deleted (because controls are owned by their parent
        /// control, not the sizer).
        /// </remarks>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Replace(Control oldControl, Control newControl, bool recursive);

        /// <summary>
        /// Detaches the given <paramref name="oldsz"/> from the sizer and replaces it
        /// with the given <paramref name="newsz"/>.
        /// </summary>
        /// <param name="oldsz">Old sizer item.</param>
        /// <param name="newsz">New sizer item.</param>
        /// <param name="recursive">Whether to to search the given element recursively in subsizers</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>The detached child sizer is deleted.</remarks>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Replace(ISizer oldsz, ISizer newsz, bool recursive);

        /// <summary>
        /// Detaches all children from the sizer.
        /// </summary>
        /// <remarks>
        /// Notice that child sizers are always deleted, as a general consequence of the
        /// principle that sizers own their sizer children, but don't own their control children
        /// (because they are already owned by their parent controls).
        /// </remarks>
        void Clear();

        /// <summary>
        /// Inform sizer about the first direction that has been decided (by parent item).
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="size"></param>
        /// <param name="availableOtherDir"></param>
        /// <returns><c>true</c> if it made use of the information
        /// (and recalculated min size). </returns>
        bool InformFirstDirection(int direction, int size, int availableOtherDir);

        /// <summary>
        /// Call this to give the sizer a minimal size.
        /// </summary>
        /// <param name="width">Min width.</param>
        /// <param name="height">Min height.</param>
        /// <remarks>
        /// Normally, the sizer will calculate its minimal size based purely on how much
        /// space its children need. After calling this method <see cref="GetMinSize"/> will
        /// return either the minimal size as requested by its children or
        /// the minimal size set here, depending on which is bigger.
        /// </remarks>
        void SetMinSize(int width, int height);

        /// <summary>
        /// Set an item's minimum size by its control.
        /// </summary>
        /// <param name="control">Control attached to the item.</param>
        /// <param name="width">Min width.</param>
        /// <param name="height">Min height.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This function enables an application to set the size of an item after initial creation.
        /// The control will be found recursively in the sizer's descendants.
        /// </remarks>
        bool SetItemMinSize(Control control, int width, int height);

        /// <summary>
        /// Set an item's minimum size by its sizer.
        /// </summary>
        /// <param name="sizer">Sizer attached to the item.</param>
        /// <param name="width">Min width.</param>
        /// <param name="height">Min height.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        /// <remarks>
        /// This function enables an application to set the size of an item after initial creation.
        /// The subsizer will be found recursively in the sizer's descendants.
        /// </remarks>
        bool SetItemMinSize(ISizer sizer, int width, int height);

        /// <summary>
        /// Set an item's minimum size by its index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="width">Min width.</param>
        /// <param name="height">Min height.</param>
        /// <returns><c>true</c> if operation is successful; <c>false</c> otherwise.</returns>
        bool SetItemMinSize(int index, int width, int height);

        /// <summary>
        /// Gets the current size of the sizer.
        /// </summary>
        Int32Size GetSize();

        /// <summary>
        /// Gets the current position of the sizer.
        /// </summary>
        Int32Point GetPosition();

        /// <summary>
        /// Gets the minimal size of the sizer.
        /// </summary>
        Int32Size GetMinSize();

        /// <summary>
        /// Recalculates sizes of the items. It is better to use <see cref="Layout"/>.
        /// </summary>
        void RecalcSizes();

        /// <summary>
        /// Forces layout of the children anew, e.g. after having added a child to
        /// or removed a child (control, other sizer or space) from the sizer while
        /// keeping the current dimension.
        /// </summary>
        void Layout();

        /// <summary>
        /// Computes client area size for control so that it matches the sizer's minimal size.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <remarks>
        /// Unlike <see cref="GetMinSize"/>, this method accounts for other constraints imposed
        /// on control, namely display's size (returned size will never be too large for
        /// the display) and maximum control size if previously set by
        /// <see cref="Control.MaximumSize"/>.
        /// </remarks>
        Int32Size ComputeFittingClientSize(Control control);

        /// <summary>
        /// Like <see cref="ComputeFittingClientSize"/>, but converts the result into control size.
        /// </summary>
        /// <param name="control">Control</param>
        /// <remarks>
        /// The returned value is suitable for passing to <see cref="Control.Size"/> or
        /// <see cref="Control.MinimumSize"/>.
        /// </remarks>
        Int32Size ComputeFittingWindowSize(Control control);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control">Control</param>
        /// <returns></returns>
        Int32Size Fit(Control control);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control">Control</param>
        void FitInside(Control control);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control">Control</param>
        void SetSizeHints(Control control);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void SetDimension(int x, int y, int width, int height);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int GetItemCount();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsEmpty();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="show"></param>
        /// <param name="recursive"></param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Show(Control control, bool show, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <param name="show"></param>
        /// <param name="recursive"></param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Show(ISizer sizer, bool show, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="show"></param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Show(int index, bool show);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <param name="recursive"></param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(ISizer sizer, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="recursive"></param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(Control control, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        bool IsShown(Control control);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <returns></returns>
        bool IsShown(ISizer sizer);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns></returns>
        bool IsShown(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        void ShowItems(bool show);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        void Show(bool show);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool AreAnyItemsShown();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="item">Sizer item.</param>
        /// <returns></returns>
        ISizerItem Insert(int index, ISizerItem item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">Sizer item.</param>
        /// <returns></returns>
        ISizerItem Prepend(ISizerItem item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item">Sizer item.</param>
        /// <returns></returns>
        ISizerItem Add(ISizerItem item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="newitem">Sizer item.</param>
        /// <returns></returns>
        /// <remarks>
        /// This method does not cause any layout or resizing to take place, call
        /// <see cref="Layout"/> to update the layout "on screen" after removing a child
        /// from the sizer.
        /// </remarks>
        bool Replace(int index, ISizerItem newitem);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        ISizerItem GetItem(Control control, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        ISizerItem GetItem(ISizer sizer, bool recursive);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        ISizerItem GetItem(int index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        ISizerItem GetItemById(int id, bool recursive = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Prepend(Control control, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="control">
        /// The control to be added to the sizer. Its initial size (either set explicitly by
        /// the user or calculated internally when using default size) is interpreted as the
        /// minimal and in many cases also the initial size.
        /// </param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Insert(int index, Control control, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="sizer"></param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Insert(int index, int width, int height, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="control">
        /// The control to be added to the sizer. Its initial size (either set explicitly by
        /// the user or calculated internally when using default size) is interpreted as the
        /// minimal and in many cases also the initial size.
        /// </param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Add(Control control, ISizerFlags sizerFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizer"></param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Add(ISizer sizer, ISizerFlags sizerFlags);

        /// <summary>
        /// Appends a spacer child to the sizer.
        /// </summary>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Add(int width, int height, ISizerFlags sizerFlags);

        // IntPtr GetChildren();
        // void SetContainingWindow(Control window);
        // IntPtr GetContainingWindow();
    }
}