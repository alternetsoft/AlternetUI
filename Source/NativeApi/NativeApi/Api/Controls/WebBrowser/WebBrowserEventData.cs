#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    public class WebBrowserEventData : NativeEventData
    {
        public NativeStringSpan Url;
        public NativeStringSpan Target;
        public int ActionFlags;
        public NativeStringSpan MessageHandler;
        public bool IsError;
        public NativeStringSpan Text;
        public int IntVal;
        public IntPtr ClientData;
    }
}
