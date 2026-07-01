using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a specialized button control that displays a popup window.
    /// </summary>
    /// <typeparam name="TPopup">The type of the popup window.</typeparam>
    /// <typeparam name="TControl">The type of the main control of the popup window.</typeparam>
    public partial class SpeedButtonWithPopup<TPopup, TControl> : SpeedButton
        where TControl : AbstractControl, new()
        where TPopup : PopupWindow<TControl>, new()
    {
        /// <summary>
        /// Gets or sets the default color for the error border.
        /// </summary>
        public static LightDarkColor? DefaultErrorBorderColor = null;

        /// <summary>
        /// Defines the minimum time interval, in milliseconds, that must elapse 
        /// after the popup window is closed before it can be reopened.
        /// </summary>
        /// <remarks>
        /// This threshold prevents accidental reopening of the popup due to 
        /// rapid double-clicks or other quick user interactions.
        /// </remarks>
        public static int PopupReopenThresholdMs = 200;

        private TPopup? popupWindow;
        private object? data;
        private string popupWindowTitle = string.Empty;
        private DateTime popupLastClosedAt;
        private BaseCollection<BorderSettings>? errorBorder;
        private bool showErrorBorder;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpeedButtonWithPopup{TPopup, TControl}"/> class.
        /// </summary>
        public SpeedButtonWithPopup()
        {
            TextVisible = true;
            ShowComboBoxImageAtRight();
            ShowDropDownMenuWhenClicked = false;
            ClickTrigger = ClickTriggerKind.MouseDown;
        }

        /// <summary>
        /// Occurs when value is converted to <see cref="string"/>
        /// for the display purposes.
        /// </summary>
        public event EventHandler<ValueConvertEventArgs<object?, string?>>? ValueToDisplayString;

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Occurs before popup window is shown.
        /// </summary>
        public event EventHandler? BeforeShowPopup;

        /// <summary>
        /// Gets a value indicating whether the popup window has been created.
        /// </summary>
        public virtual bool IsPopupWindowCreated => popupWindow is not null;

        /// <summary>
        /// Gets the time when the popup window was closed.
        /// </summary>
        [Browsable(false)]
        public DateTime PopupLastClosedAt => popupLastClosedAt;

        /// <summary>
        /// Gets or sets the color of the error border displayed around the control.
        /// </summary>
        [Browsable(false)]
        public virtual LightDarkColor? ErrorBorderColor { get; set; }

        /// <summary>
        /// Gets milliseconds since the popup window was last closed.
        /// </summary>
        [Browsable(false)]
        public virtual long ElapsedFromPopupLastClosed
        {
            get
            {
                return DateUtils.GetAbsElapsedMilliseconds(popupLastClosedAt);
            }
        }

        /// <summary>
        /// Gets or sets selected value.
        /// </summary>
        public virtual object? Value
        {
            get
            {
                return data;
            }

            set
            {
                if (data == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    data = value;
                    UpdateBaseText();
                    RaiseValueChanged(EventArgs.Empty);
                });
            }
        }

        /// <summary>
        /// Gets or sets error border settings for the find text editor.
        /// </summary>
        [Browsable(false)]
        public virtual BaseCollection<BorderSettings>? ErrorBorder
        {
            get
            {
                return errorBorder;
            }
            set
            {
                errorBorder = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show error border around find text editor.
        /// </summary>
        public virtual bool ShowErrorBorder
        {
            get
            {
                return showErrorBorder;
            }

            set
            {
                if (showErrorBorder == value)
                    return;
                showErrorBorder = value;
                if (value)
                {
                    if (errorBorder is null)
                    {
                        var cloned = GetBorderSettings(VisualControlState.Normal)?.Clone() ?? new();
                        cloned.Color = ErrorBorderColor ?? DefaultErrorBorderColor ?? LightDarkColors.Red;
                        errorBorder = new BaseCollection<BorderSettings>();
                        errorBorder.Add(cloned);
                    }

                    InnerBordersOverride = errorBorder;
                }
                else
                {
                    InnerBordersOverride = null;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the position of the popup window relative to the button.
        /// </summary>
        [Browsable(false)]
        public virtual HVDropDownAlignment? PopupWindowPosition { get; set; }

        /// <summary>
        /// Gets or sets the title of the popup window.
        /// </summary>
        public virtual string PopupWindowTitle
        {
            get
            {
                return popupWindowTitle;
            }

            set
            {
                if (popupWindowTitle == value)
                    return;
                popupWindowTitle = value;
                if (IsPopupWindowCreated)
                    PopupWindow.Title = popupWindowTitle;
            }
        }

        /// <summary>
        /// Gets attached popup window.
        /// </summary>
        [Browsable(false)]
        public virtual TPopup PopupWindow
        {
            get
            {
                if (popupWindow is null)
                {
                    popupWindow = new();
                    popupWindow.Title = popupWindowTitle;
                    popupWindow.AfterHide += (s, e) =>
                    {
                        popupLastClosedAt = DateTime.Now;
                        OnPopupWindowClosed(s, e);
                    };
                }

                return popupWindow;
            }

            set
            {
                popupWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets a control used as a popup owner.
        /// </summary>
        public virtual AbstractControl? PopupOwner { get; set; }

        /// <summary>
        /// Gets or sets value as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public override string Text
        {
            get
            {
                string s = GetValueAsString(Value) ?? string.Empty;
                return s;
            }

            set
            {
                Value = value;
            }
        }

        internal new Image? Image
        {
            get => base.Image;
            set => base.Image = value;
        }

        internal new Image? DisabledImage
        {
            get => base.DisabledImage;
            set => base.DisabledImage = value;
        }

        /// <summary>
        /// Casts selected item to <typeparamref name="T2"/> type.
        /// </summary>
        /// <typeparam name="T2">Type of the result.</typeparam>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T2? ValueAs<T2>() => (T2?)Value;

        /// <summary>
        /// Shows popup window.
        /// </summary>
        public virtual void ShowPopup()
        {
            if (!Enabled)
                return;
            RaiseBeforeShowPopup(EventArgs.Empty);
            PopupWindow.ShowPopup(PopupOwner ?? this, PopupWindowPosition);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseBeforeShowPopup(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnBeforeShowPopup(e);
            BeforeShowPopup?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event and calls
        /// <see cref="OnValueChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseValueChanged(EventArgs e)
        {
            if (DisposingOrDisposed)
                return;
            OnValueChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets enum value as string.
        /// </summary>
        /// <returns></returns>
        public virtual string? GetValueAsString(object? d)
        {
            if (ValueToDisplayString is not null)
            {
                var e = new ValueConvertEventArgs<object?, string?>(this);
                ValueToDisplayString(null, e);
                if (e.Handled)
                    return e.Result;
            }

            if (d is ListControlItem item)
            {
                var result = item.DisplayText ?? item.Text;
                if (!string.IsNullOrEmpty(result))
                    return result;
            }

            return d?.ToString();
        }

        /// <summary>
        /// Called when the text of the control changes.
        /// </summary>
        public virtual void UpdateBaseText()
        {
            base.Text = Text ?? string.Empty;
        }

        /// <inheritdoc/>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (ElapsedFromPopupLastClosed < PopupReopenThresholdMs)
                return;
            else
            {
            }

            TogglePopupVisible(e);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (ElapsedFromPopupLastClosed < PopupReopenThresholdMs)
                return;
            else
            {
            }

            TogglePopupVisible(e);
        }

        /// <summary>
        /// Toggles the visibility of the popup window.
        /// </summary>
        protected virtual void TogglePopupVisible()
        {
            if (IsPopupWindowCreated && PopupWindow.IsVisible)
                Invoke(() => PopupWindow.HidePopup(ModalResult.Canceled));
            else
                Invoke(ShowPopup);
        }

        /// <summary>
        /// Toggles the visibility of the popup window.
        /// This method is called when the mouse is clicked on the control.
        /// </summary>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        protected virtual void TogglePopupVisible(MouseEventArgs e)
        {
            TogglePopupVisible();
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Value"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnValueChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called before popup window is shown..
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnBeforeShowPopup(EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Fired after popup window is closed. Applies value selected in the popup window
        /// to the control.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments</param>
        protected virtual void OnPopupWindowClosed(object? sender, EventArgs e)
        {
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            SafeDispose(ref popupWindow);

            base.DisposeManaged();
        }
    }
}
