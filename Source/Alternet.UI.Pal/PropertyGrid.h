#pragma once
#include "Common.h"
#include "Keyboard.h"
#include "Control.h"
#include "ApiTypes.h"
#include "Object.h"
#include "PropertyGridChoices.h"
#include "wx/propgrid/propgrid.h"
#include "wx/propgrid/propgridiface.h"
#include "wx/propgrid/props.h"
#include "wx/propgrid/advprops.h"
#include "wx/propgrid/property.h"
#include "wx/variant.h"
#include "wx/propgrid/editors.h"
#include "PropertyGridVariant.h"
#include "WxAlternet/wxAlternetColourProperty.h"

namespace Alternet::UI
{
    class PropertyGrid : public Control
    {
#include "Api/PropertyGrid.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxPropertyGrid* GetPropGrid();
        wxPropertyGridInterface* GetPropGridInterface();
        PropertyGrid(long styles);
        wxPGPropArgCls _propArg = wxPGPropArgCls(0);
        void ToPropArg(void* id);
    private:
        bool _hasBorder = true;
        long _createStyle = wxPG_DEFAULT_STYLE;
        PropertyGridVariant* _eventValue = new PropertyGridVariant();
        int _eventValidationFailureBehavior = 0;
        int _eventColumn = 0;
        void* _eventProperty = nullptr;
        string _eventPropertyName = wxStr(wxEmptyString);
        string _eventValidationFailureMessage = wxStr(wxEmptyString);

        void OnButton(wxCommandEvent& event);
        void FromEventData(PropertyGridEvent evType, wxPropertyGridEvent& event);
        void ToEventData(PropertyGridEvent evType, wxPropertyGridEvent& event);
        void OnSelected(wxPropertyGridEvent& event);
        void OnChanged(wxPropertyGridEvent& event);
        void OnChanging(wxPropertyGridEvent& event);
        void OnHighlighted(wxPropertyGridEvent& event);
        void OnRightClick(wxPropertyGridEvent& event);
        void OnDoubleClick(wxPropertyGridEvent& event);
        void OnItemCollapsed(wxPropertyGridEvent& event);
        void OnItemExpanded(wxPropertyGridEvent& event);
        void OnLabelEditBegin(wxPropertyGridEvent& event);
        void OnLabelEditEnding(wxPropertyGridEvent& event);
        void OnColBeginDrag(wxPropertyGridEvent& event);
        void OnColDragging(wxPropertyGridEvent& event);
        void OnColEndDrag(wxPropertyGridEvent& event);
    };
}
