#include "wxAlternetColourProperty.h"
#include <wx/dc.h>
#include <wx/propgrid/propgrid.h>
#include <wx/colourdata.h>
#include <wx/colordlg.h>
#include <wx/odcombo.h>
#include <wx/wxchar.h>
#include <wx/variant.h>
#include <wx/propgrid/propgriddefs.h>
#include <wx/settings.h>

// Drawing ARGB on standard DC is supported by OSX and GTK3
#if defined(__WXOSX__) || defined(__WXGTK3__)
#define wxPG_DC_SUPPORTS_ALPHA 1
#else
#define wxPG_DC_SUPPORTS_ALPHA 0
#endif // __WXOSX__ || __WXGTK3__

#define wxPG_USE_GC_FOR_ALPHA  (wxUSE_GRAPHICS_CONTEXT && !wxPG_DC_SUPPORTS_ALPHA)

#if wxPG_USE_GC_FOR_ALPHA
#include "wx/dcgraph.h"
#ifndef WX_PRECOMP
#include "wx/dcclient.h" // for wxDynamicCast purposes
#include "wx/dcmemory.h" // for wxDynamicCast purposes
#endif // WX_PRECOMP
#if wxUSE_METAFILE
#include "wx/metafile.h"  // for wxDynamicCast purposes
#endif // wxUSE_METAFILE
#endif // wxPG_USE_GC_FOR_ALPHA

#define _PG_PROP_HIDE_CUSTOM_COLOUR        wxPGFlags::Reserved_2
#define _PG_PROP_COLOUR_HAS_ALPHA          wxPGFlags::Reserved_3


template<> inline wxVariant WXVARIANT(const wxColourPropertyValue& value)
{
    wxVariant variant;
    variant << value;
    return variant;
}

namespace Alternet::UI
{
    // wxEnumProperty based classes cannot use wxPG_PROP_CLASS_SPECIFIC_1
#define wxPG_PROP_HIDE_CUSTOM_COLOUR        wxPGPropertyFlags::Reserved_2
#define wxPG_PROP_COLOUR_HAS_ALPHA          wxPGPropertyFlags::Reserved_3

    const wxSize wxCOLOR_DEFAULT_IMAGE_SIZE = wxSize(-1, -1);

    wxArrayString wxAlternetColourProperty::KnownColorLabels = wxArrayString();
	wxArrayInt wxAlternetColourProperty::KnownColorValues = wxArrayInt();
	wxArrayULong wxAlternetColourProperty::KnownColorColors = wxArrayULong();

	wxPG_IMPLEMENT_PROPERTY_CLASS(wxAlternetColourProperty, wxAlternetSystemColourProperty,
		TextCtrlAndButton)
	
	wxPGChoices wxAlternetColourProperty::gs_wxColourProperty_choicesCache = wxPGChoices();

	wxAlternetColourProperty::wxAlternetColourProperty(const wxString& label,
		const wxString& name,
		const wxColour& value)
		: wxAlternetSystemColourProperty(label, name, nullptr, nullptr,
			&gs_wxColourProperty_choicesCache, value)
	{
		if (wxTheColourDatabase)
		{
			// Extend colour database with PG-specific colours.
			for (unsigned int i = 0; i < KnownColorLabels.Count(); i++)
			{
				// Don't take into account user-defined custom colour.
				if (KnownColorValues[i] != wxPG_COLOUR_CUSTOM)
				{
					wxColour clr = wxTheColourDatabase->Find(KnownColorLabels[i]);
					// Use standard wx colour value if its label was found,
					// otherwise register custom PG colour.
					if (!clr.IsOk())
					{
						clr.Set(KnownColorColors[i]);
						wxTheColourDatabase->AddColour(KnownColorLabels[i], clr);
					}
				}
			}
		}

		Init(value);

		m_flags |= wxPGFlags::Reserved_1 /*wxPG_PROP_TRANSLATE_CUSTOM*/;
	}

	wxAlternetColourProperty::~wxAlternetColourProperty()
	{
	}

	void wxAlternetColourProperty::Init(wxColour colour)
	{
        if (!colour.IsOk())
        {
            wxAlternetSystemColourProperty::Init(wxPG_COLOUR_UNSPECIFIED, colour);
            return;
        }

		m_value = WXVARIANT(colour);
		int ind = ColToInd(colour);
		if (ind < 0)
			ind = m_choices.GetCount() - 1;
		SetIndex(ind);
	}

