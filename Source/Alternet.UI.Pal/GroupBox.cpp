#include "GroupBox.h"

namespace Alternet::UI
{
    GroupBox::GroupBox() :
        _title(*this, u"", &Control::IsWxWindowCreated,
            &GroupBox::RetrieveTitle, &GroupBox::ApplyTitle)
    {
        GetDelayedValues().Add(&_title);
    }

    GroupBox::~GroupBox()
    {
    }

    string GroupBox::GetText()
    {
        return _title.Get();
    }

    void GroupBox::SetText(const string& value)
    {
        _title.Set(value);
    }

    class wxStaticBox2 : public wxStaticBox, public wxWidgetExtender
    {
    public:
        wxStaticBox2(){}
        wxStaticBox2(wxWindow* parent, wxWindowID id,
            const wxString& label,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxString& name = wxASCII_STR(wxStaticBoxNameStr))            
        {
            Create(parent, id, label, pos, size, style, name);
        }
    };

    wxWindow* GroupBox::CreateWxWindowUnparented()
    {
        return new wxStaticBox2();
    }

    wxWindow* GroupBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto staticBox = new wxStaticBox2(parent, wxID_ANY,
            "",
            wxDefaultPosition,
            wxDefaultSize,
            0,
            wxASCII_STR(wxStaticBoxNameStr));

        return staticBox;
    }

    SizeD GroupBox::GetAutoPaddingRightBottom()
    {
        auto padding = GetIntrinsicPadding(/*preferredSize:*/ false);

        return SizeD(padding.Right, padding.Bottom);
    }

    SizeD GroupBox::GetAutoPaddingLeftTop()
    {
        auto padding = GetIntrinsicPadding(/*preferredSize:*/ false);

        return SizeD(padding.Left, padding.Top);
    }

    int GroupBox::GetTopBorderForSizer()
    {
        auto staticBox = GetStaticBox();
        int topBorder = 0, otherBorder = 0;
        staticBox->GetBordersForSizer(&topBorder, &otherBorder);
        return topBorder;
    }
    
    int GroupBox::GetOtherBorderForSizer()
    {
        auto staticBox = GetStaticBox();
        int topBorder = 0, otherBorder = 0;
        staticBox->GetBordersForSizer(&topBorder, &otherBorder);
        return otherBorder;
    }

    Thickness GroupBox::GetIntrinsicPadding(bool preferredSize)
    {
        auto staticBox = GetStaticBox();

        // See wxWidgets source, sizer.cpp:2647, void wxStaticBoxSizer::RepositionChildren(const wxSize& minSize).
        int topBorder = 0, otherBorder = 0;
        staticBox->GetBordersForSizer(&topBorder, &otherBorder);

#if defined( __WXGTK20__ )
        double border = 3;
        if (preferredSize)
            return Thickness(border, topBorder, otherBorder + border, otherBorder + border);
        return Thickness(border, border, border, topBorder + border);
#elif defined(__WXOSX__) && wxOSX_USE_COCOA
        double border = 3;
        double rightBorder = 7;
        return Thickness(border, border + (preferredSize ? topBorder : 0), preferredSize ? rightBorder : border, border);
#else
        return toDip(Thickness(otherBorder, topBorder, otherBorder, otherBorder), staticBox);
#endif
    }

    wxStaticBox* GroupBox::GetStaticBox()
    {
        return dynamic_cast<wxStaticBox*>(GetWxWindow());
    }

    string GroupBox::RetrieveTitle()
    {
        auto value = wxStr(GetStaticBox()->GetLabel());

        if (value.empty())
            return u"";

        return value;
    }

    void GroupBox::ApplyTitle(const string& value)
    {
        GetStaticBox()->SetLabel(wxStr(value));
    }
}
