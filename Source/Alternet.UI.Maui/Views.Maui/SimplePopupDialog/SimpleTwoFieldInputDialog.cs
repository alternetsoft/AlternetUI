using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a dialog that allows the user to input values for two fields.
    /// </summary>
    /// <remarks>This class is a specialized version of <see cref="SimpleInputDialog"/>
    /// designed for scenarios where two input fields are required.
    /// It provides functionality for capturing and validating user input for both fields.</remarks>
    public partial class SimpleTwoFieldInputDialog : SimpleInputDialog
    {
        private BaseEntry? secondEntry;
        private Border? secondEntryBorder;
        private Label? secondLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleTwoFieldInputDialog"/> class.
        /// </summary>
        public SimpleTwoFieldInputDialog()
        {
        }

        /// <summary>
        /// Gets or sets the text for the second label.
        /// </summary>
        public string SecondMessage
        {
            get => secondLabel?.Text ?? string.Empty;

            set
            {
                if(secondLabel is not null)
                    secondLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the text entered in the second input field.
        /// </summary>
        public string SecondText
        {
            get => secondEntry?.Text ?? string.Empty;

            set
            {
                if(secondEntry is not null)
                    secondEntry.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the placeholder text for the second input field.
        /// </summary>
        public string SecondPlaceholder
        {
            get => secondEntry?.Placeholder ?? string.Empty;

            set
            {
                if (secondEntry is not null)
                    secondEntry.Placeholder = value;
            }
        }

        /// <summary>
        /// Gets the second input field of the dialog.
        /// </summary>
        public Entry SecondEntry => secondEntry!;

        /// <summary>
        /// Gets the border of the second input field.
        /// </summary>
        public Border SecondEntryBorder => secondEntryBorder!;

        /// <summary>
        /// Gets the second label displaying the message in the dialog.
        /// </summary>
        public Label SecondLabel => secondLabel!;

        /// <inheritdoc/>
        public override void ResetColors()
        {
            if (secondLabel is null)
                return;
            base.ResetColors();

            secondLabel.TextColor = Label.TextColor;
            secondEntryBorder!.Stroke = EntryBorder.Stroke;
            secondEntryBorder.BackgroundColor = EntryBorder.BackgroundColor;
            secondEntry!.TextColor = Entry.TextColor;
            secondEntry.PlaceholderColor = Entry.PlaceholderColor;
        }

        /// <summary>
        /// Gets the default message for the second input field.
        /// </summary>
        /// <returns>A string representing the default message.</returns>
        public virtual string GetDefaultSecondMessage()
        {
            return "Enter second value:";
        }

        /// <summary>
        /// Gets the default placeholder text for the second input field.
        /// </summary>
        /// <returns>A string representing the default placeholder text.</returns>
        public virtual string GetDefaultSecondPlaceholder()
        {
            return string.Empty;
        }

        /// <inheritdoc/>
        protected override void CreateOtherContent()
        {
            secondLabel = new Label
            {
                Text = GetDefaultSecondMessage(),
                Margin = GetLabelMargin(),
            };

            ContentLayout.Children.Add(secondLabel);

            secondEntryBorder = CreateEntryBorder();

            secondEntry = new BaseEntry
            {
                Placeholder = GetDefaultSecondPlaceholder(),
            };

            secondEntry.EscapeClicked += (s, e) =>
            {
                OnCancelButtonClicked(UI.DialogCloseAction.EscapeKey);
            };

            secondEntry.Completed += (s, e) =>
            {
                OnOkButtonClicked(UI.DialogCloseAction.EnterKey);
            };

            secondEntryBorder.Content = secondEntry;
            ContentLayout.Children.Add(secondEntryBorder);
        }
    }
}