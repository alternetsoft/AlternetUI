using System;
using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class MouseInputProvider : DisposableObject
    {
        private readonly Action?[] events = new Action?[(int)WxEventIdentifiers.Max + 1];
        private readonly Native.Mouse nativeMouse;

        private Control? targetControl;
        private long timestamp;
        private int delta;

        public MouseInputProvider(Native.Mouse nativeMouse)
        {
            this.nativeMouse = nativeMouse;

            nativeMouse.MouseChanged += NativeMouse_MouseChanged;

            events[(int)WxEventIdentifiers.MouseMove] = ReportMouseMove;
            events[(int)WxEventIdentifiers.MouseWheel] = ReportMouseWheel;

            events[(int)WxEventIdentifiers.MouseDoubleClickLeft]        = () => { ReportMouseDoubleClick(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseDoubleClickMiddle]      = () => { ReportMouseDoubleClick(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseDoubleClickRight]       = () => { ReportMouseDoubleClick(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseDoubleClickXButton1]    = () => { ReportMouseDoubleClick(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseDoubleClickXButton2]    = () => { ReportMouseDoubleClick(MouseButton.XButton2); };

            events[(int)WxEventIdentifiers.MouseDownLeft]       = () => { ReportMouseDown(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseDownMiddle]     = () => { ReportMouseDown(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseDownRight]      = () => { ReportMouseDown(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseDownXButton1]   = () => { ReportMouseDown(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseDownXButton2]   = () => { ReportMouseDown(MouseButton.XButton2); };

            events[(int)WxEventIdentifiers.MouseUpLeft]         = () => { ReportMouseUp(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseUpMiddle]       = () => { ReportMouseUp(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseUpRight]        = () => { ReportMouseUp(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseUpXButton1]     = () => { ReportMouseUp(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseUpXButton2]     = () => { ReportMouseUp(MouseButton.XButton2); };
        }

        private void NativeMouse_MouseChanged(object? sender, NativeEventArgs<MouseEventData> e)
        {
            var mappedEvent = WxApplicationHandler.MapToEventIdentifier(e.Data.mouseEventKind);

            targetControl = GetTargetControl(e.Data.targetControl, true);
            timestamp = e.Data.timestamp;
            delta = e.Data.delta;

            events[(int)mappedEvent]?.Invoke();

            e.Handled = false;
        }

        protected override void DisposeManaged()
        {
            nativeMouse.MouseChanged -= NativeMouse_MouseChanged;
        }

        private static Control? GetTargetControl(IntPtr targetControlPointer, bool setHoveredControl)
        {
            if (targetControlPointer == IntPtr.Zero)
                return null;

            var hoveredControl = Control.GetHoveredControl();

            var nativeHoveredHandler = hoveredControl?.Handler as WxControlHandler;

            if(nativeHoveredHandler is not null)
            {
                if (nativeHoveredHandler.NativeControl.NativePointer == targetControlPointer)
                    return hoveredControl;
            }

            var nobj = NativeObject.GetFromNativePointer<NativeObject>(
                targetControlPointer,
                null);
            if (nobj is not Native.Control c)
                return null;

            var result = WxControlHandler.NativeControlToHandler(c)?.Control;
            if(setHoveredControl && result is not null)
                Control.HoveredControl = result;

            return result;
        }

        private void ReportMouseDoubleClick(MouseButton button)
        {
            Mouse.ReportMouseDoubleClick(targetControl, timestamp, button, out _);
        }

        private void ReportMouseWheel()
        {
            Mouse.ReportMouseWheel(targetControl, timestamp, delta, out _);
        }

        private void ReportMouseUp(MouseButton button)
        {
            Mouse.ReportMouseUp(targetControl, timestamp, button, out _);
        }

        private void ReportMouseDown(MouseButton button)
        {
            Mouse.ReportMouseDown(targetControl, timestamp, button, out _);
        }

        private void ReportMouseMove()
        {
            Mouse.ReportMouseMove(targetControl, timestamp, out _);
        }
     }
}