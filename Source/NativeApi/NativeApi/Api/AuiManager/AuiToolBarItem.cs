#pragma warning disable
using ApiCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace NativeApi.Api
{
	//https://docs.wxwidgets.org/3.2/classwx_aui_tool_bar_item.html
	public class AuiToolBarItem
	{

	}
}

/*

    public static void SetWindow(IntPtr w) {}
    public static IntPtr GetWindow() => default;

    public static void SetId(int newId) {}
    public static int GetId() => default;

    public static void SetKind(int newKind){}
    public static int GetKind() => default;

    public static void SetState(int newState){}
    public static int GetState() => default;

    public static void SetSizerItem(IntPtr s){}
    public static IntPtr GetSizerItem() => default;

    public static void SetLabel(string s){}
    public static string GetLabel() => default;

    public static void SetShortHelp(string s)
    public static string GetShortHelp() => default;

    public static void SetLongHelp(string s)
    public static string GetLongHelp() => default;

    public static void SetMinSize(Size s){}
    public static Size GetMinSize() => default;

    public static void SetSpacerPixels(int s){}
    public static int GetSpacerPixels() => default;

    public static void SetProportion(int p){}
    public static int GetProportion() => default;

    public static void SetActive(bool b){}
    public static bool IsActive() => default;

    public static void SetHasDropDown(bool b){}
    public static bool HasDropDown() => default;

    public static void SetSticky(bool b){}
    public static bool IsSticky() => default;

    public static void SetUserData(long l){}
    public static long GetUserData() => default;

    public static void SetAlignment(int l){}
    public static int GetAlignment() => default;

    public static bool CanBeToggled() => default;



    void SetBitmap(const wxBitmapBundle& bmp)
    const wxBitmapBundle& GetBitmapBundle()
    wxBitmap GetBitmapFor(IntPtr wnd) => default;
    wxBitmap GetBitmap()

    void SetDisabledBitmap(const wxBitmapBundle& bmp) { m_disabledBitmap = bmp; }
    const wxBitmapBundle& GetDisabledBitmapBundle() const { return m_disabledBitmap; }
    wxBitmap GetDisabledBitmapFor(wxWindow* wnd) const { return m_disabledBitmap.GetBitmapFor(wnd); }
    wxBitmap GetDisabledBitmap() const { return GetBitmapFor(m_window); }

    // Return the bitmap for the current state, normal or disabled.
    wxBitmap GetCurrentBitmapFor(wxWindow* wnd) const;

    void SetHoverBitmap(const wxBitmapBundle& bmp)
    const wxBitmapBundle& GetHoverBitmapBundle()
    wxBitmap GetHoverBitmap()
 
 */