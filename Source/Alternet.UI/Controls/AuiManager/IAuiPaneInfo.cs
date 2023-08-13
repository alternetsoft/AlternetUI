using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IAuiPaneInfo : IDisposable
    {
        IntPtr Handle { get; }

        void SafeSet(IAuiPaneInfo source);

        bool IsOk();

        bool IsFixed();

        bool IsResizable();

        bool IsShown();

        bool IsFloating();

        bool IsDocked();

        bool IsToolbar();

        bool IsTopDockable();

        bool IsBottomDockable();

        bool IsLeftDockable();

        bool IsRightDockable();

        bool IsDockable();

        bool IsFloatable();

        bool IsMovable();

        bool IsDestroyOnClose();

        bool IsMaximized();

        bool HasCaption();

        bool HasGripper();

        bool HasBorder();

        bool HasCloseButton();

        bool HasMaximizeButton();

        bool HasMinimizeButton();

        bool HasPinButton();

        bool HasGripperTop();

        IAuiPaneInfo Window(Control window);

        IAuiPaneInfo Name(string value);

        IAuiPaneInfo Caption(string value);

        //void Icon(IntPtr bitmapBundle);

        IAuiPaneInfo Left();

        IAuiPaneInfo Right();

        IAuiPaneInfo Top();

        IAuiPaneInfo Bottom();

        IAuiPaneInfo Center();

        IAuiPaneInfo Direction(GenericDirection direction);

        IAuiPaneInfo Layer(int layer);

        IAuiPaneInfo Row(int row);

        IAuiPaneInfo Position(int pos);

        IAuiPaneInfo BestSize(int x, int y);

        IAuiPaneInfo MinSize(int x, int y);

        IAuiPaneInfo MaxSize(int x, int y);

        IAuiPaneInfo FloatingPosition(int x, int y);

        IAuiPaneInfo FloatingSize(int x, int y);

        IAuiPaneInfo Fixed();

        IAuiPaneInfo Resizable(bool resizable = true);

        IAuiPaneInfo Dock();

        IAuiPaneInfo Float();

        IAuiPaneInfo Hide();

        IAuiPaneInfo Show(bool show = true);

        IAuiPaneInfo CaptionVisible(bool visible = true);

        IAuiPaneInfo Maximize();

        IAuiPaneInfo Restore();

        IAuiPaneInfo PaneBorder(bool visible = true);

        IAuiPaneInfo Gripper(bool visible = true);

        IAuiPaneInfo GripperTop(bool attop = true);

        IAuiPaneInfo CloseButton(bool visible = true);

        IAuiPaneInfo MaximizeButton(bool visible = true);

        IAuiPaneInfo MinimizeButton(bool visible = true);

        IAuiPaneInfo PinButton(bool visible = true);

        IAuiPaneInfo DestroyOnClose(bool b = true);

        IAuiPaneInfo TopDockable(bool b = true);

        IAuiPaneInfo BottomDockable(bool b = true);

        IAuiPaneInfo LeftDockable(bool b = true);

        IAuiPaneInfo RightDockable(bool b = true);

        IAuiPaneInfo Floatable(bool b = true);

        IAuiPaneInfo Movable(bool b = true);

        IAuiPaneInfo DockFixed(bool b = true);

        IAuiPaneInfo Dockable(bool b = true);

        bool IsValid();

        IAuiPaneInfo DefaultPane();

        IAuiPaneInfo CenterPane();

        IAuiPaneInfo ToolbarPane();

        void SetFlag(int flag, bool option_state);

        bool HasFlag(int flag);
    }
}
