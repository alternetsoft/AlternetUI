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
        if (_tool != nullptr)
            throwExInvalidOp;

        bool checked = _flags.IsSet(ToolbarItemFlags::Checked);

        bool separator = IsSeparator();

        _tool = new wxToolBarToolBase(
            nullptr,
            separator ? wxID_SEPARATOR : IdManager::AllocateId(),
            CoerceWxToolText(_text));

        _tool->SetToggle(checked);

        if (!separator)
            s_itemsByIdsMap[_tool->GetId()] = this;
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
        if (_tool == nullptr)
            throwExInvalidOp;

        if (_parentToolbar != nullptr)
        {
            _parentToolbar = nullptr;
        }

        if (!IsSeparator())
        {
            auto id = _tool->GetId();
            s_itemsByIdsMap.erase(id);
            IdManager::FreeId(id);
        }

        delete _tool;
        _tool = nullptr;
    }

    void ToolbarItem::RecreateWxTool()
    {
        bool wasCreated = _tool != nullptr;

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

    wxToolBarToolBase* ToolbarItem::GetWxTool()
    {
        return _tool;
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
        if (_tool != nullptr && _parentToolbar != nullptr)
            _tool->Enable(value);
    }

    void ToolbarItem::SetParentToolbar(Toolbar* value, optional<int> index)
    {
        _parentToolbar = value;
        _indexInParentToolbar = index;

        if (value != nullptr)
        {
            _tool->Enable(_flags.IsSet(ToolbarItemFlags::Enabled));

            bool checked = _flags.IsSet(ToolbarItemFlags::Checked);
            if (_tool->CanBeToggled())
                _tool->Toggle(checked);
        }
    }

    Toolbar* ToolbarItem::GetParentToolbar()
    {
        return _parentToolbar;
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
        _tool->SetLabel(CoerceWxToolText(value));
        RecreateWxTool();
    }

    bool ToolbarItem::GetChecked()
    {
        return _flags.IsSet(ToolbarItemFlags::Checked);
    }

    void ToolbarItem::SetChecked(bool value)
    {
        _flags.Set(ToolbarItemFlags::Checked, value);

        if (value && !_tool->CanBeToggled())
        {
            RecreateWxTool();
        }

        bool checked = _flags.IsSet(ToolbarItemFlags::Checked);
        if (_tool != nullptr && _tool->CanBeToggled() && _parentToolbar != nullptr)
            _tool->Toggle(checked);
    }

    Menu* ToolbarItem::GetDropDownMenu()
    {
        auto wxMenu = _tool->GetDropdownMenu();
        if (wxMenu == nullptr)
            return nullptr;

        auto menu = Menu::TryFindMenuByWxMenu(wxMenu);
        if (menu == nullptr)
            throwExInvalidOp;

        menu->AddRef();

        return menu;
    }

    void ToolbarItem::SetDropDownMenu(Menu* value)
    {
        _tool->SetDropdownMenu(value->GetWxMenu());
    }
}
