// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>
using System.Runtime.InteropServices;

namespace Alternet.UI.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TextInputEventData
    {
        public char keyChar;
        public long timestamp;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class KeyEventData
    {
        public Key key;
        public long timestamp;
        public bool isRepeat;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MouseButtonEventData
    {
        public long timestamp;
        public MouseButton changedButton;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MouseEventData
    {
        public long timestamp;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MouseWheelEventData
    {
        public long timestamp;
        public int delta;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class TreeViewItemEventData
    {
        public System.IntPtr item;
    }
    
}