	wxString wxAlternetColourProperty::ValueToString(wxVariant& value,
        wxPGPropValFormatFlags argFlags) const
	{
		const wxPGEditor* editor = GetEditorClass();
		if (editor != wxPGEditor_Choice &&
			editor != wxPGEditor_ChoiceAndButton &&
			editor != wxPGEditor_ComboBox)
			argFlags |= wxPGPropValFormatFlags::PropertySpecific;

		return wxAlternetSystemColourProperty::ValueToString(value, argFlags);
	}

	wxColour wxAlternetColourProperty::GetColour(int index) const
	{
		return wxColour(KnownColorLabels[m_choices.GetValue(index)]);
	}

	wxVariant wxAlternetColourProperty::DoTranslateVal(wxColourPropertyValue& v) const
	{
		return WXVARIANT(v.m_colour);
	}

	wxSize wxAlternetColourProperty::OnMeasureImage(int) const
	{
		return wxCOLOR_DEFAULT_IMAGE_SIZE;
	}

    static const char* const gs_cp_es_syscolour_labels[] = {
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("AppWorkspace"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ActiveBorder"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ActiveCaption"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ButtonFace"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ButtonHighlight"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ButtonShadow"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ButtonText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("CaptionText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ControlDark"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("ControlLight"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Desktop"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("GrayText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Highlight"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("HighlightText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("InactiveBorder"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("InactiveCaption"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("InactiveCaptionText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Menu"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Scrollbar"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Tooltip"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("TooltipText"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("Window"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("WindowFrame"),
        /* TRANSLATORS: Keyword of system colour */ wxTRANSLATE("WindowText"),
        /* TRANSLATORS: Custom colour choice entry */ wxTRANSLATE("Custom"),
        NULL
    };

    static const long gs_cp_es_syscolour_values[] = {
        wxSYS_COLOUR_APPWORKSPACE,
        wxSYS_COLOUR_ACTIVEBORDER,
        wxSYS_COLOUR_ACTIVECAPTION,
        wxSYS_COLOUR_BTNFACE,
        wxSYS_COLOUR_BTNHIGHLIGHT,
        wxSYS_COLOUR_BTNSHADOW,
        wxSYS_COLOUR_BTNTEXT ,
        wxSYS_COLOUR_CAPTIONTEXT,
        wxSYS_COLOUR_3DDKSHADOW,
        wxSYS_COLOUR_3DLIGHT,
        wxSYS_COLOUR_BACKGROUND,
        wxSYS_COLOUR_GRAYTEXT,
        wxSYS_COLOUR_HIGHLIGHT,
        wxSYS_COLOUR_HIGHLIGHTTEXT,
        wxSYS_COLOUR_INACTIVEBORDER,
        wxSYS_COLOUR_INACTIVECAPTION,
        wxSYS_COLOUR_INACTIVECAPTIONTEXT,
        wxSYS_COLOUR_MENU,
        wxSYS_COLOUR_SCROLLBAR,
        wxSYS_COLOUR_INFOBK,
        wxSYS_COLOUR_INFOTEXT,
        wxSYS_COLOUR_WINDOW,
        wxSYS_COLOUR_WINDOWFRAME,
        wxSYS_COLOUR_WINDOWTEXT,
        wxPG_COLOUR_CUSTOM
    };

    // IMPLEMENT_VARIANT_OBJECT_EXPORTED_SHALLOWCMP(wxColourPropertyValue, WXDLLIMPEXP_PROPGRID)

    // Class body is in advprops.h

    wxPG_IMPLEMENT_PROPERTY_CLASS(wxAlternetSystemColourProperty, wxEnumProperty, Choice)


    void wxAlternetSystemColourProperty::Init(int type, const wxColour& colour)
    {
        wxColourPropertyValue cpv;

        if (!colour.IsOk())
            cpv = wxColourPropertyValue(wxPG_COLOUR_UNSPECIFIED, wxColour());
        else
            cpv.Init(type, colour);

        // Color selection cannot be changed.
        m_flags |= wxPGFlags::Reserved_1 /*wxPG_PROP_STATIC_CHOICES*/;

        m_value = WXVARIANT(cpv);

        OnSetValue();
    }


    static wxPGChoices gs_wxAlternetSystemColourProperty_choicesCache;


    wxAlternetSystemColourProperty::wxAlternetSystemColourProperty(
        const wxString& label, const wxString& name,
        const wxColourPropertyValue& value)
        : wxEnumProperty(label,
            name,
            gs_cp_es_syscolour_labels,
            gs_cp_es_syscolour_values,
            &gs_wxAlternetSystemColourProperty_choicesCache)
    {
        Init(value.m_type, value.m_colour);
    }


    wxAlternetSystemColourProperty::wxAlternetSystemColourProperty(
        const wxString& label, const wxString& name,
        const char* const* labels, const long* values, wxPGChoices* choicesCache,
        const wxColourPropertyValue& value)
        : wxEnumProperty(label, name, labels, values, choicesCache)
    {
        Init(value.m_type, value.m_colour);
    }


    wxAlternetSystemColourProperty::wxAlternetSystemColourProperty(
        const wxString& label, const wxString& name,
        const char* const* labels, const long* values, wxPGChoices* choicesCache,
        const wxColour& value)
        : wxEnumProperty(label, name, labels, values, choicesCache)
    {
        Init(wxPG_COLOUR_CUSTOM, value);
    }


    wxAlternetSystemColourProperty::~wxAlternetSystemColourProperty() { }


    wxColourPropertyValue wxAlternetSystemColourProperty::GetVal(const wxVariant* pVariant) const
    {
        if (!pVariant)
            pVariant = &m_value;

        if (pVariant->IsNull())
            return wxColourPropertyValue(wxPG_COLOUR_UNSPECIFIED, wxColour());

        const wxString valType(pVariant->GetType());
        if (valType == wxS("wxColourPropertyValue"))
        {
            wxColourPropertyValue v;
            v << *pVariant;
            return v;
        }

        wxColour col;
        bool variantProcessed = true;

        if (valType == wxS("wxColour*"))
        {
            wxColour* pCol = wxStaticCast(pVariant->GetWxObjectPtr(), wxColour);
            col = *pCol;
        }
        else if (valType == wxS("wxColour"))
        {
            col << *pVariant;
        }
        else if (valType == wxArrayInt_VariantType)
        {
            // This code is mostly needed for wxPython bindings, which
            // may offer tuple of integers as colour value.
            wxArrayInt arr;
            arr << *pVariant;

            if (arr.size() >= 3)
            {
                int r, g, b;
                int a = 255;

                r = arr[0];
                g = arr[1];
                b = arr[2];
                if (arr.size() >= 4)
                    a = arr[3];

                col = wxColour(r, g, b, a);
            }
            else
            {
                variantProcessed = false;
            }
        }
        else
        {
            variantProcessed = false;
        }

        if (!variantProcessed)
            return wxColourPropertyValue(wxPG_COLOUR_UNSPECIFIED, wxColour());

        wxColourPropertyValue v2(wxPG_COLOUR_CUSTOM, col);

        int colInd = ColToInd(col);
        if (colInd != wxNOT_FOUND)
            v2.m_type = colInd;

        return v2;
    }

    wxVariant wxAlternetSystemColourProperty::DoTranslateVal(wxColourPropertyValue& v) const
    {
        return WXVARIANT(v);
    }

    int wxAlternetSystemColourProperty::ColToInd(const wxColour& colour) const
    {
        const unsigned int i_max = m_choices.GetCount();
        for (unsigned int i = 0; i < i_max; i++)
        {
            const int ind = m_choices[i].GetValue();
            // Skip custom colour
            if (ind == wxPG_COLOUR_CUSTOM)
                continue;

            if (colour == GetColour(ind))
            {
                /*wxLogDebug(wxS("%s(%s): Index %i for ( getcolour(%i,%i,%i), colour(%i,%i,%i))"),
                    GetClassName(),GetLabel(),
                    (int)i,(int)GetColour(ind).Red(),(int)GetColour(ind).Green(),(int)GetColour(ind).Blue(),
                    (int)colour.Red(),(int)colour.Green(),(int)colour.Blue());*/
                return ind;
            }
        }
        return wxNOT_FOUND;
    }

    void wxAlternetSystemColourProperty::OnSetValue()
    {
        // Convert from generic wxobject ptr to wxPGVariantDataColour
        if (m_value.IsType(wxS("wxColour*")))
        {
            wxColour* pCol = wxStaticCast(m_value.GetWxObjectPtr(), wxColour);
            m_value = WXVARIANT(*pCol);
        }

        wxColourPropertyValue val = GetVal(&m_value);

        if (val.m_type == wxPG_COLOUR_UNSPECIFIED)
        {
            m_value.MakeNull();
            return;
        }
        else
        {

            if (val.m_type < wxPG_COLOUR_WEB_BASE)
                val.m_colour = GetColour(val.m_type);

            m_value = TranslateVal(val);
        }

        int ind = wxNOT_FOUND;

        if (m_value.IsType(wxS("wxColourPropertyValue")))
        {
            wxColourPropertyValue cpv;
            cpv << m_value;
            wxColour col = cpv.m_colour;

            if (!col.IsOk())
            {
                SetValueToUnspecified();
                SetIndex(wxNOT_FOUND);
                return;
            }

            if (cpv.m_type < wxPG_COLOUR_WEB_BASE ||
                (((int)m_flags & (int)wxPGFlags::Reserved_2) /*wxPG_PROP_HIDE_CUSTOM_COLOUR*/))
            {
                ind = GetIndexForValue(cpv.m_type);
            }
            else
            {
                cpv.m_type = wxPG_COLOUR_CUSTOM;
                ind = GetCustomColourIndex();
            }
        }
        else
        {
            wxColour col;
            col << m_value;

            if (!col.IsOk())
            {
                SetValueToUnspecified();
                SetIndex(wxNOT_FOUND);
                return;
            }

            ind = ColToInd(col);

            if (ind == wxNOT_FOUND &&
                !(m_flags & _PG_PROP_HIDE_CUSTOM_COLOUR))
                ind = GetCustomColourIndex();
        }

        SetIndex(ind);
    }


    wxColour wxAlternetSystemColourProperty::GetColour(int index) const
    {
        return wxSystemSettings::GetColour((wxSystemColour)index);
    }

    wxString wxAlternetSystemColourProperty::ColourToString(const wxColour& col,
        int index,
        wxPGPropValFormatFlags argFlags) const
    {
        if (index == wxNOT_FOUND)
        {
            if (!col.IsOk())
                return wxEmptyString;

            if (((int)argFlags & (int)wxPGPropValFormatFlags::FullValue)
                || ((int)m_flags & (int)_PG_PROP_COLOUR_HAS_ALPHA))
            {
                return wxString::Format(wxS("(%i,%i,%i,%i)"),
                    (int)col.Red(),
                    (int)col.Green(),
                    (int)col.Blue(),
                    (int)col.Alpha());
            }
            else
            {
                return wxString::Format(wxS("(%i,%i,%i)"),
                    (int)col.Red(),
                    (int)col.Green(),
                    (int)col.Blue());
            }
        }
        else
        {
            return m_choices.GetLabel(index);
        }
    }

    wxString wxAlternetSystemColourProperty::ValueToString(wxVariant& value,
        wxPGPropValFormatFlags argFlags) const
    {
        wxColourPropertyValue val = GetVal(&value);

        int index;

        if ((int)argFlags & (int)wxPGPropValFormatFlags::ValueIsCurrent)
        {
            // GetIndex() only works reliably if wxPG_VALUE_IS_CURRENT flag is set,
            // but we should use it whenever possible.
            index = GetIndex();

            // If custom color was selected, use invalid index, so that
            // ColorToString() will return properly formatted color text.
            if (index == GetCustomColourIndex() &&
                !(m_flags & _PG_PROP_HIDE_CUSTOM_COLOUR))
                index = wxNOT_FOUND;
        }
        else
        {
            index = m_choices.Index(val.m_type);
        }

        return ColourToString(val.m_colour, index, argFlags);
    }


    wxSize wxAlternetSystemColourProperty::OnMeasureImage(int) const
    {
        return wxCOLOR_DEFAULT_IMAGE_SIZE;
    }


    int wxAlternetSystemColourProperty::GetCustomColourIndex() const
    {
        return m_choices.Index(wxPG_COLOUR_CUSTOM);
    }


    bool wxAlternetSystemColourProperty::QueryColourFromUser(wxVariant& variant) const
    {
        wxASSERT(!m_value.IsType(wxPG_VARIANT_TYPE_STRING));
        bool res = false;

        wxPropertyGrid* propgrid = GetGrid();
        wxASSERT(propgrid);

        if (propgrid == nullptr)
            return res;

        // Must only occur when user triggers event
        if (!propgrid->HasInternalFlag(wxPropertyGrid::wxPG_FL_IN_HANDLECUSTOMEDITOREVENT))
            return res;

        wxColourPropertyValue val = GetVal();

        val.m_type = wxPG_COLOUR_CUSTOM;

        wxColourData data;
        data.SetChooseFull(true);
        data.SetChooseAlpha(((int)m_flags & (int)_PG_PROP_COLOUR_HAS_ALPHA) != 0);
        data.SetColour(val.m_colour);
        for (int i = 0; i < wxColourData::NUM_CUSTOM; i++)
        {
            unsigned char n = i * (256 / wxColourData::NUM_CUSTOM);
            data.SetCustomColour(i, wxColour(n, n, n));
        }

        wxColourDialog dialog(propgrid, &data);
        if (dialog.ShowModal() == wxID_OK)
        {
            wxColourData retData = dialog.GetColourData();
            val.m_colour = retData.GetColour();

            variant = DoTranslateVal(val);

            SetValueInEvent(variant);

            res = true;
        }

        return res;
    }


    bool wxAlternetSystemColourProperty::IntToValue(wxVariant& variant, int number, int argFlags) const
    {
        int index = number;
        const int type = m_choices.GetValue(index);

        if (type == wxPG_COLOUR_CUSTOM)
        {
            if (!(argFlags & wxPGPropValFormatFlags::PropertySpecific))
                return QueryColourFromUser(variant);

            // Call from event handler.
            // User will be asked for custom color later on in OnEvent().
            wxColourPropertyValue val = GetVal();
            variant = DoTranslateVal(val);
        }
        else
        {
            variant = TranslateVal(type, GetColour(type));
        }

        return true;
    }

    // Need to do some extra event handling.
    bool wxAlternetSystemColourProperty::OnEvent(wxPropertyGrid* propgrid,
        wxWindow* WXUNUSED(primary),
        wxEvent& event)
    {
        bool askColour = false;

        if (propgrid->IsMainButtonEvent(event))
        {
            // We need to handle button click in case editor has been
            // switched to one that has wxButton as well.
            askColour = true;
        }
        else if (event.GetEventType() == wxEVT_COMBOBOX)
        {
            // Must override index detection since at this point GetIndex()
            // will return old value.
            wxOwnerDrawnComboBox* cb =
                static_cast<wxOwnerDrawnComboBox*>(propgrid->GetEditorControl());

            if (cb)
            {
                int index = cb->GetSelection();

                if (index == GetCustomColourIndex() &&
                    !(m_flags & _PG_PROP_HIDE_CUSTOM_COLOUR))
                    askColour = true;
            }
        }

        if (askColour && !propgrid->WasValueChangedInEvent())
        {
            wxVariant variant;
            if (QueryColourFromUser(variant))
                return true;
        }
        return false;
    }

    void wxAlternetSystemColourProperty::OnCustomPaint(wxDC& dc, const wxRect& rect,
        wxPGPaintData& paintdata)
    {
        auto b = rect.width;
        if (b > rect.height)
            b = rect.height;
        auto rect2 = wxRect(rect.x, rect.y, b, b);
        auto rect3 = rect2.CenterIn(rect);

        wxColour col;

        if (paintdata.m_choiceItem >= 0 &&
            paintdata.m_choiceItem < (int)m_choices.GetCount() &&
            (paintdata.m_choiceItem != GetCustomColourIndex() ||
                (int)m_flags & (int)_PG_PROP_HIDE_CUSTOM_COLOUR))
        {
            int colInd = m_choices[paintdata.m_choiceItem].GetValue();
            col = GetColour(colInd);
        }
        else if (!IsValueUnspecified())
        {
            col = GetVal().m_colour;
        }

        if (col.IsOk())
        {
#if wxPG_USE_GC_FOR_ALPHA
            wxGCDC* gdc = NULL;
            if (col.Alpha() != wxALPHA_OPAQUE)
            {
                if (wxPaintDC* paintdc = wxDynamicCast(&dc, wxPaintDC))
                {
                    gdc = new wxGCDC(*paintdc);
                }
                else if (wxMemoryDC* memdc = wxDynamicCast(&dc, wxMemoryDC))
                {
                    gdc = new wxGCDC(*memdc);
                }
#if wxUSE_METAFILE && defined(wxMETAFILE_IS_ENH)
                else if (wxMetafileDC* metadc = wxDynamicCast(&dc, wxMetafileDC))
                {
                    gdc = new wxGCDC(*metadc);
                }
#endif
                else
                {
                    wxFAIL_MSG(wxS("Unknown wxDC kind"));
                }
            }

            if (gdc)
            {
                gdc->SetBrush(col);
                gdc->DrawRectangle(rect3);
                delete gdc;
            }
            else
#endif // wxPG_USE_GC_FOR_ALPHA
            {
                dc.SetBrush(col);
                dc.DrawRectangle(rect3);
            }
        }
    }


    bool wxAlternetSystemColourProperty::StringToValue(
        wxVariant& value, const wxString& text, wxPGPropValFormatFlags argFlags) const
    {
        const int custIndex = GetCustomColourIndex();
        wxString custColName;
        if (custIndex != wxNOT_FOUND)
            custColName = m_choices.GetLabel(custIndex);

        wxString colStr(text);
        colStr.Trim(true);
        colStr.Trim(false);

        const bool isCustomColour = (colStr == custColName);

        wxColour customColour;
        bool conversionSuccess = false;

        if (!isCustomColour)
        {
            if (colStr.Find(wxS("(")) == 0)
            {
                // Eliminate whitespace
                colStr.Replace(wxS(" "), wxEmptyString);

                int commaCount = colStr.Freq(wxS(','));
                if (commaCount == 2)
                {
                    // Convert (R,G,B) to rgb(R,G,B)
                    colStr = wxS("rgb") + colStr;
                }
                else if (commaCount == 3)
                {
                    // We have int alpha, CSS format that wxColour takes as
                    // input processes float alpha. So, let's parse the colour
                    // ourselves instead of trying to convert it to a format
                    // that wxColour::FromString() understands.
                    int r = -1, g = -1, b = -1, a = -1;
                    wxSscanf(colStr, wxS("(%i,%i,%i,%i)"), &r, &g, &b, &a);
                    customColour.Set(r, g, b, a);
                    conversionSuccess = customColour.IsOk();
                }
            }

            if (!conversionSuccess)
                conversionSuccess = customColour.Set(colStr);
        }

        if (!conversionSuccess && m_choices.GetCount() &&
            !(m_flags & _PG_PROP_HIDE_CUSTOM_COLOUR) &&
            isCustomColour)
        {
            if (!(argFlags & wxPGPropValFormatFlags::EditableValue))
            {
                // This really should not occur...
                // wxASSERT(false);
                return false;
            }

            if (!QueryColourFromUser(value))
            {
                if (!((int)argFlags & (int)wxPGPropValFormatFlags::PropertySpecific))
                    return false;
                // If query for value comes from the event handler
                // use current pending value to be processed later on in OnEvent().
                SetValueInEvent(value);
            }
        }
        else
        {
            wxColourPropertyValue val;

            bool done = false;

            if (!conversionSuccess)
            {
                // Try predefined colour first
                int index;
                bool res = ValueFromString_(value, &index, colStr, (wxPGPropValFormatFlags)argFlags);
                if (res && index >= 0)
                {
                    val.m_type = index;
                    if (val.m_type < m_choices.GetCount())
                        val.m_type = m_choices[val.m_type].GetValue();

                    // Get proper colour for type.
                    val.m_colour = GetColour(val.m_type);

                    done = true;
                }
            }
            else
            {
                val.m_type = wxPG_COLOUR_CUSTOM;
                val.m_colour = customColour;
                done = true;
            }

            if (!done)
            {
                return false;
            }

            value = DoTranslateVal(val);
        }

        return true;
    }


    bool wxAlternetSystemColourProperty::DoSetAttribute(const wxString& name, wxVariant& value)
    {
        if (name == wxPG_COLOUR_ALLOW_CUSTOM)
        {
            bool allow = value.GetBool();

            if (allow && ((int)m_flags & (int)_PG_PROP_HIDE_CUSTOM_COLOUR))
            {
                // Show custom choice
                /* TRANSLATORS: Custom color choice entry */
                m_choices.Add(_("Custom"), wxPG_COLOUR_CUSTOM);
                m_flags &= ~(_PG_PROP_HIDE_CUSTOM_COLOUR);
            }
            else if (!allow && !(m_flags & _PG_PROP_HIDE_CUSTOM_COLOUR))
            {
                // Hide custom choice
                m_choices.RemoveAt(GetCustomColourIndex());
                m_flags |= _PG_PROP_HIDE_CUSTOM_COLOUR;
            }
            return true;
        }
        else if (name == wxPG_COLOUR_HAS_ALPHA)
        {
            ChangeFlag(_PG_PROP_COLOUR_HAS_ALPHA, value.GetBool());
            return true;
        }
        return wxEnumProperty::DoSetAttribute(name, value);
    }

}