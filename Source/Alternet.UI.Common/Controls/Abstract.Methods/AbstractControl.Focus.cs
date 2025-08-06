using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class AbstractControl
    {
        /// <summary>
        /// Gets or sets whether to draw debug rectangle around focused control.
        /// </summary>
        public static bool ShowDebugFocusRect = false;

        private static WeakReferenceValue<AbstractControl> focusedControl = new();
        private bool canSelect = true;
        private bool tabStop = true;

        /// <summary>
        /// Occurs when <see cref="FocusNextControl"/> is called.
        /// </summary>
        public static event EventHandler<GlobalFocusNextEventArgs>? GlobalFocusNextControl;

        /// <summary>
        /// Gets focused control.
        /// </summary>
        /// <remarks>
        /// Do not change this property, this is done by the library.
        /// </remarks>
        public static AbstractControl? FocusedControl
        {
            get
            {
                var focused = focusedControl.Value;

                if(focused is not null)
                {
                    if (focused.Focused)
                    {
                        return focused;
                    }
                }

                focusedControl.Value = App.Handler.GetFocusedControl();
                return focusedControl.Value;
            }

            set
            {
                if (focusedControl.Value == value)
                    return;
                RaiseFocusedControlChanged(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can receive focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the control can receive focus;
        /// otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// If this property returns true, it means that calling <see cref="SetFocus"/> will put
        /// focus either to this control or one of its children.
        /// </remarks>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Focus")]
        public virtual bool CanFocus
        {
            get
            {
                if (!IsHandleCreated)
                    return false;

                return Visible && Enabled && CanSelect;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control, or one of its child
        /// controls, currently has the input focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the control or one of its child controls
        /// currently has the input focus; otherwise, <see langword="false" />.</returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool ContainsFocus
        {
            get
            {
                var focused = FocusedControl;
                if (focused is null)
                    return false;
                if (focused == this)
                    return true;
                var result = focused.HasIndirectParent(this);
                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool Focused
        {
            get
            {
                return FocusedControl == this;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control
        /// using the TAB key.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        /// <returns>
        /// Return false to indicate that while this control can,
        /// in principle, have focus if the user clicks
        /// it with the mouse, it shouldn't be included
        /// in the TAB traversal chain when using the keyboard.
        /// </returns>
        public virtual bool TabStop
        {
            get
            {
                return tabStop;
            }

            set
            {
                UpdateFocusFlags(canSelect, value);
            }
        }

        /// <summary>
        /// Gets whether control is graphic control. Graphic controls can not be selected using mouse,
        /// do not accept focus and ignore keyboard.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public virtual bool IsGraphicControl
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(true)]
        public virtual bool CanSelect
        {
            get
            {
                return canSelect;
            }

            set
            {
                UpdateFocusFlags(value, TabStop);
            }
        }

        /// <summary>
        /// Same as <see cref="FocusedControl"/>.
        /// </summary>
        /// <returns></returns>
        public static AbstractControl? GetFocusedControl() => FocusedControl;

        /// <summary>
        /// Internal method. Do not use it.
        /// </summary>
        public static bool FocusedControlEquals(AbstractControl? control)
        {
            return AbstractControl.focusedControl.Value == control;
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns><see langword="true"/> if the input focus request was
        /// successful; otherwise, <see langword="false"/>.</returns>
        /// <remarks>The <see cref="SetFocus"/> method returns true if the
        /// control successfully received input focus.</remarks>
        public virtual bool SetFocus()
        {
            return false;
        }

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> if the input focus request was successful;
        ///   otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// Same as <see cref="SetFocus"/>.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool Focus() => SetFocus();

        /// <summary>
        /// Sets input focus to the control if it can accept it.
        /// </summary>
        public virtual bool SetFocusIfPossible()
        {
            if (DisposingOrDisposed || !Visible)
                return false;

            if (CanFocus)
                return SetFocus();

            if(!HasChildren)
                return false;

            var items = GetFocusableChildren(true, true);

            if(items.Length == 0)
                return false;
            foreach (var item in items)
            {
                if(item.SetFocusIfPossible())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets input focus to the control when all application messages are
        /// processed and application goes to the idle state.
        /// </summary>
        public virtual void SetFocusIdle()
        {
            if (!Visible)
                return;
            Post(() =>
            {
                SetFocusIfPossible();
            });
        }

        /// <summary>
        /// Gets whether this control has focusable child controls.
        /// </summary>
        /// <param name="recursive">Whether to process child controls recursively</param>
        /// <returns></returns>
        public virtual bool HasFocusableChildren(bool recursive)
        {
            var items = GetFocusableChildren(recursive);
            var first = items.FirstOrDefault();
            return first != null;
        }

        /// <summary>
        /// Gets collection of the focusable children controls optionally sorted by
        /// the <see cref="TabIndex"/> property.
        /// </summary>
        /// <param name="recursive">Whether to process child controls recursively</param>
        /// <param name="sortByTabIndex">Whether to sort control by
        /// the <see cref="TabIndex"/> property</param>
        /// <returns></returns>
        public virtual AbstractControl[] GetFocusableChildren(bool recursive, bool sortByTabIndex)
        {
            var result = GetFocusableChildren(recursive).ToArray();
            if(sortByTabIndex)
                Array.Sort(result, Comparison);
            return result;

            int Comparison(AbstractControl x, AbstractControl y)
            {
                int result;

                bool DistanceIsAlmostEqual(Coord a1, Coord a2)
                {
                    var distance = Math.Abs(a1 - a2);

                    return distance < 5;
                }

                int CompareDistance(Coord a1, Coord a2)
                {
                    if (DistanceIsAlmostEqual(a1, a2))
                        return 0;
                    else
                        return a1.CompareTo(a2);
                }

                var xLocation = x.ClientToScreen(x.Location);
                var yLocation = y.ClientToScreen(y.Location);

                result = CompareDistance(xLocation.Y, yLocation.Y);

                if (result != 0)
                    return result;

                result = CompareDistance(xLocation.X, yLocation.X);

                if (result != 0)
                    return result;

                var xHash = x.Parent?.GetHashCode() ?? 0;
                var yHash = y.Parent?.GetHashCode() ?? 0;
                result = xHash.CompareTo(yHash);

                if (result != 0)
                    return result;

                result = x.TabIndex.CompareTo(y.TabIndex);

                if (result != 0)
                    return result;

                var xIndex = x.IndexInParent ?? 0;
                var yIndex = y.IndexInParent ?? 0;
                result = xIndex.CompareTo(yIndex);
                return result;
            }
        }

        /// <summary>
        /// Gets collection of the focusable children controls.
        /// </summary>
        /// <param name="recursive">Whether to process child controls recursively</param>
        /// <returns></returns>
        public virtual IEnumerable<AbstractControl> GetFocusableChildren(bool recursive)
        {
            IEnumerable<AbstractControl> containerItems;

            if (recursive)
                containerItems = ChildrenRecursive;
            else
                containerItems = Children;

            foreach (var control in containerItems)
            {
                if (control.Parent == this && !recursive)
                    continue;

                if (!control.TabStop || !control.VisibleOnScreen || !control.IsEnabled
                    || !control.CanSelect || !control.CanFocus || control.IsGraphicControl
                    || control.HasFocusableChildren(true))
                    continue;

                yield return control;
            }
        }

        /// <summary>
        /// Focuses the next control.
        /// </summary>
        /// <param name="forward"><see langword="true"/> to move forward in the
        /// tab order; <see langword="false"/> to move backward in the tab
        /// order.</param>
        /// <param name="recursive"><see langword="true"/> to include nested
        /// (children of child controls) child controls; otherwise,
        /// <see langword="false"/>.</param>
        public virtual void FocusNextControl(bool forward = true, bool recursive = true)
        {
            if(GlobalFocusNextControl is not null)
            {
                GlobalFocusNextEventArgs e = new(forward, recursive);
                GlobalFocusNextControl(this, e);
                if (e.Handled)
                    return;
            }

            if (ParentWindow is null)
                return;

            AbstractControl[] GetItems(AbstractControl container)
            {
                var result = container.GetFocusableChildren(recursive, true);
                return result;
            }

            int IndexOfControl(AbstractControl control, AbstractControl[] items)
            {
                var result = Array.IndexOf(items, control);
                return result;
            }

            bool FocusFirstOrLast(bool first, AbstractControl[] items)
            {
                if (items.Length > 0)
                {
                    if (first)
                        items[0].SetFocusIdle();
                    else
                        items[items.Length - 1].SetFocusIdle();
                    return true;
                }

                return false;
            }

            AbstractControl[] items;

            /*
            if (recursive)
            {
                items = GetItems(this);
                if(FocusFirstOrLast(forward, items))
                    return;
            }
            */

            items = GetItems(Root);

            var indexInParent = IndexOfControl(this, items);

            if (indexInParent >= 0)
            {
                if (forward)
                {
                    indexInParent++;
                    if (indexInParent >= items.Length)
                        indexInParent = 0;
                }
                else
                {
                    indexInParent--;
                    if (indexInParent < 0)
                        indexInParent = items.Length - 1;
                }

                var control = items[indexInParent];
                control.SetFocusIdle();
            }
            else
            {
                FocusFirstOrLast(forward, items);
            }
        }

        /// <summary>
        /// Updates focus related flags.
        /// </summary>
        protected virtual void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            this.canSelect = canSelect;
            this.tabStop = tabStop;
        }

        private static void RaiseFocusedControlChanged(AbstractControl? newFocused)
        {
            if (App.Terminating)
                return;

            var prevFocused = focusedControl.Value;

            focusedControl.Value = newFocused;

            FocusedControlChanged?.Invoke(newFocused, EventArgs.Empty);

            PlessKeyboard.ResetKeysStatesInMemory();

            if (DebugUtils.IsDebugDefined && ShowDebugFocusRect)
            {
                prevFocused?.Parent?.Invalidate();
                newFocused?.Parent?.Invalidate();
            }
        }
    }
}
