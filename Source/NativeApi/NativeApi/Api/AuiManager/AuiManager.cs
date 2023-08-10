#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_manager.html
	public class AuiManager
	{
        public static void Delete(IntPtr attr) { }

        public static IntPtr CreateAuiManager() => throw new Exception();

    }
}