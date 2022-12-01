#include "ToolbarItem.h"
#include "IdManager.h"
#include "Toolbar.h"

namespace Alternet::UI
{
    ToolbarItem::ToolbarItem() : _flags(ToolbarItemFlags::Enabled)
    {
        CreateToolInfo();
    }

    ToolbarItem::~ToolbarItem()
    {
        DestroyWxTool();
    }

    void ToolbarItem::CreateToolInfo()
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

        auto getKind = [&]()
        {
            if (separator)
                return wxItemKind::wxITEM_SEPARATOR;
            if (_dropDownMenu != nullptr)
                return wxItemKind::wxITEM_DROPDOWN;
            if (_isCheckable)
                return wxItemKind::wxITEM_CHECK;
            return wxItemKind::wxITEM_NORMAL;
        };

        _toolInfo->kind = getKind();

        if (_dropDownMenu != nullptr)
            _toolInfo->dropDownMenu = _dropDownMenu->GetWxMenu();

        if (!separator)
            s_itemsByIdsMap[_toolInfo->id] = this;

        ApplyChecked();
    }

    wxToolBar* ToolbarItem::GetToolbar()
    {
        if (_parentToolbar == nullptr)
            return nullptr;

        return _parentToolbar->GetWxToolBar();
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

    void ToolbarItem::RecreateTool()
    {
        bool wasCreated = _toolInfo != nullptr;

        auto parent = _parentToolbar;
        auto index = _indexInParentToolbar;
        if (wasCreated)
            DestroyWxTool();

        CreateToolInfo();

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
        return _text == u"-";
    }

    bool ToolbarItem::GetIsCheckable()
    {
        return _isCheckable;
    }

    void ToolbarItem::SetIsCheckable(bool value)
    {
        if (_isCheckable == value)
            return;

        _isCheckable = value;
        RecreateTool();
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
        auto it = s_itemsByIdsMap.find(id);
        if (it == s_itemsByIdsMap.end())
            throwEx(u"Cannot find toolbar item by id");

        return it->second;
    }

    void ToolbarItem::RaiseClick()
    {
        if (_toolInfo->kind == wxITEM_CHECK)
            _flags.Set(ToolbarItemFlags::Checked, !_flags.IsSet(ToolbarItemFlags::Checked));

        RaiseEvent(ToolbarItemEvent::Click);
    }

    bool ToolbarItem::GetEnabled()
    {
        return _flags.IsSet(ToolbarItemFlags::Enabled);
    }

    void ToolbarItem::SetEnabled(bool value)
    {
        _flags.Set(ToolbarItemFlags::Enabled, value);
    }

    void ToolbarItem::SetParentToolbar(Toolbar* value, optional<int> index)
    {
        _parentToolbar = value;
        _indexInParentToolbar = index;
    }

    Toolbar* ToolbarItem::GetParentToolbar()
    {
        return _parentToolbar;
    }

    void ToolbarItem::InsertWxTool(int index)
    {
        auto toolbar = GetToolbar();
        if (toolbar == nullptr)
            return;

        auto info = GetToolInfo();
        auto tool = toolbar->InsertTool(index, info->id, info->text, info->image, wxBitmapBundle(), info->kind);

        if (info->dropDownMenu != nullptr)
            tool->SetDropdownMenu(info->dropDownMenu);

        _wxTool = tool;
    }

    void ToolbarItem::RemoveWxTool(int index)
    {
        auto toolbar = GetToolbar();
        if (toolbar == nullptr)
            return;

        if (toolbar != nullptr)
            toolbar->RemoveTool(index);

        _wxTool = nullptr;
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
        RecreateTool();
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
        RecreateTool();
    }

    bool ToolbarItem::GetChecked()
    {
        return _flags.IsSet(ToolbarItemFlags::Checked);
    }

    void ToolbarItem::SetChecked(bool value)
    {
        _flags.Set(ToolbarItemFlags::Checked, value);
        ApplyChecked();
    }

    void ToolbarItem::ApplyChecked()
    {
        auto toolbar = GetToolbar();
        if (toolbar == nullptr)
            return;

        bool checked = _flags.IsSet(ToolbarItemFlags::Checked);
        GetToolbar()->ToggleTool(_toolInfo->id, checked);
    }

    Menu* ToolbarItem::GetDropDownMenu()
    {
        if (_dropDownMenu != nullptr)
            _dropDownMenu->AddRef();
        return _dropDownMenu;
    }

    void ToolbarItem::SetDropDownMenu(Menu* value)
    {
        _dropDownMenu = value;
        RecreateTool();
    }
}
