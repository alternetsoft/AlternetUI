using System;
using System.Text;
using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Markup;

namespace InputSample
{
    public partial class MouseInputWindow : Window
    {
        public MouseInputWindow()
        {
            InitializeComponent();

            InputManager.Current.PreProcessInput += InputManager_PreProcessInput;

            mouseCaptureLabel.Text = MouseUncapturedLabel;

            SetSizeToContent();

            Application.Current.Idle += Current_Idle;
        }

        const string MouseUncapturedLabel = "Press mouse button here to capture mouse.";
        const string MouseCapturedLabel = "Release mouse button anywhere to release the capture.";

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
            var leftButton = Mouse.LeftButton == MouseButtonState.Pressed;
            var middleButton = Mouse.MiddleButton == MouseButtonState.Pressed;
            var rightButton = Mouse.RightButton == MouseButtonState.Pressed;
            var x1Button = Mouse.XButton1 == MouseButtonState.Pressed;
            var x2Button = Mouse.XButton2 == MouseButtonState.Pressed;

            var sLeft = leftButton ? "Left" : string.Empty;
            var sMiddle = middleButton ? "Middle" : string.Empty;
            var sRight = rightButton ? "Right" : string.Empty;
            var sExtended1 = x1Button ? "Extended1" : string.Empty;
            var sExtended2 = x2Button ? "Extended2" : string.Empty;

            var s = $"{sLeft} {sMiddle} {sRight} {sExtended1} {sExtended2}";

            buttonInfo.Text = s;
            Application.DoEvents();
        }

        private void Current_Idle(object? sender, EventArgs e)
        {
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

        private void MouseCaptureBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mouseCaptureBorder.CaptureMouse();
            mouseCaptureLabel.Text = MouseCapturedLabel;
            Application.DoEvents();
        }

        private void MouseCaptureBorder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseCaptureBorder.ReleaseMouseCapture();
            mouseCaptureLabel.Text = MouseUncapturedLabel;
            Application.DoEvents();
        }

        private void MouseCaptureBorder_MouseCaptureLost(object sender, EventArgs e)
        {
            MessageBox.Show("Mouse capture was lost.");
        }

        private void MouseCaptureBorder_MouseEnter(object sender, EventArgs e)
        {
            LogMessage(lbWindow, "MouseCaptureBorder_MouseEnter");
        }

        private void MouseCaptureBorder_MouseLeave(object sender, EventArgs e)
        {
            LogMessage(lbWindow, "MouseCaptureBorder_MouseLeave");
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

        private void LogMessage(ListBox lb, string m)
        {
            var messageNumber = 0;

            if (lb.Tag is not null)
                messageNumber = (int)lb.Tag;

            lb.Items.Add($"{++messageNumber} {m}");

            lb.Tag = messageNumber;

            lb.SelectedIndex = lb.Items.Count - 1;
            Application.DoEvents();
        }

        private void LogMouseMove(
            ListBox lb,
            MouseEventArgs e,
            string objectName,
            string eventName,
            IInputElement element) =>
            LogMessage(lb, $"{eventName} [{FormatPoint(e.GetPosition(element))}]");

        private void LogMouseButton(
            ListBox lb,
            MouseButtonEventArgs e,
            string objectName,
            string eventName,
            IInputElement element) =>
            LogMessage(
                lb,
                $"{eventName} [{e.ChangedButton}, {FormatPoint(e.GetPosition(element))}]");

        private void LogMouseWheel(
            ListBox lb,
            MouseWheelEventArgs e,
            string objectName,
            string eventName,
            IInputElement element) =>
            LogMessage(
                lb,
                $"{eventName} [{e.Delta}, {FormatPoint(e.GetPosition(element))}]");

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbButton, e, "HelloButton", "MouseMove", (IInputElement)sender);
        private void HelloButton_PreviewMouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbButton, e, "HelloButton", "PreviewMouseMove", (IInputElement)sender);

        private void HelloButton_MouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "MouseDown", (IInputElement)sender);
        private void HelloButton_PreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "PreviewMouseDown", (IInputElement)sender);
        
        private void HelloButton_MouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "MouseUp", (IInputElement)sender);
        private void HelloButton_PreviewMouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "PreviewMouseUp", (IInputElement)sender);

        private void StackPanel_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            LogMouseMove(lbStackPanel, e, "StackPanel", "PreviewMouseMove", (IInputElement)sender);

            var control = (Control)sender;
            UpdateMousePositionLabel(control, e.GetPosition(control));
        }

        static string FormatPoint(Point pt) => FormatPoint(new Int32Point((int)pt.X, (int)pt.Y));
        static string FormatPoint(Int32Point pt) => $"{pt.X}, {pt.Y}";

        private void UpdateMousePositionLabel(Control control, Point clientPosition)
        {
            var screenPosition = control.ClientToScreen(clientPosition);
            var devicePosition = control.ScreenToDevice(screenPosition);

            var text = $"Mouse position: " +
                $"[Client: {FormatPoint(clientPosition)}], " +
                $"[Screen: {FormatPoint(screenPosition)}], " +
                $"[Device: {FormatPoint(devicePosition)}]]";

            mousePositionLabel.Text = text;
            Application.DoEvents();
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbStackPanel, e, "StackPanel", "MouseMove", (IInputElement)sender);

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "MouseDown", (IInputElement)sender);
        private void StackPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "PreviewMouseDown", (IInputElement)sender);

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "MouseUp", (IInputElement)sender);
        private void StackPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "PreviewMouseUp", (IInputElement)sender);

        private void Window_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbWindow, e, "Window", "MouseMove", (IInputElement)sender);
        private void Window_PreviewMouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbWindow, e, "Window", "PreviewMouseMove", (IInputElement)sender);

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "MouseDown", (IInputElement)sender);
        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "PreviewMouseDown", (IInputElement)sender);

        private void Window_MouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "MouseUp", (IInputElement)sender);
        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "PreviewMouseUp", (IInputElement)sender);

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "MouseDoubleClick", (IInputElement)sender);
        private void Window_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "PreviewMouseDoubleClick", (IInputElement)sender);

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "MouseLeftButtonDown", (IInputElement)sender);
        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "PreviewMouseLeftButtonDown", (IInputElement)sender);

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) =>
            LogMouseWheel(lbWindow, e, "Window", "MouseWheel", (IInputElement)sender);
        private void Window_PreviewMouseWheel(object sender, MouseWheelEventArgs e) =>
            LogMouseWheel(lbWindow, e, "Window", "PreviewMouseWheel", (IInputElement)sender);
    }
}