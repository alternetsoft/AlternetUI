#pragma once

#include <wx/defs.h>
#include <wx/propgrid/props.h>
#include <wx/propgrid/advprops.h>

#include "wxAlternet.h"

namespace Alternet::UI
{
    // Has dropdown list of wxWidgets system colours. Value used is
    // of wxColourPropertyValue type.
    class wxAlternetSystemColourProperty : public wxEnumProperty
    {
        WX_PG_DECLARE_PROPERTY_CLASS(wxSystemColourProperty)
    public:

        wxAlternetSystemColourProperty(const wxString& label = wxPG_LABEL,
            const wxString& name = wxPG_LABEL,
            const wxColourPropertyValue&
            value = wxColourPropertyValue());
        virtual ~wxAlternetSystemColourProperty();

        virtual void OnSetValue() wxOVERRIDE;
        virtual bool IntToValue(wxVariant& variant,
            int number,
            int argFlags = 0) const wxOVERRIDE;

        // Override in derived class to customize how colours are printed as
        // strings.
        virtual wxString ColourToString(const wxColour& col, int index,
            int argFlags = 0) const;

        // Returns index of entry that triggers colour picker dialog
        // (default is last).
        virtual int GetCustomColourIndex() const;

        virtual wxString ValueToString(wxVariant& value, int argFlags = 0) const wxOVERRIDE;
        virtual bool StringToValue(wxVariant& variant,
            const wxString& text,
            int argFlags = 0) const wxOVERRIDE;
        virtual bool OnEvent(wxPropertyGrid* propgrid,
            wxWindow* primary, wxEvent& event) wxOVERRIDE;
        virtual bool DoSetAttribute(const wxString& name, wxVariant& value) wxOVERRIDE;
        virtual wxSize OnMeasureImage(int item) const wxOVERRIDE;
        virtual void OnCustomPaint(wxDC& dc,
            const wxRect& rect, wxPGPaintData& paintdata) wxOVERRIDE;

        // Helper function to show the colour dialog
        bool QueryColourFromUser(wxVariant& variant) const;

        // Default is to use wxSystemSettings::GetColour(index). Override to use
        // custom colour tables etc.
        virtual wxColour GetColour(int index) const;

        wxColourPropertyValue GetVal(const wxVariant* pVariant = NULL) const;

    protected:

        // Special constructors to be used by derived classes.
        wxAlternetSystemColourProperty(const wxString& label, const wxString& name,
            const char* const* labels, const long* values, wxPGChoices* choicesCache,
            const wxColourPropertyValue& value);
        wxAlternetSystemColourProperty(const wxString& label, const wxString& name,
            const char* const* labels, const long* values, wxPGChoices* choicesCache,
            const wxColour& value);

        void Init(int type, const wxColour& colour);

        // Utility functions for internal use
        virtual wxVariant DoTranslateVal(wxColourPropertyValue& v) const;
        wxVariant TranslateVal(wxColourPropertyValue& v) const
        {
            return DoTranslateVal(v);
        }
        wxVariant TranslateVal(int type, const wxColour& colour) const
        {
            wxColourPropertyValue v(type, colour);
            return DoTranslateVal(v);
        }

        // Translates colour to a int value, return wxNOT_FOUND if no match.
        int ColToInd(const wxColour& colour) const;
    };

    class wxAlternetColourProperty : public wxAlternetSystemColourProperty
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