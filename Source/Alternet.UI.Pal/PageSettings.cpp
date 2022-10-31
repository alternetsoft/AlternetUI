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
        return false;
    }
    
    void PageSettings::SetLandscape(bool value)
    {
    }
    
    Thickness PageSettings::GetMargins()
    {
        return _margins;
    }
    
    void PageSettings::SetMargins(const Thickness& value)
    {
        _margins = value;
    }
    
    PrinterSettings* PageSettings::GetPrinterSettings()
    {
        return nullptr;
    }
    
    void PageSettings::SetPrinterSettings(PrinterSettings* value)
    {
    }
    
    PaperKind PageSettings::GetPaperSize()
    {
        return PaperKind();
    }
    
    void PageSettings::SetPaperSize(PaperKind value)
    {
    }
    
    PrinterResolutionKind PageSettings::GetPrinterResolution()
    {
        return PrinterResolutionKind();
    }
    
    void PageSettings::SetPrinterResolution(PrinterResolutionKind value)
    {
    }
    
    Size PageSettings::GetCustomPaperSize()
    {
        return Size();
    }
    
    void PageSettings::SetCustomPaperSize(const Size& value)
    {
    }
    
    bool PageSettings::GetUseCustomPaperSize()
    {
        return false;
    }

    void PageSettings::SetUseCustomPaperSize(bool value)
    {
    }
}
