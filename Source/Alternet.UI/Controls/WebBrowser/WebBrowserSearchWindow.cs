using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class WebBrowserSearchWindow : DialogWindow
    {
        private readonly CheckBox findWrapCheckBox = new(CommonStrings.Default.FindOptionWrap);
        private readonly CheckBox findEntireWordCheckBox = new(CommonStrings.Default.FindOptionMatchWholeWord);
        private readonly CheckBox findMatchCaseCheckBox = new(CommonStrings.Default.FindOptionMatchCase);
        private readonly CheckBox findHighlightResultCheckBox = new(CommonStrings.Default.FindOptionHighlight);
        private readonly CheckBox findBackwardsCheckBox = new(CommonStrings.Default.FindOptionBackwards);

        private readonly VerticalStackPanel findPanel = new()
        {
            Margin = 5,
            Padding = 5,
        };

        private readonly TextBox findTextBox = new()
        {
            SuggestedWidth = 300,
            Margin = 5,
        };

        private readonly Button findButton = new(CommonStrings.Default.ButtonFind);
        private readonly Button findClearButton = new(CommonStrings.Default.ButtonClear);
        private readonly Button closeButton = new(CommonStrings.Default.ButtonCancel);
        private readonly WebBrowser webBrowser;
        private readonly WebBrowserFindParams findParams;

        public WebBrowserSearchWindow(WebBrowser webBrowser, WebBrowserFindParams findParams)
        {
            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Resizable = false;
            Title = CommonStrings.Default.ButtonFind;

            findButton.IsDefault = true;
            closeButton.IsCancel = true;

            this.webBrowser = webBrowser;
            this.findParams = findParams;

            findPanel.Parent = this;
            findTextBox.Parent = findPanel;

            findButton.Click += FindButton_Click;
            findClearButton.Click += FindClearButton_Click;
            closeButton.Click += CloseButton_Click;

            ControlSet optionControls = new(
                findWrapCheckBox,
                findEntireWordCheckBox,
                findMatchCaseCheckBox,
                findHighlightResultCheckBox,
                findBackwardsCheckBox);

            optionControls.Margin(5).Parent(findPanel);

            HorizontalStackPanel buttonPanel = new()
            {
                Parent = findPanel,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            ControlSet buttons = new(findButton, findClearButton, closeButton);
            buttons.Margin(5).Parent(buttonPanel);

            FindParamsToControls();

            this.SetSizeToContent();
            this.MinimumSize = Size;
            StartLocation = WindowStartLocation.CenterScreen;
        }

        private void CloseButton_Click(object? sender, EventArgs e)
        {
            this.ModalResult = ModalResult.Canceled;
            FindParamsFromControls();
            Close();
        }

        private void FindButton_Click(object? sender, EventArgs e)
        {
            FindParamsFromControls();
            int findResult = webBrowser.Find(findTextBox.Text, findParams);
            Application.DebugLog("Find Result = " + findResult.ToString());
        }

        private void FindParamsToControls()
        {
            findWrapCheckBox.IsChecked = findParams.Wrap;
            findEntireWordCheckBox.IsChecked = findParams.EntireWord;
            findMatchCaseCheckBox.IsChecked = findParams.MatchCase;
            findHighlightResultCheckBox.IsChecked = findParams.HighlightResult;
            findBackwardsCheckBox.IsChecked = findParams.Backwards;
        }

        private void FindClearButton_Click(object? sender, EventArgs e)
        {
            findTextBox.Text = string.Empty;
            webBrowser.FindClearResult();
        }

        private void FindParamsFromControls()
        {
            findParams.Wrap = findWrapCheckBox.IsChecked;
            findParams.EntireWord = findEntireWordCheckBox.IsChecked;
            findParams.MatchCase = findMatchCaseCheckBox.IsChecked;
            findParams.HighlightResult = findHighlightResultCheckBox.IsChecked;
            findParams.Backwards = findBackwardsCheckBox.IsChecked;
        }
    }
}
