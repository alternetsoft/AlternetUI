﻿using System;
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
        private TPopup? popupWindow;
        private object? data;
        private string popupWindowTitle = string.Empty;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SpeedButtonWithPopup{TPopup, TControl}"/> class.
        /// </summary>
        public SpeedButtonWithPopup()
        {
            // Ensure that control occupies space even if value is null.
            base.Text = " ";

            TextVisible = true;
            ShowComboBoxImageAtRight();
            ShowDropDownMenuWhenClicked = false;
        }

        /// <summary>
        /// Occurs when <see cref="Value"/> property is changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets a value indicating whether the popup window has been created.
        /// </summary>
        public virtual bool IsPopupWindowCreated => popupWindow is not null;

        /// <summary>
        /// Gets or sets selected color.
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
                    popupWindow = new ();
                    popupWindow.Title = popupWindowTitle;
                    popupWindow.AfterHide += OnPopupWindowClosed;
                }

                return popupWindow;
            }

            set
            {
                popupWindow = value;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="string"/>.
        /// </summary>
        [Browsable(false)]
        public new string? Text
        {
            get
            {
                string s = GetValueAsString(Value) ?? " ";
                if (string.IsNullOrEmpty(s))
                    s = " ";
                return s;
            }

            set
            {
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
            PopupWindow.ShowPopup(this);
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
        /// Called when the text of the control changes.
        /// </summary>
        public virtual void UpdateBaseText()
        {
            base.Text = Text ?? " ";
        }

        /// <inheritdoc/>
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ShowPopup();
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
        /// Gets enum value as string.
        /// </summary>
        /// <returns></returns>
        protected virtual string? GetValueAsString(object? d)
        {
            return d?.ToString();
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
