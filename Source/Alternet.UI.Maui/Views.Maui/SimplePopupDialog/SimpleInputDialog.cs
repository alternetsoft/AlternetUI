using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a simple input dialog with a title, message, input field, and action buttons.
    /// </summary>
    public partial class SimpleInputDialog : SimplePopupDialog
    {
        private readonly BaseEntry entry;
        private readonly Border entryBorder;
        private readonly Label label;

        private Alternet.UI.AsyncUtils.ValueWaiter<string>? valueWaiter;

        static SimpleInputDialog()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleInputDialog"/> class.
        /// </summary>
        public SimpleInputDialog()
        {
            Title = GetDefaultTitle();

            label = new Label
            {
                Text = GetDefaultMessage(),
                Margin = new Thickness(5, 5, 5, 5),
            };

            ContentLayout.Children.Add(label);

            entryBorder = new Border
            {
                StrokeThickness = 1,
                Margin = new Thickness(5),
                Padding = new Thickness(0),
                StrokeShape = new RoundRectangle { CornerRadius = 5 },
            };

            entry = new BaseEntry
            {
                Placeholder = GetDefaultPlaceholder(),
            };

            entry.EscapeClicked += (s, e) =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.EscapeKey);
            };

            entry.Completed += (s, e) =>
            {
                OnOkButtonClicked(UI.DialogCloseAction.EnterKey);
            };

            entryBorder.Content = entry;
            ContentLayout.Children.Add(entryBorder);

            Buttons.Required();
            ResetColors();
        }

        /// <summary>
        /// Gets or sets the message displayed in the dialog.
        /// </summary>
        public virtual string Message
        {
            get => label.Text;
            set => label.Text = value;
        }

        /// <summary>
        /// Gets or sets the text entered in the input field.
        /// </summary>
        public virtual string Text
        {
            get => entry.Text;
            set => entry.Text = value;
        }

        /// <summary>
        /// Gets or sets the placeholder text for the input field.
        /// </summary>
        public virtual string Placeholder
        {
            get => entry.Placeholder;
            set => entry.Placeholder = value;
        }

        /// <summary>
        /// Gets the input field of the dialog.
        /// </summary>
        public Entry Entry => entry;

        /// <summary>
        /// Gets the border of the input field.
        /// </summary>
        public Border EntryBorder => entryBorder;

        /// <summary>
        /// Gets the label displaying the message in the dialog.
        /// </summary>
        public Label Label => label;

        /// <summary>
        /// Creates a new instance of <see cref="SimpleInputDialog"/>
        /// configured as a "Go To Line" dialog.
        /// </summary>
        /// <returns>A <see cref="SimpleInputDialog"/> instance with pre-configured title,
        /// message, and numeric input keyboard.</returns>
        public static SimpleInputDialog CreateGoToLineDialog()
        {
            SimpleInputDialog result = new();

            result.Title = "Go To Line";
            result.Message = "Line number";
            result.Text = "1";
            result.Entry.Keyboard = Keyboard.Numeric;

            return result;
        }

        /// <summary>
        /// Works similar to <see cref="Page.DisplayPromptAsync"/> but is implemented
        /// internally in the library.
        /// </summary>
        public static Task<string> DisplayPromptAsync(
                object? owner,
                string title,
                string message,
                string accept = "OK",
                string cancel = "Cancel",
                string? placeholder = null,
                int maxLength = -1,
                Microsoft.Maui.Keyboard? keyboard = default,
                string initialValue = "")
        {
            var page = UI.MauiUtils.GetObjectPage(owner);
            AbsoluteLayout? absoluteLayout = UI.MauiUtils.GetObjectAbsoluteLayout(owner);

            if(page is not null && absoluteLayout is not null)
            {
                SimpleInputDialog? dialog
                    = UI.MauiUtils.FindViewInContainer<SimpleInputDialog>(absoluteLayout);

                dialog ??= new();

                if(dialog.valueWaiter is not null)
                {
                    dialog.valueWaiter.SetValue(null!);
                    dialog.valueWaiter = null;
                }

                AssignProperties(dialog);
                dialog.valueWaiter = new();

                dialog.SetAlignedPosition(absoluteLayout, DefaultAlignedPosition);
                dialog.Owner = page;
                dialog.IsVisible = true;
                dialog.Entry.Focus();

                return dialog.valueWaiter.WaitForValueAsync();
            }

            void AssignProperties(SimpleInputDialog dialog)
            {
                dialog.Title = title;
                dialog.Message = message;
                dialog.Text = initialValue;
                if (maxLength >= 0)
                    dialog.Entry.MaxLength = maxLength;
                else
                    dialog.Entry.MaxLength = int.MaxValue;
                dialog.Entry.Keyboard = keyboard;
            }

            return DefaultDisplayPromptAsync(
                page,
                title,
                message,
                accept,
                cancel,
                placeholder,
                maxLength,
                keyboard,
                initialValue);
        }

        /// <summary>
        /// Calls <see cref="Page.DisplayPromptAsync"/>.
        /// </summary>
        public static Task<string> DefaultDisplayPromptAsync(
                object? owner,
                string title,
                string message,
                string accept = "OK",
                string cancel = "Cancel",
                string? placeholder = null,
                int maxLength = -1,
                Microsoft.Maui.Keyboard? keyboard = default,
                string initialValue = "")
        {
            var page = UI.MauiUtils.GetObjectPage(owner);

            if(page is null)
                return Task.FromResult<string>(null!);

            return page.DisplayPromptAsync(
                title,
                message,
                accept,
                cancel,
                placeholder,
                maxLength,
                keyboard,
                initialValue);
        }

        /// <inheritdoc/>
        public override void OnOkButtonClicked(Alternet.UI.DialogCloseAction action)
        {
            base.OnOkButtonClicked(action);

            if (valueWaiter is not null)
            {
                valueWaiter.SetValue(Entry.Text);
                valueWaiter = null;
            }
        }

        /// <inheritdoc/>
        public override void OnCancelButtonClicked(Alternet.UI.DialogCloseAction action)
        {
            base.OnCancelButtonClicked(action);

            if (valueWaiter is not null)
            {
                valueWaiter.SetValue(null!);
                valueWaiter = null;
            }
        }

        /// <inheritdoc/>
        public override void ResetColors()
        {
            if (label is null)
                return;

            base.ResetColors();

            var backColor = GetBackColor();
            var textColor = GetTextColor();

            var borderColor = GetBorderColor();
            var placeholderColor = GetPlaceholderColor();

            label.TextColor = textColor;
            entryBorder.Stroke = borderColor;
            entryBorder.BackgroundColor = backColor;
            entry.TextColor = textColor;
            entry.PlaceholderColor = placeholderColor;
        }

        /// <summary>
        /// Gets the default title for the input dialog.
        /// </summary>
        /// <returns>A string representing the default title.</returns>
        public virtual string GetDefaultTitle()
        {
            return "Input";
        }

        /// <summary>
        /// Gets the default message for the input dialog.
        /// </summary>
        /// <returns>A string representing the default message.</returns>
        public virtual string GetDefaultMessage()
        {
            return "Enter value:";
        }

        /// <summary>
        /// Gets the default placeholder text for the input field.
        /// </summary>
        /// <returns>A string representing the default placeholder text.</returns>
        public virtual string GetDefaultPlaceholder()
        {
            return "Type here";
        }
    }
}