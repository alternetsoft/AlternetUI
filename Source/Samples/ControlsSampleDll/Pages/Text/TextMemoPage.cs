using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    public class TextMemoPage : Panel
    {
        private readonly PanelMultilineTextBox memoPanel = new();
        private readonly Timer timer = new(100);

        public TextMemoPage()
        {
            memoPanel.TextBox.KeyDown += TextBox_KeyDown;
            memoPanel.TextBox.TextUrl += OnMultiLineTextBoxTextUrl;
            //memoPanel.FileNewClick += MemoPanel_FileNewClick;
            //memoPanel.FileOpenClick += MemoPanel_FileOpenClick;
            //memoPanel.FileSaveClick += MemoPanel_FileSaveClick;

            var multilineDemoText = DemoUtils.LoremIpsum;

            memoPanel.TextBox.CurrentPositionChanged += TextBox_CurrentPositionChanged;

            PerformLayout();

            timer.TickAction = () =>
            {
                if (Visible)
                    memoPanel.TextBox.IdleAction();
            };

            timer.StartRepeated();

            memoPanel.Parent = this;
            memoPanel.TextBox.Text = multilineDemoText;
            memoPanel.TextBox.SetInsertionPoint(0);

            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdNew, false);
            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdOpen, false);
            memoPanel.ToolBar.SetToolEnabled(memoPanel.ButtonIdSave, false);
        }

        protected override void DisposeManaged()
        {
            timer.Stop();
            base.DisposeManaged();
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
            if (!TextInputPage.LogPosition)
                return;

            var currentPos = memoPanel.TextBox.CurrentPosition;
            if (currentPos is null)
                return;
            var name = memoPanel.TextBox.Name ?? memoPanel.TextBox.GetType().Name;
            var prefix = $"{name}.CurrentPos:";
            App.LogReplace($"{prefix} {currentPos.Value + 1}", prefix);
        }

        public static void OnMultiLineTextBoxTextUrl(object? sender, UrlEventArgs e)
        {
            App.Log("TextBox: Url clicked =>" + e.Url);
            var modifiers = AllPlatformDefaults.PlatformCurrent.TextBoxUrlClickModifiers;
            if (e.Modifiers != modifiers)
            {
                var modifiersText = ModifierKeysConverter.ToString(modifiers, true);
                App.Log($"Use {modifiersText}+Click to open in the default browser: " + e.Url);
            }
        }

        internal class TextMemoPageProperties : BaseOwnedObject<TextMemoPage>
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