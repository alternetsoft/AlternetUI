#include "FontDialog.h"

namespace Alternet::UI
{

    Font* FontDialog::GetFont() 
    {
        wxFontData& _data = GetDialog()->GetFontData();
        auto font = new Font();
        font->SetWxFont(_data.GetChosenFont());
        return font;
    }

    void FontDialog::SetFont(Font* value)
    {
        wxFontData& _data = GetDialog()->GetFontData();
        _data.SetInitialFont(value->GetWxFont());
    }

    optional<string> FontDialog::GetTitle()
    {
        return _title;
    }

    void FontDialog::SetTitle(optional<string> value)
    {
        _title = value;
        GetDialog()->SetTitle(wxStr(value.value_or(u"")));
    }

    ModalResult FontDialog::ShowModal(Window* owner)
    {
        bool ownerChanged = _owner != owner;
        _owner = owner;
        if (ownerChanged)
            RecreateDialog();

        auto result = GetDialog()->ShowModal();

        if (result == wxID_OK)
        {
            //_color = GetDialog()->GetColourData().GetColour();
            return ModalResult::Accepted;
        }
        else if (result == wxID_CANCEL)
            return ModalResult::Canceled;
        else
            throwExNoInfo;
    }

    void FontDialog::RecreateDialog()
    {
        DestroyDialog();
        CreateDialog();
    }

    wxFontDialog* FontDialog::GetDialog()
    {
        if (_dialog == nullptr)
            RecreateDialog();

        return _dialog;
    }

    void FontDialog::DestroyDialog()
    {
        if (_dialog != nullptr)
        {
            delete _dialog;
            _dialog = nullptr;
        }

        /*if (_data != nullptr)
        {
            delete _data;
            _data = nullptr;
        }*/
    }

    FontDialog::FontDialog()
    {
        //_color = Color(*wxBLACK);
    }

    FontDialog::~FontDialog()
    {
        DestroyDialog();
    }

    void FontDialog::CreateDialog()
    {
        auto owner = _owner != nullptr ? _owner->GetWxWindow() : nullptr;

        //_data = new wxColourData();
        //_data->SetColour(_color);

        _dialog = new wxFontDialog(owner);

        if (_title.has_value())
            _dialog->SetTitle(wxStr(_title.value()));
    }
}
