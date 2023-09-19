#include "wxAlternetColourProperty.h"

namespace Alternet::UI
{
	static const char* const gs_cp_es_normcolour_labels[] = {
		wxTRANSLATE("Black"),
		wxTRANSLATE("Maroon"),
		wxTRANSLATE("Navy"),
		wxTRANSLATE("Purple"),
		wxTRANSLATE("Teal"),
		wxTRANSLATE("Gray"),
		wxTRANSLATE("Green"),
		wxTRANSLATE("Olive"),
		wxTRANSLATE("Brown"),
		wxTRANSLATE("Blue"),
		wxTRANSLATE("Fuchsia"),
		wxTRANSLATE("Red"),
		wxTRANSLATE("Orange"),
		wxTRANSLATE("Silver"),
		wxTRANSLATE("Lime"),
		wxTRANSLATE("Aqua"),
		wxTRANSLATE("Yellow"),
		wxTRANSLATE("White"),
		/* TRANSLATORS: Custom colour choice entry */ wxTRANSLATE("Custom"),
		NULL
	};

	static const long gs_cp_es_normcolour_values[] = {
		0,
		1,
		2,
		3,
		4,
		5,
		6,
		7,
		8,
		9,
		10,
		11,
		12,
		13,
		14,
		15,
		16,
		17,
		wxPG_COLOUR_CUSTOM
	};

	static const unsigned long gs_cp_es_normcolour_colours[] = {
		wxPG_COLOUR(0,0,0),
		wxPG_COLOUR(128,0,0),
		wxPG_COLOUR(0,0,128),
		wxPG_COLOUR(128,0,128),
		wxPG_COLOUR(0,128,128),
		wxPG_COLOUR(128,128,128),
		wxPG_COLOUR(0,128,0),
		wxPG_COLOUR(128,128,0),
		wxPG_COLOUR(166,124,81),
		wxPG_COLOUR(0,0,255),
		wxPG_COLOUR(255,0,255),
		wxPG_COLOUR(255,0,0),
		wxPG_COLOUR(247,148,28),
		wxPG_COLOUR(192,192,192),
		wxPG_COLOUR(0,255,0),
		wxPG_COLOUR(0,255,255),
		wxPG_COLOUR(255,255,0),
		wxPG_COLOUR(255,255,255),
		wxPG_COLOUR(0,0,0)
	};

	wxPG_IMPLEMENT_PROPERTY_CLASS(wxAlternetColourProperty, wxSystemColourProperty,
		TextCtrlAndButton)

	static wxPGChoices gs_wxColourProperty_choicesCache;

	wxAlternetColourProperty::wxAlternetColourProperty(const wxString& label,
		const wxString& name,
		const wxColour& value)
		: wxSystemColourProperty(label, name, gs_cp_es_normcolour_labels,
			gs_cp_es_normcolour_values,
			&gs_wxColourProperty_choicesCache, value)
	{
		wxASSERT_MSG(wxTheColourDatabase, wxS("No colour database"));
		if (wxTheColourDatabase)
		{
			// Extend colour database with PG-specific colours.
			const char* const* colourLabels = gs_cp_es_normcolour_labels;
			for (int i = 0; *colourLabels; colourLabels++, i++)
			{
				// Don't take into account user-defined custom colour.
				if (gs_cp_es_normcolour_values[i] != wxPG_COLOUR_CUSTOM)
				{
					wxColour clr = wxTheColourDatabase->Find(*colourLabels);
					// Use standard wx colour value if its label was found,
					// otherwise register custom PG colour.
					if (!clr.IsOk())
					{
						clr.Set(gs_cp_es_normcolour_colours[i]);
						wxTheColourDatabase->AddColour(*colourLabels, clr);
					}
				}
			}
		}

		Init(value);

		m_flags |= wxPG_PROP_TRANSLATE_CUSTOM;
	}

	wxAlternetColourProperty::~wxAlternetColourProperty()
	{
	}

	void wxAlternetColourProperty::Init(wxColour colour)
	{
		if (!colour.IsOk())
			colour = *wxWHITE;
		m_value = WXVARIANT(colour);
		int ind = ColToInd(colour);
		if (ind < 0)
			ind = m_choices.GetCount() - 1;
		SetIndex(ind);
	}

	wxString wxAlternetColourProperty::ValueToString(wxVariant& value,
		int argFlags) const
	{
		const wxPGEditor* editor = GetEditorClass();
		if (editor != wxPGEditor_Choice &&
			editor != wxPGEditor_ChoiceAndButton &&
			editor != wxPGEditor_ComboBox)
			argFlags |= wxPG_PROPERTY_SPECIFIC;

		return wxSystemColourProperty::ValueToString(value, argFlags);
	}

	wxColour wxAlternetColourProperty::GetColour(int index) const
	{
		return wxColour(gs_cp_es_normcolour_labels[m_choices.GetValue(index)]);
	}

	wxVariant wxAlternetColourProperty::DoTranslateVal(wxColourPropertyValue& v) const
	{
		return WXVARIANT(v.m_colour);
	}

}