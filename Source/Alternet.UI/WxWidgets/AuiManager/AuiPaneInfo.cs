using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class AuiPaneInfo : BaseControlItem, IDisposable, IAuiPaneInfo
    {
        private readonly AuiManager owner;
        private IntPtr handle;

        public AuiPaneInfo(AuiManager owner)
        {
            this.owner = owner;
            handle = Native.AuiPaneInfo.CreateAuiPaneInfo();
        }

        public AuiPaneInfo(AuiManager owner, IntPtr handle)
        {
            this.owner = owner;
            this.handle = handle;
        }

        ~AuiPaneInfo()
        {
            Dispose();
        }

        public IntPtr Handle { get => handle; }

        public AuiManager Owner => owner;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.AuiPaneInfo.Delete(handle);
                handle = IntPtr.Zero;
            }

            GC.SuppressFinalize(this);
        }

        public SizeI GetBestSizePixel()
        {
            return Native.AuiPaneInfo.GetBestSize(handle);
        }

        public SizeI GetMinSizePixel()
        {
            return Native.AuiPaneInfo.GetMinSize(handle);
        }

        public SizeI GetMaxSizePixel()
        {
            return Native.AuiPaneInfo.GetMaxSize(handle);
        }

        public void SafeSet(IAuiPaneInfo source)
        {
            Native.AuiPaneInfo.SafeSet(handle, source.Handle);
        }

        public bool IsOk()
        {
            return Native.AuiPaneInfo.IsOk(handle);
        }

        public bool IsFixed()
        {
            return Native.AuiPaneInfo.IsFixed(handle);
        }

        public bool IsResizable()
        {
            return Native.AuiPaneInfo.IsResizable(handle);
        }

        public bool IsShown()
        {
            return Native.AuiPaneInfo.IsShown(handle);
        }

        public bool IsFloating()
        {
            return Native.AuiPaneInfo.IsFloating(handle);
        }

        public bool IsDocked()
        {
            return Native.AuiPaneInfo.IsDocked(handle);
        }

        public bool IsToolbar()
        {
            return Native.AuiPaneInfo.IsToolbar(handle);
        }

        public bool IsTopDockable()
        {
            return Native.AuiPaneInfo.IsTopDockable(handle);
        }

        public bool IsBottomDockable()
        {
            return Native.AuiPaneInfo.IsBottomDockable(handle);
        }

        public bool IsLeftDockable()
        {
            return Native.AuiPaneInfo.IsLeftDockable(handle);
        }

        public bool IsRightDockable()
        {
            return Native.AuiPaneInfo.IsRightDockable(handle);
        }

        public bool IsDockable()
        {
            return Native.AuiPaneInfo.IsDockable(handle);
        }

        public bool IsFloatable()
        {
            return Native.AuiPaneInfo.IsFloatable(handle);
        }

        public bool IsMovable()
        {
            return Native.AuiPaneInfo.IsMovable(handle);
        }

        public bool IsDestroyOnClose()
        {
            return Native.AuiPaneInfo.IsDestroyOnClose(handle);
        }

        public bool IsMaximized()
        {
            return Native.AuiPaneInfo.IsMaximized(handle);
        }

        public bool HasCaption()
        {
            return Native.AuiPaneInfo.HasCaption(handle);
        }

        public bool HasGripper()
        {
            return Native.AuiPaneInfo.HasGripper(handle);
        }

        public bool HasBorder()
        {
            return Native.AuiPaneInfo.HasBorder(handle);
        }

        public bool HasCloseButton()
        {
            return Native.AuiPaneInfo.HasCloseButton(handle);
        }

        public bool HasMaximizeButton()
        {
            return Native.AuiPaneInfo.HasMaximizeButton(handle);
        }

        public bool HasMinimizeButton()
        {
            return Native.AuiPaneInfo.HasMinimizeButton(handle);
        }

        public bool HasPinButton()
        {
            return Native.AuiPaneInfo.HasPinButton(handle);
        }

        public bool HasGripperTop()
        {
            return Native.AuiPaneInfo.HasGripperTop(handle);
        }

        public IAuiPaneInfo Window(Control window)
        {
            Native.AuiPaneInfo.Window(handle, AuiManager.ToHandle(window));
            return this;
        }

        public IAuiPaneInfo Name(string value)
        {
            Native.AuiPaneInfo.Name(handle, value);
            return this;
        }

        public IAuiPaneInfo Caption(string value)
        {
            Native.AuiPaneInfo.Caption(handle, value);
            return this;
        }

        public IAuiPaneInfo Left()
        {
            Native.AuiPaneInfo.Left(handle);
            return this;
        }

        public IAuiPaneInfo Right()
        {
            Native.AuiPaneInfo.Right(handle);
            return this;
        }

        public IAuiPaneInfo Top()
        {
            Native.AuiPaneInfo.Top(handle);
            return this;
        }

        public IAuiPaneInfo Bottom()
        {
            Native.AuiPaneInfo.Bottom(handle);
            return this;
        }

        public IAuiPaneInfo Center()
        {
            Native.AuiPaneInfo.Center(handle);
            return this;
        }

        public IAuiPaneInfo Direction(AuiManagerDock direction)
        {
            Native.AuiPaneInfo.Direction(handle, (int)direction);
            return this;
        }

        public IAuiPaneInfo Layer(int layer)
        {
            Native.AuiPaneInfo.Layer(handle, layer);
            return this;
        }

        public IAuiPaneInfo Row(int row)
        {
            Native.AuiPaneInfo.Row(handle, row);
            return this;
        }

        public IAuiPaneInfo Position(int pos)
        {
            Native.AuiPaneInfo.Position(handle, pos);
            return this;
        }

        public IAuiPaneInfo BestSizePixel(int x, int y)
        {
            Native.AuiPaneInfo.BestSize(handle, x, y);
            return this;
        }

        public IAuiPaneInfo BestSizeDip(double x, double y)
        {
            var px = owner.PixelFromDip(x);
            var py = owner.PixelFromDip(y);
            Native.AuiPaneInfo.BestSize(handle, px, py);
            return this;
        }

        public IAuiPaneInfo MinSizePixel(int x, int y)
        {
            Native.AuiPaneInfo.MinSize(handle, x, y);
            return this;
        }

        public IAuiPaneInfo MinSizeDip(SizeD size)
        {
            var px = owner.PixelFromDip(size.Width);
            var py = owner.PixelFromDip(size.Height);
            Native.AuiPaneInfo.MinSize(handle, px, py);
            return this;
        }

        public IAuiPaneInfo MinSizeDip(double width, double height)
        {
            var px = owner.PixelFromDip(width);
            var py = owner.PixelFromDip(height);
            Native.AuiPaneInfo.MinSize(handle, px, py);
            return this;
        }

        public IAuiPaneInfo MaxSizePixel(int x, int y)
        {
            Native.AuiPaneInfo.MaxSize(handle, x, y);
            return this;
        }

        public IAuiPaneInfo MaxSizeDip(SizeD size)
        {
            var px = owner.PixelFromDip(size.Width);
            var py = owner.PixelFromDip(size.Height);
            Native.AuiPaneInfo.MaxSize(handle, px, py);
            return this;
        }

        public IAuiPaneInfo FloatingPositionPixel(int x, int y)
        {
            Native.AuiPaneInfo.FloatingPosition(handle, x, y);
            return this;
        }

        public IAuiPaneInfo FloatingSizePixel(int x, int y)
        {
            Native.AuiPaneInfo.FloatingSize(handle, x, y);
            return this;
        }

        public IAuiPaneInfo Fixed()
        {
            Native.AuiPaneInfo.Fixed(handle);
            return this;
        }

        public IAuiPaneInfo Resizable(bool resizable = true)
        {
            Native.AuiPaneInfo.Resizable(handle, resizable);
            return this;
        }

        public IAuiPaneInfo Dock()
        {
            Native.AuiPaneInfo.Dock(handle);
            return this;
        }

        public IAuiPaneInfo Float()
        {
            Native.AuiPaneInfo.Float(handle);
            return this;
        }

        public IAuiPaneInfo Hide()
        {
            Native.AuiPaneInfo.Hide(handle);
            return this;
        }

        public IAuiPaneInfo Show(bool show = true)
        {
            Native.AuiPaneInfo.Show(handle, show);
            return this;
        }

        public IAuiPaneInfo CaptionVisible(bool visible = true)
        {
            Native.AuiPaneInfo.CaptionVisible(handle, visible);
            return this;
        }

        public IAuiPaneInfo Maximize()
        {
            Native.AuiPaneInfo.Maximize(handle);
            return this;
        }

        public IAuiPaneInfo Restore()
        {
            Native.AuiPaneInfo.Restore(handle);
            return this;
        }

        public IAuiPaneInfo PaneBorder(bool visible = true)
        {
            Native.AuiPaneInfo.PaneBorder(handle, visible);
            return this;
        }

        public IAuiPaneInfo Gripper(bool visible = true)
        {
            Native.AuiPaneInfo.Gripper(handle, visible);
            return this;
        }

        public IAuiPaneInfo GripperTop(bool attop = true)
        {
            Native.AuiPaneInfo.GripperTop(handle, attop);
            return this;
        }

        public IAuiPaneInfo CloseButton(bool visible = true)
        {
            Native.AuiPaneInfo.CloseButton(handle, visible);
            return this;
        }

        public IAuiPaneInfo MaximizeButton(bool visible = true)
        {
            Native.AuiPaneInfo.MaximizeButton(handle, visible);
            return this;
        }

        public IAuiPaneInfo MinimizeButton(bool visible = true)
        {
            Native.AuiPaneInfo.MinimizeButton(handle, visible);
            return this;
        }

        public IAuiPaneInfo PinButton(bool visible = true)
        {
            Native.AuiPaneInfo.PinButton(handle, visible);
            return this;
        }

        public IAuiPaneInfo DestroyOnClose(bool b = true)
        {
            Native.AuiPaneInfo.DestroyOnClose(handle, b);
            return this;
        }

        public IAuiPaneInfo TopDockable(bool b = true)
        {
            Native.AuiPaneInfo.TopDockable(handle, b);
            return this;
        }

        public IAuiPaneInfo BottomDockable(bool b = true)
        {
            Native.AuiPaneInfo.BottomDockable(handle, b);
            return this;
        }

        public IAuiPaneInfo LeftDockable(bool b = true)
        {
            Native.AuiPaneInfo.LeftDockable(handle, b);
            return this;
        }

        public IAuiPaneInfo Image(ImageSet? bitmap)
        {
            Native.AuiPaneInfo.Image(handle, (UI.Native.ImageSet?)bitmap?.Handler);
            return this;
        }

        public IAuiPaneInfo RightDockable(bool b = true)
        {
            Native.AuiPaneInfo.RightDockable(handle, b);
            return this;
        }

        public IAuiPaneInfo Floatable(bool b = true)
        {
            Native.AuiPaneInfo.Floatable(handle, b);
            return this;
        }

        public IAuiPaneInfo Movable(bool b = true)
        {
            Native.AuiPaneInfo.Movable(handle, b);
            return this;
        }

        public IAuiPaneInfo DockFixed(bool b = true)
        {
            Native.AuiPaneInfo.DockFixed(handle, b);
            return this;
        }

        public IAuiPaneInfo Dockable(bool b = true)
        {
            Native.AuiPaneInfo.Dockable(handle, b);
            return this;
        }

        public bool IsValid()
        {
            return Native.AuiPaneInfo.IsValid(handle);
        }

        public IAuiPaneInfo DefaultPane()
        {
            Native.AuiPaneInfo.DefaultPane(handle);
            return this;
        }

        public IAuiPaneInfo CenterPane()
        {
            Native.AuiPaneInfo.CenterPane(handle);
            return this;
        }

        public IAuiPaneInfo ToolbarPane()
        {
            Native.AuiPaneInfo.ToolbarPane(handle);
            return this;
        }

        public void SetFlag(int flag, bool option_state)
        {
            Native.AuiPaneInfo.SetFlag(handle, flag, option_state);
        }

        public bool HasFlag(int flag)
        {
            return Native.AuiPaneInfo.HasFlag(handle, flag);
        }
    }
}
