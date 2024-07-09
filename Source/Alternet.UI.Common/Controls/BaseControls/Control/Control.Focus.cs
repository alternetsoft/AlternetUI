using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public partial class Control
    {
        private static Control? focusedControl;

        /// <summary>
        /// Gets or sets focused control for internal purposes. Use <see cref="GetFocusedControl"/>
        /// instead of this property.
        /// </summary>
        /// <remarks>
        /// Do not change this property, this is done by the library.
        /// </remarks>
        public static Control? FocusedControl
        {
            get => focusedControl;

            set
            {
                if (focusedControl == value)
                    return;
                focusedControl = value;
                FocusedControlChanged?.Invoke(focusedControl, EventArgs.Empty);
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
        public virtual bool CanFocus /* WinForms */
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
        public bool ContainsFocus /* WinForms */
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
        public virtual bool Focused /* WinForms */
        {
            get
            {
                return Handler.IsFocused;
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
        public virtual bool TabStop /* WinForms */
        {
            get
            {
                return Handler.TabStop;
            }

            set
            {
                if (TabStop == value)
                    return;
                UpdateFocusFlags(CanSelect, value);
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
                return !Handler.CanSelect;
            }

            set
            {
                if (IsGraphicControl == value)
                    return;
                UpdateFocusFlags(!value, !value);
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
                return Handler.CanSelect;
            }

            set
            {
                if (CanSelect == value)
                    return;
                UpdateFocusFlags(value, TabStop);
            }
        }

        /// <summary>
        /// Returns the currently focused control, or <see langword="null"/> if
        /// no control is focused.
        /// </summary>
        public static Control? GetFocusedControl()
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
            return Handler.SetFocus();
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
            if (CanFocus)
                return SetFocus();
            else
                return false;
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
            Handler.FocusNextControl(forward, nested);
        }

        private void UpdateFocusFlags(bool canSelect, bool tabStop)
        {
            Handler.SetFocusFlags(canSelect, tabStop && canSelect, canSelect);
        }
    }
}
