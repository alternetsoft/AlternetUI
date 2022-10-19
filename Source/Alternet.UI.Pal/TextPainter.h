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
        static wxString Wrap(wxDC* dc, wxString str, int maxWidth, int maxHeight, TextWrapping wrapping, TextTrimming trimming)
        {
            if (wrapping == TextWrapping::Word)
                return WrapByWord(dc, str, maxWidth, maxHeight, wrapping, trimming);
            else
                return WrapByCharacter(dc, str, maxWidth, maxHeight, wrapping, trimming);
        }
    
    private:
        static wxString WrapByCharacter(wxDC* dc, wxString str, int maxWidth, int maxHeight, TextWrapping wrapping, TextTrimming trimming)
        {
            wxArrayInt widths;
            dc->GetPartialTextExtents(str, widths);

            int lineWidth = 0;

            bool needToWrap = (wrapping == TextWrapping::Character);
            bool needToTrim = (trimming == TextTrimming::Character);

            bool needToTrimLastLine = false;

            wxArrayString lines;
            wxString currentLine;
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
                    if (needToWrap && lineWidth + charWidth > maxWidth)
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
                    lines.Add(currentLine);
                    currentLine.Clear();
                    if (c != '\n')
                        currentLine.Append(c);

                    lineWidth = 0;
                }
                else
                {
                    currentLine.Append(c);
                }

                auto newLineWidth = lineWidth + charWidth;
                if ((!needToWrap) && needToTrim && newLineWidth > maxWidth)
                {
                    needToTrimLastLine = true;
                    break;
                }

                lineWidth = newLineWidth;
            }

            lines.Add(currentLine);

            return GetTrimmedString(dc, lines, trimming, maxHeight, needToTrimLastLine);
        }

        static void SplitToWords(const wxString& text, wxArrayString& words)
        {
            auto length = text.Length();
            wxString word;
            for (int i = 0; i < length; i++)
            {
                auto ch = text[i];
                if (isspace(ch))
                {
                    if (!word.empty())
                    {
                        words.Add(word);
                    }

                    if (ch == '\n')
                        words.Add("\n");

                    word = "";
                }
                else
                {
                    word += ch;
                }
            }

            if (!word.empty())
            {
                words.Add(word);
            }
        }

        static void GetStringsWidths(wxDC* dc, const wxArrayString& strings, wxArrayInt& widths)
        {
            for (auto str : strings)
                widths.Add(dc->GetTextExtent(str).x);
        }

        static wxString WrapByWord(wxDC* dc, wxString str, int maxWidth, int maxHeight, TextWrapping wrapping, TextTrimming trimming)
        {
            wxArrayString words;
            SplitToWords(str, words);

            wxArrayInt widths;
            GetStringsWidths(dc, words, widths);

            int lineWidth = 0;

            bool needToTrim = (trimming == TextTrimming::Character);

            bool needToTrimLastLine = false;

            auto spaceWidth = dc->GetTextExtent(" ").x;

            wxArrayString lines;
            wxString currentLine;
            int wordCount = words.Count();
            for (int i = 0; i < wordCount; i++)
            {
                wxString word = words[i];

                auto wordWidth = widths[i];
                bool needLineBreak = false;

                wxString toAppendString;
                int toAppendWidth;
                if (currentLine.IsEmpty())
                {
                    toAppendString = word;
                    toAppendWidth = wordWidth;
                }
                else
                {
                    toAppendString = " " + word;
                    toAppendWidth = wordWidth + spaceWidth;
                }

                if (word == "\n")
                {
                    needLineBreak = true;
                    toAppendString = "";
                    toAppendWidth = 0;
                }
                else
                {
                    if (lineWidth + toAppendWidth > maxWidth)
                    {
                        if (lineWidth > 0)
                        {
                            needLineBreak = true;

                            toAppendString = word;
                            toAppendWidth = wordWidth;
                        }
                    }
                }

                if (needLineBreak)
                {
                    lines.Add(currentLine);
                    currentLine.Clear();
                    currentLine.Append(toAppendString);

                    lineWidth = 0;
                }
                else
                {
                    currentLine.Append(toAppendString);
                }

                auto newLineWidth = lineWidth + toAppendWidth;
                if (needToTrim && newLineWidth > maxWidth)
                {
                    needToTrimLastLine = true;
                    break;
                }

                lineWidth = newLineWidth;
            }

            lines.Add(currentLine);

            return GetTrimmedString(dc, lines, trimming, maxHeight, needToTrimLastLine);
        }

        static wxString GetTrimmedString(wxDC* dc, const wxArrayString& lines, TextTrimming trimming, int maxHeight, bool needToTrimLastLine)
        {
            wxString result;
            bool needToTrim = (trimming == TextTrimming::Character);

            int textHeight = 0;

            int linesLastIndex = lines.GetCount() - 1;

            int i = 0;
            for (auto line : lines)
            {
                if (needToTrim)
                {
                    int lineHeight = dc->GetTextExtent(line).y;
                    textHeight += lineHeight;
                    if (textHeight > maxHeight)
                        break;
                }

                result.Append(line);

                if (i++ < linesLastIndex)
                    result.Append('\n');
            }

            if (needToTrimLastLine)
            {
                if (trimming == TextTrimming::Character)
                    result.RemoveLast();
                else
                    throwExNoInfo;
            }

            return result;
        }
    };
}

namespace Alternet::UI
{
    class TextPainter
    {
    private:
        wxDC* _dc;
        wxGraphicsContext* _graphicsContext;
        bool _useDC;
    public:
        TextPainter(wxDC* dc, wxGraphicsContext* graphicsContext, bool useDC) :
            _dc(dc), _graphicsContext(graphicsContext), _useDC(useDC)
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
            {
                auto window = DrawingContext::GetWindow(_dc);
                str = TextWrapper::Wrap(_dc, str, fromDip(maximumWidth, window), INT_MAX, wrapping, TextTrimming::None);
            }

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
            
            auto window = DrawingContext::GetWindow(_dc);

            if (useBounds)
            {
                auto wrapped = TextWrapper::Wrap(
                    _dc,
                    wxStr(text),
                    fromDip(bounds.Width, window),
                    fromDip(bounds.Height, window),
                    wrapping,
                    trimming);

                auto rect = fromDip(Rect(bounds.GetLocation(), bounds.GetSize()), window);
                
                if (trimming == TextTrimming::Pixel)
                    _dc->SetClippingRegion(rect);
                
                _dc->DrawLabel(wrapped, rect, GetWxAlignment(horizontalAlignment, verticalAlignment));
                
                if (trimming == TextTrimming::Pixel)
                    _dc->DestroyClippingRegion();
            }
            else
            {
                _dc->DrawText(wxStr(text), fromDip(bounds.GetLocation(), window));
            }

            _dc->SetTextForeground(oldTextForeground);
            _dc->SetFont(oldFont);
        }

        BYREF_ONLY(TextPainter);
    };


    /*
        Alternative implementation for wxGraphicsContext:
        auto o = fromDipF(origin, _dc->GetWindow());
        _graphicsContext->SetFont(_graphicsContext->CreateFont(font->GetWxFont()));
        _graphicsContext->SetBrush(GetGraphicsBrush(brush));
        _graphicsContext->DrawText(wxStr(text), o.X, o.Y);
    */
}
