#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Font.h"
#include "SolidBrush.h"


namespace
{
    using namespace Alternet::UI;

    class TextWrapper
    {
    public:
        static wxString Wrap(wxDC* dc, wxString str, int maxWidth, TextWrapping wrapping)
        {
            if (wrapping == TextWrapping::Character)
                return WrapByCharacter(dc, str, maxWidth);
            else if (wrapping == TextWrapping::Word)
                return WrapByWord(dc, str, maxWidth);
            else
                return str;
        }

    private:
        static wxString WrapByWord(wxDC* dc, wxString str, int maxWidth)
        {
            //auto parts = Explode(str, wrapping);

            //wxArrayInt widths;
            //GetPartsWidths(dc, str, parts, wrapping, widths);

            //int curLineLength = 0;
            //wxString strBuilder;
            //for (int i = 0; i < parts.Count(); i += 1)
            //{
            //    auto part = parts[i];
            //    auto partWidth = widths[i];

            //    // If adding the new part to the current line would be too long,
            //    // then put it on a new line (and split it up if it's too long).
            //    if (curLineLength + partWidth > maxWidth)
            //    {
            //        // Only move down to a new line if we have text on the current line.
            //        // Avoids situation where wrapped whitespace causes emptylines in text.
            //        if (curLineLength > 0)
            //        {
            //            strBuilder.Append('\n');
            //            curLineLength = 0;
            //        }

            //        part.Trim();
            //    }

            //    strBuilder.Append(part);
            //    curLineLength += partWidth;
            //}

            //return strBuilder;

            return str;
        }

        static wxString WrapByCharacter(wxDC* dc, wxString str, int maxWidth)
        {
            wxArrayInt widths;
            dc->GetPartialTextExtents(str, widths);

            int lineWidth = 0;
            wxString stringBuilder;
            for (int i = 0; i < str.Length(); i++)
            {
                auto c = str.GetChar(i);
                
                int charWidth;
                if (i == 0)
                    charWidth = widths[i];
                else
                    charWidth = widths[i] - widths[i - 1];

                bool needLineBreak = false;

                if (c == '\n')
                {
                    needLineBreak = true;
                }
                else
                {
                    if (lineWidth + charWidth > maxWidth)
                    {
                        if (lineWidth > 0)
                        {
                            // Only move down to a new line if we have text on the current line.
                            // Avoids situation where wrapped whitespace causes emptylines in text.
                            needLineBreak = true;
                        }
                    }
                }

                if (needLineBreak)
                {
                    stringBuilder.Append('\n');
                    lineWidth = 0;
                }
                else
                    stringBuilder.Append(c);
    
                lineWidth += charWidth;
            }

            return stringBuilder;
        }


        static void GetPartsWidths(wxDC* dc, const wxString& str, const wxArrayString& parts, TextWrapping wrapping, wxArrayInt& widths)
        {
            if (wrapping == TextWrapping::Character)
            {
                dc->GetPartialTextExtents(str, widths);
            }
            else if (wrapping == TextWrapping::Word)
            {
                for (auto part : parts)
                    widths.Add(dc->GetTextExtent(part).x);
            }
            else
                throwExNoInfo;
        }

        static wxArrayString Explode(wxString str, TextWrapping textWrapping)
        {
            if (textWrapping == TextWrapping::Word)
                return wxSplit(str, ' ', '\0');
            else if (textWrapping == TextWrapping::Character)
            {
                wxArrayString result;
                for (int i = 0; i < str.length(); i++)
                    result.Add(str[i]);
                return result;
            }
            else
                throwExNoInfo;
        }
    };
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
            DrawTextCore(
                text,
                /*useBounds:*/false,
                Rect(origin, Size()),
                font,
                brush,
                (TextHorizontalAlignment)0,
                (TextVerticalAlignment)0,
                (TextTrimming)0,
                (TextWrapping)0);
        }

        void DrawTextAtRect(
            const string& text,
            const Rect& bounds,
            Font* font,
            Brush* brush,
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAlignment,
            TextTrimming trimming,
            TextWrapping wrapping)
        {
            DrawTextCore(
                text,
                /*useBounds:*/true,
                bounds,
                font,
                brush,
                horizontalAlignment,
                verticalAlignment,
                trimming,
                wrapping);
        }

        Size MeasureText(const string& text, Font* font, double maximumWidth, TextWrapping wrapping)
        {
            wxCoord x = 0, y = 0;
            auto wxFont = font->GetWxFont();
            auto oldFont = _dc->GetFont();
            _dc->SetFont(wxFont); // just passing font as a GetMultiLineTextExtent argument doesn't work on macOS/Linux
            
            wxString str = wxStr(text);
            if (!isnan(maximumWidth))
                str = TextWrapper::Wrap(_dc, str, maximumWidth, wrapping);

            _dc->GetMultiLineTextExtent(str, &x, &y, nullptr, &wxFont);
            _dc->SetFont(oldFont);
            return toDip(wxSize(x, y), _dc->GetWindow());
        }

    private:

        int GetWxAlignment(TextHorizontalAlignment horizontal, TextVerticalAlignment vertical)
        {
            int result = 0;

            switch (horizontal)
            {
            case TextHorizontalAlignment::Left:
                result |= wxALIGN_LEFT;
                break;
            case TextHorizontalAlignment::Center:
                result |= wxALIGN_CENTER_HORIZONTAL;
                break;
            case TextHorizontalAlignment::Right:
                result |= wxALIGN_RIGHT;
                break;
            default:
                throwExNoInfo;
            }

            switch (vertical)
            {
            case TextVerticalAlignment::Top:
                result |= wxALIGN_TOP;
                break;
            case TextVerticalAlignment::Center:
                result |= wxALIGN_CENTER_VERTICAL;
                break;
            case TextVerticalAlignment::Bottom:
                result |= wxALIGN_BOTTOM;
                break;
            default:
                throwExNoInfo;
            }

            return result;
        }

        void DrawTextCore(
            const string& text,
            bool useBounds,
            const Rect& bounds,
            Font* font,
            Brush* brush,
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAlignment,
            TextTrimming trimming,
            TextWrapping wrapping)
        {
            auto solidBrush = dynamic_cast<SolidBrush*>(brush);
            if (solidBrush == nullptr)
                throwExInvalidArg(solidBrush, u"Only SolidBrush objects are supported");

            auto oldTextForeground = _dc->GetTextForeground();
            auto oldFont = _dc->GetFont();
            _dc->SetTextForeground(solidBrush->GetWxBrush().GetColour());
            _dc->SetFont(font->GetWxFont());
            
            auto window = GetWindow();

            if (useBounds)
            {
                auto wrapped = TextWrapper::Wrap(_dc, wxStr(text), fromDip(bounds.Width, window), wrapping);
                _dc->DrawLabel(
                    wrapped,
                    fromDip(Rect(bounds.GetLocation() + _translation, bounds.GetSize()), window),
                    GetWxAlignment(horizontalAlignment, verticalAlignment));
            }
            else
            {
                _dc->DrawText(wxStr(text), fromDip(bounds.GetLocation() + _translation, window));
            }

            _dc->SetTextForeground(oldTextForeground);
            _dc->SetFont(oldFont);
        }

        wxWindow* GetWindow()
        {
            auto window = _dc->GetWindow();
            if (window == nullptr)
                return ParkingWindow::GetWindow();
            else
                return window;
        }

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
