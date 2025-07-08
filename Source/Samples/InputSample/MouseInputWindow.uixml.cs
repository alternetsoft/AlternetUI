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

        private void HelloButton_Click(object sender, EventArgs e)
        {
            App.Log("Hello Button Clicked");
        }

        private void MouseCaptureBorder_MouseDown(object sender, MouseEventArgs e)
        {
            mouseCaptureBorder.CaptureMouse();
            mouseCaptureLabel.Text = MouseCapturedLabel;
            mouseCaptureLabel.Refresh();
            UpdateMouseButtons();
        }

        private void MouseCaptureBorder_MouseUp(object sender, MouseEventArgs e)
        {
            mouseCaptureBorder.ReleaseMouseCapture();
            mouseCaptureLabel.Text = MouseUncapturedLabel;
            mouseCaptureLabel.Refresh();
            UpdateMouseButtons();
        }

        private void MouseCaptureBorder_MouseCaptureLost(object sender, EventArgs e)
        {
            App.Log("Mouse capture was lost.");
        }

        private void MouseCaptureBorder_MouseEnter(object sender, EventArgs e)
        {
            App.Log("MouseCaptureBorder_MouseEnter");
            UpdateMouseButtons();
        }

        private void MouseCaptureBorder_MouseLeave(object sender, EventArgs e)
        {
            App.Log("MouseCaptureBorder_MouseLeave");
            UpdateMouseButtons();
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

            App.LogReplace($"{prefix} [{FormatPoint(Mouse.GetPosition(element as AbstractControl))}]", prefix);
            UpdateMouseButtons();
        }

        private void LogMouseButton(
            MouseEventArgs e,
            string objectName,
            string eventName,
            object? element)
        {
            App.Log(
                $"{objectName}.{eventName} [{e.ChangedButton}, {FormatPoint(Mouse.GetPosition(element as AbstractControl))}]");
            UpdateMouseButtons();
        }

        private void LogMouseWheel(
            MouseEventArgs e,
            string objectName,
            string eventName,
            object? element)
        {
            App.Log(
                $"{objectName}.{eventName} [{e.Delta}, {FormatPoint(Mouse.GetPosition(element as AbstractControl))}]");
            UpdateMouseButtons();
        }

        private void HelloButton_MouseMove(object sender, MouseEventArgs e) =>
            LogMouseMove(e, "HelloButton", "Move", (AbstractControl)sender);

        private void HelloButton_MouseDown(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "HelloButton", "Down", (AbstractControl)sender);
        
        private void HelloButton_MouseUp(object sender, MouseEventArgs e) =>
            LogMouseButton(e, "HelloButton", "Up", (AbstractControl)sender);

        static string FormatPoint(PointD pt) => FormatPoint(new PointI((int)pt.X, (int)pt.Y));
        static string FormatPoint(PointI pt) => $"{pt.X}, {pt.Y}";

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