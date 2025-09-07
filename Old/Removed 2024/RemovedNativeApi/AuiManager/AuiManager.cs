#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_manager.html
	public class AuiManager
	{
        public static void Delete(IntPtr handle) { }

        public static IntPtr CreateAuiManager() => throw new Exception();
        public static IntPtr CreateAuiManager2(IntPtr managedWnd,
            uint flags) => throw new Exception();

        public static void UnInit(IntPtr handle) {}

        public static void SetFlags(IntPtr handle, uint flags) { }
        public static uint GetFlags(IntPtr handle) => throw new Exception();

        public static bool AlwaysUsesLiveResize() => throw new Exception();
        public static bool HasLiveResize(IntPtr handle) => throw new Exception();

        public static void SetManagedWindow(IntPtr handle, IntPtr managedWnd) { }
        public static IntPtr GetManagedWindow(IntPtr handle) => throw new Exception();

        public static IntPtr GetManager(IntPtr window) => throw new Exception();

        public static void SetArtProvider(IntPtr handle, IntPtr artProvider) {}
        public static IntPtr GetArtProvider(IntPtr handle) => throw new Exception();

        public static bool DetachPane(IntPtr handle, IntPtr window) =>
            throw new Exception();

        public static void Update(IntPtr handle) {}

        public static string SavePerspective(IntPtr handle) => throw new Exception();
        public static bool LoadPerspective(IntPtr handle,
            string perspective, bool update) => throw new Exception();

        public static void SetDockSizeConstraint(IntPtr handle, double widthPct,
            double heightPct) => throw new Exception();
        public static void RestoreMaximizedPane(IntPtr handle) =>
            throw new Exception();

        public static IntPtr GetPane(IntPtr handle, IntPtr window) =>
            throw new Exception();
        public static IntPtr GetPaneByName(IntPtr handle, string name) =>
            throw new Exception();
        //public static IntPtr[] GetAllPanes(IntPtr handle) => throw new Exception();

        public static bool AddPane(IntPtr handle, IntPtr window, IntPtr paneInfo) =>
            throw new Exception();

        public static bool AddPane2(IntPtr handle, IntPtr window, IntPtr paneInfo,
            double dropPosX,
            double dropPosY) => throw new Exception();

        public static bool AddPane3(IntPtr handle, IntPtr window, int direction,
            string caption) => throw new Exception();

        public static bool InsertPane(IntPtr handle, IntPtr window,
            IntPtr insertLocPaneInfo,
                     int insertLevel) => throw new Exception();

        public static string SavePaneInfo(IntPtr handle, IntPtr paneInfo) =>
            throw new Exception();
        public static void LoadPaneInfo(IntPtr handle, string panePart,
            IntPtr paneInfo) => throw new Exception();

        public static SizeD GetDockSizeConstraint(IntPtr handle) => throw new Exception();

        public static void ClosePane(IntPtr handle, IntPtr paneInfo) =>
            throw new Exception();
        public static void MaximizePane(IntPtr handle, IntPtr paneInfo) =>
            throw new Exception();
        public static void RestorePane(IntPtr handle, IntPtr paneInfo) =>
            throw new Exception();

        public static IntPtr CreateFloatingFrame(IntPtr handle,
            IntPtr parentWindow, IntPtr paneInfo) => throw new Exception();
        public static bool CanDockPanel(IntPtr handle, IntPtr paneInfo) =>
            throw new Exception();
    }
}