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

    uint8_t ColorDialog::GetColorR()
    {
		return _color.R;
    }
    
    uint8_t ColorDialog::GetColorG() {
		return _color.G;
    }
    
    uint8_t ColorDialog::GetColorB()
    {
		return _color.B;
    }
    
    uint8_t ColorDialog::GetColorA()
    {
		return _color.A;
    }
    
    uint8_t ColorDialog::GetColorState()
    {
		return _color.state;
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
        {
            RecreateDialog();
            GetDialog()->GetColourData().SetColour(_color);
        }

        auto result = GetDialog()->ShowModal();

        _color = GetDialog()->GetColourData().GetColour();

        if (result == wxID_OK)
        {
            return ModalResult::Accepted;
        }
        else
            return ModalResult::Canceled;
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
