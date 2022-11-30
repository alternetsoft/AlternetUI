#include "ToolbarItem.h"
#include "IdManager.h"
#include "Toolbar.h"

namespace Alternet::UI
{
    ToolbarItem::ToolbarItem() : _flags(ToolbarItemFlags::Enabled)
    {
        CreateWxTool();
    }

    ToolbarItem::~ToolbarItem()
    {
        DestroyWxTool();
    }

    void ToolbarItem::CreateWxTool()
    {
        if (_toolInfo != nullptr)
            throwExInvalidOp;

        bool checked = _flags.IsSet(ToolbarItemFlags::Checked);

        bool separator = IsSeparator();

        wxBitmapBundle image;
        if (_image != nullptr)
        {
            image = _image->GetBitmapBundle();
        }
        else
            image = wxBitmap(wxSize(16, 16));

        _toolInfo = new ToolInfo();

        _toolInfo->id = separator ? wxID_SEPARATOR : IdManager::AllocateId();
        _toolInfo->text = CoerceWxToolText(_text);
        _toolInfo->image = image;

        if (!separator)
            s_itemsByIdsMap[_toolInfo->id] = this;
    }

    /*static*/ wxString ToolbarItem::CoerceWxToolText(const string& value)
    {
        // Have to pass space because the validation check does not allow for empty string.
        auto text = value.empty() ? wxString(" ") : wxStr(value);
        text.Replace("_", "&");

        return text;
    }

    void ToolbarItem::DestroyWxTool()
    {
        if (_toolInfo == nullptr)
            throwExInvalidOp;

        if (_parentToolbar != nullptr)
        {
            _parentToolbar = nullptr;
        }

        if (!IsSeparator())
        {
            auto id = _toolInfo->id;
            s_itemsByIdsMap.erase(id);
            IdManager::FreeId(id);
        }

        delete _toolInfo;
        _toolInfo = nullptr;
    }

    void ToolbarItem::RecreateWxTool()
    {
        bool wasCreated = _toolInfo != nullptr;

        auto parent = _parentToolbar;
        auto index = _indexInParentToolbar;
        if (wasCreated)
            DestroyWxTool();

        CreateWxTool();

        if (wasCreated)
        {
            if (parent != nullptr)
            {
                if (!index.has_value())
                    throwExInvalidOp;
                parent->InsertItemAt(index.value(), this);
            }
        }
    }

    bool ToolbarItem::IsSeparator()
    {
        return false;
    }

    wxWindow* ToolbarItem::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxPanel();
    }

    ToolbarItem::ToolInfo* ToolbarItem::GetToolInfo()
    {
        return _toolInfo;
    }

    ToolbarItem* ToolbarItem::GetToolbarItemById(int id)
    {
        return nullptr;
    }

    void ToolbarItem::RaiseClick()
    {
    }

    bool ToolbarItem::GetEnabled()
    {
        return _flags.IsSet(ToolbarItemFlags::Enabled);
    }

    void ToolbarItem::SetEnabled(bool value)
    {
        _flags.Set(ToolbarItemFlags::Enabled, value);
        //if (_toolInfo != nullptr && _parentToolbar != nullptr)
        //    _toolInfo->Enable(value);
    }

    void ToolbarItem::SetParentToolbar(Toolbar* value, optional<int> index)
    {
        _parentToolbar = value;
        _indexInParentToolbar = index;

        //RecreateWxTool();

        if (value != nullptr)
        {
            //_toolInfo->Enable(_flags.IsSet(ToolbarItemFlags::Enabled));

            //bool checked = _flags.IsSet(ToolbarItemFlags::Checked);
            //if (_toolInfo->CanBeToggled())
            //    _toolInfo->Toggle(checked);
        }
    }

    Toolbar* ToolbarItem::GetParentToolbar()
    {
        return _parentToolbar;
    }

    ImageSet* ToolbarItem::GetImage()
    {
        if (_image != nullptr)
            _image->AddRef();
        return _image;
    }

    void ToolbarItem::SetImage(ImageSet* value)
    {
        if (_image != nullptr)
            _image->Release();
        _image = value;
        if (_image != nullptr)
            _image->AddRef();
        RecreateWxTool();
    }

    void ToolbarItem::ShowCore()
    {
    }

    Rect ToolbarItem::RetrieveBounds()
    {
        return Rect();
    }

    void ToolbarItem::ApplyBounds(const Rect& value)
    {
    }

    Size ToolbarItem::SizeToClientSize(const Size& size)
    {
        return size;
    }

    void ToolbarItem::UpdateWxWindowParent()
    {
    }

    string ToolbarItem::GetManagedCommandId()
    {
        return string();
    }

    void ToolbarItem::SetManagedCommandId(const string& value)
    {
    }

    string ToolbarItem::GetText()
    {
        return _text;
    }

    void ToolbarItem::SetText(const string& value)
    {
        bool wasSeparator = IsSeparator();
        _text = value;
        _toolInfo->text = CoerceWxToolText(value);
        RecreateWxTool();
    }

    bool ToolbarItem::GetChecked()
    {
        return _flags.IsSet(ToolbarItemFlags::Checked);
    }

    void ToolbarItem::SetChecked(bool value)
    {
        _flags.Set(ToolbarItemFlags::Checked, value);

        if (value/* && !_toolInfo->CanBeToggled()*/)
        {
            RecreateWxTool();
        }

        bool checked = _flags.IsSet(ToolbarItemFlags::Checked);
        //if (_toolInfo != nullptr && _toolInfo->CanBeToggled() && _parentToolbar != nullptr)
        //    _toolInfo->Toggle(checked);
    }

    Menu* ToolbarItem::GetDropDownMenu()
    {
        //auto wxMenu = _toolInfo->GetDropdownMenu();
        //if (wxMenu == nullptr)
        //    return nullptr;

        //auto menu = Menu::TryFindMenuByWxMenu(wxMenu);
        //if (menu == nullptr)
        //    throwExInvalidOp;

        //menu->AddRef();

        //return menu;
        return nullptr;
    }

    void ToolbarItem::SetDropDownMenu(Menu* value)
    {
        //_toolInfo->SetDropdownMenu(value->GetWxMenu());
    }
}
