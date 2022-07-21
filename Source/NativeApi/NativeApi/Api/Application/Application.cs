using System;
using System.ComponentModel;
using ApiCommon;

namespace NativeApi.Api
{
    public class Application
    {
        public Application() => throw new Exception();

        public string Name { get; set; }

        public void Run(Window window) => throw new Exception();

        public event EventHandler Idle { add => throw new Exception(); remove => throw new Exception(); }

        public Keyboard Keyboard { get; }
        public Mouse Mouse { get; }

        public bool InUixmlPreviewerMode { get; set; }
    }
}