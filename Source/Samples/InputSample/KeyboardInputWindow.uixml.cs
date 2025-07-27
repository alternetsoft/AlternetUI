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

            Icon = KnownIcons.Default;

            lb.BindApplicationLog();

            checkBoxKeyPreview.BindBoolProp(this, nameof(KeyPreview));
            checkBoxHandledInForm.BindBoolProp(this, nameof(HandledInForm));
            checkBoxHandledInButton.BindBoolProp(this, nameof(HandledInButton));
            checkBoxHandledInStackPanel.BindBoolProp(this, nameof(HandledInStackPanel));
            checkBoxLogRepeated.BindBoolProp(this, nameof(LogRepeated));

            SetSizeToContent();
            PerformLayout();
        }

        private void UpdateModifierKeys()
        {
            var macOs = App.IsMacOS;

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

            var s = $"Modifiers: {sControl} {sShift} {sAlt} {sWindows} {sMacOs}".Trim();
            
            buttonInfo.Text = s;
            buttonInfo.Refresh();
        }

        private void HelloButton_Click(object sender, EventArgs e)
        {
            App.Log("Hello button clicked");
        }

        public bool HandledInButton { get; set; } = false;

        public bool HandledInForm { get; set; } = false;

        public bool HandledInStackPanel { get; set; } = false;

        public bool LogRepeated { get; set; } = false;

        private void Window_TextInput(object sender, KeyPressEventArgs e)
        {
            LogTextInput(e, "Window", "KeyPress");
            if (HandledInForm)
                e.Handled = true;
        }

        private void HelloButton_TextInput(object sender, KeyPressEventArgs e)
        {
            LogTextInput(e, "HelloButton", "KeyPress");
            if (HandledInButton)
                e.Handled = true;
        }

        private void StackPanel_TextInput(object sender, KeyPressEventArgs e)
        {
            LogTextInput(e, "StackPanel", "KeyPress");
            if (HandledInStackPanel)
                e.Handled = true;
        }

        private void LogKey(KeyEventArgs e, string objectName, string eventName)
        {
            if (e.IsRepeat && !LogRepeated)
                return;

            App.Log($"{objectName}.{eventName} [{e.Key}], ({e.ModifierKeys}), Repeat: {e.IsRepeat}");
        }

        private void LogTextInput(KeyPressEventArgs e, string objectName, string eventName) =>
            App.Log($"{objectName}.{eventName} '{e.KeyChar}'");

        private void HelloButton_KeyDown(object sender, KeyEventArgs e)
        {
            LogKey(e, "HelloButton", "KeyDown");
            if (HandledInButton)
                e.Handled = true;
        }

        private void StackPanel_KeyDown(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "KeyDown");
            if (HandledInStackPanel)
                e.Handled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateModifierKeys();
            LogKey(e, "Window", "KeyDown");
            if (e.Key == Key.D && e.ModifierKeys == Alternet.UI.ModifierKeys.ControlShift)
            {
                e.Handled = true;
                messageLabel.BackgroundColor =
                    messageLabel.BackgroundColor != Color.Red ? Color.Red : Color.Green;
            }

            if (HandledInForm)
                e.Handled = true;
        }

        private void HelloButton_KeyUp(object sender, KeyEventArgs e)
        {
            LogKey(e, "HelloButton", "KeyUp");
            if (HandledInButton)
                e.Handled = true;
        }

        private void StackPanel_KeyUp(object sender, KeyEventArgs e)
        {
            LogKey(e, "StackPanel", "KeyUp");
            if (HandledInStackPanel)
                e.Handled = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateModifierKeys();
            LogKey(e, "Window", "KeyUp");
            if (HandledInForm)
                e.Handled = true;
        }
    }
}