using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
#endif

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a custom entry control that extends the functionality of the <see cref="Entry"/> class.
    /// </summary>
    public partial class BaseEntry : Entry
    {
        private static Alternet.UI.WeakReferenceValue<BaseEntry> focusedEntry = new();

        static BaseEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntry"/> class.
        /// </summary>
        public BaseEntry()
        {
            SizeChanged += (s, e) =>
            {
                SizeChangedAction?.Invoke();
            };

            Completed += (s, e) =>
            {
                CompletedAction?.Invoke();
            };

            Focused += (s, e) =>
            {
                focusedEntry.Value = this;
                if (SelectAllOnFocus)
                    SelectAll();
                FocusedAction?.Invoke();
            };

            Unfocused += (s, e) =>
            {
                if (focusedEntry.Value == this)
                    focusedEntry.Value = null;
                UnfocusedAction?.Invoke();
            };

            HandlerChanged += (s, e) =>
            {
                if(Handler is null)
                {
                    if (focusedEntry.Value == this)
                        focusedEntry.Value = null;
                }
                else
                {
#if WINDOWS
                    if (Handler.PlatformView is not Microsoft.UI.Xaml.Controls.TextBox platformView)
                        return;
                    platformView.KeyDown -= OnPlatformViewKeyDown;
                    platformView.KeyDown += OnPlatformViewKeyDown;
#endif
                }
            };
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BaseEntry"/> class.
        /// </summary>
        ~BaseEntry()
        {
            if (focusedEntry.Value == this)
                focusedEntry.Value = null;
        }

        /// <summary>
        /// Occurs when the Tab key is pressed while the entry is focused.
        /// </summary>
        public event EventHandler? TabClicked;

        /// <summary>
        /// Occurs when the Escape key is pressed while the entry is focused.
        /// </summary>
        public event EventHandler? EscapeClicked;

        /// <summary>
        /// Gets the currently focused <see cref="BaseEntry"/> instance, if any.
        /// </summary>
        public static BaseEntry? FocusedEntry
        {
            get
            {
                return focusedEntry.Value;
            }
        }

        /// <summary>
        /// Called when a key is pressed while the entry is focused.
        /// </summary>
        public Action<UI.Key>? KeyDownAction { get; set; }

        /// <summary>
        /// Called when size of the entry changes.
        /// </summary>
        public Action? SizeChangedAction { get; set; }

        /// <summary>
        /// Called when the entry is completed (e.g., when the user presses Enter).
        /// </summary>
        public Action? CompletedAction { get; set; }

        /// <summary>
        /// Called when the Tab key is pressed while the entry is focused.
        /// </summary>
        public Action? TabClickedAction { get; set; }

        /// <summary>
        /// Called when the Escape key is pressed while the entry is focused.
        /// </summary>
        public Action? EscapeClickedAction { get; set; }

        /// <summary>
        /// Called when text in the entry changes.
        /// </summary>
        public Action<string, string>? TextChangedAction { get; set; }

        /// <summary>
        /// Gets or sets an action which is invoked when the entry gains focus.
        /// </summary>
        public Action? FocusedAction { get; set; }

        /// <summary>
        /// Gets or sets an action which is invoked when the entry loses focus.
        /// </summary>
        public Action? UnfocusedAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all text should be selected when
        /// the control gains focus.
        /// </summary>
        public bool SelectAllOnFocus { get; set; } = true;

        /// <summary>
        /// Selects all the text in the entry field.
        /// Sets the cursor position to the start and the selection length to the full text length.
        /// </summary>
        public virtual void SelectAll()
        {
#if ANDROID
            var editText = Handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
            if (editText != null)
            {
                editText.SetSelection(0, editText.Text?.Length ?? 0);
            }
#endif

#if IOS || MACCATALYST
        var textField = Handler?.PlatformView as UIKit.UITextField;
        if (textField != null)
        {
            var start = textField.BeginningOfDocument;
            var end = textField.EndOfDocument;
            textField.SelectedTextRange = textField.GetTextRange(start, end);
        }
#endif

#if WINDOWS
        var textBox = Handler?.PlatformView as Microsoft.UI.Xaml.Controls.TextBox;
        textBox?.SelectAll();
#endif
        }

        /// <summary>
        /// Raises the <see cref="EscapeClicked"/> event.
        /// </summary>
        public virtual void RaiseEscapeClicked()
        {
            EscapeClicked?.Invoke(this, EventArgs.Empty);
            EscapeClickedAction?.Invoke();
        }

        /// <summary>
        /// Raises the <see cref="TabClicked"/> event.
        /// </summary>
        public virtual void RaiseTabClicked()
        {
            TabClicked?.Invoke(this, EventArgs.Empty);
            TabClickedAction?.Invoke();
        }

        /// <summary>
        /// Sets all events actions to null, effectively removing any event handlers.
        /// </summary>
        public virtual void ResetEventActions()
        {
            SizeChangedAction = null;
            CompletedAction = null;
            TextChangedAction = null;
            FocusedAction = null;
            UnfocusedAction = null;
            TabClickedAction = null;
            EscapeClickedAction = null;
            KeyDownAction = null;
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);
            TextChangedAction?.Invoke(oldValue, newValue);
        }

#if WINDOWS
        private void OnPlatformViewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            var key = Alternet.UI.MauiKeyboardHandler.Default.Convert(e.Key);

            KeyDownAction?.Invoke(key);

            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                RaiseEscapeClicked();
                return;
            }

            if (e.Key == Windows.System.VirtualKey.Tab)
            {
                RaiseTabClicked();
                return;
            }
        }
#endif
    }
}
