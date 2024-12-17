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
        private static AbstractControl? focusedControl;

        /// <summary>
        /// Gets or sets focused control for internal purposes. Use <see cref="GetFocusedControl"/>
        /// instead of this property.
        /// </summary>
        /// <remarks>
        /// Do not change this property, this is done by the library.
        /// </remarks>
        public static AbstractControl? FocusedControl
        {
            get => focusedControl;

            set
            {
                if (focusedControl == value)
                    return;
                focusedControl = value;
                FocusedControlChanged?.Invoke(focusedControl, EventArgs.Empty);
                PlessKeyboard.ResetKeysStatesInMemory();
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
        public bool ContainsFocus
        {
            get
            {
                var focused = GetFocusedControl();
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
                var focused = GetFocusedControl();
                if (focused == this)
                    return true;
                return false;
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
                return false;
            }

            set
            {
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
                return false;
            }

            set
            {
            }
        }

        /// <summary>
        /// Returns the currently focused control, or <see langword="null"/> if
        /// no control is focused.
        /// </summary>
        public static AbstractControl? GetFocusedControl()
        {
            if (FocusedControl?.Focused ?? false)
                return FocusedControl;

            var result = App.Handler.GetFocusedControl();
            FocusedControl = result;
            return result;
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
            if (!IsDisposed && Visible && CanFocus)
                return SetFocus();
            else
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
            App.AddIdleTask(() =>
            {
                SetFocusIfPossible();
            });
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
            if (ParentWindow is null)
                return;

            IEnumerable<TabOrderItem> GetItems(AbstractControl container)
            {
                foreach (var control in container.ChildrenRecursive)
                {
                    if (!control.TabStop || !control.Visible || !control.CanSelect || !control.CanFocus
                        || control.HasChildren)
                        continue;

                    if (control.Parent == this && !nested)
                        continue;

                    yield return new(control);
                }
            }

            TabOrderItem[] GetSortedItems(AbstractControl container)
            {
                var result = GetItems(container).ToArray();
                Array.Sort(result);
                return result;
            }

            int IndexOfControl(AbstractControl control, TabOrderItem[] items)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].Control == control)
                        return i;
                }

                return -1;
            }

            bool FocusFirstOrLast(bool first, TabOrderItem[] items)
            {
                if (items.Length > 0)
                {
                    if (first)
                        items[0].Control.SetFocusIdle();
                    else
                        items[items.Length - 1].Control.SetFocusIdle();
                    return true;
                }

                return false;
            }

            TabOrderItem[] items;

            if (nested)
            {
                items = GetSortedItems(this);
                if(FocusFirstOrLast(forward, items))
                    return;
            }

            items = GetSortedItems(Root);

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

                var control = items[indexInParent].Control;
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
        }

        private struct TabOrderItem : IComparable<TabOrderItem>, IComparable
        {
            public AbstractControl Control;
            public int[] TabIndex = [];

            public TabOrderItem(AbstractControl control)
            {
                Control = control;
            }

            public readonly int CompareTo(TabOrderItem other)
            {
                return 0;
            }

            public readonly int CompareTo(object obj)
            {
                if (obj is TabOrderItem item)
                    return CompareTo(item);
                throw new NotImplementedException();
            }
        }
    }
}
