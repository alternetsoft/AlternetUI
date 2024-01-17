using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        /// <param name="controlType">Type of the control.</param>
        /// <param name="renderSize">Render size. Ignored on most operating systems.</param>
        public static IReadOnlyFontAndColor GetStaticDefaultFontAndColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return new FontAndColor.ControlStaticDefaultFontAndColor(controlType, renderSize);
        }

        /*/// <summary>Test method.</summary>
        public static void TestPopupWindow(Control control, PointI pos, SizeI sz)
        {
            Native.WxOtherFactory.TestPopupWindow(control.WxWidget, pos, sz);
        }*/

        /// <summary>
        /// Substitutes <paramref name="control"/> with dummy <see cref="Control"/>, adding
        /// <paramref name="control"/> as its child.
        /// </summary>
        /// <param name="control">Control to substitute.</param>
        /// <returns></returns>
        public static Control SubstituteControl(Control control)
        {
            var parent = new Control();
            control.Parent = parent;
            control = parent;
            return control;
        }

        /// <summary>
        /// Returns the currently focused control, or <see langword="null"/> if
        /// no control is focused.
        /// </summary>
        public static Control? GetFocusedControl()
        {
            return ControlHandler.GetFocusedControl();
        }

        /// <summary>
        /// Generates new group index.
        /// </summary>
        public static int NewGroupIndex() => groupIndexCounter++;

        /// <summary>
        /// Gets <see cref="ControlDefaults"/> fof the specified <see cref="ControlTypeId"/>.
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        public static ControlDefaults GetDefaults(ControlTypeId controlId) =>
            AllPlatformDefaults.PlatformCurrent.Controls[controlId];

        /// <summary>
        /// Raises the window to the top of the window hierarchy (Z-order).
        /// This function only works for top level windows.
        /// </summary>
        /// <remarks>
        /// Notice that this function only requests the window manager to raise this window
        /// to the top of Z-order. Depending on its configuration, the window manager may
        /// raise the window, not do it at all or indicate that a window requested to be
        /// raised in some other way, e.g.by flashing its icon if it is minimized.
        /// </remarks>
        public virtual void Raise() => NativeControl.Raise();

        /// <summary>
        /// Sets the index of the child control in the <see cref="Children"/>.
        /// </summary>
        /// <param name="child">The item to search for.</param>
        /// <param name="newIndex">The new index value of the item.</param>
        /// <exception cref="ArgumentException">The item is not in the collection.</exception>
        /// <remarks>
        /// If <paramref name="newIndex"/> = -1, moves to the end of the collection.
        /// </remarks>
        public void SetChildIndex(Control child, int newIndex)
        {
            Children.SetItemIndex(child, newIndex);
            PerformLayout(false);
        }

        /// <summary>
        /// Brings the control to the front of the z-order.
        /// </summary>
        public void BringToFront() => Parent?.SetChildIndex(this, 0);

        /// <summary>
        /// Sends the control to the back of the z-order.
        /// </summary>
        public void SendToBack() => Parent?.SetChildIndex(this, -1);

        /// <summary>
        /// Lowers the window to the bottom of the window hierarchy (Z-order).
        /// This function only works for top level windows.
        /// </summary>
        public virtual void Lower() => NativeControl.Lower();

        /// <summary>
        /// Gets the background brush for specified state of the control.
        /// </summary>
        public virtual Brush? GetBackground(GenericControlState state)
        {
            return Backgrounds?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Gets the foreground brush for specified state of the control.
        /// </summary>
        public virtual Brush? GetForeground(GenericControlState state)
        {
            return Foregrounds?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Gets known svg color depending on the value of
        /// <see cref="IsDarkBackground"/> property.
        /// </summary>
        /// <param name="knownSvgColor">Known svg color identifier.</param>
        public virtual Color GetSvgColor(KnownSvgColor knownSvgColor)
        {
            return SvgColors.GetSvgColor(knownSvgColor, IsDarkBackground);
        }

        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        public virtual IReadOnlyFontAndColor GetDefaultFontAndColor()
        {
            return new FontAndColor.ControlDefaultFontAndColor(this);
        }

        /// <summary>
        /// Hides tooltip if it is visible. This method doesn't change <see cref="ToolTip"/>
        /// property.
        /// </summary>
        public virtual void HideToolTip()
        {
            if (NativeControl is null)
                return;
            NativeControl.UnsetToolTip();
            NativeControl.ToolTip = toolTip;
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        public virtual void ResetForegroundColor(ResetColorType method)
        {
            ResetColor(false, method);
        }

        /// <summary>
        /// Resets bacgkround color to the default value.
        /// </summary>
        public virtual void ResetBackgroundColor(ResetColorType method)
        {
            ResetColor(true, method);
        }

        /// <summary>
        /// Resets bacgkround color to the default value.
        /// </summary>
        public virtual void ResetBackgroundColor()
        {
            BackgroundColor = null;
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        public virtual void ResetForegroundColor()
        {
            ForegroundColor = null;
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        public virtual void ResetForeColor()
        {
            ForegroundColor = null;
        }

        /// <summary>
        /// Gets the subset of <see cref="Children"/> collection with
        /// child controls of specific type.
        /// </summary>
        /// <remarks>
        /// This method is useful, for example, when you need to get
        /// all <see cref="Button"/> or <see cref="CheckBox"/> child controls.
        /// </remarks>
        public IEnumerable<T> ChildrenOfType<T>()
        {
            if (HasChildren)
                return Children.OfType<T>();
            return Array.Empty<T>();
        }

        /// <summary>
        /// Executes a delegate asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters
        /// of the same number and type that are contained in the args parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to the
        /// given method. This can be <c>null</c> if no arguments are needed.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result
        /// of the operation.</returns>
        public IAsyncResult BeginInvoke(Delegate method, object?[] args)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return SynchronizationService.BeginInvoke(method, args);
        }

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's
        /// update region, which is the area that will be repainted at the next
        /// paint operation), and causes a paint message to be sent to the
        /// control.</summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region to invalidate.</param>
        public void Invalidate(RectD rect)
        {
            NativeControl.RefreshRect(rect, true);
        }

        /// <summary>
        /// Same as <see cref="Invalidate(RectD)"/> but has additional
        /// parameter <paramref name="eraseBackground"/>.
        /// </summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region to invalidate.</param>
        /// <param name="eraseBackground">Specifies whether to erase background.</param>
        public void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            NativeControl.RefreshRect(rect, eraseBackground);
        }

        /// <summary>
        /// Creates native control if its not already created.
        /// </summary>
        public virtual void HandleNeeded()
        {
            NativeControl?.Required();
        }

        /// <summary>
        /// Executes a delegate asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="method">A delegate to a method that takes no
        /// parameters.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of
        /// the operation.</returns>
        public virtual IAsyncResult BeginInvoke(Delegate method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return BeginInvoke(method, []);
        }

        /// <summary>
        /// Executes an action asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="action">An action to execute.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result
        /// of the operation.</returns>
        public virtual IAsyncResult BeginInvoke(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return BeginInvoke(action, []);
        }

        /// <summary>
        /// Retrieves the return value of the asynchronous operation represented
        /// by the <see cref="IAsyncResult"/> passed.
        /// </summary>
        /// <param name="result">The <see cref="IAsyncResult"/> that represents
        /// a specific invoke asynchronous operation, returned when calling
        /// <see cref="BeginInvoke(Delegate)"/>.</param>
        /// <returns>The <see cref="object"/> generated by the
        /// asynchronous operation.</returns>
        public virtual object? EndInvoke(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            return SynchronizationService.EndInvoke(result);
        }

        /// <summary>
        /// Executes the specified delegate, on the thread that owns the control,
        /// with the specified list of arguments.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of
        /// the same number and type that are contained in the
        /// <c>args</c> parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to
        /// the specified method. This parameter can be <c>null</c> if the
        /// method takes no arguments.</param>
        /// <returns>An <see cref="object"/> that contains the return value
        /// from the delegate being invoked, or <c>null</c> if the delegate has
        /// no return value.</returns>
        public virtual object? Invoke(Delegate method, object?[] args)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return SynchronizationService.Invoke(method, args);
        }

        /// <summary>
        /// Executes the specified delegate on the thread that owns the control.
        /// </summary>
        /// <param name="method">A delegate that contains a method to be called
        /// in the control's thread context.</param>
        /// <returns>An <see cref="object"/> that contains the return value from
        /// the delegate being invoked, or <c>null</c> if the delegate has no
        /// return value.</returns>
        public virtual object? Invoke(Delegate method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return Invoke(method, []);
        }

        /// <summary>
        /// Executes the specified action on the thread that owns the control.
        /// </summary>
        /// <param name="action">An action to be called in the control's
        /// thread context.</param>
        public virtual void Invoke(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Invoke(action, []);
        }

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        public virtual void CaptureMouse()
        {
            Handler?.CaptureMouse();
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        public virtual void ReleaseMouseCapture()
        {
            Handler?.ReleaseMouseCapture();
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public virtual void RaiseClick(EventArgs e)
        {
            OnClick(e);
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Idle"/> event and calls
        /// <see cref="OnIdle(EventArgs)"/>.
        /// See <see cref="Idle"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public virtual void RaiseIdle(EventArgs e)
        {
            OnIdle(e);
            Idle?.Invoke(this, e);
        }

        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        /// <remarks>Showing the control is equivalent to setting the
        /// <see cref="Visible"/> property to <c>true</c>.
        /// After the <see cref="Show"/> method is called, the
        /// <see cref="Visible"/> property
        /// returns a value of <c>true</c> until the <see cref="Hide"/> method
        /// is called.</remarks>
        public virtual void Show() => Visible = true;

        /// <summary>
        /// Gets the child control at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child control
        /// to get.</param>
        /// <returns>The child control at the specified index in the
        /// <see cref="Children"/> list.</returns>
        public virtual Control? GetChildOrNull(int index = 0)
        {
            if (!HasChildren)
                return null;
            if (index >= Children.Count || index < 0)
                return null;
            return Children[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public virtual ControlSet Group(params Control[] controls) => new(controls);

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which are members of the
        /// specified group.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public virtual ControlSet GetGroup(int groupIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.MemberOfGroup(groupIndex))
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetGroup(groupIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Gets all child controls recursively.
        /// </summary>
        public virtual ControlSet GetChildrenRecursive()
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                result.Add(control);
                ControlSet subSet = control.GetChildrenRecursive();
                result.AddRange(subSet.Items);
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Sets or releases mouse capture.
        /// </summary>
        /// <param name="value"><c>true</c> to set mouse capture; <c>false</c> to release it.</param>
        public virtual void SetMouseCapture(bool value)
        {
            NativeControl.SetMouseCapture(value);
        }

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which have <see cref="ColumnIndex"/>
        /// property equal to <paramref name="columnIndex"/>.
        /// </summary>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public virtual ControlSet GetColumnGroup(int columnIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.ColumnIndex == columnIndex)
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetColumnGroup(columnIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which have <see cref="RowIndex"/>
        /// property equal to <paramref name="rowIndex"/>.
        /// </summary>
        /// <param name="rowIndex">Column index.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public virtual ControlSet GetRowGroup(int rowIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.RowIndex == rowIndex)
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetRowGroup(rowIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Checks whether this control is a member of the specified group.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        public virtual bool MemberOfGroup(int groupIndex)
        {
            var indexes = GroupIndexes;

            if (indexes is null)
                return false;
            return Array.IndexOf<int>(indexes, groupIndex) >= 0;
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginUpdate"/>
        /// and <see cref="EndUpdate"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public virtual void DoInsideUpdate(Action action)
        {
            BeginUpdate();
            try
            {
                action();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Notifies that no more native control recreates are allowed.
        /// </summary>
        /// <remarks>
        /// When some properties are assigned, native control could be recreated.
        /// Sometimes it is not desired.
        /// This method sets a restriction on a control recreate, so debug message will be logged.
        /// </remarks>
        public void DisableRecreate()
        {
            NativeControl.DisableRecreate();
        }

        /// <summary>
        /// Allows control recreate which was previously disabled using <see cref="DisableRecreate"/>.
        /// </summary>
        /// See more details in <see cref="DisableRecreate"/>.
        public void EnableRecreate()
        {
            NativeControl.EnableRecreate();
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public virtual void DoInsideLayout(Action action)
        {
            SuspendLayout();
            try
            {
                action();
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Returns enumeration with the list of visible child controls.
        /// </summary>
        /// <seealso cref="GetVisibleChildOrNull"/>
        public virtual IReadOnlyList<Control> GetVisibleChildren()
        {
            if (HasChildren)
            {
                List<Control> result = [];
                foreach (var item in Children)
                {
                    if (item.Visible)
                        result.Add(item);
                }

                return result;
            }

            return Array.Empty<Control>();
        }

        /// <summary>
        /// Gets the child control at the specified index in the
        /// list of visible child controls.
        /// </summary>
        /// <param name="index">The zero-based index of the child control
        /// to get.</param>
        /// <returns>The child control at the specified index in the
        /// visible child controls list.</returns>
        /// <seealso cref="GetVisibleChildren"/>
        public virtual Control? GetVisibleChildOrNull(int index = 0)
        {
            var childs = GetVisibleChildren();
            foreach (Control control in childs)
            {
                if (!control.Visible)
                    continue;
                if (index == 0)
                    return control;
                index--;
            }

            return null;
        }

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <remarks>
        /// Hiding the control is equivalent to setting the
        /// <see cref="Visible"/> property to <c>false</c>.
        /// After the <see cref="Hide"/> method is called, the
        /// <see cref="Visible"/> property
        /// returns a value of <c>false</c> until the <see cref="Show"/> method
        /// is called.
        /// </remarks>
        public virtual void Hide() => Visible = false;

        /// <summary>
        /// Creates the <see cref="Graphics"/> for the control.
        /// </summary>
        /// <returns>The <see cref="Graphics"/> for the control.</returns>
        /// <remarks>
        /// The <see cref="Graphics"/> object that you retrieve through the
        /// <see cref="CreateDrawingContext"/> method should not normally
        /// be retained after the current UI event has been processed,
        /// because anything painted
        /// with that object will be erased with the next paint event. Therefore
        /// you cannot cache
        /// the <see cref="Graphics"/> object for reuse, except to use
        /// non-visual methods like
        /// <see cref="Graphics.MeasureText(string, Font)"/>.
        /// Instead, you must call <see cref="CreateDrawingContext"/> every time
        /// that you want to use the <see cref="Graphics"/> object,
        /// and then call its Dispose() when you are finished using it.
        /// </remarks>
        public virtual Graphics CreateDrawingContext() =>
            Handler.CreateDrawingContext();

        /// <summary>
        /// Same as <see cref="CreateDrawingContext"/>. Added mainly for legacy code.
        /// </summary>
        /// <returns></returns>
        public virtual Graphics CreateGraphics() =>
            Handler.CreateGraphics();

        /// <summary>
        /// Sets the specified bounds of the control to new location and size.
        /// </summary>
        /// <param name="newBounds">New location and size.</param>
        /// <param name="specified">Specifies which bounds to use when applying new
        /// location and size.</param>
        public virtual void SetBounds(RectD newBounds, BoundsSpecified specified)
        {
            SetBounds(newBounds.X, newBounds.Y, newBounds.Width, newBounds.Height, specified);
        }

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
        public virtual void SetBounds(
            double x,
            double y,
            double width,
            double height,
            BoundsSpecified specified)
        {
            var bounds = Bounds;
            RectD result = new(x, y, width, height);

            if ((specified & BoundsSpecified.X) == 0)
                result.X = bounds.X;

            if ((specified & BoundsSpecified.Y) == 0)
                result.Y = bounds.Y;

            if ((specified & BoundsSpecified.Width) == 0)
                result.Width = bounds.Width;

            if ((specified & BoundsSpecified.Height) == 0)
                result.Height = bounds.Height;

            if (result != bounds)
                Handler.Bounds = result;
        }

        /// <summary>
        /// Forces the control to invalidate itself and immediately redraw itself
        /// and any child controls. Calls <see cref="Invalidate()"/> and <see cref="Update"/>.
        /// </summary>
        public virtual void Refresh()
        {
            Invalidate();
            Update();
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public virtual void Invalidate()
        {
            if (Parent != null || this is Window)
                Handler?.Invalidate();
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public virtual void Update()
        {
            if (Parent != null || this is Window)
                Handler?.Update();
        }

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        /// <remarks>
        /// The layout logic of the control is suspended until the
        /// <see cref="ResumeLayout"/> method is called.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/>
        /// methods are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the
        /// <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then
        /// call the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public void SuspendLayout()
        {
            layoutSuspendCount++;
        }

        /// <summary>
        /// Gets the sizer of which this control is a member, if any, otherwise <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public ISizer? GetContainingSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetContainingSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Gets the sizer associated with the control by a previous call to <see cref="SetSizer"/>,
        /// or <c>null</c>.
        /// </summary>
        public ISizer? GetSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Sets the control to have the given layout sizer.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        /// <remarks>
        /// The control will then own the object, and will take care of its deletion.
        /// If an existing layout constraints object is already owned by the control,
        /// it will be deleted if the <paramref name="deleteOld"/> parameter is <c>true</c>.
        /// </remarks>
        /// <remarks>
        /// This function will also update layout so that the sizer will be effectively
        /// used to layout the control children whenever it is resized.
        /// </remarks>
        /// <remarks>
        /// This function enables and disables Layout automatically.
        /// </remarks>
        public void SetSizer(ISizer? sizer, bool deleteOld = true)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizer(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizer(sizer.Handle, deleteOld);
        }

        /// <summary>
        /// Gets <see cref="Display"/> where this control is shown.
        /// </summary>
        /// <returns></returns>
        public Display GetDisplay() => new(this);

        /// <summary>
        /// This method calls SetSizer() and then updates the initial control size to the
        /// size needed to accommodate all sizer elements and sets the size hints which,
        /// if this control is a top level one, prevent the user from resizing it to be
        /// less than this minimal size.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        public void SetSizerAndFit(ISizer? sizer, bool deleteOld = false)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizerAndFit(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizerAndFit(sizer.Handle, deleteOld);
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point on the screen
        /// to client-area coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that specifies the
        /// screen coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public PointD ScreenToClient(PointD point)
        {
            return Handler.ScreenToClient(point);
        }

        /// <summary>
        /// Converts the client-area coordinates of a specified point to
        /// screen coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that contains the
        /// client coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public PointD ClientToScreen(PointD point)
        {
            return Handler.ClientToScreen(point);
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point in
        /// device-independent units (1/96th inch per unit) to device (pixel) coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that specifies the
        /// screen device-independent coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public PointI ScreenToDevice(PointD point)
        {
            return Handler.ScreenToDevice(point);
        }

        /// <summary>
        /// Converts the device (pixel) coordinates of a specified point
        /// to coordinates in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that contains the coordinates
        /// in device-independent units (1/96th inch per unit) to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public PointD DeviceToScreen(PointI point)
        {
            return Handler.DeviceToScreen(point);
        }

        /// <summary>
        /// Changes <see cref="Cursor"/> property.
        /// </summary>
        /// <param name="value">New cursor.</param>
        public void SetCursor(Cursor? value)
        {
            Cursor = value;
        }

        /// <summary>
        /// Resumes the usual layout logic.
        /// </summary>
        /// <param name="performLayout"><c>true</c> to execute pending
        /// layout requests; otherwise, <c>false</c>.</param>
        /// <remarks>
        /// Resumes the usual layout logic after <see cref="SuspendLayout"/> has
        /// been called.
        /// When the <c>performLayout</c> parameter is set to <c>true</c>,
        /// an immediate layout occurs.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/> methods
        /// are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the
        /// <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then call
        /// the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public virtual void ResumeLayout(bool performLayout = true)
        {
            layoutSuspendCount--;
            if (layoutSuspendCount < 0)
                throw new InvalidOperationException();

            if (!IsLayoutSuspended)
            {
                if (performLayout)
                    PerformLayout();
            }
        }

        /// <summary>
        /// Maintains performance while performing slow operations on a control
        /// by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        public virtual int BeginUpdate()
        {
            updateCount++;
            Handler?.BeginUpdate();
            return updateCount;
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the
        /// <see cref="BeginUpdate"/> method.
        /// </summary>
        public virtual int EndUpdate()
        {
            updateCount--;
            Handler?.EndUpdate();
            return updateCount;
        }

        /// <summary>
        /// Gets child with the specified id.
        /// </summary>
        /// <param name="id">Child control id.</param>
        public Control? FindChild(ObjectUniqueId? id)
        {
            if (id is null)
                return null;
            foreach (var item in Children)
            {
                if (item.UniqueId == id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SuspendLayout"/> method was called before calling
        /// the <see cref="PerformLayout"/> method,
        /// the layout is suppressed.
        /// </remarks>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        public virtual void PerformLayout(bool layoutParent = true)
        {
            if (IsLayoutSuspended)
                return;

            if (inLayout)
                return;

            inLayout = true;
            try
            {
                if (layoutParent)
                    Parent?.PerformLayout();

                OnLayout();
            }
            finally
            {
                inLayout = false;
            }

            RaiseLayoutUpdated();
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="availableSize">The available space that a parent element
        /// can allocate a child control.</param>
        /// <returns>A <see cref="SuggestedSize"/> representing the width and height of
        /// a rectangle, in device-independent units (1/96th inch per unit).</returns>
        public virtual SizeD GetPreferredSize(SizeD availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Sets bounds of the control using <paramref name="rect"/> and <paramref name="flags"/>.
        /// </summary>
        /// <param name="rect">Rectangle.</param>
        /// <param name="flags">Flags.</param>
        public virtual void SetBounds(RectD rect, SetBoundsFlags flags)
        {
            NativeControl.SetBoundsEx(rect, (int)flags);
        }

        /// <summary>
        /// Starts the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to start
        /// the initialization of a control.
        /// The <see cref="EndInit"/> method ends the initialization. Using the
        /// <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void BeginInit()
        {
            SuspendLayout();
            Handler?.BeginInit();
        }

        /// <summary>
        /// Ends the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to end
        /// the initialization of a control.
        /// The <see cref="BeginInit"/> method starts the initialization. Using
        /// the <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void EndInit()
        {
            Handler?.EndInit();
            ResumeLayout();
        }

        /// <summary>
        /// Same as <see cref="Enabled"/> but implemented as method.
        /// </summary>
        /// <param name="value"></param>
        public void SetEnabled(bool value) => Enabled = value;

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns><see langword="true"/> if the input focus request was
        /// successful; otherwise, <see langword="false"/>.</returns>
        /// <remarks>The <see cref="SetFocus"/> method returns true if the
        /// control successfully received input focus.</remarks>
        public virtual bool SetFocus()
        {
            return Handler.SetFocus();
        }

        /// <summary>
        /// Sets input focus to the control if it can accept it.
        /// </summary>
        public virtual bool SetFocusIfPossible()
        {
            if (CanAcceptFocus)
                return SetFocus();
            else
                return false;
        }

        /// <summary>
        /// Saves screenshot of this control.
        /// </summary>
        /// <param name="fileName">Name of the file to which screenshot
        /// will be saved.</param>
        /// <remarks>This function works only on Windows.</remarks>
        public virtual void SaveScreenshot(string fileName)
        {
            Handler?.SaveScreenshot(fileName);
        }

        /// <summary>
        /// Pops up the given menu at the specified coordinates, relative to this window,
        /// and returns control when the user has dismissed the menu.
        /// </summary>
        /// <remarks>
        /// If a menu item is selected, the corresponding menu event is generated and will
        /// be processed as usual. If coordinates are not specified (-1,-1), the current
        /// mouse cursor position is used.
        /// </remarks>
        /// <remarks>
        /// It is recommended to not explicitly specify coordinates when calling PopupMenu
        /// in response to mouse click, because some of the ports(namely, on Linux)
        /// can do a better job of positioning the menu in that case.
        /// </remarks>
        /// <param name="menu">The menu to pop up.</param>
        /// <param name="x">The X position in dips where the menu will appear.</param>
        /// <param name="y">The Y position in dips where the menu will appear.</param>
        /// <remarks>Position is specified in device independent units (1/96 inch).</remarks>
        public virtual void ShowPopupMenu(ContextMenu? menu, double x = -1, double y = -1)
        {
            if (menu is null || menu.Items.Count == 0)
                return;
            var e = new CancelEventArgs();
            menu.RaiseOpening(e);
            if (e.Cancel)
                return;
            Handler.ShowPopupMenu(menu, x, y);
            menu.RaiseClosing(e);
        }

        /// <summary>
        /// Gets children as <see cref="ControlSet"/>.
        /// </summary>
        /// <param name="recursive">Whether to get all children recurively.</param>
        /// <returns></returns>
        public virtual ControlSet GetChildren(bool recursive = false)
        {
            if (recursive)
                return GetChildrenRecursive();
            else
                return ControlSet.New(Children);
        }

        /// <summary>
        /// Resets <see cref="SuggestedHeight"/> property.
        /// </summary>
        public virtual void ResetSuggestedHeight()
        {
            SuggestedHeight = double.NaN;
        }

        /// <summary>
        /// Resets <see cref="SuggestedWidth"/> property.
        /// </summary>
        public virtual void ResetSuggestedWidth()
        {
            SuggestedWidth = double.NaN;
        }

        /// <summary>
        /// Resets <see cref="SuggestedSize"/> property.
        /// </summary>
        public virtual void ResetSuggestedSize()
        {
            SuggestedSize = SizeD.NaN;
        }

        /// <summary>
        /// Gets <see cref="ToolTip"/> value for use in the native control.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Override this method to customize tooltip. For example <see cref="SpeedButton"/>
        /// overrides it to add shortcuts.
        /// </remarks>
        public virtual string? GetRealToolTip()
        {
            return ToolTip;
        }

        /// <summary>
        /// Gets children as <see cref="ControlSet"/>.
        /// </summary>
        /// <param name="recursive">Whether to get all children recurively.</param>
        /// <returns></returns>
        public virtual ControlSet GetChildren<T>(bool recursive = false)
        {
            ControlSet result;

            if (recursive)
                result = GetChildrenRecursive();
            else
            if (HasChildren)
                result = ControlSet.New(Children);
            else
                result = ControlSet.Empty;

            var limitedResult = new List<Control>();

            foreach(var item in result.Items)
            {
                if (item is T)
                    limitedResult.Add(item);
            }

            return ControlSet.New(limitedResult);
        }

        /// <summary>
        /// Sets children font.
        /// </summary>
        /// <param name="font">New font value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenFont(Font? font, bool recursive = false)
        {
            GetChildren(recursive).Font(font);
        }

        /// <summary>
        /// Sets children background color.
        /// </summary>
        /// <param name="color">New background color value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenBackgroundColor(Color? color, bool recursive = false)
        {
            GetChildren(recursive).BackgroundColor(color);
        }

        /// <summary>
        /// Sets children background color.
        /// </summary>
        /// <param name="color">New background color value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenBackgroundColor<T>(Color? color, bool recursive = false)
        {
            GetChildren<T>(recursive).BackgroundColor(color);
        }

        /// <summary>
        /// Sets children foreground color.
        /// </summary>
        /// <param name="color">New foreground color value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenForegroundColor(Color? color, bool recursive = false)
        {
            GetChildren(recursive).ForegroundColor(color);
        }

        /// <summary>
        /// Sets children foreground color.
        /// </summary>
        /// <param name="color">New foreground color value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenForegroundColor<T>(Color? color, bool recursive = false)
        {
            GetChildren<T>(recursive).ForegroundColor(color);
        }

        /// <summary>
        /// Focuses the next control.
        /// </summary>
        /// <param name="forward"><see langword="true"/> to move forward in the
        /// tab order; <see langword="false"/> to move backward in the tab
        /// order.</param>
        /// <param name="nested"><see langword="true"/> to include nested
        /// (children of child controls) child controls; otherwise,
        /// <see langword="false"/>.</param>
        public virtual void FocusNextControl(bool forward = true, bool nested = true)
        {
            Handler?.FocusNextControl(forward, nested);
        }

        /// <summary>
        /// Begins a drag-and-drop operation.
        /// </summary>
        /// <remarks>
        /// Begins a drag operation. The <paramref name="allowedEffects"/>
        /// determine which drag operations can occur.
        /// </remarks>
        /// <param name="data">The data to drag.</param>
        /// <param name="allowedEffects">One of the
        /// <see cref="DragDropEffects"/> values.</param>
        /// <returns>
        /// A value from the <see cref="DragDropEffects"/> enumeration that
        /// represents the final effect that was
        /// performed during the drag-and-drop operation.
        /// </returns>
        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            allowedEffects &= ~DragDropEffects.Scroll;
            return Handler.DoDragDrop(data, allowedEffects);
        }

        /// <summary>
        /// Forces the re-creation of the underlying native control.
        /// </summary>
        public virtual void RecreateWindow()
        {
            Handler?.NativeControl?.RecreateWindow();
        }

        /// <summary>
        /// Returns the DPI of the display used by this control.
        /// </summary>
        /// <remarks>
        /// The returned value is different for different windows on
        /// systems with support for per-monitor DPI values,
        /// such as Microsoft Windows.
        /// </remarks>
        /// <returns>
        /// A <see cref="Size"/> value that represents DPI of the display
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        public virtual SizeD GetDPI()
        {
            if (Handler != null)
                return Handler.GetDPI();
            if (Parent != null)
                return Parent.GetDPI();
            throw new NotSupportedException();
        }

        /// <summary>
        /// Checks whether using transparent background might work.
        /// </summary>
        /// <returns><c>true</c> if background transparency is supported.</returns>
        /// <remarks>
        /// If this function returns <c>false</c>, setting <see cref="BackgroundStyle"/> with
        /// <see cref="ControlBackgroundStyle.Transparent"/> is not going to work. If it
        /// returns <c>true</c>, setting transparent style should normally succeed.
        /// </remarks>
        /// <remarks>
        /// Notice that this function would typically be called on the parent of a
        /// control you want to set transparent background style for as the control for
        /// which this method is called must be fully created.
        /// </remarks>
        public bool IsTransparentBackgroundSupported()
        {
            return NativeControl?.IsTransparentBackgroundSupported() ?? false;
        }

        /// <summary>
        /// Called when the control should reposition its child controls.
        /// </summary>
        public virtual void OnLayout()
        {
            Handler?.OnLayout();
        }

        /// <summary>
        /// Call this function to force one or both scrollbars to be always shown, even if
        /// the control is big enough to show its entire contents without scrolling.
        /// </summary>
        /// <param name="hflag">Whether the horizontal scroll bar should always be visible.</param>
        /// <param name="vflag">Whether the vertical scroll bar should always be visible.</param>
        /// <remarks>
        /// This function is currently only implemented under Mac/Carbon.
        /// </remarks>
        public void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
            NativeControl?.AlwaysShowScrollbars(hflag, vflag);
        }

        /// <summary>
        /// Performs some action for the each child of the control.
        /// </summary>
        /// <typeparam name="T">Specifies type of the child control.</typeparam>
        /// <param name="action">Specifies action which will be called for the
        /// each child.</param>
        public virtual void ForEachChild<T>(Action<T> action)
        {
            foreach (var child in ChildrenOfType<T>())
                action(child);
        }

        /// <summary>
        /// Disable control recreate when properties that require control
        /// recreation are changed.
        /// </summary>
        public virtual void BeginIgnoreRecreate()
        {
            Handler?.BeginIgnoreRecreate();
        }

        /// <summary>
        /// Creates new <see cref="HorizontalStackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual HorizontalStackPanel AddHorizontalStackPanel()
        {
            var result = new HorizontalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="Button"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual Button AddButton(string text, Action? action = null)
        {
            var result = new Button(text)
            {
                Parent = this,
            };

            if (action is not null)
                result.Click += Result_Click;

            return result;

            void Result_Click(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="CheckBox"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual CheckBox AddCheckBox(string text, Action? action = null)
        {
            var result = new CheckBox(text)
            {
                Parent = this,
            };

            if (action is not null)
                result.CheckedChanged += Result_CheckedChanged;

            return result;

            void Result_CheckedChanged(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="Label"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual Label AddLabel(string text)
        {
            var result = new Label(text)
            {
                Parent = this,
            };

            return result;
        }

        /// <summary>
        /// Gets whether one of this control's parents equals <paramref name="testParent"/>.
        /// </summary>
        /// <param name="testParent">Control to test as an indirect parent.</param>
        public virtual bool HasIndirectParent(Control? testParent)
        {
            var p = Parent;
            while (true)
            {
                if (p == testParent)
                    return true;
                if (p == null)
                    return false;
                p = p.Parent;
            }
        }

        /// <summary>
        /// Creates new <see cref="VerticalStackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual VerticalStackPanel AddVerticalStackPanel()
        {
            var result = new VerticalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual GroupBox AddGroupBox(string? title = default)
        {
            var result = new GroupBox
            {
                Parent = this,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="StackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual StackPanel AddStackPanel(bool isVertical = true)
        {
            StackPanelOrientation orientation;

            if (isVertical)
                orientation = StackPanelOrientation.Vertical;
            else
                orientation = StackPanelOrientation.Horizontal;

            var result = new StackPanel
            {
                Parent = this,
                Orientation = orientation,
            };
            return result;
        }

        /// <summary>
        /// Sets background color if <see cref="UseDebugBackgroundColor"/> is <c>true</c>
        /// and DEBUG conditional is defined.
        /// </summary>
        /// <param name="color">Debug background color.</param>
        /// <param name="debugMsg">Optional debug message to show in log.</param>
        [Conditional("DEBUG")]
        public virtual void DebugBackgroundColor(Color? color, string? debugMsg = default)
        {
            if (UseDebugBackgroundColor)
            {
                BackgroundColor = color;
                if (debugMsg is not null)
                    LogUtils.LogColor(debugMsg, color);
            }
        }

        /// <summary>
        /// Enable control recreate if it's required after it was previously
        /// disabled by <see cref="BeginIgnoreRecreate"/>
        /// </summary>
        public virtual void EndIgnoreRecreate()
        {
            Handler?.EndIgnoreRecreate();
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public int PixelFromDip(double value)
        {
            return Native.Control.DrawingFromDip(value, this.WxWidget);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public SizeI PixelFromDip(SizeD value)
        {
            return new(PixelFromDip(value.Width), PixelFromDip(value.Height));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public PointI PixelFromDip(PointD value)
        {
            return new(PixelFromDip(value.X), PixelFromDip(value.Y));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public RectI PixelFromDip(RectD value)
        {
            return new(PixelFromDip(value.Location), PixelFromDip(value.Size));
        }

        /// <summary>
        /// Same as <see cref="Application.Log"/>.
        /// </summary>
        /// <param name="s"></param>
        public void Log(object? s) => Application.Log(s);

        /// <summary>
        /// Gets scale factor used in device-independent units (1/96th inch per unit) to/from
        /// pixels conversions.
        /// </summary>
        /// <returns></returns>
        public double GetPixelScaleFactor()
        {
            return Native.Control.DrawingDPIScaleFactor(this.WxWidget);
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        public SizeD PixelToDip(SizeI value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        public PointD PixelToDip(PointI value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        public RectD PixelToDip(RectI value)
        {
            return new(PixelToDip(value.Location), PixelToDip(value.Size));
        }

        /// <summary>
        /// Converts pixels to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        public double PixelToDip(int value)
        {
            return Native.Control.DrawingToDip(value, this.WxWidget);
        }

        /// <summary>
        /// Sets image for the specified control state.
        /// </summary>
        /// <param name="value">Image.</param>
        /// <param name="state">Control state.</param>
        public void SetImage(Image? value, GenericControlState state = GenericControlState.Normal)
        {
            StateObjects ??= new();
            StateObjects.Images ??= new();
            StateObjects.Images.SetObject(value, state);
        }

        /// <summary>
        /// Sets background brush for the specified control state.
        /// </summary>
        /// <param name="value">Background brush.</param>
        /// <param name="state">Control state.</param>
        public void SetBackground(Brush? value, GenericControlState state = GenericControlState.Normal)
        {
            StateObjects ??= new();
            StateObjects.Backgrounds ??= new();
            StateObjects.Backgrounds.SetObject(value, state);
        }

        /// <summary>
        /// Sets border settings for the specified control state.
        /// </summary>
        /// <param name="value">Border settings.</param>
        /// <param name="state">Control state.</param>
        public void SetBorder(BorderSettings? value, GenericControlState state = GenericControlState.Normal)
        {
            StateObjects ??= new();
            StateObjects.Borders ??= new();
            StateObjects.Borders.SetObject(value, state);
        }

        /// <summary>
        /// Resets the <see cref="Control.Font" /> property to its default value.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetFont()
        {
            Font = null;
        }

        /// <summary>
        /// Resets the <see cref="Control.Cursor" /> property to its default value.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetCursor()
        {
            Cursor = null;
        }

        /// <summary>
        /// Resets the <see cref="Control.BackColor" /> property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void ResetBackColor()
        {
            BackgroundColor = null;
        }

        /// <summary>
        /// Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="p">The client coordinate <see cref="PointD" /> to convert.</param>
        /// <returns>A <see cref="PointD" /> that represents the converted <see cref="PointD" />,
        /// <paramref name="p" />, in screen coordinates.</returns>
        public PointD PointToScreen(PointD p)
        {
            return ClientToScreen(p);
        }

        /// <summary>
        /// Computes the location of the specified screen point into client coordinates.
        /// </summary>
        /// <param name="p">The screen coordinate <see cref="PointD" /> to convert.</param>
        /// <returns> A <see cref="PointD" /> that represents the converted
        /// <see cref="PointD" />, <paramref name="p" />, in client coordinates.</returns>
        public PointD PointToClient(PointD p)
        {
            return ScreenToClient(p);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public double PixelFromDipF(double value)
        {
            return Native.Control.DrawingFromDipF(value, this.WxWidget);
        }

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update
        /// region, which is the area that will be repainted at the next paint operation), and
        /// causes a paint message to be sent to the control. Optionally, invalidates the
        /// child controls assigned to the control.</summary>
        /// <param name="region">The <see cref="Region" /> to invalidate.</param>
        /// <param name="invalidateChildren">
        /// <see langword="true" /> to invalidate the control's child controls;
        /// otherwise, <see langword="false"/>.</param>
        public void Invalidate(Region? region, bool invalidateChildren = false)
        {
            if (region is null || !region.IsOk || region.IsEmpty)
                return;
            var rect = region.GetBounds();
            Invalidate(rect);
        }

        /// <summary>
        /// Sets system scrollbar properties.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <param name="visible">Is scrollbar visible or not.</param>
        /// <param name="value">Thumb position.</param>
        /// <param name="largeChange">Large change value (when scrolls page up or down).</param>
        /// <param name="maximum">Scrollbar Range.</param>
        public void SetScrollBar(bool isVertical, bool visible, int value, int largeChange, int maximum)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            NativeControl.SetScrollBar(orientation, visible, value, largeChange, maximum);
        }

        /// <summary>
        /// Gets whether system scrollbar is visible.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public bool IsScrollBarVisible(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.IsScrollBarVisible(orientation);
        }

        /// <summary>
        /// Gets system scrollbar thumb position.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarValue(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarValue(orientation);
        }

        /// <summary>
        /// Gets system scrollbar large change value.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarLargeChange(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarLargeChange(orientation);
        }

        /// <summary>
        /// Gets system scrollbar max range.
        /// </summary>
        /// <param name="isVertical">Vertical or horizontal scroll bar.</param>
        /// <returns></returns>
        public int GetScrollBarMaximum(bool isVertical)
        {
            Native.ScrollBarOrientation orientation = isVertical
                ? Native.ScrollBarOrientation.Vertical : Native.ScrollBarOrientation.Horizontal;
            return NativeControl.GetScrollBarMaximum(orientation);
        }
    }
}
