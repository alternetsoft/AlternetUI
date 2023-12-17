using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class KeyboardInputWindow : Window
    {
        private const int KeyDownIndex = 0;
        private const int KeyUpIndex = 1;
        private const int TextInputIndex = 2;
        private const int lbCount = 3;

        public KeyboardInputWindow()
        {
            InitializeComponent();

            PlatformSpecificInitialize();

            SetSizeToContent();

            static object CreateItem() => string.Empty;

            lbButton.Items.SetCount(lbCount, CreateItem);
            lbStackPanel.Items.SetCount(lbCount, CreateItem);
            lbWindow.Items.SetCount(lbCount, CreateItem);

            ControlSet labels = new(
                labelInfo,
                labelWindow,
                labelStackPanel,
                labelButton);
            labels.SuggestedWidthToMax();
            PerformLayout();
            SetSizeToContent();
        }

        private void PlatformSpecificInitialize()
        {
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            UpdateModifierKeys();
            if (e.Key == Key.D && e.ModifierKeys == (Alternet.UI.ModifierKeys.Control | Alternet.UI.ModifierKeys.Shift))
            {
                e.Handled = true;
                messageLabel.BackgroundColor =
                    messageLabel.BackgroundColor != Color.Red ? Color.Red : Color.Green;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            UpdateModifierKeys();
        }

        private void UpdateModifierKeys()
        {
            var macOs = Application.IsMacOS;

            var modifiers = Keyboard.Modifiers;

            var control = (modifiers & Alternet.UI.ModifierKeys.Control) != 0;
            var shift = (modifiers & Alternet.UI.ModifierKeys.Shift) != 0;
            var alt = (modifiers & Alternet.UI.ModifierKeys.Alt) != 0;
            var windows = (modifiers & Alternet.UI.ModifierKeys.Windows) != 0;

            var sControl = control ? (macOs ? "Cmd(Ctrl)" : "Ctrl") : string.Empty;
            var sShift = shift ? "Shift" : string.Empty;
            var sAlt =  alt ? "Alt" : string.Empty;
            var sWindows = windows ? "Windows" : string.Empty;

            var sMacOs = string.Empty;
            if (macOs)
            {
                var rawModifiers = Keyboard.RawModifiers;
                var macControl = (rawModifiers & RawModifierKeys.MacControl) != 0;
                var macCommand = (rawModifiers & RawModifierKeys.MacCommand) != 0;
                var macOption = (rawModifiers & RawModifierKeys.MacOption) != 0;

                var sMacControl = macControl ? "MacControl": string.Empty;
                var sMacCommand = macCommand ? "MacCommand": string.Empty;
                var sMacOption = macOption ? "MacOption": string.Empty;
                sMacOs = $"{sMacControl} {sMacCommand} {sMacOption}";
            }

            var s = $"{sControl} {sShift} {sAlt} {sWindows} {sMacOs}";

            buttonInfo.Text = s;
            buttonInfo.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void HelloButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello!", "Button Clicked");
        }

        int messageNumber;

        private void LogMessage(int index, ListBox lb, string m)
        {
            lb.Items[index] = m;
            lb.Refresh();
        }

        private void Window_TextInput(object sender, KeyPressEventArgs e) =>
            LogTextInput(TextInputIndex, lbWindow, e, "Window", "KeyPress");

        private void LogKey(int index, ListBox lb, KeyEventArgs e, string objectName, string eventName) =>
            LogMessage(index, lb, $"{++messageNumber} {eventName} [{e.Key}], Repeat: {e.IsRepeat}");
        private void LogTextInput(int index, ListBox lb, KeyPressEventArgs e, string objectName, string eventName) =>
            LogMessage(index, lb, $"{++messageNumber} {eventName} '{e.KeyChar}'");

        private void HelloButton_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(KeyDownIndex, lbButton, e, "HelloButton", "KeyDown");

        private void StackPanel_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(KeyDownIndex, lbStackPanel, e, "StackPanel", "KeyDown");

        private void Window_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(KeyDownIndex, lbWindow, e, "Window", "KeyDown");

        private void HelloButton_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(KeyUpIndex, lbButton, e, "HelloButton", "KeyUp");

        private void StackPanel_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(KeyUpIndex, lbStackPanel, e, "StackPanel", "KeyUp");

        private void Window_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(KeyUpIndex, lbWindow, e, "Window", "KeyUp");
    }
}