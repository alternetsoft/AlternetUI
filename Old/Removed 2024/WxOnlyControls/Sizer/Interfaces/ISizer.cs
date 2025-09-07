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
    internal interface ISizer : IDisposableObject
    {
        /// <summary>
        /// Does the actual calculation of children's minimal sizes.
        /// </summary>
        SizeI CalcMin();

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
        SizeI GetSize();

        /// <summary>
        /// Gets the current position of the sizer.
        /// </summary>
        PointI GetPosition();

        /// <summary>
        /// Gets the minimal size of the sizer.
        /// </summary>
        SizeI GetMinSize();

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
        SizeI ComputeFittingClientSize(Control control);

        /// <summary>
        /// Like <see cref="ComputeFittingClientSize"/>, but converts the result into control size.
        /// </summary>
        /// <param name="control">Control</param>
        /// <remarks>
        /// The returned value is suitable for passing to <see cref="Control.Size"/> or
        /// <see cref="Control.MinimumSize"/>.
        /// </remarks>
        SizeI ComputeFittingWindowSize(Control control);

        /// <summary>
        /// Tells the sizer to resize the control so that its client area matches the sizer's
        /// minimal size (<see cref="ComputeFittingClientSize"/> is called to determine it).
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <returns>The new window size.</returns>
        /// <remarks>
        /// This is commonly done in the constructor of the control itself.
        /// </remarks>
        SizeI Fit(Control control);

        /// <summary>
        /// Tell the sizer to resize the virtual size of the control to match the sizer's minimal size.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <remarks>
        /// This will not alter the on screen size of the control, but may cause the
        /// addition/removal/alteration of scrollbars required to view the virtual area
        /// in controls which manage it.
        /// </remarks>
        void FitInside(Control control);

        /// <summary>
        /// This method first calls <see cref="Fit"/> and then SetSizeHints() on the control passed to it.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <remarks>
        /// This function is only when control is actually a <see cref="Window"/>, since
        /// SetSizeHints only has any effect in these classes. It does nothing in normal controls.
        /// </remarks>
        /// <remarks>
        /// Note that window does not need to be attached to this sizer and it is,
        /// in fact, common to call this function on the sizer associated with the panel
        /// covering the client area of a window passing it the window object,
        /// as this has the desired effect of adjusting the window size to the
        /// size fitting the panel.
        /// </remarks>
        void SetSizeHints(Control control);

        /// <summary>
        /// Call this to force the sizer to take the given dimension and thus force the
        /// items owned by the sizer to resize themselves according to the rules defined by
        /// its parameters.
        /// </summary>
        /// <param name="x">Left position.</param>
        /// <param name="y">Top position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        void SetDimension(int x, int y, int width, int height);

        /// <summary>
        /// Gets number of added sizer items.
        /// </summary>
        int GetItemCount();

        /// <summary>
        /// Gets whether sizer is empty and there are no items added to it.
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Shows or hides item with the attached control.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="show"><c>true</c> to show item; <c>false</c> to hide it.</param>
        /// <param name="recursive">Use to show or hide elements found in subitems.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// To make an item disappear or reappear, use <see cref="Show(Control,bool,bool)"/>
        /// followed by <see cref="Layout"/>.
        /// </remarks>
        bool Show(Control control, bool show, bool recursive);

        /// <summary>
        /// Shows or hides item with the attached sizer.
        /// </summary>
        /// <param name="sizer">Affected sizer.</param>
        /// <param name="show"><c>true</c> to show item; <c>false</c> to hide it.</param>
        /// <param name="recursive">Use to show or hide elements found in subsizers.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// To make a sizer item disappear or reappear, use <see cref="Show(ISizer,bool,bool)"/>
        /// followed by <see cref="Layout"/>.
        /// </remarks>
        bool Show(ISizer sizer, bool show, bool recursive);

        /// <summary>
        /// Shows the item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="show"><c>true</c> to show item; <c>false</c> to hide it.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        /// <remarks>
        /// To make an item disappear or reappear, use <see cref="Show(int,bool)"/>
        /// followed by <see cref="Layout"/>.
        /// </remarks>
        bool Show(int index, bool show);

        /// <summary>
        /// Hides item with the attached sizer.
        /// </summary>
        /// <param name="sizer">Affected sizer.</param>
        /// <param name="recursive">Use to show or hide elements found in subitems.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(ISizer sizer, bool recursive);

        /// <summary>
        /// Hides item with the attached control.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="recursive">Use to show or hide elements found in subitems.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(Control control, bool recursive);

        /// <summary>
        /// Hides the item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if the child item was found, <c>false</c> otherwise.</returns>
        bool Hide(int index);

        /// <summary>
        /// Gets whether item with the attached control is shown.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <returns><c>true</c> if control is shown; <c>false</c> otherwise.</returns>
        bool IsShown(Control control);

        /// <summary>
        /// Gets whether item with the attached sizer is shown.
        /// </summary>
        /// <param name="sizer">Sizer.</param>
        /// <returns><c>true</c> if sizer is shown; <c>false</c> otherwise.</returns>
        bool IsShown(ISizer sizer);

        /// <summary>
        /// Gets whether item with the specified index is shown.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><c>true</c> if item is shown; <c>false</c> otherwise.</returns>
        bool IsShown(int index);

        /// <summary>
        /// Show or hide all items managed by the sizer.
        /// </summary>
        /// <param name="show"><c>true</c> to show items; <c>false</c> to hide them.</param>
        void ShowItems(bool show = true);

        /// <summary>
        /// Show or hide all items managed by the sizer.
        /// </summary>
        /// <param name="show"><c>true</c> to show items; <c>false</c> to hide them.</param>
        void Show(bool show = true);

        /// <summary>
        /// Gets whether any items are shown.
        /// </summary>
        bool AreAnyItemsShown();

        /// <summary>
        /// Inserts sizer item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="item">Sizer item.</param>
        ISizerItem Insert(int index, ISizerItem item);

        /// <summary>
        /// Inserts sizer item at the beginning of the items list.
        /// </summary>
        /// <param name="item">Sizer item.</param>
        ISizerItem Prepend(ISizerItem item);

        /// <summary>
        /// Adds sizer item.
        /// </summary>
        /// <param name="item">Sizer item.</param>
        ISizerItem Add(ISizerItem item);

        /// <summary>
        /// Replaces sizer item at the specified index.
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
        /// Finds the <see cref="ISizerItem"/> which holds the given control.
        /// </summary>
        /// <param name="control">Control.</param>
        /// <param name="recursive"><c>true</c> to search in subsizers too.</param>
        /// <returns><see cref="ISizerItem"/> or <c>null</c> if there is no item with
        /// the specified control.</returns>
        ISizerItem? GetItem(Control control, bool recursive);

        /// <summary>
        /// Finds the <see cref="ISizerItem"/> which holds the given subsizer.
        /// </summary>
        /// <param name="sizer">Sizer</param>
        /// <param name="recursive"><c>true</c> to search in subsizers too.</param>
        /// <returns><see cref="ISizerItem"/> or <c>null</c> if there is no item with
        /// the specified sizer.</returns>
        ISizerItem? GetItem(ISizer sizer, bool recursive);

        /// <summary>
        /// Gets item with the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <returns><see cref="ISizerItem"/> or <c>null</c> if there is no item with
        /// the specified index.</returns>
        ISizerItem? GetItem(int index);

        /// <summary>
        /// Finds the <see cref="ISizerItem"/> with the specified id.
        /// </summary>
        /// <param name="id">Item identifier.</param>
        /// <param name="recursive"><c>true</c> to search in subsizers too.</param>
        /// <returns><see cref="ISizerItem"/> or <c>null</c> if there is no item with
        /// the specified id.</returns>
        ISizerItem? GetItemById(int id, bool recursive = false);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
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
        ISizerItem Prepend(Control control, ISizerFlags sizerFlags);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
        /// </summary>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Prepend(ISizer sizer, ISizerFlags sizerFlags);

        /// <summary>
        /// Same as adding of the item, but prepends the items to the beginning of the
        /// list of items owned by this sizer.
        /// </summary>
        /// <param name="width">Width of the spacer.</param>
        /// <param name="height">Height of the spacer.</param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Prepend(int width, int height, ISizerFlags sizerFlags);

        /// <summary>
        /// Insert a child into the sizer before any existing item at the specified index.
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
        /// Insert a child into the sizer before any existing item at the specified index.
        /// </summary>
        /// <param name="index">Item index.</param>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
        /// <param name="sizerFlags">
        /// A <see cref="ISizerFlags"/> object that enables you
        /// to specify most of the other parameters more conveniently.
        /// </param>
        ISizerItem Insert(int index, ISizer sizer, ISizerFlags sizerFlags);

        /// <summary>
        /// Insert a child into the sizer before any existing item at the specified index.
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
        /// Appends a child to the sizer.
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
        /// Appends a child to the sizer.
        /// </summary>
        /// <param name="sizer">
        /// The (child-)sizer to be added to the sizer. This allows placing a child sizer
        /// in a sizer and thus to create hierarchies of sizers (typically a vertical box
        /// as the top sizer and several horizontal boxes on the level beneath).
        /// </param>
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