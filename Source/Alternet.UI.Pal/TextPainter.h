#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Font.h"
#include "SolidBrush.h"


namespace
{
}

namespace Alternet::UI
{
    class TextPainter
    {
    private:
        wxDC* _dc;
        Size _translation;
    public:
        TextPainter(wxDC* dc, Size translation) : _dc(dc), _translation(translation)
        {
        }

        void DrawTextAtPoint(
            const string& text,
            const Point& origin,
            Font* font,
            Brush* brush)
        {
            auto solidBrush = dynamic_cast<SolidBrush*>(brush);
            if (solidBrush == nullptr)
                throwExInvalidArg(solidBrush, u"Only SolidBrush objects are supported");

            auto oldTextForeground = _dc->GetTextForeground();
            auto oldFont = _dc->GetFont();
            _dc->SetTextForeground(solidBrush->GetWxBrush().GetColour());
            _dc->SetFont(font->GetWxFont());
            _dc->DrawText(wxStr(text), fromDip(origin + _translation, _dc->GetWindow()));
            _dc->SetTextForeground(oldTextForeground);
            _dc->SetFont(oldFont);
        }

        void DrawTextAtRect(
            const string& text,
            const Rect& bounds,
            Font* font,
            Brush* brush,
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAlignment,
            TextTrimming trimming)
        {
        }

        Size MeasureText(const string& text, Font* font, double maximumWidth)
        {
            wxCoord x = 0, y = 0;
            auto wxFont = font->GetWxFont();
            auto oldFont = _dc->GetFont();
            _dc->SetFont(wxFont); // just passing font as a GetMultiLineTextExtent argument doesn't work on macOS/Linux
            _dc->GetMultiLineTextExtent(wxStr(text), &x, &y, nullptr, &wxFont);
            _dc->SetFont(oldFont);
            return toDip(wxSize(x, y), _dc->GetWindow());
        }

    private:
        BYREF_ONLY(TextPainter);
    };


    /*
        Alternative implementation for wxGraphicsContext:
        auto o = fromDipF(origin + _translation, _dc->GetWindow());
        _graphicsContext->SetFont(_graphicsContext->CreateFont(font->GetWxFont()));
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));
        _graphicsContext->DrawText(wxStr(text), o.X, o.Y);
    */
}
