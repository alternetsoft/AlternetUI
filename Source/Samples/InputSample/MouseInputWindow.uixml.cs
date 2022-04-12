using System;
using Alternet.Drawing;
using Alternet.UI;

namespace InputSample
{
    public partial class MouseInputWindow : Window
    {
        public MouseInputWindow()
        {
            InitializeComponent();

            InputManager.Current.PreProcessInput += InputManager_PreProcessInput;
        }

        private void UpdateMouseButtons()
        {
            //controlPressedCheckBox.IsChecked = (Keyboard.Modifiers & ModifierKeys.Control) != 0;
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
            if (cancelMouseMoveInputCheckBox.IsChecked)
            {
                if (e.Input is MouseEventArgs ke)
                {
                    if (e.Input.RoutedEvent == MouseMoveEvent)
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

        private void LogMouseMove(MouseEventArgs e, string objectName, string eventName, IInputElement element) =>
            LogMessage($"{++messageNumber} {objectName}_{eventName} [{e.GetPosition(element)}]");

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "HelloButton", "MouseMove", (IInputElement)sender);
        private void HelloButton_PreviewMouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "HelloButton", "PreviewMouseMove", (IInputElement)sender);

        private void StackPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            LogMouseMove(e, "StackPanel", "PreviewMouseMove", (IInputElement)sender);
            if (handlePreviewEventsCheckBox.IsChecked)
                e.Handled = true;
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "StackPanel", "MouseMove", (IInputElement)sender);

        private void Window_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "Window", "MouseMove", (IInputElement)sender);
        private void Window_PreviewMouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "Window", "PreviewMouseMove", (IInputElement)sender);
    }
}