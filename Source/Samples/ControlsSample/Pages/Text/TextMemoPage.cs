using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextMemoPage : VerticalStackPanel
    {
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        private readonly MultilineTextBox multiLineTextBox = new()
        {
            SuggestedSize = new(350, 130),
        };

        private readonly ComboBoxAndLabel wordWrapComboBox = new("Word Wrap:");
        private readonly Button multilineGoToButton = new("Go To...");

        public TextMemoPage()
        {
            Group(multiLineTextBox, wordWrapComboBox, multilineGoToButton).Margin(5).Parent(this)
                .HorizontalAlignment(HorizontalAlignment.Left);
            if (!Application.IsMacOs)
                multiLineTextBox.AutoUrlOpen = true;

            var multilineDemoText = LoremIpsum;

            if (!SystemSettings.AppearanceIsDark || Application.IsWindowsOS)
            {
                multiLineTextBox.AutoUrl = true;
                multilineDemoText += "\nSample url: https://www.alternet-ui.com/\n";
            }

            multiLineTextBox.Text = LoremIpsum;
            multiLineTextBox.TextUrl += MultiLineTextBox_TextUrl;
            multiLineTextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            multilineGoToButton.ClickAction = multiLineTextBox.ShowDialogGoToLine;

            wordWrapComboBox.MainControl.BindEnumProp(multiLineTextBox, nameof(TextBox.TextWrap));

            Idle += TextInputPage_Idle;
        }

        private void TextBox_CurrentPositionChanged(object? sender, EventArgs e)
        {
            if (TextInputPage.LogPosition)
                TextBoxUtils.LogPosition(sender);
        }

        internal static void MultiLineTextBox_TextUrl(object? sender, UrlEventArgs e)
        {
            Application.Log("TextBox: Url clicked =>" + e.Url);
            var modifiers = AllPlatformDefaults.PlatformCurrent.TextBoxUrlClickModifiers;
            if (e.Modifiers != modifiers)
            {
                var modifiersText = ModifierKeysConverter.ToString(modifiers, true);
                Application.Log($"Use {modifiersText}+Click to open in the default browser: " + e.Url);
            }
        }

        private void TextInputPage_Idle(object? sender, EventArgs e)
        {
            if(Visible)
                multiLineTextBox.IdleAction();
        }

    }
}