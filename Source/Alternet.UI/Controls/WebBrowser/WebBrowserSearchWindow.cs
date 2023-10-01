using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class WebBrowserSearchWindow : Window
    {
        private readonly CheckBox findWrapCheckBox = new()
        {
            Text = "Wrap",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly CheckBox findEntireWordCheckBox = new()
        {
            Text = "Entire Word",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly CheckBox findMatchCaseCheckBox = new()
        {
            Text = "Match Case",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly CheckBox findHighlightResultCheckBox = new()
        {
            Text = "Highlight",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly CheckBox findBackwardsCheckBox = new()
        {
            Text = "Backwards",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly StackPanel findPanel = new()
        {
            Margin = new Thickness(5, 5, 5, 5),
            Orientation = StackPanelOrientation.Vertical,
            Padding = 5,
        };

        private readonly TextBox findTextBox = new()
        {
            SuggestedWidth = 300,
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly Button findButton = new()
        {
            Text = "Find",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly Button findClearButton = new()
        {
            Text = "Find Clear",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly Button closeButton = new()
        {
            Text = "Close",
            Margin = new Thickness(5, 5, 5, 5),
        };

        private readonly WebBrowser webBrowser;
        private readonly WebBrowserFindParams findParams;

        public WebBrowserSearchWindow(WebBrowser webBrowser, WebBrowserFindParams findParams)
        {
            ShowInTaskbar = false;
            MinimizeEnabled = false;
            MaximizeEnabled = false;
            Resizable = false;
            StartLocation = WindowStartLocation.CenterScreen;
            Title = CommonStrings.Default.ButtonFind;

            findButton.IsDefault = true;
            closeButton.IsCancel = true;

            this.webBrowser = webBrowser;
            this.findParams = findParams;

            this.Children.Add(findPanel);

            findPanel.Children.Add(findTextBox);

            findButton.Click += FindButton_Click;
            findClearButton.Click += FindClearButton_Click;
            closeButton.Click += CloseButton_Click;

            findPanel.Children.Add(findWrapCheckBox);
            findPanel.Children.Add(findEntireWordCheckBox);
            findPanel.Children.Add(findMatchCaseCheckBox);
            findPanel.Children.Add(findHighlightResultCheckBox);
            findPanel.Children.Add(findBackwardsCheckBox);

            HorizontalStackPanel buttonPanel = new()
            {
                Parent = findPanel,
                HorizontalAlignment = HorizontalAlignment.Right,
            };

            buttonPanel.Children.Add(findButton);
            buttonPanel.Children.Add(findClearButton);
            buttonPanel.Children.Add(closeButton);

            FindParamsToControls();

            this.SetSizeToContent();
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
