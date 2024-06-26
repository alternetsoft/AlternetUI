﻿#pragma warning disable
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
        public string Url;
        public string Target;
        public int ActionFlags;
        public string MessageHandler;
        public bool IsError;
        public string Text;
        public int IntVal;
        public IntPtr ClientData;
    }
}
