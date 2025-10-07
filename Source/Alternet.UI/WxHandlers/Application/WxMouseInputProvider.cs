using System;

using Alternet.UI.Native;

namespace Alternet.UI
{
    internal class WxMouseInputProvider : DisposableObject
    {
        private readonly Action?[] events = new Action?[(int)WxEventIdentifiers.Max + 1];
        private readonly Native.Mouse nativeMouse;

        private AbstractControl? targetControl;
        private long timestamp;
        private int delta;

        public WxMouseInputProvider(Native.Mouse nativeMouse)
        {
            this.nativeMouse = nativeMouse;
            Native.Mouse.GlobalObject = nativeMouse;

            nativeMouse.MouseChanged += NativeMouse_MouseChanged;

            events[(int)WxEventIdentifiers.SetCursor] = () =>
            {
            };

            events[(int)WxEventIdentifiers.EnterWindow] = ReportEnterWindow;
            events[(int)WxEventIdentifiers.LeaveWindow] = ReportLeaveWindow;

            events[(int)WxEventIdentifiers.MouseMove] = ReportMouseMove;
            events[(int)WxEventIdentifiers.MouseWheel] = ReportMouseWheel;

            events[(int)WxEventIdentifiers.MouseDoubleClickLeft]
                = () => { ReportMouseDoubleClick(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseDoubleClickMiddle]
                = () => { ReportMouseDoubleClick(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseDoubleClickRight]
                = () => { ReportMouseDoubleClick(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseDoubleClickXButton1]
                = () => { ReportMouseDoubleClick(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseDoubleClickXButton2]
                = () => { ReportMouseDoubleClick(MouseButton.XButton2); };

            events[(int)WxEventIdentifiers.MouseDownLeft]
                = () => { ReportMouseDown(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseDownMiddle]
                = () => { ReportMouseDown(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseDownRight]
                = () => { ReportMouseDown(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseDownXButton1]
                = () => { ReportMouseDown(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseDownXButton2]
                = () => { ReportMouseDown(MouseButton.XButton2); };

            events[(int)WxEventIdentifiers.MouseUpLeft]
                = () => { ReportMouseUp(MouseButton.Left); };
            events[(int)WxEventIdentifiers.MouseUpMiddle]
                = () => { ReportMouseUp(MouseButton.Middle); };
            events[(int)WxEventIdentifiers.MouseUpRight]
                = () => { ReportMouseUp(MouseButton.Right); };
            events[(int)WxEventIdentifiers.MouseUpXButton1]
                = () => { ReportMouseUp(MouseButton.XButton1); };
            events[(int)WxEventIdentifiers.MouseUpXButton2]
                = () => { ReportMouseUp(MouseButton.XButton2); };
        }

        private void NativeMouse_MouseChanged(object? sender, NativeEventArgs<MouseEventData> e)
        {
            var mappedEvent = WxApplicationHandler.MapToEventIdentifier(e.Data.mouseEventKind);

            if (mappedEvent == WxEventIdentifiers.None)
                return;

            targetControl = GetTargetControl(e.Data.targetControl);
            timestamp = e.Data.timestamp;
            delta = e.Data.delta;

            if ((int)mappedEvent >= events.Length)
                return;

            InsideTryCatch(events[(int)mappedEvent]);

            e.Handled = false;
        }

        protected override void DisposeManaged()
        {
            nativeMouse.MouseChanged -= NativeMouse_MouseChanged;
            Native.Mouse.GlobalObject = null;
        }

        private static AbstractControl? GetTargetControl(IntPtr targetControlPointer)
        {
            AbstractControl? newHoveredControl = null;

            try
            {
                if (targetControlPointer == IntPtr.Zero)
                {
                    return null;
                }

                var hoveredControl = AbstractControl.GetHoveredControl();

                var nativeHoveredHandler = Control.RequireHandler(hoveredControl) as WxControlHandler;

                if (nativeHoveredHandler is not null)
                {
                    if (nativeHoveredHandler.NativeControl.NativePointer == targetControlPointer)
                    {
                        newHoveredControl = hoveredControl;
                        return hoveredControl;
                    }
                }

                var nativeObject = NativeObject.GetFromNativePointer<NativeObject>(
                    targetControlPointer,
                    null);
                if (nativeObject is not Native.Control c)
                    return null;

                newHoveredControl = WxControlHandler.NativeControlToHandler(c)?.ControlOrNull;
                return newHoveredControl;
            }
            finally
            {
            }
        }

        private void ReportMouseDoubleClick(MouseButton button)
        {
            AbstractControl.BubbleMouseDoubleClick(targetControl, timestamp, button, null, out _);
        }

        private void ReportMouseWheel()
        {
            AbstractControl.BubbleMouseWheel(targetControl, timestamp, delta, null, out _);
        }

        private void ReportMouseUp(MouseButton button)
        {
            AbstractControl.BubbleMouseUp(targetControl, timestamp, button, null, out _);
        }

        private void ReportMouseDown(MouseButton button)
        {
            AbstractControl.BubbleMouseDown(targetControl, timestamp, button, null, out _);
        }

        private void ReportEnterWindow()
        {
            AbstractControl.BubbleMouseEnter(targetControl, timestamp, null, out _);
        }

        private void ReportLeaveWindow()
        {
            AbstractControl.BubbleMouseLeave(targetControl, timestamp, null, out _);
        }

        private void ReportMouseMove()
        {
            AbstractControl.BubbleMouseMove(targetControl, timestamp, null, out _);
        }
    }
}