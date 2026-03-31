using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements a search window for <see cref="WebBrowser"/> control.
    /// </summary>
    public partial class WindowWebBrowserSearch : Window
    {
        private readonly CheckBox findWrapCheckBox = new(CommonStrings.Default.FindOptionWrap);
        private readonly CheckBox findEntireWordCheckBox
            = new(CommonStrings.Default.FindOptionMatchWholeWord);

        private readonly CheckBox findMatchCaseCheckBox
            = new(CommonStrings.Default.FindOptionMatchCase);

        private readonly CheckBox findHighlightResultCheckBox
            = new(CommonStrings.Default.FindOptionHighlight);

        private readonly CheckBox findBackwardsCheckBox
            = new(CommonStrings.Default.FindOptionBackwards);

        private readonly VerticalStackPanel findPanel = new()
        {
            Margin = 5,
            Padding = 5,
        };

        private readonly TextBox findTextBox = new()
        {
            MinWidth = 450,
            Margin = 5,
        };

        private readonly Button findButton = new(CommonStrings.Default.ButtonFind);
        private readonly Button findClearButton = new(CommonStrings.Default.ButtonClear);
        private readonly Button closeButton = new(CommonStrings.Default.ButtonCancel);

        private WebBrowserFindParams findParams;

        /// <summary>
        /// Initializes a new instance of the WindowWebBrowserSearch class with optional search parameters.
        /// </summary>
        /// <remarks>This constructor configures the window for use as a search dialog within a web
        /// browser context. It sets up the window's appearance, default button behaviors, and initializes controls
        /// based on the provided or default search parameters.</remarks>
        /// <param name="prm">The search parameters to initialize the window with. If null, default parameters are used.</param>
        public WindowWebBrowserSearch(WebBrowserFindParams? prm = null)
        {
            prm ??= new();

            HasSystemMenu = false;
            IsToolWindow = true;
            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Resizable = true;
            KeyPreview = true;
            Title = CommonStrings.Default.ButtonFind;

            findButton.IsDefault = true;
            closeButton.IsCancel = true;

            this.findParams = prm;

            findPanel.Parent = this;
            findTextBox.Parent = findPanel;

            findButton.Click += OnFindButtonClick;
            findClearButton.Click += OnFindClearButtonClick;
            closeButton.Click += OnCloseButtonClick;

            findHighlightResultCheckBox.Enabled = false;

            ParamsToControls(this.findParams);

            Group(
                findWrapCheckBox,
                findEntireWordCheckBox,
                findMatchCaseCheckBox,
                findHighlightResultCheckBox,
                findBackwardsCheckBox).Margin(5).Parent(findPanel)
                .WhenCheckedChanged(OnCheckedChanged);

            HorizontalStackPanel buttonPanel = new()
            {
                Parent = findPanel,
                HorizontalAlignment = UI.HorizontalAlignment.Right,
            };

            Group(findButton, findClearButton, closeButton).Margin(5).Parent(buttonPanel);

            StartLocation = WindowStartLocation.CenterScreen;
            ActiveControl = findTextBox;

            this.SetSizeToContent(WindowSizeToContentMode.WidthAndHeight);
            this.MinimumSize = Size;
        }

        /// <summary>
        /// Gets the first visible WebBrowser control contained within the current context, if any.
        /// </summary>
        public virtual WebBrowser? WebBrowser => ControlUtils.FindVisibleControl<WebBrowser>();

        /// <summary>
        /// Gets or sets the parameters used for find operations in the web browser control.
        /// </summary>
        /// <remarks>Changing this property updates the associated controls to reflect the new find
        /// parameters. Use this property to configure search behavior such as case sensitivity or search
        /// direction.</remarks>
        public virtual WebBrowserFindParams FindParams
        {
            get
            {
                return findParams;
            }

            set
            {
                if (findParams == value)
                    return;
                findParams = value;
                ParamsToControls(value);
            }
        }

        /// <summary>
        /// Updates the state of the find-related UI controls to reflect the specified search parameters.
        /// </summary>
        /// <remarks>Use this method to synchronize the UI controls with the current search settings, such
        /// as when loading or restoring user preferences.</remarks>
        /// <param name="prm">An object containing the search parameters whose values are used to update the corresponding UI controls.
        /// Cannot be null.</param>
        public virtual void ParamsToControls(WebBrowserFindParams prm)
        {
            findWrapCheckBox.IsChecked = prm.Wrap;
            findEntireWordCheckBox.IsChecked = prm.EntireWord;
            findMatchCaseCheckBox.IsChecked = prm.MatchCase;
            findHighlightResultCheckBox.IsChecked = prm.HighlightResult;
            findBackwardsCheckBox.IsChecked = prm.Backwards;
        }

        /// <summary>
        /// Updates the specified parameter object with the current values from the find dialog controls.
        /// </summary>
        /// <remarks>Use this method to synchronize the state of the dialog's controls with a
        /// WebBrowserFindParams instance before performing a search operation.</remarks>
        /// <param name="prm">The parameter object to populate with values from the dialog's controls. Cannot be null.</param>
        public virtual void ParamsFromControls(WebBrowserFindParams prm)
        {
            prm.Wrap = findWrapCheckBox.IsChecked;
            prm.EntireWord = findEntireWordCheckBox.IsChecked;
            prm.MatchCase = findMatchCaseCheckBox.IsChecked;
            prm.HighlightResult = findHighlightResultCheckBox.IsChecked;
            prm.Backwards = findBackwardsCheckBox.IsChecked;
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.IsHandledOrSuppressed)
                return;

            if (e.IsEscape)
            {
                OnCloseButtonClick(this, EventArgs.Empty);
                e.Suppressed();
            }
            else
            if (e.IsEnter)
            {
                OnFindButtonClick(this, EventArgs.Empty);
                e.Suppressed();
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event for find option checkboxes.
        /// </summary>
        /// <remarks>This method synchronizes the current state of the UI controls with the internal
        /// find parameters when any checkbox state changes.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnCheckedChanged(object? sender, EventArgs e)
        {
            ParamsFromControls(findParams);
        }

        /// <summary>
        /// Handles the Click event for the close button.
        /// </summary>
        /// <remarks>This method closes the search window when the close button is clicked.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnCloseButtonClick(object? sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the Click event for the find button.
        /// </summary>
        /// <remarks>This method synchronizes the find parameters with the UI controls, performs a
        /// search in the web browser using the current text and parameters, and logs the result if debugging is enabled.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnFindButtonClick(object? sender, EventArgs e)
        {
            ParamsFromControls(findParams);
            var findResult = WebBrowser?.Find(findTextBox.Text, findParams);
            if (findResult is not null)
                App.DebugLogIf("Find Result = " + findResult.ToString(), false);
        }

        /// <summary>
        /// Handles the Click event for the clear button.
        /// </summary>
        /// <remarks>This method clears the search text box and removes any search result highlighting
        /// from the web browser control.</remarks>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        protected virtual void OnFindClearButtonClick(object? sender, EventArgs e)
        {
            findTextBox.Text = string.Empty;
            WebBrowser?.FindClearResult();
        }
    }
}
