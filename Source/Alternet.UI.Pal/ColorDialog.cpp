#include "ColorDialog.h"

namespace Alternet::UI
{
    ColorDialog::ColorDialog()
    {
        _color = Color(*wxBLACK);
    }

    ColorDialog::~ColorDialog()
    {
        DestroyDialog();
    }

    void ColorDialog::CreateDialog()
    {
        auto owner = _owner != nullptr ? _owner->GetWxWindow() : nullptr;

        _data = new wxColourData();
        _data->SetColour(_color);

        _dialog = new wxColourDialog(owner, _data);
        
        if (_title.has_value())
            _dialog->SetTitle(wxStr(_title.value()));
    }

    Color ColorDialog::GetColor()
    {
        return _color;
    }

    void ColorDialog::SetColor(const Color& value)
    {
        _color = value;
        GetDialog()->GetColourData().SetColour(value);
    }

    optional<string> ColorDialog::GetTitle()
    {
        return _title;
    }

    void ColorDialog::SetTitle(optional<string> value)
    {
        _title = value;
        GetDialog()->SetTitle(wxStr(value.value_or(u"")));
    }

    ModalResult ColorDialog::ShowModal(Window* owner)
    {
        bool ownerChanged = _owner != owner;
        _owner = owner;
        if (ownerChanged)
            RecreateDialog();

        auto result = GetDialog()->ShowModal();

        if (result == wxID_OK)
        {
            _color = GetDialog()->GetColourData().GetColour();
            return ModalResult::Accepted;
        }
        else if (result == wxID_CANCEL)
            return ModalResult::Canceled;
        else
            throwExNoInfo;
    }

    void ColorDialog::RecreateDialog()
    {
        DestroyDialog();
        CreateDialog();
    }

    wxColourDialog* ColorDialog::GetDialog()
    {
        if (_dialog == nullptr)
            RecreateDialog();

        return _dialog;
    }

    void ColorDialog::DestroyDialog()
    {
        if (_dialog != nullptr)
        {
            delete _dialog;
            _dialog = nullptr;
        }

        if (_data != nullptr)
        {
            delete _data;
            _data = nullptr;
        }
    }
}
