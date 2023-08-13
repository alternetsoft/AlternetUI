using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    // https://learn.microsoft.com/ru-ru/dotnet/api/system.idisposable?view=net-7.0

    /// <summary>
    /// The central class of the advanced docking and floating toolbars and
    /// panes framework.
    /// </summary>
    /// <remarks>
    /// Manages the panes associated with it, using a pane's
    /// <see cref="IAuiPaneInfo"/> information to determine each pane's docking
    /// and floating behaviour.
    /// </remarks>
    /// <remarks>
    /// It uses a replaceable dock art class to do all drawing, so all drawing
    /// is localized in one area, and may be customized depending on an
    /// application's specific needs.
    /// </remarks>
    /// <remarks>
    /// Works as follows: the programmer adds panes to the class, or makes changes
    /// to existing pane properties (dock position, floating state, show state, etc.).
    /// To apply these changes, Update() function is called. This batch processing
    /// can be used to avoid flicker, by modifying more than one pane at a time,
    /// and then "committing" all of the changes at once by calling Update().
    /// </remarks>
    public class AuiManager : IDisposable
    {
        private IntPtr handle;

        /// <summary>
        /// Creates <see cref="AuiManager"/> instance.
        /// </summary>
        public AuiManager()
        {
            handle = Native.AuiManager.CreateAuiManager();
        }

        /// <summary>
        /// Called when <see cref="AuiManager"/> instance is destroyed.
        /// </summary>
        ~AuiManager()
        {
            Dispose();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.AuiManager.Delete(handle);
                handle = IntPtr.Zero;
            }
        }

        public void UnInit()
        {
            Native.AuiManager.UnInit(handle);
        }

        public void SetFlags(AuiManagerOption flags)
        {
            Native.AuiManager.SetFlags(handle, (uint)flags);
        }

        public AuiManagerOption GetFlags()
        {
            return (AuiManagerOption)Native.AuiManager.GetFlags(handle);
        }

        public bool AlwaysUsesLiveResize()
        {
            return Native.AuiManager.AlwaysUsesLiveResize();
        }

        public bool HasLiveResize()
        {
            return Native.AuiManager.HasLiveResize(handle);
        }

        public void Update()
        {
            Native.AuiManager.Update(handle);
        }

        public string SavePerspective()
        {
            return Native.AuiManager.SavePerspective(handle);
        }

        public bool LoadPerspective(string perspective, bool update = true)
        {
            return Native.AuiManager.LoadPerspective(handle, perspective, update);
        }

        public void SetDockSizeConstraint(double widthPct, double heightPct)
        {
            Native.AuiManager.SetDockSizeConstraint(handle, widthPct, heightPct);
        }

        public void RestoreMaximizedPane()
        {
            Native.AuiManager.RestoreMaximizedPane(handle);
        }

        public IAuiPaneInfo GetPane(Control control)
        {
            return ToPaneInfo(Native.AuiManager.GetPane(handle, ToHandle(control)));
        }

        public IAuiPaneInfo GetPane(string name)
        {
            return ToPaneInfo(Native.AuiManager.GetPaneByName(handle, name));
        }

        public void SetManagedWindow(Window managedWnd)
        {
            Native.AuiManager.SetManagedWindow(handle, ToHandle(managedWnd));
        }

        public bool DetachPane(Control control)
        {
            return Native.AuiManager.DetachPane(handle, ToHandle(control));
        }

        public bool AddPane(Control control, IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.AddPane(
                handle,
                ToHandle(control),
                ToHandle(paneInfo));
        }

        public bool AddPane(
            Control control,
            IAuiPaneInfo paneInfo,
            double dropPosX,
            double dropPosY)
        {
            return Native.AuiManager.AddPane2(
                handle,
                ToHandle(control),
                ToHandle(paneInfo),
                dropPosX,
                dropPosY);
        }

        public bool AddPane(Control window, GenericDirection direction, string caption)
        {
            return Native.AuiManager.AddPane3(
                handle,
                ToHandle(window),
                (int)direction,
                caption);
        }

        public bool InsertPane(
            Control window,
            IAuiPaneInfo insertLocPaneInfo,
            int insertLevel)
        {
            return Native.AuiManager.InsertPane(
                handle,
                ToHandle(window),
                ToHandle(insertLocPaneInfo),
                insertLevel);
        }

        public string SavePaneInfo(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.SavePaneInfo(handle, ToHandle(paneInfo));
        }

        public void LoadPaneInfo(string panePart, IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.LoadPaneInfo(handle, panePart, ToHandle(paneInfo));
        }

        public Size GetDockSizeConstraint()
        {
            return Native.AuiManager.GetDockSizeConstraint(handle);
        }

        public void ClosePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.ClosePane(handle, ToHandle(paneInfo));
        }

        public void MaximizePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.MaximizePane(handle, ToHandle(paneInfo));
        }

        public void RestorePane(IAuiPaneInfo paneInfo)
        {
            Native.AuiManager.RestorePane(handle, ToHandle(paneInfo));
        }

        public bool CanDockPanel(IAuiPaneInfo paneInfo)
        {
            return Native.AuiManager.CanDockPanel(handle, ToHandle(paneInfo));
        }

        public IAuiPaneInfo CreateAuiPaneInfo()
        {
            return new AuiPaneInfo();
        }

        internal static IAuiPaneInfo ToPaneInfo(IntPtr handle)
        {
            return new AuiPaneInfo(handle);
        }

        internal static IntPtr ToHandle(IAuiPaneInfo paneInfo)
        {
            return paneInfo.Handle;
        }

        internal static IntPtr ToHandle(Control window)
        {
            return window.Handler.NativeControl!.WxWidget;
        }
    }
}

/*


public void SetArtProvider(IntPtr artProvider) { }
public IntPtr GetArtProvider(){};

//public IntPtr[] GetAllPanes(){};


public IntPtr CreateFloatingFrame(,
    IntPtr parentWindow, IAuiPaneInfo paneInfo){};
*/