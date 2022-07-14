using System;
/*
 We are keeping copies of core events here, so they can be used 
 without referencing Avalonia itself, e. g. from projects that
 are using WPF, GTK#, etc
 */
namespace Alternet.UI.Integration.Remoting
{
    public abstract class InputEventMessageBase
    {
        public ModifierKeys[] Modifiers { get; set; }
    }

    public abstract class PointerEventMessageBase : InputEventMessageBase
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    [AlternetUIRemoteMessageGuid("EBEB9CD7-3B43-4616-95BC-0EC4A037B73F")]
    public class PointerMovedEventMessage : PointerEventMessageBase
    {
        
    }

    [AlternetUIRemoteMessageGuid("0F491075-E417-4AF4-84A0-68A48C01AC9E")]
    public class PointerPressedEventMessage : PointerEventMessageBase
    {
        public MouseButton Button { get; set; }
    }
    
    [AlternetUIRemoteMessageGuid("0BBC00B0-EF6E-41F5-A5E3-66A8C6EBFD9D")]
    public class PointerReleasedEventMessage : PointerEventMessageBase
    {
        public MouseButton Button { get; set; }
    }

    [AlternetUIRemoteMessageGuid("5D05CF63-A444-4850-97CC-48DF8AE4940F")]
    public class ScrollEventMessage : PointerEventMessageBase
    {
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }
    }

    [AlternetUIRemoteMessageGuid("7F0E860A-11FA-4FCF-AB77-C25A6110A1AD")]
    public class KeyEventMessage : InputEventMessageBase
    {
        public bool IsDown { get; set; }
        public Key Key { get; set; }
    }

    [AlternetUIRemoteMessageGuid("535982A1-8A9F-45AF-813D-138EC79AF022")]
    public class TextInputEventMessage : InputEventMessageBase
    {
        public string Text { get; set; }
    }

}
