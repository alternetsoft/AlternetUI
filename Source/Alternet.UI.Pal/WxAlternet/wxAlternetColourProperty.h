#pragma once
#include "wx/defs.h"
#include "wx/propgrid/props.h"
#include "wx/propgrid/advprops.h"

namespace Alternet::UI
{
    class WXDLLIMPEXP_PROPGRID wxAlternetColourProperty : public wxSystemColourProperty
    {
        WX_PG_DECLARE_PROPERTY_CLASS(wxAlternetColourProperty)
    public:
        wxAlternetColourProperty(const wxString& label = wxPG_LABEL,
            const wxString& name = wxPG_LABEL,
            const wxColour& value = *wxWHITE);
        virtual ~wxAlternetColourProperty();

        virtual wxString ValueToString(wxVariant& value, int argFlags = 0) const wxOVERRIDE;
        virtual wxColour GetColour(int index) const wxOVERRIDE;

    protected:
        virtual wxVariant DoTranslateVal(wxColourPropertyValue& v) const wxOVERRIDE;

    private:
        void Init(wxColour colour);
    };

}