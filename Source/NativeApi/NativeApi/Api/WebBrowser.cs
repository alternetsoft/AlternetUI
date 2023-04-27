using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    public class WebBrowser : Control
    {
        public bool Editable { get; set; }

        public WebViewZoom Zoom { get; set; }

        public string GetCurrentTitle() => throw new Exception();

        public string GetCurrentURL() => throw new Exception();

        public void LoadURL(string url) => throw new Exception();

        public event EventHandler Navigating { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler Navigated { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler Loaded { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler Error { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler NewWindow { add => throw new Exception(); remove => throw new Exception(); }

        public event EventHandler TitleChanged { add => throw new Exception(); remove => throw new Exception(); }
    }
}
