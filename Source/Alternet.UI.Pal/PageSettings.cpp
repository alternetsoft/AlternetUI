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

    Coord PageSettings::GetMarginBottom()
    {
        return _margins.Bottom;
    }

    Coord PageSettings::GetMarginTop()
    {
        return _margins.Top;
    }

    Coord PageSettings::GetMarginRight()
    {
        return _margins.Right;
    }

    Coord PageSettings::GetMarginLeft()
    {
        return _margins.Left;
    }

    void PageSettings::SetMarginTop(Coord value)
    {
        _margins.Top = value;
    }

    void PageSettings::SetMarginBottom(Coord value)
    {
        _margins.Bottom = value;
    }

    void PageSettings::SetMarginRight(Coord value)
    {
        _margins.Right = value;
    }

    void PageSettings::SetMarginLeft(Coord value)
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