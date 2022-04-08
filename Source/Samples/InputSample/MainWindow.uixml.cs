using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class MainWindow : Window
    {
        bool runningUnderMacOS;

        public MainWindow()
        {
            InitializeComponent();

#if NETCOREAPP
            runningUnderMacOS = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.OSX);
#endif

            macKeysPanel.Visible = runningUnderMacOS;

            InputManager.Current.PreProcessInput += InputManager_PreProcessInput;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.IsDown || e.IsUp)
                throw new InvalidOperationException();

            UpdateModifierKeys();
            if (e.Key == Key.D && e.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                e.Handled = true;
                messageLabel.Background = messageLabel.Background != Brushes.Red ? Brushes.Red : Brushes.Green;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (!e.IsUp || e.IsDown)
                throw new InvalidOperationException();

            UpdateModifierKeys();
        }

        private void UpdateModifierKeys()
        {
            controlPressedCheckBox.IsChecked = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
            shiftPressedCheckBox.IsChecked = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            altPressedCheckBox.IsChecked = (Keyboard.Modifiers & ModifierKeys.Alt) != 0;
            windowsPressedCheckBox.IsChecked = (Keyboard.Modifiers & ModifierKeys.Windows) != 0;

            if (runningUnderMacOS)
            {
                macControlPressedCheckBox.IsChecked = (Keyboard.RawModifiers & RawModifierKeys.MacControl) != 0;
                macCommandPressedCheckBox.IsChecked = (Keyboard.RawModifiers & RawModifierKeys.MacCommand) != 0;
                macOptionPressedCheckBox.IsChecked = (Keyboard.RawModifiers & RawModifierKeys.MacOption) != 0;
            }
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

        private void LogMessage(string m)
        {
            lb.Items.Add(m);
            lb.SelectedIndex = lb.Items.Count - 1;
        }

        private void Window_TextInput(object sender, TextInputEventArgs e) => LogTextInput(e, "Window", "TextInput");

        private void LogKey(KeyEventArgs e, string objectName, string eventName) => LogMessage($"{++messageNumber} {objectName}_{eventName} [{e.Key}], Repeat: {e.IsRepeat}");
        private void LogTextInput(TextInputEventArgs e, string objectName, string eventName) => LogMessage($"{++messageNumber} {objectName}_{eventName} '{e.KeyChar}'");

        private void HelloButton_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "KeyDown");

        private void StackPanel_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "StackPanel", "KeyDown");

        private void Window_KeyDown(object sender, KeyEventArgs e) => LogKey(e, "Window", "KeyDown");

        private void HelloButton_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "KeyUp");

        private void StackPanel_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "StackPanel", "KeyUp");

        private void Window_KeyUp(object sender, KeyEventArgs e) => LogKey(e, "Window", "KeyUp");

        private void HelloButton_PreviewKeyDown(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "PreviewKeyDown");

        private void StackPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "PreviewKeyDown");
            if (handlePreviewEventsCheckBox.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e) => LogKey(e, "Window", "PreviewKeyDown");

        private void HelloButton_PreviewKeyUp(object sender, KeyEventArgs e) => LogKey(e, "HelloButton", "PreviewKeyUp");

        private void StackPanel_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "PreviewKeyUp");
            if (handlePreviewEventsCheckBox.IsChecked)
                e.Handled = true;
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e) => LogKey(e, "Window", "PreviewKeyUp");
    }
}