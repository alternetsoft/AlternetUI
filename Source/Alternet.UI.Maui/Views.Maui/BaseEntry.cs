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
            Focused += (s, e) =>
            {
                focusedEntry.Value = this;
                if (SelectAllOnFocus)
                    SelectAll();
            };

            Unfocused += (s, e) =>
            {
                if (focusedEntry.Value == this)
                    focusedEntry.Value = null;
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
                    var platformView = Handler.PlatformView as Microsoft.UI.Xaml.Controls.TextBox;
                    if (platformView is null)
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
            CursorPosition = 0;
            SelectionLength = Text?.Length ?? 0;
        }

        /// <summary>
        /// Raises the <see cref="EscapeClicked"/> event.
        /// </summary>
        public virtual void RaiseEscapeClicked()
        {
            EscapeClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override void OnTextChanged(string oldValue, string newValue)
        {
            base.OnTextChanged(oldValue, newValue);
        }

#if WINDOWS
        private void OnPlatformViewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Escape)
            {
                RaiseEscapeClicked();
            }
        }
#endif
    }
}
