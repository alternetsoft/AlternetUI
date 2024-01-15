#pragma warning disable
using System;
using Alternet.Drawing;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : Control
    {
        public static void SetParkingWindowFont(Font? font) { }

        [NativeEvent(cancellable: true)]
        public event EventHandler? Closing;

        public event EventHandler StateChanged;
        public event EventHandler SizeChanged;
        public event NativeEventHandler<CommandEventData>? InputBindingCommandExecuted;
        public event EventHandler LocationChanged;

        public string Title { get; set; }
        public WindowStartLocation WindowStartLocation { get; set; }
        public bool ShowInTaskbar { get; set; }
        public bool MinimizeEnabled { get; set; }
        public bool MaximizeEnabled { get; set; }
        public bool CloseEnabled { get; set; }
        public bool AlwaysOnTop { get; set; }
        public bool IsToolWindow { get; set; }
        public bool Resizable { get; set; }
        public bool HasBorder { get; set; }
        public bool HasTitleBar { get; set; }
        public bool HasSystemMenu { get; set; }
        public bool IsPopupWindow { get; set; }
        public ModalResult ModalResult { get; set; }
        public bool Modal => default;

        public void ShowModal() { }
        public void Close() { }

        public void Activate() { }
        public static Window ActiveWindow { get; }
        public Window[] OwnedWindows { get; }
        public WindowState State { get; set; }
        public IconSet? Icon { get; set; }
        public MainMenu? Menu { get; set; }
        public Toolbar? Toolbar { get; set; }
        public IntPtr WxStatusBar { get; set; }

        public void AddInputBinding(string managedCommandId, Key key, ModifierKeys modifiers) { }
        public void RemoveInputBinding(string managedCommandId) { }

    }
}