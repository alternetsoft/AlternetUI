#include "PageSettings.h"

namespace Alternet::UI
{
    PageSettings::PageSettings()
    {
    }
    
    PageSettings::~PageSettings()
    {
    }
    
    Rect PageSettings::GetBounds()
    {
        return Rect();
    }
    
    bool PageSettings::GetColor()
    {
        return false;
    }
    
    void PageSettings::SetColor(bool value)
    {
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
        return Thickness();
    }
    
    void PageSettings::SetMargins(const Thickness& value)
    {
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

    wxPageSetupDialogData PageSettings::GetPageSetupDialogData()
    {
        _pageSetupDialogData.SetMarginTopLeft(wxPoint(10, 10));
        _pageSetupDialogData.SetMarginBottomRight(wxPoint(10, 10));

        return _pageSetupDialogData;
    }
}
