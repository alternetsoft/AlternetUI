// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class ComboBox : Control
    {
        static ComboBox()
        {
            SetEventCallback();
        }
        
        public ComboBox()
        {
            SetNativePointer(NativeApi.ComboBox_Create_());
        }
        
        public ComboBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool AllowMouseWheel
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetAllowMouseWheel_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetAllowMouseWheel_(NativePointer, value);
            }
        }
        
        public string EmptyTextHint
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEmptyTextHint_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetEmptyTextHint_(NativePointer, value);
            }
        }
        
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetHasBorder_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetHasBorder_(NativePointer, value);
            }
        }
        
        public int ItemsCount
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetItemsCount_(NativePointer);
            }
            
        }
        
        public bool IsEditable
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetIsEditable_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetIsEditable_(NativePointer, value);
            }
        }
        
        public int SelectedIndex
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetSelectedIndex_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetSelectedIndex_(NativePointer, value);
            }
        }
        
        public int TextSelectionStart
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetTextSelectionStart_(NativePointer);
            }
            
        }
        
        public int TextSelectionLength
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetTextSelectionLength_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.PointI TextMargins
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetTextMargins_(NativePointer);
            }
            
        }
        
        public int OwnerDrawStyle
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetOwnerDrawStyle_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetOwnerDrawStyle_(NativePointer, value);
            }
        }
        
        public System.IntPtr PopupWidget
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetPopupWidget_(NativePointer);
            }
            
        }
        
        public System.IntPtr EventDc
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventDc_(NativePointer);
            }
            
        }
        
        public Alternet.Drawing.RectI EventRect
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventRect_(NativePointer);
            }
            
        }
        
        public int EventItem
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventItem_(NativePointer);
            }
            
        }
        
        public int EventFlags
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventFlags_(NativePointer);
            }
            
        }
        
        public int EventResultInt
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventResultInt_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetEventResultInt_(NativePointer, value);
            }
        }
        
        public bool EventCalled
        {
            get
            {
                CheckDisposed();
                return NativeApi.ComboBox_GetEventCalled_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.ComboBox_SetEventCalled_(NativePointer, value);
            }
        }
        
        public void DismissPopup()
        {
            CheckDisposed();
            NativeApi.ComboBox_DismissPopup_(NativePointer);
        }
        
        public void ShowPopup()
        {
            CheckDisposed();
            NativeApi.ComboBox_ShowPopup_(NativePointer);
        }
        
        public int DefaultOnMeasureItemWidth()
        {
            CheckDisposed();
            return NativeApi.ComboBox_DefaultOnMeasureItemWidth_(NativePointer);
        }
        
        public int DefaultOnMeasureItem()
        {
            CheckDisposed();
            return NativeApi.ComboBox_DefaultOnMeasureItem_(NativePointer);
        }
        
        public void DefaultOnDrawBackground()
        {
            CheckDisposed();
            NativeApi.ComboBox_DefaultOnDrawBackground_(NativePointer);
        }
        
        public void DefaultOnDrawItem()
        {
            CheckDisposed();
            NativeApi.ComboBox_DefaultOnDrawItem_(NativePointer);
        }
        
        public System.IntPtr CreateItemsInsertion()
        {
            CheckDisposed();
            return NativeApi.ComboBox_CreateItemsInsertion_(NativePointer);
        }
        
        public void AddItemToInsertion(System.IntPtr insertion, string item)
        {
            CheckDisposed();
            NativeApi.ComboBox_AddItemToInsertion_(NativePointer, insertion, item);
        }
        
        public void CommitItemsInsertion(System.IntPtr insertion, int index)
        {
            CheckDisposed();
            NativeApi.ComboBox_CommitItemsInsertion_(NativePointer, insertion, index);
        }
        
        public void InsertItem(int index, string value)
        {
            CheckDisposed();
            NativeApi.ComboBox_InsertItem_(NativePointer, index, value);
        }
        
        public void RemoveItemAt(int index)
        {
            CheckDisposed();
            NativeApi.ComboBox_RemoveItemAt_(NativePointer, index);
        }
        
        public void ClearItems()
        {
            CheckDisposed();
            NativeApi.ComboBox_ClearItems_(NativePointer);
        }
        
        public void SelectTextRange(int start, int length)
        {
            CheckDisposed();
            NativeApi.ComboBox_SelectTextRange_(NativePointer, start, length);
        }
        
        public void SelectAllText()
        {
            CheckDisposed();
            NativeApi.ComboBox_SelectAllText_(NativePointer);
        }
        
        public void SetItem(int index, string value)
        {
            CheckDisposed();
            NativeApi.ComboBox_SetItem_(NativePointer, index, value);
        }
        
        static GCHandle eventCallbackGCHandle;
        public static ComboBox? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.ComboBoxEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<ComboBox>(obj, p => new ComboBox(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.ComboBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.ComboBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.ComboBoxEvent.SelectedItemChanged:
                {
                    OnPlatformEventSelectedItemChanged(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.MeasureItem:
                {
                    OnPlatformEventMeasureItem(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.MeasureItemWidth:
                {
                    OnPlatformEventMeasureItemWidth(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.DrawItem:
                {
                    OnPlatformEventDrawItem(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.DrawItemBackground:
                {
                    OnPlatformEventDrawItemBackground(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.AfterShowPopup:
                {
                    OnPlatformEventAfterShowPopup(); return IntPtr.Zero;
                }
                case NativeApi.ComboBoxEvent.AfterDismissPopup:
                {
                    OnPlatformEventAfterDismissPopup(); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected ComboBoxEvent value: " + e);
            }
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr ComboBoxEventCallbackType(IntPtr obj, ComboBoxEvent e, IntPtr param);
            
            public enum ComboBoxEvent
            {
                SelectedItemChanged,
                MeasureItem,
                MeasureItemWidth,
                DrawItem,
                DrawItemBackground,
                AfterShowPopup,
                AfterDismissPopup,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEventCallback_(ComboBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr ComboBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetAllowMouseWheel_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetAllowMouseWheel_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string ComboBox_GetEmptyTextHint_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEmptyTextHint_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetHasBorder_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetHasBorder_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetItemsCount_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetIsEditable_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetIsEditable_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetSelectedIndex_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetSelectedIndex_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetTextSelectionStart_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetTextSelectionLength_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.PointI ComboBox_GetTextMargins_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetOwnerDrawStyle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetOwnerDrawStyle_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ComboBox_GetPopupWidget_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ComboBox_GetEventDc_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectI ComboBox_GetEventRect_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetEventItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetEventFlags_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_GetEventResultInt_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEventResultInt_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool ComboBox_GetEventCalled_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetEventCalled_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_DismissPopup_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_ShowPopup_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_DefaultOnMeasureItemWidth_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int ComboBox_DefaultOnMeasureItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_DefaultOnDrawBackground_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_DefaultOnDrawItem_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr ComboBox_CreateItemsInsertion_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_AddItemToInsertion_(IntPtr obj, System.IntPtr insertion, string item);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_CommitItemsInsertion_(IntPtr obj, System.IntPtr insertion, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_InsertItem_(IntPtr obj, int index, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_RemoveItemAt_(IntPtr obj, int index);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_ClearItems_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SelectTextRange_(IntPtr obj, int start, int length);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SelectAllText_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void ComboBox_SetItem_(IntPtr obj, int index, string value);
            
        }
    }
}
