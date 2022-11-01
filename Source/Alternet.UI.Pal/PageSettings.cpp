#include "PageSettings.h"
#include "Api/DrawingTypes.h"

namespace Alternet::UI
{
    PageSettings::PageSettings()
    {
    }
    
    PageSettings::~PageSettings()
    {
    }
    
    bool PageSettings::GetColor()
    {
        return _color;
    }
    
    void PageSettings::SetColor(bool value)
    {
        _color = value;
    }
    
    bool PageSettings::GetLandscape()
    {
        return _landscape;
    }
    
    void PageSettings::SetLandscape(bool value)
    {
        _landscape = value;
    }
    
    Thickness PageSettings::GetMargins()
    {
        return _margins;
    }
    
    void PageSettings::SetMargins(const Thickness& value)
    {
        _margins = value;
    }
    
    PaperKind PageSettings::GetPaperSize()
    {
        return _paperSize;
    }
    
    void PageSettings::SetPaperSize(PaperKind value)
    {
        _paperSize = value;
    }
    
    PrinterResolutionKind PageSettings::GetPrinterResolution()
    {
        return _printerResolutionKind;
    }
    
    void PageSettings::SetPrinterResolution(PrinterResolutionKind value)
    {
        _printerResolutionKind = value;
    }
    
    Size PageSettings::GetCustomPaperSize()
    {
        return _customPaperSize;
    }
    
    void PageSettings::SetCustomPaperSize(const Size& value)
    {
        _customPaperSize = value;
    }
    
    bool PageSettings::GetUseCustomPaperSize()
    {
        return _useCustomPaperSize;
    }

    void PageSettings::SetUseCustomPaperSize(bool value)
    {
        _useCustomPaperSize = value;
    }
}