#pragma once

#include <wx/window.h>
#include <wx/renderer.h>
#include <wx/dcmirror.h>

class wxAlternetRendererNative : public wxRendererNative
{
public:
    inline static int MinSplitterSize = 4;
    inline static wxAlternetRendererNative* my = nullptr;

    wxRendererNative& _renderer;

    wxAlternetRendererNative(wxRendererNative& renderer)
        : wxRendererNative(),
        _renderer(renderer)
    {
    }

    static void UpdateRenderer()
    {
        if (my == nullptr)
        {
            my = new wxAlternetRendererNative(wxRendererNative::Get());
            wxRendererNative::Set(my);
        }
    }

    // get the splitter parameters: the x field of the returned point is the
    // sash width and the y field is the border width
    wxSplitterRenderParams GetSplitterParams(const wxWindow* win) override
    {
        auto params = _renderer.GetSplitterParams(win);

        auto width = params.widthSash;
        if (width < MinSplitterSize)
            width = MinSplitterSize;

        auto result = wxSplitterRenderParams(width, params.border, params.isHotSensitive);

        return result;
    }

    int  DrawHeaderButton(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0,
        wxHeaderSortIconType sortArrow = wxHDR_SORT_ICON_NONE,
        wxHeaderButtonParams* params = NULL) override
    {
        return _renderer.DrawHeaderButton(win,
            dc,
            rect,
            flags,
            sortArrow,
            params);
    }

    // Draw the contents of a header control button (label, sort arrows, etc.)
    // Normally only called by DrawHeaderButton.
    int  DrawHeaderButtonContents(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0,
        wxHeaderSortIconType sortArrow = wxHDR_SORT_ICON_NONE,
        wxHeaderButtonParams* params = NULL) override
    {
        return _renderer.DrawHeaderButtonContents(win,
            dc,
            rect,
            flags,
            sortArrow,
            params);
    }

    // Returns the default height of a header button, either a fixed platform
    // height if available, or a generic height based on the window's font.
    int GetHeaderButtonHeight(wxWindow* win) override
    {
        return _renderer.GetHeaderButtonHeight(win);
    }

    // Returns the margin on left and right sides of header button's label
    int GetHeaderButtonMargin(wxWindow* win) override
    {
        return _renderer.GetHeaderButtonMargin(win);
    }

    // draw the expanded/collapsed icon for a tree control item
    void DrawTreeItemButton(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawTreeItemButton(win,
            dc,
            rect,
            flags);
    }

    // draw the border for sash window: this border must be such that the sash
    // drawn by DrawSash() blends into it well
    void DrawSplitterBorder(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawSplitterBorder(win,
            dc,
            rect,
            flags);
    }

    // draw a (vertical) sash
    void DrawSplitterSash(wxWindow* win,
        wxDC& dcReal,
        const wxSize& sizeReal,
        wxCoord position,
        wxOrientation orient,
        int flags = 0) override
    {
        /*_renderer.DrawSplitterSash(win,
            dc,
            size,
            position,
            orient,
            flags);*/

            // to avoid duplicating the same code for horizontal and vertical sashes,
            // simply mirror the DC instead if needed (i.e. if horz splitter)
        wxMirrorDC dc(dcReal, orient != wxVERTICAL);
        wxSize size = dc.Reflect(sizeReal);

        const wxCoord h = size.y;
        wxCoord offset = 0;

        wxDCPenChanger setPen(dc, *wxTRANSPARENT_PEN);

        wxDCBrushChanger setBrush(dc, wxBrush(win->GetBackgroundColour()));
        dc.DrawRectangle(position, 0, size.x, h);
    }

    // draw a combobox dropdown button
    //
    // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
    void DrawComboBoxDropButton(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawComboBoxDropButton(win,
            dc,
            rect,
            flags);
    }

    // draw a dropdown arrow
    //
    // flags may use wxCONTROL_PRESSED and wxCONTROL_CURRENT
    void DrawDropArrow(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawDropArrow(win,
            dc,
            rect,
            flags);
    }

    // draw check button
    //
    // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
    void DrawCheckBox(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawCheckBox(win,
            dc,
            rect,
            flags);
    }

    // draw check mark
    //
    // flags may use wxCONTROL_DISABLED
    void DrawCheckMark(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawCheckMark(win,
            dc,
            rect,
            flags);
    }

