#include "Control.h"

namespace Alternet::UI
{
    Control::Control():
        _flags(
            *this,
            ControlFlags::None,
            &Control::IsWxWindowCreated,
            {
                {ControlFlags::Visible, std::make_tuple(&Control::RetrieveVisible, &Control::ApplyVisible)}
            })
    {
    }

    Control::~Control()
    {
        //delete GetControl();
    }

    Control* Control::GetParent()
    {
        return _parent;
    }

    wxWindow* Control::GetWxWindow()
    {
        return _wxWindow;
    }

    void Control::CreateWxWindow()
    {
        wxWindow* parentWxWindow = nullptr;

        auto parent = GetParent();
        if (parent != nullptr)
            parentWxWindow = parent->GetWxWindow();

        _wxWindow = CreateWxWindowCore(parentWxWindow);
    }

    bool Control::IsWxWindowCreated()
    {
        return _wxWindow != nullptr;
    }

    wxWindow* Control::GetParentingWxWindow()
    {
        return GetWxWindow();
    }

    bool Control::RetrieveVisible()
    {
        assert(_wxWindow);
        return _wxWindow->IsShown();
    }

    void Control::ApplyVisible(bool value)
    {
        assert(_wxWindow);
        if (value)
            _wxWindow->Show();
        else
            _wxWindow->Hide();
    }

    SizeF Control::GetSize()
    {
        return SizeF();
    }

    void Control::SetSize(const SizeF& value)
    {
    }

    PointF Control::GetLocation()
    {
        return PointF();
    }

    void Control::SetLocation(const PointF& value)
    {
    }

    RectangleF Control::GetBounds()
    {
        return RectangleF();
    }

    void Control::SetBounds(const RectangleF& value)
    {
    }

    bool Control::GetVisible()
    {
        return _flags.Get(ControlFlags::Visible);
    }

    void Control::SetVisible(bool value)
    {
        _flags.Set(ControlFlags::Visible, value);
    }

    void Control::AddChild(Control* control)
    {
        verifyNonNull(control);

        _children.push_back(control);
        control->_parent = this;
        if (!control->IsWxWindowCreated())
            control->CreateWxWindow();
        else
            assert(false);
    }

    void Control::RemoveChild(Control* control)
    {
        _children.erase(std::find(_children.begin(), _children.end(), control));
        control->_parent = nullptr;
    }

    SizeF Control::GetPreferredSize(const SizeF& availableSize)
    {
        return SizeF();
    }
}