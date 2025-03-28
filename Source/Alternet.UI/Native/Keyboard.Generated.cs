// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class Keyboard : NativeObject
    {
        static Keyboard()
        {
            SetEventCallback();
        }
        
        public Keyboard()
        {
            SetNativePointer(NativeApi.Keyboard_Create_());
        }
        
        public Keyboard(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static char InputChar
        {
            get
            {
                return NativeApi.Keyboard_GetInputChar_();
            }
            
        }
        
        public static byte InputEventCode
        {
            get
            {
                return NativeApi.Keyboard_GetInputEventCode_();
            }
            
        }
        
        public static bool InputHandled
        {
            get
            {
                return NativeApi.Keyboard_GetInputHandled_();
            }
            
            set
            {
                NativeApi.Keyboard_SetInputHandled_(value);
            }
        }
        
        public static Alternet.UI.Key InputKey
        {
            get
            {
                return NativeApi.Keyboard_GetInputKey_();
            }
            
        }
        
        public static bool InputIsRepeat
        {
            get
            {
                return NativeApi.Keyboard_GetInputIsRepeat_();
            }
            
        }
        
        public static Alternet.UI.KeyStates GetKeyState(Alternet.UI.Key key)
        {
            return NativeApi.Keyboard_GetKeyState_(key);
        }
        
        static GCHandle eventCallbackGCHandle;
        public static Keyboard? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.KeyboardEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<Keyboard>(obj, p => new Keyboard(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.Keyboard_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.KeyboardEvent e, IntPtr parameter)
        {
            KeyPress?.Invoke(); return IntPtr.Zero;
        }
        
        public Action? KeyPress;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr KeyboardEventCallbackType(IntPtr obj, KeyboardEvent e, IntPtr param);
            
            public enum KeyboardEvent
            {
                KeyPress,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Keyboard_SetEventCallback_(KeyboardEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr Keyboard_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern char Keyboard_GetInputChar_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern byte Keyboard_GetInputEventCode_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Keyboard_GetInputHandled_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void Keyboard_SetInputHandled_(bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.Key Keyboard_GetInputKey_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool Keyboard_GetInputIsRepeat_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.UI.KeyStates Keyboard_GetKeyState_(Alternet.UI.Key key);
            
        }
    }
}
