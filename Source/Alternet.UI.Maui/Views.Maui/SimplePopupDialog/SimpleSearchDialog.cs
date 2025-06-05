using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Maui
{
    /// <summary>
    /// Represents a dialog for performing a simple search and replace operations.
    /// </summary>
    /// <remarks>This dialog is designed to collect user input for a search operation and replace
    /// operations. It inherits functionality from <see cref="SimpleTwoFieldInputDialog"/>.</remarks>
    public partial class SimpleSearchDialog : SimpleTwoFieldInputDialog
    {
        /// <summary>
        /// Gets the default text displayed on the "Find Previous" button.
        /// </summary>
        public static string DefaultFindPrevButtonText = "Find previous";

        /// <summary>
        /// Gets the default text displayed on the "Find next" button.
        /// </summary>
        public static string DefaultFindNextButtonText = "Find next";

        /// <summary>
        /// Gets the default text displayed on the "Replace previous" button.
        /// </summary>
        public static string DefaultReplacePrevButtonText = "Replace previous";

        /// <summary>
        /// Gets the default text displayed on the "Replace next" button.
        /// </summary>
        public static string DefaultReplaceNextButtonText = "Replace next";

        /// <summary>
        /// Gets the default text displayed on the "Replace" button.
        /// </summary>
        public static string DefaultReplaceButtonText = "Replace";

        /// <summary>
        /// Gets the default text displayed on the "Replace all" button.
        /// </summary>
        public static string DefaultReplaceAllButtonText = "Replace all";

        /// <summary>
        /// Represents the default title used for search dialogs.
        /// </summary>
        public static string DefaultSearchDialogTitle = "Search";

        /// <summary>
        /// Represents the default title used for replace dialogs.
        /// </summary>
        public static string DefaultReplaceDialogTitle = "Replace";

        /// <summary>
        /// Represents the default message used for search entry.
        /// </summary>
        public static string DefaultSearchForMessage = "Search for";

        /// <summary>
        /// Represents the default message used for replace entry.
        /// </summary>
        public static string DefaultReplaceWithMessage = "Replace with";

        private readonly SimpleToolBarView.IToolBarItem firstButton;
        private readonly SimpleToolBarView.IToolBarItem secondButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSearchDialog"/> class.
        /// </summary>
        public SimpleSearchDialog()
        {
            SecondLabel.IsVisible = false;
            SecondEntryBorder.IsVisible = false;

            Title = DefaultSearchDialogTitle;
            Message = DefaultSearchForMessage;
            SecondMessage = DefaultReplaceWithMessage;
            Placeholder = string.Empty;
            SecondPlaceholder = string.Empty;

            firstButton = Buttons.AddDialogButton(DefaultFindPrevButtonText, null, null, () =>
            {
                OnFirstButtonClicked();
            });

            secondButton = Buttons.AddDialogButton(DefaultFindNextButtonText, null, null, () =>
            {
                OnSecondButtonClicked();
            });

            AddCancelButton();
        }

        /// <summary>
        /// Occurs when the "Replace" button is clicked.
        /// </summary>
        /// <remarks>This event is typically used to handle actions triggered by the
        /// "Replace" button in a user interface. Subscribers can use this event to
        /// perform operations such as replacing text or updating
        /// data.</remarks>
        public virtual Action? ReplaceButtonClicked { get; set; }

        /// <summary>
        /// Occurs when the "Replace All" button is clicked.
        /// </summary>
        /// <remarks>This event is typically used to handle actions triggered by the
        /// "Replace All" button in a user interface. Subscribers can use this event to
        /// perform operations such as replacing text or updating
        /// data.</remarks>
        public virtual Action? ReplaceAllButtonClicked { get; set; }

        /// <summary>
        /// Occurs when the "Find Next" button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the action triggered
        /// by the "Find Next" button. The
        /// event provides no additional data beyond the sender and event arguments.</remarks>
        public virtual Action? FindNextButtonClicked { get; set; }

        /// <summary>
        /// Occurs when the "Find Previous" button is clicked.
        /// </summary>
        /// <remarks>Subscribe to this event to handle the action triggered
        /// by the "Find Previous" button. The
        /// event provides no additional data beyond the sender and event arguments.</remarks>
        public virtual Action? FindPreviousButtonClicked { get;set; }

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

                firstButton.Text = value ? DefaultReplaceButtonText : DefaultFindPrevButtonText;
                secondButton.Text = value ? DefaultReplaceAllButtonText : DefaultFindNextButtonText;
            }
        }

        /// <inheritdoc/>
        public override bool NeedOkButton => false;

        /// <inheritdoc/>
        public override bool NeedCancelButton => false;

        /// <inheritdoc/>
        public override void OnOkButtonClicked(DialogCloseAction action)
        {
            base.OnOkButtonClicked(action);
            if (IsReplaceVisible)
                ReplaceButtonClicked?.Invoke();
            else
                FindNextButtonClicked?.Invoke();
        }

        /// <summary>
        /// Raises the appropriate event when the second button is clicked,
        /// based on the current <see cref="IsReplaceVisible"/> state.
        /// </summary>
        /// <remarks>If <see cref="IsReplaceVisible"/> is <see langword="true"/>,
        /// the <see cref="ReplaceAllButtonClicked"/> event is raised.
        /// Otherwise, the <see cref="FindNextButtonClicked"/> event
        /// is raised.</remarks>
        protected virtual void OnSecondButtonClicked()
        {
            if (IsReplaceVisible)
                ReplaceAllButtonClicked?.Invoke();
            else
                FindNextButtonClicked?.Invoke();
        }

        /// <summary>
        /// Raises the appropriate event based on the visibility state of the Replace button.
        /// </summary>
        /// <remarks>If <see cref="IsReplaceVisible"/> is <see langword="true"/>, the
        /// <see cref="ReplaceButtonClicked"/> event is raised. Otherwise, the
        /// <see cref="FindPreviousButtonClicked"/> event
        /// is raised.</remarks>
        protected virtual void OnFirstButtonClicked()
        {
            if (IsReplaceVisible)
                ReplaceButtonClicked?.Invoke();
            else
                FindPreviousButtonClicked?.Invoke();
        }
    }
}
