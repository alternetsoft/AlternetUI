using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class WindowWebBrowserSearch : Window
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

        public WindowWebBrowserSearch(WebBrowserFindParams? prm = null)
        {
            prm ??= new();

            HasSystemMenu = false;
            IsToolWindow = true;
            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Resizable = true;
            Title = CommonStrings.Default.ButtonFind;

            findButton.IsDefault = true;
            closeButton.IsCancel = true;

            this.findParams = prm;

            findPanel.Parent = this;
            findTextBox.Parent = findPanel;

            findButton.Click += FindButton_Click;
            findClearButton.Click += FindClearButton_Click;
            closeButton.Click += CloseButton_Click;

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

        public virtual WebBrowser? WebBrowser => ControlUtils.FindVisibleControl<WebBrowser>();

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

        public virtual void ParamsToControls(WebBrowserFindParams prm)
        {
            findWrapCheckBox.IsChecked = prm.Wrap;
            findEntireWordCheckBox.IsChecked = prm.EntireWord;
            findMatchCaseCheckBox.IsChecked = prm.MatchCase;
            findHighlightResultCheckBox.IsChecked = prm.HighlightResult;
            findBackwardsCheckBox.IsChecked = prm.Backwards;
        }

        public virtual void ParamsFromControls(WebBrowserFindParams prm)
        {
            prm.Wrap = findWrapCheckBox.IsChecked;
            prm.EntireWord = findEntireWordCheckBox.IsChecked;
            prm.MatchCase = findMatchCaseCheckBox.IsChecked;
            prm.HighlightResult = findHighlightResultCheckBox.IsChecked;
            prm.Backwards = findBackwardsCheckBox.IsChecked;
        }

        private void OnCheckedChanged(object? sender, EventArgs e)
        {
            ParamsFromControls(findParams);
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void FindButton_Click(object? sender, EventArgs e)
        {
            ParamsFromControls(findParams);
            var findResult = WebBrowser?.Find(findTextBox.Text, findParams);
            if (findResult is not null)
                App.DebugLog("Find Result = " + findResult.ToString());
        }

        private void FindClearButton_Click(object? sender, EventArgs e)
        {
            findTextBox.Text = string.Empty;
            WebBrowser?.FindClearResult();
        }
    }
}
