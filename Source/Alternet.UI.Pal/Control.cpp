#include "Control.h"

namespace Alternet::UI
{
    Control::Control():
        _flags(
            *this,
            ControlFlags::Visible,
            &Control::IsWxWindowCreated,
            {
                {ControlFlags::Visible, std::make_tuple(&Control::RetrieveVisible, &Control::ApplyVisible)}
            }),
        _bounds(*this, RectangleF(), &Control::IsWxWindowCreated, &Control::RetrieveBounds, &Control::ApplyBounds),
        _delayedValues({&_flags, &_bounds})
    {
    }

    Control::~Control()
    {
        //delete GetControl();
    }

    DrawingContext* Control::OpenPaintDrawingContext()
    {
        return nullptr;
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
        wxWindow* parentingWxWindow = nullptr;
        if (_parent != nullptr)
            parentingWxWindow = _parent->GetParentingWxWindow();
        
        _wxWindow = CreateWxWindowCore(parentingWxWindow);

        _delayedValues.Apply();

        for (auto child : _children)
        {
            if (!child->IsWxWindowCreated())
                child->CreateWxWindow();
        }

        OnWxWindowCreated();
    }

    void Control::OnWxWindowCreated()
    {
    }

    DelayedValues& Control::GetDelayedValues()
    {
        return _delayedValues;
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

    RectangleF Control::RetrieveBounds()
    {
        return toDip(_wxWindow->GetClientRect(), _wxWindow);
    }

    void Control::ApplyBounds(const RectangleF& value)
    {
        wxRect rect(fromDip(value, _wxWindow));
        _wxWindow->SetPosition(rect.GetPosition());
        _wxWindow->SetClientSize(rect.GetSize());
    }

    SizeF Control::GetSize()
    {
        return GetBounds().GetSize();
    }

    void Control::SetSize(const SizeF& value)
    {
        SetBounds(RectangleF(GetLocation(), value));
    }

    PointF Control::GetLocation()
    {
        return GetBounds().GetLocation();
    }

    void Control::SetLocation(const PointF& value)
    {
        SetBounds(RectangleF(value, GetSize()));
    }

    RectangleF Control::GetBounds()
    {
        return _bounds.Get();
    }

    void Control::SetBounds(const RectangleF& value)
    {
        _bounds.Set(value);
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

        if (IsWxWindowCreated())
        {
            if (!control->IsWxWindowCreated())
                control->CreateWxWindow();
            else
                assert(false);
        }
    }

    void Control::RemoveChild(Control* control)
    {
        _children.erase(std::find(_children.begin(), _children.end(), control));
        control->_parent = nullptr;
    }

    SizeF Control::GetPreferredSize(const SizeF& availableSize)
    {
        return SizeF{ 20, 20 };
    }
}