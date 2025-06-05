using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a dialog for performing a simple search and replace operations.
    /// </summary>
    /// <remarks>This dialog is designed to collect user input for a search operation and replace
    /// operations. It inherits functionality from <see cref="SimpleTwoFieldInputDialog"/>.</remarks>
    public partial class SimpleSearchDialog : SimpleTwoFieldInputDialog
    {
        public static string DefaultFindPrevButtonText = "Find previous";

        public static string DefaultFindNextButtonText = "Find next";

        public static string DefaultReplacePrevButtonText = "Replace previous";

        public static string DefaultReplaceNextButtonText = "Replace next";

        public static string DefaultSearchDialogTitle = "Search";

        public static string DefaultReplaceDialogTitle = "Replace";

        public static string DefaultSearchForMessage = "Search for";

        public static string DefaultreplaceWithMessage = "Replace with";

        private readonly SimpleToolBarView.IToolBarItem findPreviousButton;
        private readonly SimpleToolBarView.IToolBarItem findNextButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSearchDialog"/> class.
        /// </summary>
        public SimpleSearchDialog()
        {
            SecondLabel.IsVisible = false;
            SecondEntryBorder.IsVisible = false;

            Title = DefaultSearchDialogTitle;
            Message = DefaultSearchForMessage;
            SecondMessage = DefaultreplaceWithMessage;
            Placeholder = string.Empty;
            SecondPlaceholder = string.Empty;

            findPreviousButton = Buttons.AddDialogButton(DefaultFindPrevButtonText, null, null, () =>
            {
            });

            findNextButton = Buttons.AddDialogButton(DefaultFindNextButtonText, null, null, () =>
            {
            });

            AddCancelButton();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the replace functionality is visible.
        /// </summary>
        public virtual bool IsReplaceVisible
        {
            get
            {
                return SecondLabel.IsVisible;
            }

            set
            {
                if (IsReplaceVisible == value)
                    return;
                SecondLabel.IsVisible = value;
                SecondEntryBorder.IsVisible = value;

                Title = value ? DefaultReplaceDialogTitle : DefaultSearchDialogTitle;

                findPreviousButton.Text
                    = value ? DefaultReplacePrevButtonText : DefaultFindPrevButtonText;

                findNextButton.Text
                    = value ? DefaultReplaceNextButtonText : DefaultFindNextButtonText;
            }
        }

        /// <inheritdoc/>
        public override bool NeedOkButton => false;

        /// <inheritdoc/>
        public override bool NeedCancelButton => false;
    }
}
