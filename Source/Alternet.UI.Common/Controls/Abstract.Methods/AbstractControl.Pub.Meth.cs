﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Adds <see cref="IControlNotification"/> object to the global list of notifications.
        /// </summary>
        /// <param name="n">Notification object to add.</param>
        /// <remarks>
        /// You must also call <see cref="RemoveGlobalNotification"/> when subscriber is disposed.
        /// </remarks>
        public static void AddGlobalNotification(IControlNotification n)
        {
            globalNotifications ??= new();
            globalNotifications.Add(n);
        }

        /// <summary>
        /// Removes <see cref="IControlNotification"/> object from the global list of notifications.
        /// </summary>
        /// <param name="n">Notification object to remove.</param>
        public static void RemoveGlobalNotification(IControlNotification n)
        {
            if (globalNotifications is null)
                return;
            globalNotifications.Remove(n);
        }

        /// <summary>
        /// Finds control which accepts mouse events (checks whether <see cref="InputTransparent"/>
        /// property is <c>true</c>). Returns control specified as a parameter or one
        /// of its parent controls.
        /// </summary>
        /// <param name="control">Control to check.</param>
        /// <returns></returns>
        public static AbstractControl? GetMouseTargetControl(AbstractControl? control)
        {
            var result = control;

            while (result is not null)
            {
                if (result.InputTransparent)
                    result = result.Parent;
                else
                    return result;
            }

            return result;
        }

        /// <summary>
        /// Called when the control should
        /// reposition its child controls.
        /// </summary>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.OnLayout"/>.
        /// </remarks>
        /// <param name="container">Container control which childs will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        /// <param name="getBounds">Returns rectangle in which layout is performed.</param>
        /// <param name="items">List of controls to layout.</param>
        public static void DefaultOnLayout(
            AbstractControl container,
            LayoutStyle layout,
            Func<RectD> getBounds,
            IReadOnlyList<AbstractControl> items)
        {
            if(layout == LayoutStyle.Scroll)
            {
                OldLayout.LayoutWhenScroll(container, getBounds, items, true);
                return;
            }

            var space = getBounds();

            var number = LayoutWhenDocked(container, ref space, items);

            void UpdateItems()
            {
                if (number == 0 || number == items.Count)
                    return;
                var newItems = new List<AbstractControl>();
                foreach (var item in items)
                {
                    if (item.Dock == DockStyle.None && !item.IgnoreLayout)
                        newItems.Add(item);
                }

                items = newItems;
            }

            switch (layout)
            {
                case LayoutStyle.Basic:
                    UpdateItems();
                    UI.AbstractControl.PerformDefaultLayout(container, space, items);
                    break;
                case LayoutStyle.Vertical:
                    UpdateItems();
                    LayoutWhenVertical(container, space, items);
                    break;
                case LayoutStyle.Horizontal:
                    UpdateItems();
                    LayoutWhenHorizontal(container, space, items);
                    break;
            }
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units.
        /// </summary>
        /// <param name="availableSize">The available space that a parent element
        /// can allocate a child control.</param>
        /// <returns>A <see cref="SuggestedSize"/> representing the width and height of
        /// a rectangle, in device-independent units.</returns>
        /// <remarks>
        /// This is a default implementation which is called from
        /// <see cref="AbstractControl.GetPreferredSize(SizeD)"/>.
        /// </remarks>
        /// <param name="container">Container control which childs will be processed.</param>
        /// <param name="layout">Layout style to use.</param>
        public static SizeD DefaultGetPreferredSize(
            AbstractControl container,
            SizeD availableSize,
            LayoutStyle layout)
        {
            switch (layout)
            {
                case LayoutStyle.Dock:
                case LayoutStyle.None:
                default:
                    return GetPreferredSizeDefaultLayout(container, availableSize);
                case LayoutStyle.Basic:
                    return AbstractControl.GetPreferredSizeDefaultLayout(container, availableSize);
                case LayoutStyle.Vertical:
                    return AbstractControl.GetPreferredSizeWhenVertical(container, availableSize);
                case LayoutStyle.Horizontal:
                    return AbstractControl.GetPreferredSizeWhenHorizontal(container, availableSize);
                case LayoutStyle.Scroll:
                    return AbstractControl.GetPreferredSizeWhenScroll(container, availableSize);
            }
        }

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

        /// <summary>
        /// Returns the currently hovered control, or <see langword="null"/> if
        /// no control is under the mouse.
        /// </summary>
        [Browsable(false)]
        public static AbstractControl? GetHoveredControl()
        {
            return HoveredControl;
        }

        /// <summary>
        /// Generates new group index.
        /// </summary>
        [Browsable(false)]
        public static int NewGroupIndex() => groupIndexCounter++;

        /// <summary>
        /// Gets <see cref="ControlDefaults"/> fof the specified <see cref="ControlTypeId"/>.
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        public static ControlDefaults GetDefaults(ControlTypeId controlId) =>
            AllPlatformDefaults.PlatformCurrent.Controls[controlId];

        /// <summary>
        /// Called by the child control when its property is changed.
        /// </summary>
        /// <param name="child">Child control.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="directChild">Whether child is direct or not (child of the child).</param>
        /// <remarks>
        /// It's up to the child control to decide on what property changes
        /// to inform the container control. Call this method
        /// in the parent control to notify about property change.
        /// </remarks>
        /// <remarks>
        /// By default in <see cref="AbstractControl"/> it is called for <see cref="Title"/>,
        /// <see cref="Enabled"/> and some other properties.
        /// </remarks>
        public virtual void OnChildPropertyChanged(
            AbstractControl child,
            string propName,
            bool directChild = true)
        {
        }

        /// <summary>
        /// Gets whether the specified character is valid for the input.
        /// This function allows to ignore unwanted characters in the input.
        /// </summary>
        /// <param name="ch">Character to check.</param>
        /// <returns></returns>
        public virtual bool IsValidInputChar(char ch)
        {
            if ((CharValidator is not null) && !IgnoreCharValidator)
            {
                if(ch < 32)
                {
                    if (Alternet.UI.CharValidator.AlwaysValidControlChars)
                        return true;
                }

                var isValid = CharValidator.IsValid(ch);
                if (!isValid)
                {
                    isValid = Array.IndexOf(Alternet.UI.CharValidator.AlwaysValidChars, ch) >= 0;
                }

                return isValid;
            }

            return true;
        }

        /// <summary>
        /// Sets <see cref="Title"/> property.
        /// </summary>
        /// <param name="title">New title</param>
        public void SetTitle(string? title)
        {
            Title = title ?? string.Empty;
        }

        /// <summary>
        /// Sets the index of the child control in the <see cref="Children"/>.
        /// </summary>
        /// <param name="child">The item to search for.</param>
        /// <param name="newIndex">The new index value of the item.</param>
        /// <exception cref="ArgumentException">The item is not in the collection.</exception>
        /// <remarks>
        /// If <paramref name="newIndex"/> = -1, moves to the end of the collection.
        /// </remarks>
        public virtual void SetChildIndex(AbstractControl child, int newIndex)
        {
            Children.SetItemIndex(child, newIndex);
            PerformLayout(false);
        }

        /// <summary>
        /// Brings the control to the front of the z-order.
        /// </summary>
        [Browsable(false)]
        public void BringToFront() => Parent?.SetChildIndex(this, 0);

        /// <summary>
        /// Sends the control to the back of the z-order.
        /// </summary>
        [Browsable(false)]
        public void SendToBack() => Parent?.SetChildIndex(this, -1);

        /// <summary>
        /// Gets the background brush for specified state of the control.
        /// </summary>
        public virtual Brush? GetBackground(VisualControlState state)
        {
            return Backgrounds?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Gets the border settings for specified state of the control.
        /// </summary>
        public virtual BorderSettings? GetBorderSettings(VisualControlState state)
        {
            return Borders?.GetObjectOrNormal(state);
        }

        /// <summary>
        /// Gets the foreground brush for specified state of the control.
        /// </summary>
        public virtual Brush? GetForeground(VisualControlState state)
        {
            return Foregrounds?.GetObjectOrNull(state);
        }

        /// <summary>
        /// Gets known svg color depending on the value of
        /// <see cref="IsDarkBackground"/> property.
        /// </summary>
        /// <param name="knownSvgColor">Known svg color identifier.</param>
        [Browsable(false)]
        public virtual Color GetSvgColor(KnownSvgColor knownSvgColor = KnownSvgColor.Normal)
        {
            return SvgColors.GetSvgColor(knownSvgColor, IsDarkBackground);
        }

        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        [Browsable(false)]
        public virtual IReadOnlyFontAndColor GetDefaultFontAndColor()
        {
            return new FontAndColor.ControlDefaultFontAndColor(this);
        }

        /// <summary>
        /// Updates <see cref="ToolTip"/> so it will be repainted in the screen
        /// if it is currently shown.
        /// </summary>
        [Browsable(false)]
        public virtual void UpdateToolTip()
        {
            var s = ToolTip;
            ToolTip = null;
            ToolTip = s;
        }

        /// <summary>
        /// Hides tooltip if it is visible. This method doesn't change <see cref="ToolTip"/>
        /// property.
        /// </summary>
        public virtual void HideToolTip()
        {
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
        [Browsable(false)]
        public virtual void ResetBackgroundColor()
        {
            BackgroundColor = null;
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        [Browsable(false)]
        public virtual void ResetForegroundColor()
        {
            ForegroundColor = null;
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        [Browsable(false)]
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
        public virtual IEnumerable<T> ChildrenOfType<T>()
            where T : AbstractControl
        {
            if (HasChildren)
                return Children.OfType<T>();
            return Array.Empty<T>();
        }

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's
        /// update region, which is the area that will be repainted at the next
        /// paint operation), and causes a paint message to be sent to the
        /// control.</summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region
        /// to invalidate.</param>
        public virtual void Invalidate(RectD rect)
        {
            RefreshRect(rect, true);
            RaiseInvalidated(new(rect));
        }

        /// <summary>
        /// Same as <see cref="Invalidate(RectD)"/> but has additional
        /// parameter <paramref name="eraseBackground"/>.
        /// </summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region
        /// to invalidate.</param>
        /// <param name="eraseBackground">Specifies whether to erase background.</param>
        public virtual void RefreshRect(RectD rect, bool eraseBackground = true)
        {
            Refresh();
        }

        /// <summary>
        /// Repaints rectangles (coordinates in pixels) in the control.
        /// </summary>
        /// <param name="rects">Array of rectangles with coordinates specified in pixels.</param>
        /// <param name="eraseBackground">Specifies whether to erase background.</param>
        public virtual void RefreshRects(IEnumerable<RectI> rects, bool eraseBackground = true)
        {
            foreach (var rect in rects)
                RefreshRect(PixelToDip(rect), eraseBackground);
        }

        /// <summary>
        /// Invalidates internally painted caret.
        /// </summary>
        [Browsable(false)]
        public virtual void InvalidateCaret()
        {
            if (caretInfo is null || !caretInfo.Visible)
                return;
            RefreshRectsUnion(caretInfo.Region);
        }

        /// <summary>
        /// Calculates bounds of the specified rectangles collection and
        /// repaints combined rectangle. Coordinates are in pixels.
        /// </summary>
        /// <param name="rects">Array of rectangles with coordinates specified in pixels.</param>
        /// <param name="eraseBackground">Specifies whether to erase background.</param>
        public virtual void RefreshRectsUnion(IEnumerable<RectI> rects, bool eraseBackground = true)
        {
            var bounds = SkiaUtils.GetBounds(rects);

            if (bounds is null)
                return;

            RefreshRect(PixelToDip(bounds.Value), eraseBackground);
        }

        /// <summary>
        /// Creates native control if its not already created.
        /// </summary>
        [Browsable(false)]
        public virtual void HandleNeeded()
        {
        }

        /// <summary>
        /// Calls <see cref="PerformLayout"/> and <see cref="Invalidate()"/>.
        /// </summary>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        /// <param name="action">
        /// The action to execute between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </param>
        [Browsable(false)]
        public virtual void PerformLayoutAndInvalidate(
            Action? action = null,
            bool layoutParent = true)
        {
            if (action is null)
                PerformLayout(layoutParent);
            else
                DoInsideLayout(action);
            Invalidate();
        }

        /// <summary>
        /// Sets value of the <see cref="Text"/> property.
        /// </summary>
        /// <param name="value">New value of the <see cref="Text"/> property.</param>
        public void SetText(object? value) => Text = value.SafeToString();

        /// <summary>
        /// Executes <see cref="Action"/> and calls <see cref="ProcessException"/>
        /// event if exception was raised during execution.
        /// </summary>
        /// <param name="action">Action to execute.</param>
        public virtual bool AvoidException(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception exception)
            {
                ThrowExceptionEventArgs data = new(exception);
                RaiseProcessException(data);
                if (data.ThrowIt)
                    throw;
                return false;
            }
        }

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        [Browsable(false)]
        public virtual void CaptureMouse()
        {
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        [Browsable(false)]
        public virtual void ReleaseMouseCapture()
        {
        }

        /// <summary>
        /// Runs generic action.
        /// </summary>
        /// <param name="action">Action to run.</param>
        public virtual void RunKnownAction(GenericControlAction action)
        {
            if (action == GenericControlAction.None)
                return;

            switch (action)
            {
                case GenericControlAction.ShowKeyboard:
                    Keyboard.ShowKeyboard(this);
                    break;
                case GenericControlAction.HideKeyboard:
                    Keyboard.HideKeyboard(this);
                    break;
                case GenericControlAction.ShowKeyboardIfNoHardware:
                    if(Keyboard.IsKeyboardPresent is false)
                        Keyboard.ShowKeyboard(this);
                    break;
                case GenericControlAction.ShowKeyboardIfUnknown:
                    if (Keyboard.IsKeyboardPresent is not true)
                        Keyboard.ShowKeyboard(this);
                    break;
            }
        }

        /// <summary>
        /// Calls appropriate mouse events using specified <see cref="TouchEventArgs"/>.
        /// </summary>
        /// <param name="e">Touch event arguments.</param>
        public virtual void TouchToMouseEvents(TouchEventArgs e)
        {
            switch (e.DeviceType)
            {
                case TouchDeviceType.Touch:
                case TouchDeviceType.Mouse:
                    switch (e.ActionType)
                    {
                        case TouchAction.Entered:
                            RaiseMouseEnter(e);
                            break;
                        case TouchAction.Pressed:
                            AbstractControl.BubbleMouseDown(
                                this,
                                DateTime.Now.Ticks,
                                e.MouseButton,
                                e.Location,
                                out _,
                                e.DeviceType);
                            break;
                        case TouchAction.Moved:
                            AbstractControl.BubbleMouseMove(
                                this,
                                DateTime.Now.Ticks,
                                e.Location,
                                out _);
                            break;
                        case TouchAction.Released:
                            AbstractControl.BubbleMouseUp(
                                this,
                                DateTime.Now.Ticks,
                                e.MouseButton,
                                e.Location,
                                out _,
                                e.DeviceType);
                            break;
                        case TouchAction.Cancelled:
                            break;
                        case TouchAction.Exited:
                            RaiseMouseLeave(e);
                            break;
                        case TouchAction.WheelChanged:
                            AbstractControl.BubbleMouseWheel(
                                        this,
                                        DateTime.Now.Ticks,
                                        e.WheelDelta,
                                        e.Location,
                                        out _);
                            break;
                        default:
                            break;
                    }

                    break;
                case TouchDeviceType.Pen:
                    break;
                default:
                    break;
            }

            e.Handled = true;
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
        public virtual void Show()
        {
            // This method must be virtual as it is overriden in some descendants.
            Visible = true;
        }

        /// <summary>
        /// Gets the child control at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child control
        /// to get.</param>
        /// <returns>The child control at the specified index in the
        /// <see cref="Children"/> list.</returns>
        public virtual AbstractControl? GetChildOrNull(int index = 0)
        {
            if (!HasChildren)
                return null;
            if (index >= Children.Count || index < 0)
                return null;
            return Children[index];
        }

        /// <summary>
        /// Creates <see cref="ControlSet"/> with the specified controls.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public virtual ControlSet Group(params AbstractControl[] controls)
        {
            return new(controls);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public virtual ControlSet Group(IReadOnlyList<AbstractControl> controls)
        {
            return new(controls);
        }

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
            List<AbstractControl> result = new();
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
        [Browsable(false)]
        public virtual ControlSet GetChildrenRecursive()
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<AbstractControl> result = new();
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
        public void SetMouseCapture(bool value)
        {
            if (value)
                CaptureMouse();
            else
                ReleaseMouseCapture();
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
            List<AbstractControl> result = new();
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
            List<AbstractControl> result = new();
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
        /// Executes <paramref name="action"/> between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        public virtual void DoInsideLayout(Action action, bool layoutParent = true)
        {
            SuspendLayout();
            try
            {
                action();
            }
            finally
            {
                ResumeLayout(true, layoutParent);
            }
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginInit"/>
        /// and <see cref="EndInit"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public virtual void DoInsideInit(Action action)
        {
            BeginInit();
            try
            {
                action();
            }
            finally
            {
                EndInit();
            }
        }

        /// <summary>
        /// Returns enumeration with the list of visible child controls.
        /// </summary>
        /// <seealso cref="GetVisibleChildOrNull"/>
        [Browsable(false)]
        public virtual IReadOnlyList<AbstractControl> GetVisibleChildren()
        {
            if (HasChildren)
            {
                List<AbstractControl> result = new();
                foreach (var item in Children)
                {
                    if (item.Visible)
                        result.Add(item);
                }

                return result;
            }

            return Array.Empty<AbstractControl>();
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
        [Browsable(false)]
        public virtual AbstractControl? GetVisibleChildOrNull(int index = 0)
        {
            var childs = GetVisibleChildren();
            foreach (AbstractControl control in childs)
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
        public void Hide() => Visible = false;

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
        [Browsable(false)]
        public virtual Graphics CreateDrawingContext()
        {
            return new PlessGraphics();
        }

        /// <summary>
        /// Same as <see cref="CreateDrawingContext"/>. Added mainly for legacy code.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual Graphics CreateGraphics() => CreateDrawingContext();

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
            Coord x,
            Coord y,
            Coord width,
            Coord height,
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

            Bounds = result;
        }

        /// <summary>
        /// Gets whether key is a valid key gesture for this control.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="modifiers">Key modifiers.</param>
        /// <returns>True if <see cref="ValidateKeyBinding"/> is False; otherwise
        /// returns result of <see cref="KeyGesture.IsValid(Key, UI.ModifierKeys)"/> call.</returns>
        /// <remarks>
        /// Override this method in order to allow/supress allowed keys
        /// for this control used in <see cref="ExecuteKeyBinding"/> method.
        /// </remarks>
        public virtual bool IsKeyBindingValid(Key key, ModifierKeys modifiers)
        {
            var isValid = KeyGesture.IsValid(key, modifiers);
            return isValid;
        }

        /// <summary>
        /// Processes input bindings and calls associated commands
        /// if their key bindings are valid and equal to key specified
        /// in the <paramref name="key"/>, <paramref name="modifiers"/> parameters.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="modifiers">Key modifiers.</param>
        /// <param name="recursive">Whether to process input bindings only for this
        /// control (False) or this control and its child controls recursively (True).</param>
        /// <returns></returns>
        public virtual bool ExecuteKeyBinding(Key key, ModifierKeys modifiers, bool recursive)
        {
            if (ValidateKeyBinding)
            {
                var isValid = IsKeyBindingValid(key, modifiers);
                if (!isValid)
                    return false;
            }

            if (recursive)
            {
                return Internal(InputBindingsRecursive);
            }
            else
            {
                if (!HasInputBindings)
                    return false;
                return Internal(InputBindings);
            }

            bool Internal(IEnumerable<InputBinding> bindings)
            {
                foreach (var binding in bindings)
                {
                    if (!binding.IsActive)
                        continue;
                    if (!binding.HasKey(key, modifiers))
                        continue;
                    var executed = binding.Execute();
                    if (executed)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Calls the specified action for all the parent controls.
        /// </summary>
        /// <typeparam name="T">Type of the action parameters.</typeparam>
        /// <param name="e">Action parameters.</param>
        /// <param name="action">Action to call.</param>
        /// <returns>True if <see cref="HandledEventArgs.Handled"/>
        /// of the <paramref name="e"/> is True.</returns>
        public virtual bool ForEachParent<T>(T e, Action<AbstractControl, T> action)
            where T : HandledEventArgs
        {
            if (Parent is null)
                return false;
            action(Parent, e);
            if (e.Handled)
                return true;
            var result = Parent.ForEachParent(e, action);
            return result;
        }

        /// <summary>
        /// Calls the specified action for all visible child controls.
        /// </summary>
        /// <typeparam name="T">Type of the action parameters.</typeparam>
        /// <param name="e">Action parameters.</param>
        /// <param name="action">Action to call.</param>
        /// <returns>True if <see cref="HandledEventArgs.Handled"/>
        /// of the <paramref name="e"/> is True.</returns>
        public virtual bool ForEachVisibleChild<T>(T e, Action<AbstractControl, T> action)
            where T : HandledEventArgs
        {
            if (!HasChildren)
                return false;
            var childs = GetVisibleChildren();
            foreach (var child in childs)
            {
                if (!child.Visible)
                    continue;
                action(child, e);
                if (e.Handled)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Forces the control to invalidate itself and immediately redraw itself
        /// and any child controls. Calls <see cref="Invalidate()"/> and <see cref="Update"/>.
        /// </summary>
        [Browsable(false)]
        public void Refresh()
        {
            Invalidate();
            Update();
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        [Browsable(false)]
        public virtual void Invalidate()
        {
            RaiseInvalidated(new(ClientRectangle));
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        [Browsable(false)]
        public virtual void Update()
        {
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
        [Browsable(false)]
        public virtual void SuspendLayout()
        {
            layoutSuspendCount++;
        }

        /// <summary>
        /// Sets 'ParentBackColor' property for all child controls.
        /// </summary>
        /// <param name="value">New property value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        [Browsable(false)]
        public virtual void SetChildrenUseParentBackColor(bool value = true, bool recursive = false)
        {
            ForEachChild((control) => control.ParentBackColor = value, recursive);
        }

        /// <summary>
        /// Sets 'ParentForeColor' property for all child controls.
        /// </summary>
        /// <param name="value">New property value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        [Browsable(false)]
        public virtual void SetChildrenUseParentForeColor(bool value = true, bool recursive = false)
        {
            ForEachChild((control) => control.ParentForeColor = value, recursive);
        }

        /// <summary>
        /// Sets 'ParentFont' property for all child controls.
        /// </summary>
        /// <param name="value">New property value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        [Browsable(false)]
        public virtual void SetChildrenUseParentFont(bool value = true, bool recursive = false)
        {
            ForEachChild((control) => control.ParentFont = value, recursive);
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever
        /// re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        [Browsable(false)]
        public virtual void RecreateHandler()
        {
            Invalidate();
        }

        /// <summary>
        /// Changes size of the control to fit the size of its content.
        /// </summary>
        /// <param name="mode">Specifies how a control will size itself to fit the size of
        /// its content.</param>
        /// <param name="additionalSpace">.</param>
        public virtual void SetSizeToContent(
            WindowSizeToContentMode mode = WindowSizeToContentMode.WidthAndHeight,
            SizeD? additionalSpace = null)
        {
            if (mode == WindowSizeToContentMode.None)
                return;

            var newSize = GetChildrenMaxPreferredSizePadded(MaximumSize.InfinityIfEmpty);

            var newSize2 = GetPaddedPreferredSize(GetPreferredSize(MaximumSize.InfinityIfEmpty));

            additionalSpace ??= SizeD.Empty;

            newSize = SizeD.Max(newSize, newSize2) + additionalSpace.Value;

            if (newSize != SizeD.Empty)
            {
                var currentSize = ClientSize;
                switch (mode)
                {
                    case WindowSizeToContentMode.Width:
                    case WindowSizeToContentMode.GrowWidth:
                        newSize.Height = currentSize.Height;
                        break;
                    case WindowSizeToContentMode.GrowHeight:
                    case WindowSizeToContentMode.Height:
                        newSize.Width = currentSize.Width;
                        break;
                }

                bool onlyGrow = mode == WindowSizeToContentMode.GrowWidth
                    || mode == WindowSizeToContentMode.GrowHeight
                    || mode == WindowSizeToContentMode.GrowWidthAndHeight;

                if (onlyGrow)
                {
                    newSize = SizeD.Max(newSize, ClientSize);
                }

                // We need to use here client size as, for example,
                // window has title bar and thick border.
                ClientSize = newSize;
            }
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point on the screen
        /// to client-area coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that specifies the
        /// screen coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public virtual PointD ScreenToClient(PointD point)
        {
            return PlessUtils.ScreenToClient(point, this);
        }

        /// <summary>
        /// Converts the client-area coordinates of a specified point to
        /// screen coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that contains the
        /// client coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public virtual PointD ClientToScreen(PointD point)
        {
            return PlessUtils.ClientToScreen(point, this);
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
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        [Browsable(false)]
        public virtual void ResumeLayout(bool performLayout = true, bool layoutParent = true)
        {
            layoutSuspendCount--;
            if (layoutSuspendCount < 0)
                throw new InvalidOperationException();

            if (!IsLayoutSuspended)
            {
                if (performLayout)
                    PerformLayout(layoutParent);
            }
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginUpdate"/>
        /// and <see cref="EndUpdate"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <remarks>
        /// Do not recreate control (or its child controls), add or remove child controls between
        /// <see cref="BeginUpdate"/> and <see cref="EndUpdate"/> calls.
        /// This method is mainly for multiple add or remove of the items in list like controls.
        /// </remarks>
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
        /// Gets used <see cref="IFileSystem"/> provider.
        /// </summary>
        /// <returns>
        /// Returns value of the <see cref="FileSystem"/> property if it is not <c>null</c>;
        /// otherwise returns <see cref="Alternet.UI.FileSystem.Default"/>.
        /// </returns>
        [Browsable(false)]
        public virtual IFileSystem GetFileSystem()
        {
            return FileSystem ?? UI.FileSystem.Default;
        }

        /// <summary>
        /// Maintains performance while performing slow operations on a control
        /// by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        /// <remarks>
        /// Do not recreate control (or its child controls), add or remove child controls between
        /// <see cref="BeginUpdate"/> and <see cref="EndUpdate"/> calls.
        /// This method is mainly for multiple add or remove of the items in list like controls.
        /// </remarks>
        [Browsable(false)]
        public virtual int BeginUpdate()
        {
            updateCount++;
            return updateCount;
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the
        /// <see cref="BeginUpdate"/> method.
        /// </summary>
        [Browsable(false)]
        public virtual int EndUpdate()
        {
            updateCount--;
            return updateCount;
        }

        /// <summary>
        /// Gets child with the specified id.
        /// </summary>
        /// <param name="id">Child control id.</param>
        public virtual AbstractControl? FindChild(ObjectUniqueId? id)
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
        /// Gets native handler of the control. For Windows this is hWnd, for other
        /// operating systems currently returns null.
        /// You should not use this property.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual IntPtr GetHandle()
        {
            return IntPtr.Zero;
        }

        /// <summary>
        /// Gets tooltip provider from <see cref="ToolTipProvider"/> property.
        /// If is empty, gets tooltip provider from the parent control (and so on recursively).
        /// If parent's tooltip provider is empty, gets it from the application
        /// (<see cref="App.ToolTipProvider"/>).
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual IToolTipProvider? GetToolTipProvider()
        {
            var result = ToolTipProvider ?? Parent?.GetToolTipProvider() ?? App.ToolTipProvider;
            return result;
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
        [Browsable(false)]
        public virtual void PerformLayout(bool layoutParent = true)
        {
            if (IsLayoutSuspended || DisposingOrDisposed || inLayout)
                return;
            if (!Visible || IgnoreLayout)
                layoutParent = false;

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

            RaiseLayoutUpdated(EventArgs.Empty);
        }

        /// <summary>
        /// Sets new suggested size value using nullable width and height values.
        /// </summary>
        public virtual void SetSuggestedSize(Coord? newSuggestedWidth, Coord? newSuggestedHeight)
        {
            var suggestedWidth = newSuggestedWidth is null
            ? Coord.NaN : newSuggestedWidth.Value;

            var suggestedHeight = newSuggestedHeight is null
            ? Coord.NaN : newSuggestedHeight.Value;

            SuggestedSize = (suggestedWidth, suggestedHeight);
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
        [Browsable(false)]
        public virtual void BeginInit()
        {
            SuspendLayout();
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
        [Browsable(false)]
        public virtual void EndInit()
        {
            ResumeLayout();
        }

        /// <summary>
        /// Same as <see cref="Visible"/> but implemented as method.
        /// </summary>
        /// <param name="value">New <see cref="Visible"/> property value.</param>
        public void SetVisible(bool value) => Visible = value;

        /// <summary>
        /// Same as <see cref="Enabled"/> but implemented as method.
        /// </summary>
        /// <param name="value">New <see cref="Enabled"/> property value.</param>
        public void SetEnabled(bool value) => Enabled = value;

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
        [Browsable(false)]
        public virtual void ResetSuggestedHeight()
        {
            SuggestedHeight = Coord.NaN;
        }

        /// <summary>
        /// Resets <see cref="SuggestedWidth"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual void ResetSuggestedWidth()
        {
            SuggestedWidth = Coord.NaN;
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
        /// Gets the validation errors for this control and its child controls.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <c>null</c>
        /// or <see cref="string.Empty"/>, to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for this control and its child controls.</returns>
        [Browsable(false)]
        public virtual IEnumerable GetErrors(string? propertyName = null)
        {
            foreach (var item in AllChildren)
            {
                if (item is INotifyDataErrorInfo errorInfo)
                {
                    foreach (var error in errorInfo.GetErrors(null))
                    {
                        yield return error;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the validation errors for this control and its child controls.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property to retrieve validation errors for; or <c>null</c>
        /// or <see cref="string.Empty"/>, to retrieve entity-level errors.
        /// </param>
        /// <returns>The validation errors for this control and its child controls.</returns>
        /// <remarks>
        /// This is the same as <see cref="GetErrors"/> but returns <see cref="IEnumerable{T}"/>.
        /// </remarks>
        public virtual IEnumerable<object> GetErrorsCollection(string? propertyName = null)
        {
            var errors = GetErrors(propertyName);

            if (errors is null)
                yield break;

            foreach (var e in errors)
                yield return e;
        }

        /// <summary>
        /// Gets children of the specified type as <see cref="ControlSet"/>.
        /// </summary>
        /// <param name="recursive">Whether to get all children recurively.</param>
        /// <returns></returns>
        /// <typeparam name="T">Type of the control to find.</typeparam>
        [Browsable(false)]
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

            var limitedResult = new List<AbstractControl>();

            foreach (var item in result.Items)
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
            ForEachChild((control) => control.Font = font, recursive);
        }

        /// <summary>
        /// Sets children background color.
        /// </summary>
        /// <param name="color">New background color value</param>
        /// <param name="recursive">Whether to apply to all children recurively.</param>
        public virtual void SetChildrenBackgroundColor(Color? color, bool recursive = false)
        {
            ForEachChild((control) => control.BackgroundColor = color, recursive);
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
            ForEachChild((control) => control.ForegroundColor = color, recursive);
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
            return DragDropEffects.None;
        }

        /// <summary>
        /// Forces the re-creation of the underlying native control.
        /// </summary>
        [Browsable(false)]
        public virtual void RecreateWindow()
        {
            Invalidate();
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
            return dpi ??= GraphicsFactory.ScaleFactorToDpi(ScaleFactor);
        }

        /// <summary>
        /// Checks whether using transparent background might work.
        /// </summary>
        /// <returns><c>true</c> if background transparency is supported.</returns>
        /// <remarks>
        /// If this function returns <c>false</c>, setting the transparent background style
        /// is not going to work. If it
        /// returns <c>true</c>, setting transparent style should normally succeed.
        /// </remarks>
        /// <remarks>
        /// Notice that this function would typically be called on the parent of a
        /// control you want to set transparent background style for as the control for
        /// which this method is called must be fully created.
        /// </remarks>
        public virtual bool IsTransparentBackgroundSupported()
        {
            return true;
        }

        /// <summary>
        /// Sets <see cref="Visible"/> property value for all the children controls.
        /// </summary>
        /// <param name="visible">New value of the <see cref="Visible"/> property.</param>
        [Browsable(false)]
        public void SetChildrenVisible(bool visible = true)
        {
            ForEachChild((c) => c.Visible = visible);
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        /// <param name="direction">Specifies the direction for the centering.</param>
        /// <remarks>
        /// If the window is a top level one (i.e. doesn't have a parent), it will be
        /// centered relative to the screen anyhow.
        /// </remarks>
        public virtual bool CenterOnParent(GenericOrientation direction = GenericOrientation.Both)
        {
            if (Parent is null)
                return false;

            var vert = HVAlignment.Center.VerticalOrNull(direction);
            var horz = HVAlignment.Center.HorizontalOrNull(direction);
            var newBounds = AlignUtils.AlignRectInRect(
                Bounds,
                Parent.ClientRectangle,
                horz,
                vert,
                true);
            Bounds = newBounds.ToRect();
            return true;
        }

        /// <summary>
        /// Performs some action for the each child of the control.
        /// </summary>
        /// <typeparam name="T">Specifies type of the child control.</typeparam>
        /// <param name="action">Specifies action which will be called for the
        /// each child.</param>
        public virtual void ForEachChild<T>(Action<T> action)
            where T : AbstractControl
        {
            if (!HasChildren)
                return;

            foreach (var child in Children)
            {
                if(child is T control)
                    action(control);
            }
        }

        /// <summary>
        /// Performs some action for the each child of the control.
        /// </summary>
        /// <param name="action">Specifies action which will be called for the each child.</param>
        /// <param name="recursive">Whether to call action
        /// for all child controls recursively.</param>
        public virtual void ForEachChild(Action<AbstractControl> action, bool recursive = false)
        {
            if (!HasChildren)
                return;

            foreach (var child in Children)
            {
                action(child);
                if (recursive)
                    child.ForEachChild(action, true);
            }
        }

        /// <summary>
        /// Performs some action for the each visible child of the control.
        /// </summary>
        /// <param name="action">Specifies action which will be called
        /// for the each visible child.</param>
        /// <param name="recursive">Whether to call action
        /// for all child controls recursively.</param>
        public virtual void ForEachVisibleChild(
            Action<AbstractControl> action,
            bool recursive = false)
        {
            if (!HasChildren)
                return;

            foreach (var child in Children)
            {
                if (!child.Visible)
                    continue;
                action(child);
                if (recursive)
                    child.ForEachVisibleChild(action, true);
            }
        }

        /// <summary>
        /// Gets whether one of this control's parents equals <paramref name="testParent"/>.
        /// </summary>
        /// <param name="testParent">Control to test as an indirect parent.</param>
        public virtual bool HasIndirectParent(AbstractControl? testParent)
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
        /// Retrieves a value indicating whether the specified control
        /// is a child of the control.</summary>
        /// <param name="control">The <see cref="AbstractControl" /> to evaluate.</param>
        /// <returns>
        /// <see langword="true" /> if the specified control is a child of the control;
        /// otherwise, <see langword="false" />.
        /// </returns>
        public virtual bool Contains(AbstractControl? control)
        {
            while (control != null)
            {
                control = control.Parent;
                if (control == null)
                    return false;
                if (control == this)
                    return true;
            }

            return false;
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
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public virtual int PixelFromDip(Coord value)
        {
            return GraphicsFactory.PixelFromDip(value, ScaleFactor);
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public SizeI PixelFromDip(SizeD value)
        {
            return new(PixelFromDip(value.Width), PixelFromDip(value.Height));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public PointI PixelFromDip(PointD value)
        {
            return new(PixelFromDip(value.X), PixelFromDip(value.Y));
        }

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public RectI PixelFromDip(RectD value)
        {
            return new(PixelFromDip(value.Location), PixelFromDip(value.Size));
        }

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        public SizeD PixelToDip(SizeI value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        public PointD PixelToDip(PointI value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        public RectD PixelToDip(RectI value)
        {
            return new(PixelToDip(value.Location), PixelToDip(value.Size));
        }

        /// <summary>
        /// Sets image for the specified control state.
        /// </summary>
        /// <param name="value">Image.</param>
        /// <param name="state">Control state.</param>
        public virtual void SetImage(
            Image? value,
            VisualControlState state = VisualControlState.Normal)
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
        public virtual void SetBackground(
            Brush? value,
            VisualControlState state = VisualControlState.Normal)
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
        public virtual void SetBorder(
            BorderSettings? value,
            VisualControlState state = VisualControlState.Normal)
        {
            StateObjects ??= new();
            StateObjects.Borders ??= new();
            StateObjects.Borders.SetObject(value, state);
        }

        /// <summary>
        /// Resets the <see cref="AbstractControl.Font" /> property to its default value.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public virtual void ResetFont()
        {
            Font = null;
        }

        /// <summary>
        /// Resets the <see cref="AbstractControl.Cursor" /> property to its default value.</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public virtual void ResetCursor()
        {
            Cursor = null;
        }

        /// <summary>
        /// Resets the <see cref="AbstractControl.BackColor" /> property to its default value.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
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
        /// Converts pixels to device-independent units.
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        public virtual Coord PixelToDip(int value)
        {
            return GraphicsFactory.PixelToDip(value, ScaleFactor);
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
        public virtual void Invalidate(Region? region, bool invalidateChildren = false)
        {
            if (region is null || !region.IsOk || region.IsEmpty)
                return;
            var rect = region.GetBounds();
            Invalidate(rect);
        }

        /// <summary>
        /// Gets background color from the default attributes.
        /// </summary>
        /// <returns></returns>
        public virtual Color? GetDefaultAttributesBgColor()
        {
            return SystemColors.Control;
        }

        /// <summary>
        /// Gets foreground color from the default attributes.
        /// </summary>
        /// <returns></returns>
        public virtual Color? GetDefaultAttributesFgColor()
        {
            return SystemColors.ControlText;
        }

        /// <summary>
        /// Gets font from the default attributes.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual Font? GetDefaultAttributesFont()
        {
            return null;
        }

        /// <summary>
        /// Resets cached value of the <see cref="ScaleFactor"/> property, so it will be retrieved
        /// from the handler next time it is used.
        /// </summary>
        [Browsable(false)]
        public virtual void ResetScaleFactor()
        {
            scaleFactor = null;
            dpi = null;
        }

        /// <summary>
        /// Calls <see cref="LocationChanged"/> and <see cref="SizeChanged"/> events
        /// if <see cref="Bounds"/> property was changed.
        /// </summary>
        [Browsable(false)]
        public virtual bool ReportBoundsChanged()
        {
            var newBounds = Bounds;

            var locationChanged = reportedBounds.Location != newBounds.Location;
            var sizeChanged = reportedBounds.Size != newBounds.Size;

            reportedBounds = newBounds;

            if (locationChanged)
                RaiseLocationChanged(EventArgs.Empty);

            if (sizeChanged)
                RaiseSizeChanged(EventArgs.Empty);

            PerformLayout();

            return locationChanged || sizeChanged;
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
        /// <remarks>Position is specified in device independent units.</remarks>
        public virtual void ShowPopupMenu(ContextMenu? menu, Coord x = -1, Coord y = -1)
        {
            menu?.Show(this, (x, y));
        }

        /// <summary>
        /// Gets child control at the specified index in the collection of child controls.
        /// </summary>
        /// <param name="index">Index of the child control.</param>
        /// <returns></returns>
        public IControl GetControl(int index) => Children[index];

        /// <summary>
        /// Sets a value that indicates which row and column control should appear in.
        /// </summary>
        /// <param name="row">The 0-based row index to set.</param>
        /// <param name="col">The 0-based column index to set.</param>
        public virtual void SetRowColumn(int row, int col)
        {
            if (row < 0)
                row = 0;
            if (col < 0)
                col = 0;
            var changed = (col != columnIndex) || (row != rowIndex);
            if (changed)
            {
                rowIndex = row;
                columnIndex = col;
                RaiseCellChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Increase minimal size to the specified value.
        /// </summary>
        /// <param name="mode">Minimal size increase mode.</param>
        /// <param name="minWidthAtLeast">Minimal width will be at least. Optional.
        /// If not specified, current control's width is used.</param>
        /// <param name="minHeightAtLeast">Minimal height will be at least. Optional.
        /// If not specified, current control's height is used.</param>
        public virtual void GrowMinSize(
            WindowSizeToContentMode mode,
            Coord? minWidthAtLeast = null,
            Coord? minHeightAtLeast = null)
        {
            if (mode == WindowSizeToContentMode.None)
                return;
            SizeD currentSize = Size;
            var minW = minWidthAtLeast ?? currentSize.Width;
            var minH = minHeightAtLeast ?? currentSize.Height;

            switch (mode)
            {
                case WindowSizeToContentMode.Width:
                    MinWidth = ControlUtils.GrowCoord(MinWidth, minW);
                    break;
                case WindowSizeToContentMode.Height:
                    MinHeight = ControlUtils.GrowCoord(MinHeight, minH);
                    break;
                case WindowSizeToContentMode.WidthAndHeight:
                    MinimumSize = ControlUtils.GrowSize(MinimumSize, (minW, minH));
                    break;
            }
        }

        /// <summary>
        /// Adds <see cref="IControlNotification"/> object to the list of notifications.
        /// </summary>
        /// <param name="n">Notification object to add.</param>
        /// <remarks>
        /// You must also call <see cref="RemoveNotification"/> when subscriber is disposed.
        /// </remarks>
        public virtual void AddNotification(IControlNotification? n)
        {
            if (n is null)
                return;
            notifications ??= new();
            notifications.Add(n);
        }

        /// <summary>
        /// Gets vertical or horizontal alignment as <see cref="CoordAlignment"/>
        /// depending on the <paramref name="isVert"/> parameter value.
        /// </summary>
        /// <param name="isVert">The flag which specifies whether to return
        /// vertical or horizontal alignment.</param>
        /// <returns></returns>
        public CoordAlignment GetCoordAlignment(bool isVert)
        {
            if (isVert)
                return (CoordAlignment)VerticalAlignment;
            else
                return (CoordAlignment)HorizontalAlignment;
        }

        /// <summary>
        /// Removes <see cref="IControlNotification"/> object from the list of notifications.
        /// </summary>
        /// <param name="n">Notification object to remove.</param>
        public virtual void RemoveNotification(IControlNotification? n)
        {
            if (notifications is null || n is null)
                return;
            notifications.Remove(n);
        }

        /// <summary>
        /// Gets top-most visible child control
        /// which has <see cref="Bounds"/> that contains the specified point.
        /// Controls are queried starting
        /// from the last control in <see cref="Children"/>.
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <returns></returns>
        public virtual AbstractControl? PointInChild(PointD point)
        {
            return PointInChilds(point).FirstOrDefault();
        }

        /// <summary>
        /// Gets whether this control contains mouse cursor.
        /// </summary>
        /// <param name="threshold">Additional value on which bounds
        /// of the control are inflated.</param>
        /// <returns></returns>
        public virtual bool ContainsMouseCursor(Coord threshold)
        {
            var rectangle = ClientRectangle;
            rectangle.Inflate(threshold, threshold);
            return rectangle.Contains(Mouse.GetPosition(this));
        }

        /// <summary>
        /// Returns a maximal preferred size of the children with an added padding.
        /// </summary>
        public virtual SizeD GetChildrenMaxPreferredSizePadded(SizeD availableSize)
        {
            return GetPaddedPreferredSize(GetChildrenMaxPreferredSize(availableSize));
        }

        /// <summary>
        /// Resets the cached best size value so it will be recalculated the next time it is needed.
        /// </summary>
        [Browsable(false)]
        public virtual void InvalidateBestSize()
        {
        }

        /// <summary>
        /// Gets whether child control ignores layout.
        /// </summary>
        /// <param name="control"></param>
        public virtual bool ChildIgnoresLayout(AbstractControl control)
        {
            return !control.Visible || control.IgnoreLayout;
        }

        /// <summary>
        /// Gets <see cref="IEnumerable"/> list of visible child controls
        /// which have <see cref="Bounds"/> that contains the specified point.
        /// Controls are returned starting
        /// from the last control in <see cref="Children"/>.
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <returns></returns>
        public virtual IEnumerable<AbstractControl> PointInChilds(PointD point)
        {
            var childs = AllChildren;

            for(int i = childs.Count - 1; i >= 0; i--)
            {
                var child = childs[i];
                if (!child.Visible)
                    continue;
                if(child.Bounds.Contains(point))
                    yield return child;
            }
        }

        /// <summary>
        /// Sets the specified generic property of this control.
        /// </summary>
        /// <param name="prop">Generic property identifier.</param>
        /// <param name="value">New value for the generic property.</param>
        public virtual void SetGenericProperty(GenericControlProperty prop, object value)
        {
            switch (prop)
            {
                case GenericControlProperty.Enabled:
                    Enabled = (bool)value;
                    break;
                case GenericControlProperty.Visible:
                    Visible = (bool)value;
                    break;
                case GenericControlProperty.Text:
                    Text = value.SafeToString();
                    break;
                case GenericControlProperty.Title:
                    TitleAsObject = value;
                    break;
            }
        }

        /// <summary>
        /// Gets the specified generic property of this control.
        /// </summary>
        /// <param name="prop">Generic property identifier.</param>
        public virtual object GetGenericProperty(GenericControlProperty prop)
        {
            switch (prop)
            {
                case GenericControlProperty.Enabled:
                    return Enabled;
                case GenericControlProperty.Visible:
                    return Visible;
                case GenericControlProperty.Text:
                    return Text;
                case GenericControlProperty.Title:
                    return TitleAsObject ?? string.Empty;
                default:
                    return 0;
            }
        }
    }
}
