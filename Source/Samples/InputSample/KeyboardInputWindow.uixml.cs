using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class KeyboardInputWindow : Window
    {
        public KeyboardInputWindow()
        {
            InitializeComponent();

            PlatformSpecificInitialize();
            InputManager.Current.PreProcessInput += InputManager_PreProcessInput;

            SetSizeToContent();
        }

        private void PlatformSpecificInitialize()
        {
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            UpdateModifierKeys();
            if (e.Key == Key.D && e.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
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
            var macOs = Application.IsMacOs;

            var modifiers = Keyboard.Modifiers;

            var control = (modifiers & ModifierKeys.Control) != 0;
            var shift = (modifiers & ModifierKeys.Shift) != 0;
            var alt = (modifiers & ModifierKeys.Alt) != 0;
            var windows = (modifiers & ModifierKeys.Windows) != 0;

            var sControl = control ? (macOs ? "Cmd" : "Ctrl") : string.Empty;
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

                var sMacControl = macControl ? "Control": string.Empty;
                var sMacCommand = macCommand ? "Command": string.Empty;
                var sMacOption = macOption ? "Option": string.Empty;
                sMacOs = $"{sMacControl} {sMacCommand} {sMacOption}";
            }

            var s = $"{sControl} {sShift} {sAlt} {sWindows} {sMacOs}";

            buttonInfo.Text = s;

            Application.DoEvents();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InputManager.Current.PreProcessInput -= InputManager_PreProcessInput;
            }

            base.Dispose(disposing);
        }

        private void HelloButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello!", "Button Clicked");
        }

        private void InputManager_PreProcessInput(object sender, PreProcessInputEventArgs e)
        {
            if (cancelF1KeyInputCheckBox.IsChecked)
            {
                if (e.Input is KeyEventArgs ke)
                {
                    if (ke.Key == Key.F1)
                        e.Cancel();
                }
            }
        }

        int messageNumber;

        private void LogMessage(ListBox lb, string m)
        {
            lb.Items.Add(m);
            lb.SelectedIndex = lb.Items.Count - 1;
            Application.DoEvents();
        }

        private void Window_TextInput(object sender, TextInputEventArgs e) =>
            LogTextInput(lbWindow, e, "Window", "TextInput");

        private void LogKey(ListBox lb, KeyEventArgs e, string objectName, string eventName) =>
            LogMessage(lb, $"{++messageNumber} {eventName} [{e.Key}], Repeat: {e.IsRepeat}");
        private void LogTextInput(ListBox lb, TextInputEventArgs e, string objectName, string eventName) =>
            LogMessage(lb, $"{++messageNumber} {eventName} '{e.KeyChar}'");

        private void HelloButton_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(lbButton, e, "HelloButton", "KeyDown");

        private void StackPanel_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(lbStackPanel, e, "StackPanel", "KeyDown");

        private void Window_KeyDown(object sender, KeyEventArgs e) =>
            LogKey(lbWindow, e, "Window", "KeyDown");

        private void HelloButton_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(lbButton, e, "HelloButton", "KeyUp");

        private void StackPanel_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(lbStackPanel, e, "StackPanel", "KeyUp");

        private void Window_KeyUp(object sender, KeyEventArgs e) =>
            LogKey(lbWindow, e, "Window", "KeyUp");

        private void HelloButton_PreviewKeyDown(object sender, KeyEventArgs e) =>
            LogKey(lbButton, e, "HelloButton", "PreviewKeyDown");

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            LogKey(lbStackPanel, e, "StackPanel", "PreviewKeyDown");
            if (handlePreviewEventsCheckBox.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) =>
            LogKey(lbWindow, e, "Window", "PreviewKeyDown");

        private void HelloButton_PreviewKeyUp(object sender, KeyEventArgs e) =>
            LogKey(lbButton, e, "HelloButton", "PreviewKeyUp");

        private void StackPanel_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            LogKey(lbStackPanel, e, "StackPanel", "PreviewKeyUp");
            if (handlePreviewEventsCheckBox.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e) =>
            LogKey(lbWindow, e, "Window", "PreviewKeyUp");
    }
}