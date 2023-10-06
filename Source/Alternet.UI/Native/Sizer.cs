// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class Sizer : NativeObject
    {
        static Sizer()
        {
        }
        
        public Sizer()
        {
            SetNativePointer(NativeApi.Sizer_Create_());
        }
        
        public Sizer(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static System.IntPtr AddWindow(System.IntPtr handle, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_AddWindow_(handle, window, proportion, flag, border, userData);
        }
        
        public static System.IntPtr AddSizer(System.IntPtr handle, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_AddSizer_(handle, sizer, proportion, flag, border, userData);
        }
        
        public static System.IntPtr AddCustomBox(System.IntPtr handle, int width, int height, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_AddCustomBox_(handle, width, height, proportion, flag, border, userData);
        }
        
        public static System.IntPtr AddWindow2(System.IntPtr handle, System.IntPtr window, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_AddWindow2_(handle, window, sizerFlags);
        }
        
        public static System.IntPtr AddSizer2(System.IntPtr handle, System.IntPtr sizer, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_AddSizer2_(handle, sizer, sizerFlags);
        }
        
        public static System.IntPtr AddCustomBox2(System.IntPtr handle, int width, int height, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_AddCustomBox2_(handle, width, height, sizerFlags);
        }
        
        public static System.IntPtr AddItem(System.IntPtr handle, System.IntPtr item)
        {
            return NativeApi.Sizer_AddItem_(handle, item);
        }
        
        public static System.IntPtr AddSpacer(System.IntPtr handle, int size)
        {
            return NativeApi.Sizer_AddSpacer_(handle, size);
        }
        
        public static System.IntPtr AddStretchSpacer(System.IntPtr handle, int prop)
        {
            return NativeApi.Sizer_AddStretchSpacer_(handle, prop);
        }
        
        public static System.IntPtr InsertWindow(System.IntPtr handle, int index, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_InsertWindow_(handle, index, window, proportion, flag, border, userData);
        }
        
        public static System.IntPtr InsertSizer(System.IntPtr handle, int index, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_InsertSizer_(handle, index, sizer, proportion, flag, border, userData);
        }
        
        public static System.IntPtr InsertCustomBox(System.IntPtr handle, int index, int width, int height, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_InsertCustomBox_(handle, index, width, height, proportion, flag, border, userData);
        }
        
        public static System.IntPtr InsertWindow2(System.IntPtr handle, int index, System.IntPtr window, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_InsertWindow2_(handle, index, window, sizerFlags);
        }
        
        public static System.IntPtr InsertSizer2(System.IntPtr handle, int index, System.IntPtr sizer, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_InsertSizer2_(handle, index, sizer, sizerFlags);
        }
        
        public static System.IntPtr InsertCustomBox2(System.IntPtr handle, int index, int width, int height, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_InsertCustomBox2_(handle, index, width, height, sizerFlags);
        }
        
        public static System.IntPtr InsertItem(System.IntPtr handle, int index, System.IntPtr item)
        {
            return NativeApi.Sizer_InsertItem_(handle, index, item);
        }
        
        public static System.IntPtr InsertSpacer(System.IntPtr handle, int index, int size)
        {
            return NativeApi.Sizer_InsertSpacer_(handle, index, size);
        }
        
        public static System.IntPtr InsertStretchSpacer(System.IntPtr handle, int index, int prop)
        {
            return NativeApi.Sizer_InsertStretchSpacer_(handle, index, prop);
        }
        
        public static System.IntPtr PrependWindow(System.IntPtr handle, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_PrependWindow_(handle, window, proportion, flag, border, userData);
        }
        
        public static System.IntPtr PrependSizer(System.IntPtr handle, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_PrependSizer_(handle, sizer, proportion, flag, border, userData);
        }
        
        public static System.IntPtr PrependCustomBox(System.IntPtr handle, int width, int height, int proportion, int flag, int border, System.IntPtr userData)
        {
            return NativeApi.Sizer_PrependCustomBox_(handle, width, height, proportion, flag, border, userData);
        }
        
        public static System.IntPtr PrependWindow2(System.IntPtr handle, System.IntPtr window, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_PrependWindow2_(handle, window, sizerFlags);
        }
        
        public static System.IntPtr PrependSizer2(System.IntPtr handle, System.IntPtr sizer, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_PrependSizer2_(handle, sizer, sizerFlags);
        }
        
        public static System.IntPtr PrependCustomBox2(System.IntPtr handle, int width, int height, System.IntPtr sizerFlags)
        {
            return NativeApi.Sizer_PrependCustomBox2_(handle, width, height, sizerFlags);
        }
        
        public static System.IntPtr PrependItem(System.IntPtr handle, System.IntPtr item)
        {
            return NativeApi.Sizer_PrependItem_(handle, item);
        }
        
        public static System.IntPtr PrependSpacer(System.IntPtr handle, int size)
        {
            return NativeApi.Sizer_PrependSpacer_(handle, size);
        }
        
        public static System.IntPtr PrependStretchSpacer(System.IntPtr handle, int prop)
        {
            return NativeApi.Sizer_PrependStretchSpacer_(handle, prop);
        }
        
        public static void SetContainingWindow(System.IntPtr handle, System.IntPtr window)
        {
            NativeApi.Sizer_SetContainingWindow_(handle, window);
        }
        
        public static System.IntPtr GetContainingWindow(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetContainingWindow_(handle);
        }
        
        public static bool Remove(System.IntPtr handle, System.IntPtr sizer)
        {
            return NativeApi.Sizer_Remove_(handle, sizer);
        }
        
        public static bool Remove2(System.IntPtr handle, int index)
        {
            return NativeApi.Sizer_Remove2_(handle, index);
        }
        
        public static bool DetachWindow(System.IntPtr handle, System.IntPtr window)
        {
            return NativeApi.Sizer_DetachWindow_(handle, window);
        }
        
        public static bool DetachSizer(System.IntPtr handle, System.IntPtr sizer)
        {
            return NativeApi.Sizer_DetachSizer_(handle, sizer);
        }
        
        public static bool Detach(System.IntPtr handle, int index)
        {
            return NativeApi.Sizer_Detach_(handle, index);
        }
        
        public static bool ReplaceWindow(System.IntPtr handle, System.IntPtr oldwin, System.IntPtr newwin, bool recursive)
        {
            return NativeApi.Sizer_ReplaceWindow_(handle, oldwin, newwin, recursive);
        }
        
        public static bool ReplaceSizer(System.IntPtr handle, System.IntPtr oldsz, System.IntPtr newsz, bool recursive)
        {
            return NativeApi.Sizer_ReplaceSizer_(handle, oldsz, newsz, recursive);
        }
        
        public static bool ReplaceItem(System.IntPtr handle, int index, System.IntPtr newitem)
        {
            return NativeApi.Sizer_ReplaceItem_(handle, index, newitem);
        }
        
        public static void Clear(System.IntPtr handle, bool delete_windows)
        {
            NativeApi.Sizer_Clear_(handle, delete_windows);
        }
        
        public static void DeleteWindows(System.IntPtr handle)
        {
            NativeApi.Sizer_DeleteWindows_(handle);
        }
        
        public static bool InformFirstDirection(System.IntPtr handle, int direction, int size, int availableOtherDir)
        {
            return NativeApi.Sizer_InformFirstDirection_(handle, direction, size, availableOtherDir);
        }
        
        public static void SetMinSize(System.IntPtr handle, int width, int height)
        {
            NativeApi.Sizer_SetMinSize_(handle, width, height);
        }
        
        public static bool SetWindowItemMinSize(System.IntPtr handle, System.IntPtr window, int width, int height)
        {
            return NativeApi.Sizer_SetWindowItemMinSize_(handle, window, width, height);
        }
        
        public static bool SetSizerItemMinSize(System.IntPtr handle, System.IntPtr sizer, int width, int height)
        {
            return NativeApi.Sizer_SetSizerItemMinSize_(handle, sizer, width, height);
        }
        
        public static bool SetCustomBoxItemMinSize(System.IntPtr handle, int index, int width, int height)
        {
            return NativeApi.Sizer_SetCustomBoxItemMinSize_(handle, index, width, height);
        }
        
        public static Alternet.Drawing.Int32Size GetSize(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetSize_(handle);
        }
        
        public static Alternet.Drawing.Int32Point GetPosition(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetPosition_(handle);
        }
        
        public static Alternet.Drawing.Int32Size GetMinSize(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetMinSize_(handle);
        }
        
        public static Alternet.Drawing.Int32Size CalcMin(System.IntPtr handle)
        {
            return NativeApi.Sizer_CalcMin_(handle);
        }
        
        public static void RepositionChildren(System.IntPtr handle, Alternet.Drawing.Int32Size minSize)
        {
            NativeApi.Sizer_RepositionChildren_(handle, minSize);
        }
        
        public static void RecalcSizes(System.IntPtr handle)
        {
            NativeApi.Sizer_RecalcSizes_(handle);
        }
        
        public static void Layout(System.IntPtr handle)
        {
            NativeApi.Sizer_Layout_(handle);
        }
        
        public static Alternet.Drawing.Int32Size ComputeFittingClientSize(System.IntPtr handle, System.IntPtr window)
        {
            return NativeApi.Sizer_ComputeFittingClientSize_(handle, window);
        }
        
        public static Alternet.Drawing.Int32Size ComputeFittingWindowSize(System.IntPtr handle, System.IntPtr window)
        {
            return NativeApi.Sizer_ComputeFittingWindowSize_(handle, window);
        }
        
        public static Alternet.Drawing.Int32Size Fit(System.IntPtr handle, System.IntPtr window)
        {
            return NativeApi.Sizer_Fit_(handle, window);
        }
        
        public static void FitInside(System.IntPtr handle, System.IntPtr window)
        {
            NativeApi.Sizer_FitInside_(handle, window);
        }
        
        public static void SetSizeHints(System.IntPtr handle, System.IntPtr window)
        {
            NativeApi.Sizer_SetSizeHints_(handle, window);
        }
        
        public static System.IntPtr GetChildren(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetChildren_(handle);
        }
        
        public static void SetDimension(System.IntPtr handle, int x, int y, int width, int height)
        {
            NativeApi.Sizer_SetDimension_(handle, x, y, width, height);
        }
        
        public static int GetItemCount(System.IntPtr handle)
        {
            return NativeApi.Sizer_GetItemCount_(handle);
        }
        
        public static bool IsEmpty(System.IntPtr handle)
        {
            return NativeApi.Sizer_IsEmpty_(handle);
        }
        
        public static System.IntPtr GetItemWindow(System.IntPtr handle, System.IntPtr window, bool recursive)
        {
            return NativeApi.Sizer_GetItemWindow_(handle, window, recursive);
        }
        
        public static System.IntPtr GetItemSizer(System.IntPtr handle, System.IntPtr sizer, bool recursive)
        {
            return NativeApi.Sizer_GetItemSizer_(handle, sizer, recursive);
        }
        
        public static System.IntPtr GetItem(System.IntPtr handle, int index)
        {
            return NativeApi.Sizer_GetItem_(handle, index);
        }
        
        public static System.IntPtr GetItemById(System.IntPtr handle, int id, bool recursive)
        {
            return NativeApi.Sizer_GetItemById_(handle, id, recursive);
        }
        
        public static bool ShowWindow(System.IntPtr handle, System.IntPtr window, bool show, bool recursive)
        {
            return NativeApi.Sizer_ShowWindow_(handle, window, show, recursive);
        }
        
        public static bool ShowSizer(System.IntPtr handle, System.IntPtr sizer, bool show, bool recursive)
        {
            return NativeApi.Sizer_ShowSizer_(handle, sizer, show, recursive);
        }
        
        public static bool ShowItem(System.IntPtr handle, int index, bool show)
        {
            return NativeApi.Sizer_ShowItem_(handle, index, show);
        }
        
        public static bool HideSizer(System.IntPtr handle, System.IntPtr sizer, bool recursive)
        {
            return NativeApi.Sizer_HideSizer_(handle, sizer, recursive);
        }
        
        public static bool HideWindow(System.IntPtr handle, System.IntPtr window, bool recursive)
        {
            return NativeApi.Sizer_HideWindow_(handle, window, recursive);
        }
        
        public static bool Hide(System.IntPtr handle, int index)
        {
            return NativeApi.Sizer_Hide_(handle, index);
        }
        
        public static bool IsShownWindow(System.IntPtr handle, System.IntPtr window)
        {
            return NativeApi.Sizer_IsShownWindow_(handle, window);
        }
        
        public static bool IsShownSizer(System.IntPtr handle, System.IntPtr sizer)
        {
            return NativeApi.Sizer_IsShownSizer_(handle, sizer);
        }
        
        public static bool IsShown(System.IntPtr handle, int index)
        {
            return NativeApi.Sizer_IsShown_(handle, index);
        }
        
        public static void ShowItems(System.IntPtr handle, bool show)
        {
            NativeApi.Sizer_ShowItems_(handle, show);
        }
        
        public static void Show(System.IntPtr handle, bool show)
        {
            NativeApi.Sizer_Show_(handle, show);
        }
        
        public static bool AreAnyItemsShown(System.IntPtr handle)
        {
            return NativeApi.Sizer_AreAnyItemsShown_(handle);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Sizer_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddWindow_(System.IntPtr handle, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddSizer_(System.IntPtr handle, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddCustomBox_(System.IntPtr handle, int width, int height, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddWindow2_(System.IntPtr handle, System.IntPtr window, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddSizer2_(System.IntPtr handle, System.IntPtr sizer, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddCustomBox2_(System.IntPtr handle, int width, int height, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddItem_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddSpacer_(System.IntPtr handle, int size);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_AddStretchSpacer_(System.IntPtr handle, int prop);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertWindow_(System.IntPtr handle, int index, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertSizer_(System.IntPtr handle, int index, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertCustomBox_(System.IntPtr handle, int index, int width, int height, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertWindow2_(System.IntPtr handle, int index, System.IntPtr window, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertSizer2_(System.IntPtr handle, int index, System.IntPtr sizer, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertCustomBox2_(System.IntPtr handle, int index, int width, int height, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertItem_(System.IntPtr handle, int index, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertSpacer_(System.IntPtr handle, int index, int size);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_InsertStretchSpacer_(System.IntPtr handle, int index, int prop);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependWindow_(System.IntPtr handle, System.IntPtr window, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependSizer_(System.IntPtr handle, System.IntPtr sizer, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependCustomBox_(System.IntPtr handle, int width, int height, int proportion, int flag, int border, System.IntPtr userData);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependWindow2_(System.IntPtr handle, System.IntPtr window, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependSizer2_(System.IntPtr handle, System.IntPtr sizer, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependCustomBox2_(System.IntPtr handle, int width, int height, System.IntPtr sizerFlags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependItem_(System.IntPtr handle, System.IntPtr item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependSpacer_(System.IntPtr handle, int size);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_PrependStretchSpacer_(System.IntPtr handle, int prop);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_SetContainingWindow_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetContainingWindow_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_Remove_(System.IntPtr handle, System.IntPtr sizer);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_Remove2_(System.IntPtr handle, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_DetachWindow_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_DetachSizer_(System.IntPtr handle, System.IntPtr sizer);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_Detach_(System.IntPtr handle, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ReplaceWindow_(System.IntPtr handle, System.IntPtr oldwin, System.IntPtr newwin, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ReplaceSizer_(System.IntPtr handle, System.IntPtr oldsz, System.IntPtr newsz, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ReplaceItem_(System.IntPtr handle, int index, System.IntPtr newitem);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_Clear_(System.IntPtr handle, bool delete_windows);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_DeleteWindows_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_InformFirstDirection_(System.IntPtr handle, int direction, int size, int availableOtherDir);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_SetMinSize_(System.IntPtr handle, int width, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_SetWindowItemMinSize_(System.IntPtr handle, System.IntPtr window, int width, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_SetSizerItemMinSize_(System.IntPtr handle, System.IntPtr sizer, int width, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_SetCustomBoxItemMinSize_(System.IntPtr handle, int index, int width, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_GetSize_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Point Sizer_GetPosition_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_GetMinSize_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_CalcMin_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_RepositionChildren_(System.IntPtr handle, Alternet.Drawing.Int32Size minSize);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_RecalcSizes_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_Layout_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_ComputeFittingClientSize_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_ComputeFittingWindowSize_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.Int32Size Sizer_Fit_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_FitInside_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_SetSizeHints_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetChildren_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_SetDimension_(System.IntPtr handle, int x, int y, int width, int height);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int Sizer_GetItemCount_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_IsEmpty_(System.IntPtr handle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetItemWindow_(System.IntPtr handle, System.IntPtr window, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetItemSizer_(System.IntPtr handle, System.IntPtr sizer, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetItem_(System.IntPtr handle, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr Sizer_GetItemById_(System.IntPtr handle, int id, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ShowWindow_(System.IntPtr handle, System.IntPtr window, bool show, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ShowSizer_(System.IntPtr handle, System.IntPtr sizer, bool show, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_ShowItem_(System.IntPtr handle, int index, bool show);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_HideSizer_(System.IntPtr handle, System.IntPtr sizer, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_HideWindow_(System.IntPtr handle, System.IntPtr window, bool recursive);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_Hide_(System.IntPtr handle, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_IsShownWindow_(System.IntPtr handle, System.IntPtr window);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_IsShownSizer_(System.IntPtr handle, System.IntPtr sizer);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_IsShown_(System.IntPtr handle, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_ShowItems_(System.IntPtr handle, bool show);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Sizer_Show_(System.IntPtr handle, bool show);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Sizer_AreAnyItemsShown_(System.IntPtr handle);
            
        }
    }
}
