#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;
using Alternet.Drawing;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_dock_art.html
	public class AuiDockArt
	{
        // Get the color of a certain setting.
        public static Color GetColor(IntPtr handle, int id) => default;

        // Get the value of a certain setting.
        public static int GetMetric(IntPtr handle, int id) => default;

        // Set a certain setting with the value color. 
        public static void SetColor(IntPtr handle, int id, Color color) { }

        public static void SetMetric(IntPtr handle, int id, int value) { }
    }
}


/*

// Get a font setting.
public static wxFont GetFont(IntPtr handle, int id) => default;

// Set a font setting. 
public static void SetFont(IntPtr handle, int id, wxFont font) {}
 
*/