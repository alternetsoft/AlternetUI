using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            memoPanel.TextBox.KeyDown += TextBox_KeyDown;
            memoPanel.TextBox.AutoUrlOpen = true;
            memoPanel.TextBox.TextUrl += MultiLineTextBox_TextUrl;
            //memoPanel.FileNewClick += MemoPanel_FileNewClick;
            //memoPanel.FileOpenClick += MemoPanel_FileOpenClick;
            //memoPanel.FileSaveClick += MemoPanel_FileSaveClick;

            if (!App.IsMacOS)
                memoPanel.TextBox.AutoUrlOpen = true;

            var multilineDemoText = LoremIpsum;

            if (!SystemSettings.AppearanceIsDark || App.IsWindowsOS)
            {
                memoPanel.TextBox.AutoUrl = true;
                multilineDemoText += "\nSample url: https://www.alternet-ui.com/\n";
            }

            memoPanel.TextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            PerformLayout();

            Idle += TextInputPage_Idle;

            memoPanel.Parent = this;
            memoPanel.TextBox.Text = multilineDemoText;
            memoPanel.TextBox.SetInsertionPoint(0);

            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdNew, false);
            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdOpen, false);
            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdSave, false);
        }

        private void TextInputPage_Idle(object? sender, EventArgs e)
        {
            if (Visible)
                memoPanel.TextBox.IdleAction();
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            static void Test()
            {
            }

            if (KnownShortcuts.RunTest.Run(e, Test))
                return;
        }

        private void TextBox_CurrentPositionChanged(object? sender, EventArgs e)
        {
            if (TextInputPage.LogPosition)
                TextBoxUtils.LogPosition(sender);
        }

        internal static void MultiLineTextBox_TextUrl(object? sender, UrlEventArgs e)
        {
            App.Log("TextBox: Url clicked =>" + e.Url);
            var modifiers = AllPlatformDefaults.PlatformCurrent.TextBoxUrlClickModifiers;
            if (e.Modifiers != modifiers)
            {
                var modifiersText = ModifierKeysConverter.ToString(modifiers, true);
                App.Log($"Use {modifiersText}+Click to open in the default browser: " + e.Url);
            }
        }

        internal class TextMemoPageProperties : BaseChildObject<TextMemoPage>
        {
            public TextMemoPageProperties(TextMemoPage owner) : base(owner)
            {
            }

            [Browsable(false)]
            public TextBox Control => Owner.memoPanel.TextBox;

            public TextBoxTextWrap WordWrap
            {
                get => Control.TextWrap;
                set => Control.TextWrap = value;
            }

            public bool ReadOnly
            {
                get => Control.ReadOnly;
                set => Control.ReadOnly = value;
            }
        }
    }
}