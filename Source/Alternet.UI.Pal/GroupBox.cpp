#include "GroupBox.h"

namespace Alternet::UI
{
    GroupBox::GroupBox() :
        _title(*this, nullopt, &Control::IsWxWindowCreated, &GroupBox::RetrieveTitle, &GroupBox::ApplyTitle)
    {
        GetDelayedValues().Add(&_title);
    }

    GroupBox::~GroupBox()
    {
    }

    optional<string> GroupBox::GetTitle()
    {
        return _title.Get();
    }

    void GroupBox::SetTitle(optional<string> value)
    {
        _title.Set(value);
    }

    wxWindow* GroupBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto staticBox = new wxStaticBox(parent, wxID_ANY, "");
        return staticBox;
    }

    Thickness GroupBox::GetIntrinsicPadding()
    {
        auto staticBox = GetStaticBox();
        
        // See wxWidgets source, sizer.cpp:2647, void wxStaticBoxSizer::RepositionChildren(const wxSize& minSize).
        int topBorder = 0, otherBorder = 0;
        staticBox->GetBordersForSizer(&topBorder, &otherBorder);

#if defined( __WXGTK20__ )
        return Thickness();
#elif defined(__WXOSX__) && wxOSX_USE_COCOA
        return Thickness(10, 10, 10, 10);
#else
        return toDip(Thickness(otherBorder, topBorder, otherBorder, otherBorder), staticBox);
#endif
    }

    wxStaticBox* GroupBox::GetStaticBox()
    {
        return dynamic_cast<wxStaticBox*>(GetWxWindow());
    }

    optional<string> GroupBox::RetrieveTitle()
    {
        auto value = wxStr(GetStaticBox()->GetLabel());

        if (value.empty())
            return nullopt;

        return value;
    }

    void GroupBox::ApplyTitle(const optional<string>& value)
    {
        GetStaticBox()->SetLabel(wxStr(value.value_or(u"")));
    }
}
