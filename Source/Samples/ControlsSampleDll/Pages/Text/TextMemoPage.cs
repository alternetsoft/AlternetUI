using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace ControlsSample
{
    internal class TextMemoPage : Panel
    {
        public static string LoremIpsum =
"Beneath a sky stitched with teacup clouds, the girl tiptoed across checkerboard moss. " +
"Each step made a peculiar sound—like libraries whispering to mushrooms. " +
"Trees bent inward to eavesdrop, their leaves rustling riddles only crickets could decipher." +
Environment.NewLine + Environment.NewLine +
"The map she carried was drawn entirely in nonsense, but somehow it felt correct. " +
"It pulsed faintly in her hands, humming with ink made from stolen dreams and marmalade." +
Environment.NewLine + Environment.NewLine +
"“Left is usually right,” said the rabbit-shaped shadow, bowing courteously. " +
"“Unless, of course, you're upside-down.”" +
Environment.NewLine + Environment.NewLine +
"And so, with a smile too wide for logic, she stepped forward—into a world where clocks " +
"melted politely and hats outgrew heads.";

        public const string LoremIpsumSmall =
            "Lorem ipsum dolor sit amet,\nconsectetur adipiscing elit. " +
            "Suspendisse tincidunt orci vitae arcu congue commodo. " +
            "Proin fermentum rhoncus dictum.\n";

        private readonly PanelMultilineTextBox memoPanel = new();
        private Timer timer = new(100);

        public TextMemoPage()
        {
            memoPanel.TextBox.KeyDown += TextBox_KeyDown;
            memoPanel.TextBox.TextUrl += MultiLineTextBox_TextUrl;
            //memoPanel.FileNewClick += MemoPanel_FileNewClick;
            //memoPanel.FileOpenClick += MemoPanel_FileOpenClick;
            //memoPanel.FileSaveClick += MemoPanel_FileSaveClick;

            var multilineDemoText = LoremIpsum;

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