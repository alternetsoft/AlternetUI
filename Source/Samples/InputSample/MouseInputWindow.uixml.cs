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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            UpdateMouseButtons();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            UpdateMouseButtons();
        }

        private void UpdateMouseButtons()
        {
            leftButtonDownCheckBox.IsChecked = Mouse.LeftButton == MouseButtonState.Pressed;
            middleButtonDownCheckBox.IsChecked = Mouse.MiddleButton == MouseButtonState.Pressed;
            rightButtonDownCheckBox.IsChecked = Mouse.RightButton == MouseButtonState.Pressed;
            x1ButtonDownCheckBox.IsChecked = Mouse.XButton1 == MouseButtonState.Pressed;
            x2ButtonDownCheckBox.IsChecked = Mouse.XButton2 == MouseButtonState.Pressed;
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

        private void CaptureMouseLabel_MouseDown(object sender, EventArgs e)
        {
            captureMouseLabel.CaptureMouse();
            captureMouseLabel.Text = "Mouse is captured. Click again to release the capture.";
        }

        private void CaptureMouseLabel_MouseUp(object sender, EventArgs e)
        {
            captureMouseLabel.ReleaseMouseCapture();
            captureMouseLabel.Text = "Click here to capture mouse.";
        }

        private void CaptureMouseLabel_MouseCaptureLost(object sender, EventArgs e)
        {
            MessageBox.Show("Mouse capture was lost.");
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

        private void LogMouseButton(MouseButtonEventArgs e, string objectName, string eventName, IInputElement element) =>
            LogMessage($"{++messageNumber} {objectName}_{eventName} [{e.ChangedButton}, {e.GetPosition(element)}]");

        private void LogMouseWheel(MouseWheelEventArgs e, string objectName, string eventName, IInputElement element) =>
            LogMessage($"{++messageNumber} {objectName}_{eventName} [{e.Delta}, {e.GetPosition(element)}]");

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "HelloButton", "MouseMove", (IInputElement)sender);
        private void HelloButton_PreviewMouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "HelloButton", "PreviewMouseMove", (IInputElement)sender);

        private void HelloButton_MouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "HelloButton", "MouseDown", (IInputElement)sender);
        private void HelloButton_PreviewMouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "HelloButton", "PreviewMouseDown", (IInputElement)sender);
        
        private void HelloButton_MouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "HelloButton", "MouseUp", (IInputElement)sender);
        private void HelloButton_PreviewMouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "HelloButton", "PreviewMouseUp", (IInputElement)sender);

        private void StackPanel_PreviewMouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "StackPanel", "PreviewMouseMove", (IInputElement)sender);
        private void StackPanel_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "StackPanel", "MouseMove", (IInputElement)sender);

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "StackPanel", "MouseDown", (IInputElement)sender);
        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "StackPanel", "PreviewMouseDown", (IInputElement)sender);

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "StackPanel", "MouseUp", (IInputElement)sender);
        private void StackPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "StackPanel", "PreviewMouseUp", (IInputElement)sender);

        private void Window_MouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "Window", "MouseMove", (IInputElement)sender);
        private void Window_PreviewMouseMove(object sender, MouseEventArgs e) => LogMouseMove(e, "Window", "PreviewMouseMove", (IInputElement)sender);

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "MouseDown", (IInputElement)sender);
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "PreviewMouseDown", (IInputElement)sender);

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "MouseUp", (IInputElement)sender);
        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "PreviewMouseUp", (IInputElement)sender);

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "MouseDoubleClick", (IInputElement)sender);
        private void Window_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "PreviewMouseDoubleClick", (IInputElement)sender);

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "MouseLeftButtonDown", (IInputElement)sender);
        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) => LogMouseButton(e, "Window", "PreviewMouseLeftButtonDown", (IInputElement)sender);

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) => LogMouseWheel(e, "Window", "MouseWheel", (IInputElement)sender);
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e) => LogMouseWheel(e, "Window", "PreviewMouseWheel", (IInputElement)sender);
    }
}