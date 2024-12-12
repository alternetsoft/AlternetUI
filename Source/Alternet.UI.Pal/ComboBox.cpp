#include "ComboBox.h"

namespace Alternet::UI
{
    void ComboBox::SetPopupControl(VListBox* value)
    {
        _popupControl = value;
    }

    void ComboBox::OnPopup()
    {
        RaiseEvent(ComboBoxEvent::AfterShowPopup);
    }

    void ComboBox::DismissPopup()
    {
        GetComboBox()->Dismiss();
    }

    void ComboBox::ShowPopup()
    {
        GetComboBox()->Popup();
    }

    void ComboBox::OnDismiss()
    {
        RaiseEvent(ComboBoxEvent::AfterDismissPopup);
    }

    // Called immediately after the popup is shown
    void wxVListBoxComboPopup2::OnPopup()
    {
        wxVListBoxComboPopup::OnPopup();
        if (_owner == nullptr)
            return;
        auto comboBox = (ComboBox*)_owner;
        comboBox->OnPopup();
    }

    // Called when popup is dismissed
    void wxVListBoxComboPopup2::OnDismiss()
    {
        wxVListBoxComboPopup::OnDismiss();
        if (_owner == nullptr)
            return;
        auto comboBox = (ComboBox*)_owner;
        comboBox->OnDismiss();
    }

    void wxOwnerDrawnComboBox2::OnDrawBackground(wxDC& dc, const wxRect& rect, int item, int flags) const
    {
        ((ComboBox*)_palControl)->OnDrawBackground(dc, rect, item, flags);
    }

    // Callback for drawing. Font, background and text colour have been
    // prepared according to selection, focus and such.
    // item: item index to be drawn, may be wxNOT_FOUND when painting combo control itself
    //       and there is no valid selection
    // flags: wxODCB_PAINTING_CONTROL is set if painting to combo control instead of list
    void wxOwnerDrawnComboBox2::OnDrawItem(wxDC& dc, const wxRect& rect, int item, int flags) const
    {
        if (item == wxNOT_FOUND)
            return;

        ((ComboBox*)_palControl)->OnDrawItem(dc, rect, item, flags);
    }

    // Callback for item height, or -1 for default
    wxCoord wxOwnerDrawnComboBox2::OnMeasureItem(size_t item) const
    {
        return ((ComboBox*)_palControl)->OnMeasureItem(item);
    }

    // Callback for item width, or -1 for default/undetermined
    wxCoord wxOwnerDrawnComboBox2::OnMeasureItemWidth(size_t item) const
    {
        return ((ComboBox*)_palControl)->OnMeasureItemWidth(item);
    }

    ComboBox::ComboBox() :
        _text(*this, u"", &ComboBox::IsUsingComboBoxControl, &ComboBox::RetrieveText, 
            &ComboBox::ApplyText)
    {
        GetDelayedValues().Add({ &_text });
    }

    bool ComboBox::GetHasBorder()
    {
        return hasBorder;
    }

