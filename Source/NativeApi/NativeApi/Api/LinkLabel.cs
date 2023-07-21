using ApiCommon;
using System;

namespace NativeApi.Api
{
    public class LinkLabel : Control
    {
        public string Text 
        { 
            get => throw new Exception(); 
            set => throw new Exception(); 
        }

        public string Url 
        { 
            get => throw new Exception(); 
            set => throw new Exception(); 
        }

        [NativeEvent(cancellable: true)]
        public event EventHandler? HyperlinkClick { 
            add => throw new Exception(); remove => throw new Exception(); }
    }
}