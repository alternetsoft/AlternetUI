#pragma warning disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class SizerFlags
    {
    }
}

/*  
 
    // construct the flags object initialized with the given proportion (0 by
    // default)
    wxSizerFlags(int proportion = 0) : m_proportion(proportion)
    {
        m_flags = 0;
        m_borderInPixels = 0;
    }

    // setters for all sizer flags, they all return the object itself so that
    // calls to them can be chained

    wxSizerFlags& Proportion(int proportion)
    {
        m_proportion = proportion;
        return *this;
    }

    wxSizerFlags& Expand()
    {
        m_flags |= wxEXPAND;
        return *this;
    }

    // notice that Align() replaces the current alignment flags, use specific
    // methods below such as Top(), Left() &c if you want to set just the
    // vertical or horizontal alignment
    wxSizerFlags& Align(int alignment) // combination of wxAlignment values
    {
        m_flags &= ~wxALIGN_MASK;
        m_flags |= alignment;

        return *this;
    }

    // this is just a shortcut for Align()
    wxSizerFlags& Centre() { return Align(wxALIGN_CENTRE); }
    wxSizerFlags& Center() { return Centre(); }

    // but all the remaining methods turn on the corresponding alignment flag
    // without affecting the existing ones
    wxSizerFlags& CentreVertical()
    {
        m_flags = (m_flags & ~wxALIGN_BOTTOM) | wxALIGN_CENTRE_VERTICAL;
        return *this;
    }

    wxSizerFlags& CenterVertical() { return CentreVertical(); }

    wxSizerFlags& CentreHorizontal()
    {
        m_flags = (m_flags & ~wxALIGN_RIGHT) | wxALIGN_CENTRE_HORIZONTAL;
        return *this;
    }

    wxSizerFlags& CenterHorizontal() { return CentreHorizontal(); }

    wxSizerFlags& Top()
    {
        m_flags &= ~(wxALIGN_BOTTOM | wxALIGN_CENTRE_VERTICAL);
        return *this;
    }

    wxSizerFlags& Left()
    {
        m_flags &= ~(wxALIGN_RIGHT | wxALIGN_CENTRE_HORIZONTAL);
        return *this;
    }

    wxSizerFlags& Right()
    {
        m_flags = (m_flags & ~wxALIGN_CENTRE_HORIZONTAL) | wxALIGN_RIGHT;
        return *this;
    }

    wxSizerFlags& Bottom()
    {
        m_flags = (m_flags & ~wxALIGN_CENTRE_VERTICAL) | wxALIGN_BOTTOM;
        return *this;
    }


    // default border size used by Border() below
    static int GetDefaultBorder()
    {
        return wxRound(GetDefaultBorderFractional());
    }

    static float GetDefaultBorderFractional()
    {
#if wxUSE_BORDER_BY_DEFAULT
    #ifdef __WXGTK20__
        // GNOME HIG says to use 6px as the base unit:
        // http://library.gnome.org/devel/hig-book/stable/design-window.html.en
        return 6;
    #elif defined(__WXMAC__)
        // Not sure if this is really the correct size for the border.
        return 5;
    #else
        // For the other platforms, we need to scale raw pixel values using the
        // current DPI, do it once (and cache the result) in another function.
        #define wxNEEDS_BORDER_IN_PX

        return DoGetDefaultBorderInPx();
    #endif
#else
        return 0;
#endif
    }


    wxSizerFlags& Border(int direction, int borderInPixels)
    {
        wxCHECK_MSG( !(direction & ~wxALL), *this,
                     wxS("direction must be a combination of wxDirection ")
                     wxS("enum values.") );

        m_flags &= ~wxALL;
        m_flags |= direction;

        m_borderInPixels = borderInPixels;

        return *this;
    }

    wxSizerFlags& Border(int direction = wxALL)
    {
#if wxUSE_BORDER_BY_DEFAULT
        return Border(direction, wxRound(GetDefaultBorderFractional()));
#else
        // no borders by default on limited size screen
        wxUnusedVar(direction);

        return *this;
#endif
    }

    wxSizerFlags& DoubleBorder(int direction = wxALL)
    {
#if wxUSE_BORDER_BY_DEFAULT
        return Border(direction, wxRound(2 * GetDefaultBorderFractional()));
#else
        wxUnusedVar(direction);

        return *this;
#endif
    }

    wxSizerFlags& TripleBorder(int direction = wxALL)
    {
#if wxUSE_BORDER_BY_DEFAULT
        return Border(direction, wxRound(3 * GetDefaultBorderFractional()));
#else
        wxUnusedVar(direction);

        return *this;
#endif
    }

    wxSizerFlags& HorzBorder()
    {
#if wxUSE_BORDER_BY_DEFAULT
        return Border(wxLEFT | wxRIGHT, wxRound(GetDefaultBorderFractional()));
#else
        return *this;
#endif
    }

    wxSizerFlags& DoubleHorzBorder()
    {
#if wxUSE_BORDER_BY_DEFAULT
        return Border(wxLEFT | wxRIGHT, wxRound(2 * GetDefaultBorderFractional()));
#else
        return *this;
#endif
    }

    // setters for the others flags
    wxSizerFlags& Shaped()
    {
        m_flags |= wxSHAPED;

        return *this;
    }

    wxSizerFlags& FixedMinSize()
    {
        m_flags |= wxFIXED_MINSIZE;

        return *this;
    }

    // makes the item ignore window's visibility status
    wxSizerFlags& ReserveSpaceEvenIfHidden()
    {
        m_flags |= wxRESERVE_SPACE_EVEN_IF_HIDDEN;
        return *this;
    }

    // accessors for wxSizer only
    int GetProportion() const { return m_proportion; }
    int GetFlags() const { return m_flags; }
    int GetBorderInPixels() const { return m_borderInPixels; }

    // Disablee sizer flags (in)consistency asserts.
    static void DisableConsistencyChecks();
 
 
 */