    void ComboBox::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    ComboBox::~ComboBox()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetComboBox();
            window->Unbind(wxEVT_COMBOBOX, 
                &ComboBox::OnSelectedItemChanged, this);
            window->Unbind(wxEVT_TEXT, &ComboBox::OnTextChanged, this);
        }
    }

    void ComboBox::SetItem(int index, const string& value)
    {
        GetItemContainer()->SetString(index, wxStr(value));
    }

    void ComboBox::InsertItem(int index, const string& value)
    {
        GetItemContainer()->Insert(wxStr(value), index);
    }

    void* ComboBox::CreateItemsInsertion()
    {
        return new wxArrayString();
    }

    void ComboBox::AddItemToInsertion(void* insertion, const string& item)
    {
        auto strings = (wxArrayString*)insertion;
        strings->Add(wxStr(item));
    }

    void ComboBox::CommitItemsInsertion(void* insertion, int index)
    {
        auto strings = (wxArrayString*)insertion;

        if(strings->GetCount()>0)
            GetItemContainer()->Insert(*strings, index);

        delete strings;
    }

    void ComboBox::RemoveItemAt(int index)
    {
        GetItemContainer()->Delete(index);
    }

    void ComboBox::ClearItems()
    {
        GetItemContainer()->Clear();
    }

    void* ComboBox::GetEventDc()
    {
        return eventDc;
    }

    RectI ComboBox::GetEventRect()
    {
        return eventRect;
    }

    int ComboBox::GetEventItem()
    {
        return eventItem;
    }

    int ComboBox::GetEventFlags()
    {
        return eventFlags;
    }

    int ComboBox::GetEventResultInt()
    {
        return eventResultInt;
    }

    bool ComboBox::GetEventCalled()
    {
        return eventCalled;
    }

    void ComboBox::SetEventResultInt(int value)
    {
        eventResultInt = value;
    }

    void ComboBox::SetEventCalled(bool value)
    {
        eventCalled = value;
    }

    constexpr auto OwnerDrawItemBackground = 1;
    constexpr auto OwnerDrawItem = 2;

    void ComboBox::OnDrawBackground(wxDC& dc, const wxRect& rect, int item, int flags)
    {
        auto drawBackground = (ownerDrawStyle & OwnerDrawItemBackground) != 0;

        if (drawBackground)
        {
            UpdateDc(dc);
            eventRect = rect;
            eventItem = item;
            eventFlags = flags;
            eventCalled = false;
            RaiseEvent(ComboBoxEvent::DrawItemBackground);
            ReleaseEventDc();

            if (eventCalled)
                return;
        }

        GetComboBox()->DefaultOnDrawBackground(dc, rect, item, flags);
    }

    int ComboBox::DefaultOnMeasureItemWidth()
    {
        return GetComboBox()->DefaultOnMeasureItemWidth(eventItem);
    }

    int ComboBox::DefaultOnMeasureItem()
    {
        return GetComboBox()->DefaultOnMeasureItem(eventItem);
    }

    void ComboBox::DefaultOnDrawBackground()
    {
        GetComboBox()->DefaultOnDrawBackground(*eventDc, eventRect, eventItem, eventFlags);
    }

    PointI ComboBox::GetTextMargins()
    {
        return GetComboBox()->GetMargins();
    }

    void ComboBox::DefaultOnDrawItem()
    {
        GetComboBox()->DefaultOnDrawItem(*eventDc, eventRect, eventItem, eventFlags);
    }

    void ComboBox::UpdateDc(wxDC& dc)
    {
        eventDc = std::addressof(dc);
    }

    void* ComboBox::GetPopupWidget()
    {
        auto result = GetComboBox()->GetPopupControl();
        return result;
    }

    void ComboBox::ReleaseEventDc()
    {
        eventDc = nullptr;
    }

    void ComboBox::OnDrawItem(wxDC& dc, const wxRect& rect, int item, int flags)
    {
        auto drawItem = (ownerDrawStyle & OwnerDrawItem) != 0;

        if (drawItem)
        {
            UpdateDc(dc);
            eventRect = rect;
            eventItem = item;
            eventFlags = flags;
            eventCalled = false;
            RaiseEvent(ComboBoxEvent::DrawItem);
            ReleaseEventDc();

            if (eventCalled)
                return;
        }

        GetComboBox()->DefaultOnDrawItem(dc, rect, item, flags);
    }

    // Callback for item height, or -1 for default
    wxCoord ComboBox::OnMeasureItem(size_t item)
    {
        eventItem = item;
        eventCalled = false;
        RaiseEvent(ComboBoxEvent::MeasureItem);
        if (eventCalled)
            return eventResultInt;
        return GetComboBox()->DefaultOnMeasureItem(item);
    }

    // Callback for item width, or -1 for default/undetermined
    wxCoord ComboBox::OnMeasureItemWidth(size_t item)
    {
        eventItem = item;
        eventCalled = false;
        RaiseEvent(ComboBoxEvent::MeasureItemWidth);
        if (eventCalled)
            return eventResultInt;
        return GetComboBox()->DefaultOnMeasureItemWidth(item);
    }

    wxWindow* ComboBox::CreateWxWindowUnparented()
    {
        auto value = new wxOwnerDrawnComboBox2();
        return value;
    }

    wxWindow* ComboBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetBorderStyle();

        if (!hasBorder)
            style = style | wxBORDER_NONE;

        auto comboStyle = _isEditable ? wxCB_DROPDOWN : wxCB_READONLY;
        style = style | comboStyle;
        auto value = new wxOwnerDrawnComboBox2(
            parent,
            wxID_ANY,
            "",
            wxDefaultPosition,
            wxDefaultSize,
            0,
            NULL,
            style);

        value->Bind(wxEVT_COMBOBOX, &ComboBox::OnSelectedItemChanged, this);
        return value;
    }

    void ComboBox::OnSelectedItemChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(ComboBoxEvent::SelectedItemChanged);
    }

    int ComboBox::GetTextSelectionStart()
    {
        if (!_isEditable)
            return 0;
        
        long from = 0, to = 0;
        GetComboBox()->GetSelection(& from, & to);
        return from;
    }

    int ComboBox::GetTextSelectionLength()
    {
        if (!_isEditable)
            return 0;

        long from = 0, to = 0;
        GetComboBox()->GetSelection(&from, &to);
        return to - from;
    }

    void ComboBox::SelectTextRange(int start, int length)
    {
        if (!_isEditable)
            return;

        GetComboBox()->SetSelection(start, start + length);
    }

    void ComboBox::SelectAllText()
    {
        GetComboBox()->SelectAll();
    }

    bool ComboBox::GetIsEditable()
    {
        return _isEditable;
    }

    void ComboBox::SetIsEditable(bool value)
    {
        if (_isEditable == value)
            return;

        auto oldSelectedIndex = GetSelectedIndex();
        
        _isEditable = value;
        RecreateWxWindowIfNeeded();

        SetSelectedIndex(oldSelectedIndex);
    }

    int ComboBox::GetSelectedIndex()
    {
        return GetComboBox()->GetSelection();
    }

    void ComboBox::SetSelectedIndex(int value)
    {
        if (value >= GetItemsCount())
            GetComboBox()->SetSelection(-1);
        else
            GetComboBox()->SetSelection(value);
/*
        // As events are not raised on programmatic selection change.
        RaiseEvent(ComboBoxEvent::SelectedItemChanged);
*/
    }

    string ComboBox::GetText()
    {
        return _text.Get();
    }

    void ComboBox::SetText(const string& value)
    {
        _text.Set(value);
    }

    void ComboBox::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
    }

    void ComboBox::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();
    }

    int ComboBox::GetItemsCount()
    {
        return GetItemContainer()->GetCount();
    }

    string ComboBox::RetrieveText()
    {
        auto result = GetComboBox()->GetValue();
        return wxStr(result);
    }

    void ComboBox::ApplyText(const string& value)
    {
        GetComboBox()->SetValue(wxStr(value));
    }

    wxControlWithItems* ComboBox::GetControlWithItems()
    {
        auto value = dynamic_cast<wxControlWithItems*>(GetWxWindow());
        return value;
    }

    string ComboBox::GetEmptyTextHint()
    {
        return wxStr(GetComboBox()->GetHint());
    }

    void ComboBox::SetEmptyTextHint(const string& value)
    {
        GetComboBox()->SetHint(wxStr(value));
    }

    int ComboBox::GetOwnerDrawStyle()
    {
        return ownerDrawStyle;
    }

    void ComboBox::SetOwnerDrawStyle(int value)
    {
        ownerDrawStyle = value;
    }

    wxVListBoxComboPopup2* ComboBox::GetPopup()
    {
        auto combo = GetComboBox();
        auto popup = combo->GetPopupControl();
        auto value = dynamic_cast<wxVListBoxComboPopup2*>(popup);
        return value;
    }

    wxOwnerDrawnComboBox2* ComboBox::GetComboBox()
    {
        auto value = dynamic_cast<wxOwnerDrawnComboBox2*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxItemContainer* ComboBox::GetItemContainer()
    {
        auto value = dynamic_cast<wxItemContainer*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    bool ComboBox::IsUsingComboBoxControl()
    {
        return dynamic_cast<wxOwnerDrawnComboBox*>(GetWxWindow()) != nullptr;
    }
}
