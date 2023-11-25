#pragma once

#include <wx/font.h>
#include <wx/tooltip.h>
#include <wx/private/richtooltip.h>
#include <wx/bmpbndl.h>
#include <wx/colour.h>
#include <wx/artprov.h>
#include <wx/generic/private/richtooltip.h>

class wxAlternetRichToolTipImpl : public wxRichToolTipImpl
{
public:
    wxAlternetRichToolTipImpl(const wxString& title, const wxString& message) :
        m_title(title),
        m_message(message)
    {
        m_tipKind = wxTipKind_Auto;

        // This is pretty arbitrary, we could follow MSW and use some multiple
        // of double-click time here.
        m_timeout = 5000;
        m_delay = 0;
    }

    void SetForegroundColour(const wxColour& col);
    void SetTitleForegroundColour(const wxColour& col);

    virtual void SetBackgroundColour(const wxColour& col,
        const wxColour& colEnd) wxOVERRIDE;
    virtual void SetCustomIcon(const wxBitmapBundle& icon) wxOVERRIDE;
    virtual void SetStandardIcon(int icon) wxOVERRIDE;
    virtual void SetTimeout(unsigned milliseconds,
        unsigned millisecondsDelay = 0) wxOVERRIDE;
    virtual void SetTipKind(wxTipKind tipKind) wxOVERRIDE;
    virtual void SetTitleFont(const wxFont& font) wxOVERRIDE;

    virtual void ShowFor(wxWindow* win, const wxRect* rect = NULL) wxOVERRIDE;

protected:
    wxString m_title,
        m_message;

private:
    wxBitmapBundle m_icon;

    wxColour m_colStart,m_colEnd, m_fgColor, m_titlefgColor;

    unsigned m_timeout,
        m_delay;

    wxTipKind m_tipKind;

    wxFont m_titleFont;
};

// =====================================

class wxRichToolTip2
{
public:
#ifdef __WXGTK__
	inline static bool AlternetRichTooltip = true;
#endif

#ifdef __WXMSW__
	inline static bool AlternetRichTooltip = true;
#endif

#ifdef __WXOSX__
	inline static bool AlternetRichTooltip = true;
#endif

	wxAlternetRichToolTipImpl* GetAlternetImpl()
	{
		return dynamic_cast<wxAlternetRichToolTipImpl*>(m_impl);
	}

	wxRichToolTip2(const wxString& title, const wxString& message)
	{
		// m_impl = new wxRichToolTipGenericImpl(title, message);
		if (AlternetRichTooltip)
			m_impl = new wxAlternetRichToolTipImpl(title, message);
		else
			m_impl = wxRichToolTipImpl::Create(title, message);
	}

	void SetForegroundColour(const wxColour& col)
	{
		auto impl = GetAlternetImpl();
		if (impl != nullptr)
			impl->SetForegroundColour(col);
	}

	void SetTitleForegroundColour(const wxColour& col)
	{
		auto impl = GetAlternetImpl();
		if (impl != nullptr)
			impl->SetTitleForegroundColour(col);
	}

	void SetBackgroundColour(const wxColour& col, const wxColour& colEnd = wxColour())
	{
		m_impl->SetBackgroundColour(col, colEnd);
	}

	void SetIcon(int icon)
	{
		m_impl->SetStandardIcon(icon);
	}

	void SetIcon(const wxBitmapBundle& icon)
	{
		m_impl->SetCustomIcon(icon);
	}

	void SetTimeout(unsigned milliseconds,
		unsigned millisecondsDelay = 0)
	{
		m_impl->SetTimeout(milliseconds, millisecondsDelay);
	}

	void SetTipKind(wxTipKind tipKind)
	{
		m_impl->SetTipKind(tipKind);
	}

	void SetTitleFont(const wxFont& font)
	{
		m_impl->SetTitleFont(font);
	}

	void ShowFor(wxWindow* win, const wxRect* rect = NULL)
	{
		wxCHECK_RET(win, wxS("Must have a valid window"));

		m_impl->ShowFor(win, rect);
	}

	~wxRichToolTip2()
	{
		delete m_impl;
	}

private:
	wxRichToolTipImpl* m_impl;

	wxDECLARE_NO_COPY_CLASS(wxRichToolTip2);
};


// =====================================


