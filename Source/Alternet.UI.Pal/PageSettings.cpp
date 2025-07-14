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

    double PageSettings::GetMarginBottom()
    {
        return _margins.Bottom;
    }

    double PageSettings::GetMarginTop()
    {
        return _margins.Top;
    }

    double PageSettings::GetMarginRight()
    {
        return _margins.Right;
    }

    double PageSettings::GetMarginLeft()
    {
        return _margins.Left;
    }
    
    void PageSettings::SetMarginTop(double value)
    {
        _margins.Top = value;
    }

    void PageSettings::SetMarginBottom(double value)
    {
        _margins.Bottom = value;
    }

    void PageSettings::SetMarginRight(double value)
    {
        _margins.Right = value;
    }

    void PageSettings::SetMarginLeft(double value)
    {
        _margins.Left = value;
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