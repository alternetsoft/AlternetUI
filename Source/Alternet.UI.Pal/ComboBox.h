#pragma once
#include "Common.h"
#include "Control.h"
#include "VListBox.h"

#include <wx/odcombo.h>

namespace Alternet::UI
{
    class wxVListBoxComboPopup2 : public wxVListBoxComboPopup, public wxWidgetExtender
    {
    private:
        Control* _owner;
    public:
        wxVListBoxComboPopup2(Control* owner)
        {
            _owner = owner;
        }

        virtual bool Create(wxWindow* parent) wxOVERRIDE
        {
            auto result = wxVListBoxComboPopup::Create(parent);
            if (result)
                SetDoubleBuffered(true);
            return result;
        }

        // Called immediately after the popup is shown
        void OnPopup() override;

        // Called when popup is dismissed
        void OnDismiss() override;

        void ItemWidthChanged(unsigned int item)
        {
            wxVListBoxComboPopup::ItemWidthChanged(item);
        }
    };

    class wxOwnerDrawnComboBox2 : public wxOwnerDrawnComboBox, public wxWidgetExtender
    {
    public:
        wxOwnerDrawnComboBox2(){}

        wxOwnerDrawnComboBox2(wxWindow* parent,
            wxWindowID id,
            const wxString& value,
            const wxPoint& pos,
            const wxSize& size,
            int n,
            const wxString choices[],
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxComboBoxNameStr))
            : wxOwnerDrawnComboBox(parent, id, value, pos, size, n, choices,
                style, validator, name)
        {
        }

        virtual void DoSetPopupControl(wxComboPopup* popup) wxOVERRIDE
        {
            wxOwnerDrawnComboBox::DoSetPopupControl(new wxVListBoxComboPopup2(_palControl));
        }

        virtual unsigned int GetCount() const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::GetCount();
        }

        virtual wxString GetString(unsigned int n) const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::GetString(n);
        }

        virtual void SetString(unsigned int n, const wxString& s) wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::SetString(n, s);
        }

        virtual int FindString(const wxString& s, bool bCase = false) const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::FindString(s, bCase);
        }

        virtual void Select(int n) wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::Select(n);
        }

        virtual int GetSelection() const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::GetSelection();
        }

        // Override these just to maintain consistency with virtual methods
        // between classes.
        virtual void Clear() wxOVERRIDE
        {
            wxOwnerDrawnComboBox::Clear();
        }

        virtual void GetSelection(long* from, long* to) const wxOVERRIDE
        {
            wxOwnerDrawnComboBox::GetSelection(from, to);
        }

        virtual void SetSelection(int n) wxOVERRIDE
        {
            wxOwnerDrawnComboBox::SetSelection(n);
        }

        // Prevent a method from being hidden
        virtual void SetSelection(long from, long to) wxOVERRIDE
        {
            wxOwnerDrawnComboBox::SetSelection(from, to);
        }

        // Return the widest item width (recalculating it if necessary)
        virtual int GetWidestItemWidth() wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::GetWidestItemWidth();
        }

        // Return the index of the widest item (recalculating it if necessary)
        virtual int GetWidestItem() wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::GetWidestItem();
        }

        virtual bool IsSorted() const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::IsSorted();
        }

        // Callback for drawing. Font, background and text colour have been
        // prepared according to selection, focus and such.
        // item: item index to be drawn, may be wxNOT_FOUND when painting combo control itself
        //       and there is no valid selection
        // flags: wxODCB_PAINTING_CONTROL is set if painting to combo control instead of list
        virtual void OnDrawItem(wxDC& dc, const wxRect& rect, int item, int flags) const wxOVERRIDE;

        void DefaultOnDrawItem(wxDC& dc, const wxRect& rect, int item, int flags) const
        {
            wxOwnerDrawnComboBox::OnDrawItem(dc, rect, item, flags);
        }

        // Callback for item height, or -1 for default
        virtual wxCoord OnMeasureItem(size_t item) const wxOVERRIDE;

        // Callback for item width, or -1 for default/undetermined
        virtual wxCoord OnMeasureItemWidth(size_t item) const wxOVERRIDE;

        // override base implementation so we can return the size for the
        // largest item
        virtual wxSize DoGetBestSize() const wxOVERRIDE
        {
            return wxOwnerDrawnComboBox::DoGetBestSize();
        }

        // Callback for background drawing. Flags are same as with
        // OnDrawItem.
        virtual void OnDrawBackground(wxDC& dc, const wxRect& rect, int item, int flags) const wxOVERRIDE;

        void DefaultOnDrawBackground(wxDC& dc, const wxRect& rect, int item, int flags) const
        {
            wxOwnerDrawnComboBox::OnDrawBackground(dc, rect, item, flags);
        }

        // Callback for item height, or -1 for default
        wxCoord DefaultOnMeasureItem(size_t item) const
        {
            return wxOwnerDrawnComboBox::OnMeasureItem(item);
        }

        // Callback for item width, or -1 for default/undetermined
        wxCoord DefaultOnMeasureItemWidth(size_t item) const
        {
            return wxOwnerDrawnComboBox::OnMeasureItemWidth(item);
        }
    };

    class ComboBox : public Control
    {
#include "Api/ComboBox.inc"
    public:
        string GetText() override;
        void SetText(const string& value) override;

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnSelectedItemChanged(wxCommandEvent& event);

        /* Fixed for MACOS ControlsSample[2411:15866] This application is trying
        to draw a very large combo box, 30 points tall.Vertically resizable combo
        boxes are not supported, but it happens that 10.4 and previous drew
        something that looked kind of sort of okay.The art in 10.5 does
        not break up in a way that supports that drawing.This application should be
        revised to stop using large combo boxes.This warning will appear once per app launch.*/
        void ApplyMinimumSize(const Size& value) override {}
        void ApplyMaximumSize(const Size& value) override {}

        void OnDrawItem(wxDC& dc, const wxRect& rect, int item, int flags);
        wxCoord OnMeasureItem(size_t item);
        wxCoord OnMeasureItemWidth(size_t item);
        void OnDrawBackground(wxDC& dc, const wxRect& rect, int item, int flags);

        void OnPopup();
        void OnDismiss();

        virtual Size GetPreferredSize(const Size& availableSize) override
        {
            return Control::GetPreferredSize(availableSize);
        }

        virtual void InvalidateBestSize() override
        {
            Control::InvalidateBestSize();
        }

    protected:
        void OnWxWindowCreated() override;
        void OnBeforeDestroyWxWindow() override;

    private:
        VListBox* _popupControl = nullptr;
        
        wxDC* eventDc = nullptr;
        RectI eventRect;
        int eventItem = -1;
        int eventFlags = 0;
        int eventResultInt = 0;
        bool eventCalled = false;
        int ownerDrawStyle = 0;

        void UpdateDc(wxDC& dc);
        void ReleaseEventDc();

        bool IsUsingComboBoxControl();
        bool hasBorder = true;

        DelayedValue<ComboBox, string> _text;

        bool _isEditable = true;

        string RetrieveText();
        void ApplyText(const string& value);

        wxVListBoxComboPopup2* GetPopup();
        wxOwnerDrawnComboBox2* GetComboBox();
        wxItemContainer* GetItemContainer();
        wxControlWithItems* GetControlWithItems();
    };
}
