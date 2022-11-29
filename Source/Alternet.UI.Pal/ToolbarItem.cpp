#include "ToolbarItem.h"

namespace Alternet::UI
{
    void ToolbarItem::CreateWxTool()
    {
    }
    void ToolbarItem::DestroyWxTool()
    {
    }
    void ToolbarItem::RecreateWxTool()
    {
    }
    bool ToolbarItem::IsSeparator()
    {
        return false;
    }
    ToolbarItem::ToolbarItem() : _flags(ToolbarItemFlags::Enabled)
    {
    }
    ToolbarItem::~ToolbarItem()
    {
    }
    wxWindow* ToolbarItem::CreateWxWindowCore(wxWindow* parent)
    {
        return nullptr;
    }
    wxToolBarToolBase* ToolbarItem::GetWxTool()
    {
        return nullptr;
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
        return false;
    }
    void ToolbarItem::SetEnabled(bool value)
    {
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
        return Size();
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
        return string();
    }
    void ToolbarItem::SetText(const string& value)
    {
    }
    bool ToolbarItem::GetChecked()
    {
        return false;
    }
    void ToolbarItem::SetChecked(bool value)
    {
    }
    Menu* ToolbarItem::GetSubmenu()
    {
        return nullptr;
    }
    void ToolbarItem::SetSubmenu(Menu* value)
    {
    }
}
