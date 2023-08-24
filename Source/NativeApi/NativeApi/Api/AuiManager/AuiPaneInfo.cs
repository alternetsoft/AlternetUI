#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_pane_info.html
	public class AuiPaneInfo
	{
        public static void Delete(IntPtr handle) { }
        public static IntPtr CreateAuiPaneInfo() => throw new Exception();

        public static void SafeSet(IntPtr handle, IntPtr source) =>
            throw new Exception();
        public static bool IsOk(IntPtr handle) => throw new Exception();
        public static bool IsFixed(IntPtr handle) => throw new Exception();
        public static bool IsResizable(IntPtr handle) => throw new Exception();
        public static bool IsShown(IntPtr handle) => throw new Exception();
        public static bool IsFloating(IntPtr handle) => throw new Exception();
        public static bool IsDocked(IntPtr handle) => throw new Exception();
        public static bool IsToolbar(IntPtr handle) => throw new Exception();
        public static bool IsTopDockable(IntPtr handle) => throw new Exception();
        public static bool IsBottomDockable(IntPtr handle) => throw new Exception();
        public static bool IsLeftDockable(IntPtr handle) => throw new Exception();
        public static bool IsRightDockable(IntPtr handle) => throw new Exception();
        public static bool IsDockable(IntPtr handle) => throw new Exception();
        public static bool IsFloatable(IntPtr handle) => throw new Exception();
        public static bool IsMovable(IntPtr handle) => throw new Exception();
        public static bool IsDestroyOnClose(IntPtr handle) => throw new Exception();
        public static bool IsMaximized(IntPtr handle) => throw new Exception();
        public static bool HasCaption(IntPtr handle) => throw new Exception();
        public static bool HasGripper(IntPtr handle) => throw new Exception();
        public static bool HasBorder(IntPtr handle) => throw new Exception();
        public static bool HasCloseButton(IntPtr handle) => throw new Exception();
        public static bool HasMaximizeButton(IntPtr handle) => throw new Exception();
        public static bool HasMinimizeButton(IntPtr handle) => throw new Exception();
        public static bool HasPinButton(IntPtr handle) => throw new Exception();
        public static bool HasGripperTop(IntPtr handle) => throw new Exception();

        public static void Window(IntPtr handle, IntPtr window) => throw new Exception();
        public static void Name(IntPtr handle, string value) => throw new Exception();
        public static void Caption(IntPtr handle, string value) => throw new Exception();
        public static void Image(IntPtr handle, ImageSet? bitmap) =>
            throw new Exception();
        public static void Left(IntPtr handle) => throw new Exception();
        public static void Right(IntPtr handle) => throw new Exception();
        public static void Top(IntPtr handle) => throw new Exception();
        public static void Bottom(IntPtr handle) => throw new Exception();
        public static void Center(IntPtr handle) => throw new Exception();
        public static void Direction(IntPtr handle, int direction) =>
            throw new Exception();
        public static void Layer(IntPtr handle, int layer) => throw new Exception();
        public static void Row(IntPtr handle, int row) => throw new Exception();
        public static void Position(IntPtr handle, int pos) => throw new Exception();
        public static void BestSize(IntPtr handle, int x, int y) =>
            throw new Exception();
        public static void MinSize(IntPtr handle, int x, int y) => throw new Exception();
        public static void MaxSize(IntPtr handle, int x, int y) => throw new Exception();
        public static void FloatingPosition(IntPtr handle, int x, int y) =>
            throw new Exception();
        public static void FloatingSize(IntPtr handle, int x, int y) =>
            throw new Exception();
        public static void Fixed(IntPtr handle) => throw new Exception();
        public static void Resizable(IntPtr handle, bool resizable = true) =>
            throw new Exception();
        public static void Dock(IntPtr handle) => throw new Exception();
        public static void Float(IntPtr handle) => throw new Exception();
        public static void Hide(IntPtr handle) => throw new Exception();
        public static void Show(IntPtr handle, bool show = true) =>
            throw new Exception();
        public static void CaptionVisible(IntPtr handle, bool visible = true)
            => throw new Exception();
        public static void Maximize(IntPtr handle) => throw new Exception();
        public static void Restore(IntPtr handle) => throw new Exception();
        public static void PaneBorder(IntPtr handle, bool visible = true) =>
            throw new Exception();
        
        public static void Gripper(IntPtr handle, bool visible = true) =>
            throw new Exception();
        public static void GripperTop(IntPtr handle, bool attop = true) =>
            throw new Exception();
        public static void CloseButton(IntPtr handle, bool visible = true) =>
            throw new Exception();
        public static void MaximizeButton(IntPtr handle, bool visible = true) =>
            throw new Exception();
        public static void MinimizeButton(IntPtr handle, bool visible = true) =>
            throw new Exception();
        public static void PinButton(IntPtr handle, bool visible = true) =>
            throw new Exception();
        public static void DestroyOnClose(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void TopDockable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void BottomDockable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void LeftDockable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void RightDockable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void Floatable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void Movable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void DockFixed(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static void Dockable(IntPtr handle, bool b = true) =>
            throw new Exception();
        public static bool IsValid(IntPtr handle) => throw new Exception();

        public static void DefaultPane(IntPtr handle) => throw new Exception();
        public static void CenterPane(IntPtr handle) => throw new Exception();
        public static void ToolbarPane(IntPtr handle) => throw new Exception();
        public static void SetFlag(IntPtr handle, int flag, bool option_state) =>
            throw new Exception();
        public static bool HasFlag(IntPtr handle, int flag) => throw new Exception();
    }
}