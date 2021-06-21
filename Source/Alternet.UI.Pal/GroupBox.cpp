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
