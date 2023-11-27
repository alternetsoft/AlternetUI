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
        private readonly TextMemoPageProperties properties;

        public TextMemoPage()
        {
            properties = new(this);
            Margin = 10;
            memoPanel.DefaultRightPaneBestSize = 250;
            memoPanel.DefaultRightPaneMinSize = 250;

            memoPanel.SuggestedSize = new(500, 400); // how without it?
            memoPanel.TextBox.KeyDown += TextBox_KeyDown;
            memoPanel.TextBox.AutoUrlOpen = true;
            memoPanel.TextBox.TextUrl += MultiLineTextBox_TextUrl;
            //memoPanel.FileNewClick += MemoPanel_FileNewClick;
            //memoPanel.FileOpenClick += MemoPanel_FileOpenClick;
            //memoPanel.FileSaveClick += MemoPanel_FileSaveClick;

            if (!Application.IsMacOS)
                memoPanel.TextBox.AutoUrlOpen = true;

            var multilineDemoText = LoremIpsum;

            if (!SystemSettings.AppearanceIsDark || Application.IsWindowsOS)
            {
                memoPanel.TextBox.AutoUrl = true;
                multilineDemoText += "\nSample url: https://www.alternet-ui.com/\n";
            }

            memoPanel.TextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            PerformLayout();

            Idle += TextInputPage_Idle;

            memoPanel.PropGrid.ApplyFlags |= PropertyGridApplyFlags.SetValueAndReload;

            memoPanel.PropGrid.SetProps(properties, true);
            memoPanel.Parent = this;
            memoPanel.TextBox.Text = multilineDemoText;
            memoPanel.TextBox.SetInsertionPoint(0);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdNew, false);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdOpen, false);
            memoPanel.Toolbar.EnableTool(memoPanel.ButtonIdSave, false);
            memoPanel.PropGrid.SuggestedInitDefaults();
            memoPanel.RightNotebook.PageChanged += RightNotebook_PageChanged;
            memoPanel.ActionsControl.Required();
            memoPanel.AddAction("Go To Line", memoPanel.TextBox.ShowDialogGoToLine);
        }

        private void RightNotebook_PageChanged(object? sender, EventArgs e)
        {
            if(memoPanel.RightNotebook.EventSelection == memoPanel.PropGridPage?.Index)
            {
                memoPanel.PropGrid.CenterSplitter();
                memoPanel.RightNotebook.PageChanged -= RightNotebook_PageChanged;
            }
        }

        internal void AddTestIdleTasks()
        {
            memoPanel.AddAction("Add Idle Tasks", () =>
            {
                for(int i = 0; i <= 5; i++)
                {
                    Application.AddIdleTask((a) =>
                    {
                        Application.Log(a);
                    }, i);
                }
            });
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