using System;
using ApiCommon;

namespace NativeApi.Api
{
    public class Window : Control
    {
        public string Title { get => throw new Exception(); set => throw new Exception(); }

        public WindowStartPosition WindowStartPosition { get => throw new Exception(); set => throw new Exception(); }

        [NativeEvent(cancellable: true)]
        public event EventHandler? Closing { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler SizeChanged { add => throw new Exception(); remove => throw new Exception(); }
    }
}