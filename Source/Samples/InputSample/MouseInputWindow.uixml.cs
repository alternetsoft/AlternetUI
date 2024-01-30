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

            lb.BindApplicationLog();

            logMoveCheckBox.BindBoolProp(this, nameof(MouseMoveLogged));
        }

        public bool MouseMoveLogged { get; set; }

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

        private void HelloButton_Click(object sender, EventArgs e)
        {
            Application.Log("Hello Button Clicked");
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
            Application.Log("Mouse capture was lost.");
        }

        private void MouseCaptureBorder_MouseEnter(object sender, EventArgs e)
        {
            Application.Log("MouseCaptureBorder_MouseEnter");
        }

        private void MouseCaptureBorder_MouseLeave(object sender, EventArgs e)
        {
            Application.Log("MouseCaptureBorder_MouseLeave");
        }

        private void LogMouseMove(
            MouseEventArgs e,
            string objectName,
            string eventName,
            object? element)
        {
            if (!MouseMoveLogged)
                return;

            var prefix = $"{ objectName }.{ eventName}";

            Application.LogReplace($"{prefix} [{FormatPoint(e.GetPosition(element as Control))}]", prefix);
        }

        private void LogMouseButton(
            MouseEventArgs e,
            string objectName,
            string eventName,
            object? element) =>
            Application.Log(
                $"{objectName}.{eventName} [{e.ChangedButton}, {FormatPoint(e.GetPosition(element as Control))}]");

        private void LogMouseWheel(
            MouseEventArgs e,
            string objectName,
            string eventName,
            object? element) =>
            Application.Log(
                $"{objectName}.{eventName} [{e.Delta}, {FormatPoint(e.GetPosition(element as Control))}]");

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(e, "HelloButton", "Move", (Control)sender);

        private void HelloButton_MouseDown(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "HelloButton", "Down", (Control)sender);
        
        private void HelloButton_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "HelloButton", "Up", (Control)sender);

        static string FormatPoint(PointD pt) => FormatPoint(new PointI((int)pt.X, (int)pt.Y));
        static string FormatPoint(PointI pt) => $"{pt.X}, {pt.Y}";

        /*internal void UpdateMousePositionLabel(Control control, PointD clientPosition)
        {
            var screenPosition = control.ClientToScreen(clientPosition);
            var devicePosition = control.ScreenToDevice(screenPosition);

            var text = $"Mouse position: " +
                $"[Client: {FormatPoint(clientPosition)}], " +
                $"[Screen: {FormatPoint(screenPosition)}], " +
                $"[Device: {FormatPoint(devicePosition)}]]";

            mousePositionLabel.Text = text;
            mousePositionLabel.Refresh();
        }*/

        private void GroupBox_MouseMove(object? sender, MouseEventArgs e)
        {
            LogMouseMove(e, "GroupBox", "Move", sender);
        }

        private void GroupBox_MouseDown(object? sender, MouseEventArgs e)
        {
            LogMouseButton(e, "GroupBox", "Down", sender);
        }

        private void GroupBox_MouseUp(object? sender, MouseEventArgs e)
        {
            LogMouseButton(e, "GroupBox", "Up", sender);
        }

        private void Window_MouseMove(object? sender, MouseEventArgs e) =>
            LogMouseMove(e, "Window", "Move", sender);

        private void Window_MouseDown(object? sender, MouseEventArgs e) =>
            LogMouseButton(e, "Window", "Down", sender);

        private void Window_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "Window", "Up", sender);

        private void Window_MouseDoubleClick(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "Window", "DoubleClick", sender);

        private void Window_MouseWheel(object sender, MouseEventArgs e) =>
            LogMouseWheel(e, "Window", "Wheel", sender);
    }
}