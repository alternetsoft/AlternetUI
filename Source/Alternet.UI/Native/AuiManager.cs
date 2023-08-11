// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class AuiManager : NativeObject
    {
        static AuiManager()
        {
        }
        
        public AuiManager()
        {
            SetNativePointer(NativeApi.AuiManager_Create_());
        }
        
        public AuiManager(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static void Delete(System.IntPtr handle)
        {
            NativeApi.AuiManager_Delete_(handle);
        }
        
        public static System.IntPtr CreateAuiManager()
        {
            var n = NativeApi.AuiManager_CreateAuiManager_();
            var m = n;
            return m;
        }
        
        public static System.IntPtr CreateAuiManager2(System.IntPtr managedWnd, uint flags)
        {
            var n = NativeApi.AuiManager_CreateAuiManager2_(managedWnd, flags);
            var m = n;
            return m;
        }
        
        public static void UnInit(System.IntPtr handle)
        {
            NativeApi.AuiManager_UnInit_(handle);
        }
        
        public static void SetFlags(System.IntPtr handle, uint flags)
        {
            NativeApi.AuiManager_SetFlags_(handle, flags);
        }
        
        public static uint GetFlags(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_GetFlags_(handle);
            var m = n;
            return m;
        }
        
        public static bool AlwaysUsesLiveResize()
        {
            var n = NativeApi.AuiManager_AlwaysUsesLiveResize_();
            var m = n;
            return m;
        }
        
        public static bool HasLiveResize(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_HasLiveResize_(handle);
            var m = n;
            return m;
        }
        
        public static void SetManagedWindow(System.IntPtr handle, System.IntPtr managedWnd)
        {
            NativeApi.AuiManager_SetManagedWindow_(handle, managedWnd);
        }
        
        public static System.IntPtr GetManagedWindow(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_GetManagedWindow_(handle);
            var m = n;
            return m;
        }
        
        public static System.IntPtr GetManager(System.IntPtr window)
        {
            var n = NativeApi.AuiManager_GetManager_(window);
            var m = n;
            return m;
        }
        
        public static void SetArtProvider(System.IntPtr handle, System.IntPtr artProvider)
        {
            NativeApi.AuiManager_SetArtProvider_(handle, artProvider);
        }
        
        public static System.IntPtr GetArtProvider(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_GetArtProvider_(handle);
            var m = n;
            return m;
        }
        
        public static bool DetachPane(System.IntPtr handle, System.IntPtr window)
        {
            var n = NativeApi.AuiManager_DetachPane_(handle, window);
            var m = n;
            return m;
        }
        
        public static void Update(System.IntPtr handle)
        {
            NativeApi.AuiManager_Update_(handle);
        }
        
        public static string SavePerspective(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_SavePerspective_(handle);
            var m = n;
            return m;
        }
        
        public static bool LoadPerspective(System.IntPtr handle, string perspective, bool update)
        {
            var n = NativeApi.AuiManager_LoadPerspective_(handle, perspective, update);
            var m = n;
            return m;
        }
        
        public static void SetDockSizeConstraint(System.IntPtr handle, double widthPct, double heightPct)
        {
            NativeApi.AuiManager_SetDockSizeConstraint_(handle, widthPct, heightPct);
        }
        
        public static void RestoreMaximizedPane(System.IntPtr handle)
        {
            NativeApi.AuiManager_RestoreMaximizedPane_(handle);
        }
        
        public static System.IntPtr GetPane(System.IntPtr handle, System.IntPtr window)
        {
            var n = NativeApi.AuiManager_GetPane_(handle, window);
            var m = n;
            return m;
        }
        
        public static System.IntPtr GetPaneByName(System.IntPtr handle, string name)
        {
            var n = NativeApi.AuiManager_GetPaneByName_(handle, name);
            var m = n;
            return m;
        }
        
        public static bool AddPane(System.IntPtr handle, System.IntPtr window, System.IntPtr paneInfo)
        {
            var n = NativeApi.AuiManager_AddPane_(handle, window, paneInfo);
            var m = n;
            return m;
        }
        
        public static bool AddPane2(System.IntPtr handle, System.IntPtr window, System.IntPtr paneInfo, double dropPosX, double dropPosY)
        {
            var n = NativeApi.AuiManager_AddPane2_(handle, window, paneInfo, dropPosX, dropPosY);
            var m = n;
            return m;
        }
        
        public static bool AddPane3(System.IntPtr handle, System.IntPtr window, int direction, string caption)
        {
            var n = NativeApi.AuiManager_AddPane3_(handle, window, direction, caption);
            var m = n;
            return m;
        }
        
        public static bool InsertPane(System.IntPtr handle, System.IntPtr window, System.IntPtr insertLocPaneInfo, int insertLevel)
        {
            var n = NativeApi.AuiManager_InsertPane_(handle, window, insertLocPaneInfo, insertLevel);
            var m = n;
            return m;
        }
        
        public static string SavePaneInfo(System.IntPtr handle, System.IntPtr paneInfo)
        {
            var n = NativeApi.AuiManager_SavePaneInfo_(handle, paneInfo);
            var m = n;
            return m;
        }
        
        public static void LoadPaneInfo(System.IntPtr handle, string panePart, System.IntPtr paneInfo)
        {
            NativeApi.AuiManager_LoadPaneInfo_(handle, panePart, paneInfo);
        }
        
        public static Alternet.Drawing.Size GetDockSizeConstraint(System.IntPtr handle)
        {
            var n = NativeApi.AuiManager_GetDockSizeConstraint_(handle);
            var m = n;
            return m;
        }
        
        public static void ClosePane(System.IntPtr handle, System.IntPtr paneInfo)
        {
            NativeApi.AuiManager_ClosePane_(handle, paneInfo);
        }
        
        public static void MaximizePane(System.IntPtr handle, System.IntPtr paneInfo)
        {
            NativeApi.AuiManager_MaximizePane_(handle, paneInfo);
        }
        
        public static void RestorePane(System.IntPtr handle, System.IntPtr paneInfo)
        {
            NativeApi.AuiManager_RestorePane_(handle, paneInfo);
        }
        
        public static System.IntPtr CreateFloatingFrame(System.IntPtr handle, System.IntPtr parentWindow, System.IntPtr paneInfo)
        {
            var n = NativeApi.AuiManager_CreateFloatingFrame_(handle, parentWindow, paneInfo);
            var m = n;
            return m;
        }
        
        public static bool CanDockPanel(System.IntPtr handle, System.IntPtr paneInfo)
        {
            var n = NativeApi.AuiManager_CanDockPanel_(handle, paneInfo);
            var m = n;
            return m;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr AuiManager_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_Delete_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_CreateAuiManager_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_CreateAuiManager2_(System.IntPtr managedWnd, uint flags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_UnInit_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_SetFlags_(System.IntPtr handle, uint flags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern uint AuiManager_GetFlags_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_AlwaysUsesLiveResize_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_HasLiveResize_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_SetManagedWindow_(System.IntPtr handle, System.IntPtr managedWnd);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_GetManagedWindow_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_GetManager_(System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_SetArtProvider_(System.IntPtr handle, System.IntPtr artProvider);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_GetArtProvider_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_DetachPane_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_Update_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string AuiManager_SavePerspective_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_LoadPerspective_(System.IntPtr handle, string perspective, bool update);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_SetDockSizeConstraint_(System.IntPtr handle, double widthPct, double heightPct);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_RestoreMaximizedPane_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_GetPane_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_GetPaneByName_(System.IntPtr handle, string name);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_AddPane_(System.IntPtr handle, System.IntPtr window, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_AddPane2_(System.IntPtr handle, System.IntPtr window, System.IntPtr paneInfo, double dropPosX, double dropPosY);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_AddPane3_(System.IntPtr handle, System.IntPtr window, int direction, string caption);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_InsertPane_(System.IntPtr handle, System.IntPtr window, System.IntPtr insertLocPaneInfo, int insertLevel);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string AuiManager_SavePaneInfo_(System.IntPtr handle, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_LoadPaneInfo_(System.IntPtr handle, string panePart, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Size AuiManager_GetDockSizeConstraint_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_ClosePane_(System.IntPtr handle, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_MaximizePane_(System.IntPtr handle, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void AuiManager_RestorePane_(System.IntPtr handle, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr AuiManager_CreateFloatingFrame_(System.IntPtr handle, System.IntPtr parentWindow, System.IntPtr paneInfo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool AuiManager_CanDockPanel_(System.IntPtr handle, System.IntPtr paneInfo);
            
        }
    }
}
