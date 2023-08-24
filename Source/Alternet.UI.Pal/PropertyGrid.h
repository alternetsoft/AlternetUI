#pragma once
#include "Common.h"
#include "Control.h"
#include "ApiTypes.h"
#include "Object.h"
#include "PropertyGridChoices.h"
#include "wx/propgrid/propgrid.h"
#include "wx/propgrid/propgridiface.h"
#include "wx/propgrid/props.h"
#include "wx/propgrid/advprops.h"
#include "wx/propgrid/property.h"

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

    private:
        bool _hasBorder = true;
        long _createStyle = wxPG_DEFAULT_STYLE;

    };
}