    // Returns the default size of a check box.
    wxSize GetCheckBoxSize(wxWindow* win, int flags = 0) override
    {
        return _renderer.GetCheckBoxSize(win, flags);
    }

    // Returns the default size of a check mark.
    wxSize GetCheckMarkSize(wxWindow* win) override
    {
        return _renderer.GetCheckMarkSize(win);
    }

    // Returns the default size of a expander.
    wxSize GetExpanderSize(wxWindow* win) override
    {
        return _renderer.GetExpanderSize(win);
    }

    // draw blank button
    //
    // flags may use wxCONTROL_PRESSED, wxCONTROL_CURRENT and wxCONTROL_ISDEFAULT
    void DrawPushButton(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawPushButton(win,
            dc,
            rect,
            flags);
    }

    // draw collapse button
    //
    // flags may use wxCONTROL_CHECKED, wxCONTROL_UNDETERMINED and wxCONTROL_CURRENT
    void DrawCollapseButton(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawCollapseButton(win,
            dc,
            rect,
            flags);
    }

    // Returns the default size of a collapse button
    wxSize GetCollapseButtonSize(wxWindow* win, wxDC& dc) override
    {
        return _renderer.GetCollapseButtonSize(win,dc);
    }

    // draw rectangle indicating that an item in e.g. a list control
    // has been selected or focused
    //
    // flags may use
    // wxCONTROL_SELECTED (item is selected, e.g. draw background)
    // wxCONTROL_CURRENT (item is the current item, e.g. dotted border)
    // wxCONTROL_FOCUSED (the whole control has focus, e.g. blue background vs. grey otherwise)
    void DrawItemSelectionRect(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawItemSelectionRect(win,
            dc,
            rect,
            flags);
    }

    // draw the focus rectangle around the label contained in the given rect
    //
    // only wxCONTROL_SELECTED makes sense in flags here
    void DrawFocusRect(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawFocusRect(win,
            dc,
            rect,
            flags);
    }

    // Draw a native wxChoice
    void DrawChoice(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawChoice(win,
            dc,
            rect,
            flags);
    }

    // Draw a native wxComboBox
    void DrawComboBox(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override 
    {
        _renderer.DrawComboBox(win,
            dc,
            rect,
            flags);
    }

    // Draw a native wxTextCtrl frame
    void DrawTextCtrl(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override
    {
        _renderer.DrawTextCtrl(win,
            dc,
            rect,
            flags);
    }

    // Draw a native wxRadioButton bitmap
    void DrawRadioBitmap(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int flags = 0) override 
    {
        _renderer.DrawRadioBitmap(win,
            dc,
            rect,
            flags);
    }

#ifdef wxHAS_DRAW_TITLE_BAR_BITMAP
    // Draw one of the standard title bar buttons
    //
    // This is currently implemented only for MSW and OS X (for the close
    // button only) because there is no way to render standard title bar
    // buttons under the other platforms, the best can be done is to use normal
    // (only) images which wxArtProvider provides for wxART_HELP and
    // wxART_CLOSE (but not any other title bar buttons)
    //
    // NB: make sure PNG handler is enabled if using this function under OS X
    void DrawTitleBarBitmap(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        wxTitleBarButton button,
        int flags = 0) override
    {
        _renderer.DrawTitleBarBitmap(win,
            dc,
            rect,
            button,
            flags);
    }
#endif // wxHAS_DRAW_TITLE_BAR_BITMAP

    // Draw a gauge with native style like a wxGauge would display.
    //
    // wxCONTROL_SPECIAL flag must be used for drawing vertical gauges.
    void DrawGauge(wxWindow* win,
        wxDC& dc,
        const wxRect& rect,
        int value,
        int max,
        int flags = 0) override
    {
        _renderer.DrawGauge(win,
            dc,
            rect,
            value,
            max,
            flags);
    }

    // Draw text using the appropriate color for normal and selected states.
    void DrawItemText(wxWindow* win,
        wxDC& dc,
        const wxString& text,
        const wxRect& rect,
        int align = wxALIGN_LEFT | wxALIGN_TOP,
        int flags = 0,
        wxEllipsizeMode ellipsizeMode = wxELLIPSIZE_END) override 
    {
        _renderer.DrawItemText(win,
            dc,
            text,
            rect,
            align,
            flags,
            ellipsizeMode);
    }

    wxRendererVersion GetVersion() const override
    {
        return _renderer.GetVersion();
    }
};

