using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextMemoPage : Control
    {
        private const string LoremIpsum =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        private readonly PanelMultilineTextBox memoPanel = new();

        public TextMemoPage()
        {
            Margin = 10;
            memoPanel.ActionsControl.Required();
            memoPanel.SuggestedSize = new(500, 400); // how without it?
            memoPanel.TextBox.KeyDown += TextBox_KeyDown;
            memoPanel.TextBox.AutoUrlOpen = true;
            memoPanel.TextBox.TextUrl += MultiLineTextBox_TextUrl;
            //memoPanel.FileNewClick += MemoPanel_FileNewClick;
            //memoPanel.FileOpenClick += MemoPanel_FileOpenClick;
            //memoPanel.FileSaveClick += MemoPanel_FileSaveClick;

            if (!Application.IsMacOs)
                memoPanel.TextBox.AutoUrlOpen = true;

            var multilineDemoText = LoremIpsum;

            if (!SystemSettings.AppearanceIsDark || Application.IsWindowsOS)
            {
                memoPanel.TextBox.AutoUrl = true;
                multilineDemoText += "\nSample url: https://www.alternet-ui.com/\n";
            }

            memoPanel.TextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            memoPanel.AddAction("Go To Line", memoPanel.TextBox.ShowDialogGoToLine);

            PerformLayout();

            Idle += TextInputPage_Idle;

            memoPanel.Parent = this;
            memoPanel.TextBox.Text = multilineDemoText;
            memoPanel.TextBox.SetInsertionPoint(0);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdNew, false);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdOpen, false);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdSave, false);

            //wordWrapComboBox.MainControl.BindEnumProp(multiLineTextBox, nameof(TextBox.TextWrap));
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            static void Test()
            {
            }

            if (KnownKeys.RunTest.Run(e, Test))
                return;
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
                memoPanel.TextBox.IdleAction();
        }

    }
}