using System;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : Control
    {
        public string Title { get => throw new Exception(); set => throw new Exception(); }

        public WindowStartLocation WindowStartLocation { get => throw new Exception(); set => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? Closing { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler SizeChanged { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler Activated { add => throw new Exception(); remove => throw new Exception(); }
        public event EventHandler Deactivated { add => throw new Exception(); remove => throw new Exception(); }

        public bool ShowInTaskbar { get => throw new Exception(); set => throw new Exception(); }

        public bool MinimizeEnabled { get => throw new Exception(); set => throw new Exception(); }
        public bool MaximizeEnabled { get => throw new Exception(); set => throw new Exception(); }
        public bool CloseEnabled { get => throw new Exception(); set => throw new Exception(); }

        public bool AlwaysOnTop { get => throw new Exception(); set => throw new Exception(); }
        public bool IsToolWindow { get => throw new Exception(); set => throw new Exception(); }
        public bool Resizable { get => throw new Exception(); set => throw new Exception(); }
        public bool HasBorder { get => throw new Exception(); set => throw new Exception(); }
        public bool HasTitleBar { get => throw new Exception(); set => throw new Exception(); }
        
        public ModalResult ModalResult { get => throw new Exception(); set => throw new Exception(); }

        public void ShowModal() => throw new Exception();

        public bool Modal => throw new Exception();

        public void Close() => throw new Exception();

        public bool IsActive { get => throw new Exception(); }
        public void Activate() => throw new Exception();
        public static Window ActiveWindow { get => throw new Exception(); }

        public Window[] OwnedWindows { get => throw new Exception(); }

        public WindowState State { get => throw new Exception(); set => throw new Exception(); }

        public event EventHandler StateChanged { add => throw new Exception(); remove => throw new Exception(); }

        public ImageSet? Icon { get => throw new Exception(); set => throw new Exception(); }
    }
}