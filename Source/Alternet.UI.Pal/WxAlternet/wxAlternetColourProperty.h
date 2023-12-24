#pragma once

#include <wx/defs.h>
#include <wx/propgrid/props.h>
#include <wx/propgrid/advprops.h>

#include "wxAlternet.h"

namespace Alternet::UI
{
    class WXDLLIMPEXP_PROPGRID wxAlternetColourProperty : public wxSystemColourProperty
    {
        WX_PG_DECLARE_PROPERTY_CLASS(wxAlternetColourProperty)
    public:
        static wxPGChoices gs_wxColourProperty_choicesCache;
        static wxArrayString KnownColorLabels;
        static wxArrayInt KnownColorValues;
        static wxArrayULong KnownColorColors;

        wxAlternetColourProperty(const wxString& label = wxPG_LABEL,
            const wxString& name = wxPG_LABEL,
            const wxColour& value = *wxWHITE);
        virtual ~wxAlternetColourProperty();

        virtual wxString ValueToString(wxVariant& value, int argFlags = 0) const wxOVERRIDE;
        virtual wxColour GetColour(int index) const wxOVERRIDE;
        virtual wxSize OnMeasureImage(int item) const wxOVERRIDE;

    protected:
        virtual wxVariant DoTranslateVal(wxColourPropertyValue& v) const wxOVERRIDE;

    private:
        void Init(wxColour colour);
    };

}