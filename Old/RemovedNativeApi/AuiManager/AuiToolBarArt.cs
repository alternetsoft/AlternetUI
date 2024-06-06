#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
    //https://docs.wxwidgets.org/3.2/classwx_aui_tool_bar_art.html
    public class AuiToolBarArt
    {
        public static void SetFlags(IntPtr handle, uint flags) { }
        public static uint GetFlags(IntPtr handle) => default;
        public static void SetTextOrientation(IntPtr handle, int orientation) { }
        public static int GetTextOrientation(IntPtr handle) => default;

        // Note that these functions work with the size in DIPs, not physical pixels.
        public static int GetElementSize(IntPtr handle, int elementId) => default;
        public static void SetElementSize(IntPtr handle, int elementId, int size) { }

        // Provide opportunity for subclasses to recalculate colors
        public static void UpdateColorsFromSystem(IntPtr handle) { }
    }
}

/*
    void SetFont(const wxFont& font) = 0;
    wxFont GetFont() = 0;
 */