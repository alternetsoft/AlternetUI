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

            mouseCaptureLabel.Text = MouseUncapturedLabel;

            SetSizeToContent();

            Application.Current.Idle += Current_Idle;
        }

        const string MouseUncapturedLabel = "Press mouse button here to capture mouse.";
        const string MouseCapturedLabel = "Release mouse button anywhere to release the capture.";

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            UpdateMouseButtons();
        }

        protected override void OnMouseUp(MouseEventArgs e)
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
            buttonInfo.Refresh();
        }

        private void Current_Idle(object? sender, EventArgs e)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void HelloButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hello!", "Button Clicked");
        }

        private void MouseCaptureBorder_MouseDown(object sender, MouseEventArgs e)
        {
            mouseCaptureBorder.CaptureMouse();
            mouseCaptureLabel.Text = MouseCapturedLabel;
            mouseCaptureLabel.Refresh();
        }

        private void MouseCaptureBorder_MouseUp(object sender, MouseEventArgs e)
        {
            mouseCaptureBorder.ReleaseMouseCapture();
            mouseCaptureLabel.Text = MouseUncapturedLabel;
            mouseCaptureLabel.Refresh();
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

        private void LogMessage(ListBox lb, string m)
        {
            var messageNumber = 0;

            if (lb.Tag is not null)
                messageNumber = (int)lb.Tag;

            lb.Items.Add($"{++messageNumber} {m}");

            lb.Tag = messageNumber;

            lb.SelectedIndex = lb.Items.Count - 1;
            lb.Refresh();
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
            MouseEventArgs e,
            string objectName,
            string eventName,
            IInputElement element) =>
            LogMessage(
                lb,
                $"{eventName} [{e.ChangedButton}, {FormatPoint(e.GetPosition(element))}]");

        private void LogMouseWheel(
            ListBox lb,
            MouseEventArgs e,
            string objectName,
            string eventName,
            IInputElement element) =>
            LogMessage(
                lb,
                $"{eventName} [{e.Delta}, {FormatPoint(e.GetPosition(element))}]");

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbButton, e, "HelloButton", "Move", (IInputElement)sender);

        private void HelloButton_MouseDown(object sender, MouseEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "Down", (IInputElement)sender);
        
        private void HelloButton_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(lbButton, e, "HelloButton", "Up", (IInputElement)sender);

        static string FormatPoint(PointD pt) => FormatPoint(new PointI((int)pt.X, (int)pt.Y));
        static string FormatPoint(PointI pt) => $"{pt.X}, {pt.Y}";

        private void UpdateMousePositionLabel(Control control, PointD clientPosition)
        {
            var screenPosition = control.ClientToScreen(clientPosition);
            var devicePosition = control.ScreenToDevice(screenPosition);

            var text = $"Mouse position: " +
                $"[Client: {FormatPoint(clientPosition)}], " +
                $"[Screen: {FormatPoint(screenPosition)}], " +
                $"[Device: {FormatPoint(devicePosition)}]]";

            mousePositionLabel.Text = text;
            mousePositionLabel.Refresh();
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbStackPanel, e, "StackPanel", "Move", (IInputElement)sender);

        private void StackPanel_MouseDown(object sender, MouseEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "Down", (IInputElement)sender);

        private void StackPanel_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(lbStackPanel, e, "StackPanel", "Up", (IInputElement)sender);

        private void Window_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(lbWindow, e, "Window", "Move", (IInputElement)sender);

        private void Window_MouseDown(object sender, MouseEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "Down", (IInputElement)sender);

        private void Window_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "Up", (IInputElement)sender);

        private void Window_MouseDoubleClick(object sender, MouseEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "DoubleClick", (IInputElement)sender);

        private void Window_MouseLeftButtonDown(object sender, MouseEventArgs e) =>
            LogMouseButton(lbWindow, e, "Window", "LeftBtnDown", (IInputElement)sender);

        private void Window_MouseWheel(object sender, MouseEventArgs e) =>
            LogMouseWheel(lbWindow, e, "Window", "Wheel", (IInputElement)sender);
    }
}