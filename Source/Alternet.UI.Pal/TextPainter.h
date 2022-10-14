#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Font.h"
#include "SolidBrush.h"


namespace
{
    class TextWrapper
    {
    public:
        TextWrapper(wxDC* dc, const wxString& text, int maximumWidth)
        {
            Wrap(dc, text, maximumWidth);
        }

        wxString const& GetWrapped() const { return _wrapped; }
    protected:
        virtual void OnOutputLine(const wxString& line)
        {
            _wrapped += line;
        }

        virtual void OnNewLine()
        {
            _wrapped += '\n';
        }

    private:

        // call OnOutputLine() and set m_eol to true
        void DoOutputLine(const wxString& line)
        {
            OnOutputLine(line);

            m_eol = true;
        }

        // this function is a destructive inspector: when it returns true it also
        // resets the flag to false so calling it again wouldn't return true any
        // more
        bool IsStartOfNewLine()
        {
            if (!m_eol)
                return false;

            m_eol = false;

            return true;
        }

        bool m_eol = false;

        void Wrap(wxDC* dc, const wxString& text, int widthMax)
        {
            const wxArrayString ls = wxSplit(text, '\n', '\0');
            for (wxArrayString::const_iterator i = ls.begin(); i != ls.end(); ++i)
            {
                wxString line = *i;

                if (i != ls.begin())
                {
                    // Do this even if the line is empty, except if it's the first one.
                    OnNewLine();
                }

                // Is this a special case when wrapping is disabled?
                if (widthMax < 0)
                {
                    DoOutputLine(line);
                    continue;
                }

                for (bool newLine = false; !line.empty(); newLine = true)
                {
                    if (newLine)
                        OnNewLine();

                    wxArrayInt widths;
                    dc->GetPartialTextExtents(line, widths);

                    const size_t posEnd = std::lower_bound(widths.begin(),
                        widths.end(),
                        widthMax) - widths.begin();

                    // Does the entire remaining line fit?
                    if (posEnd == line.length())
                    {
                        DoOutputLine(line);
                        break;
                    }

                    // Find the last word to chop off.
                    const size_t lastSpace = line.rfind(' ', posEnd);
                    if (lastSpace == wxString::npos)
                    {
                        // No spaces, so can't wrap.
                        DoOutputLine(line);
                        break;
                    }

                    // Output the part that fits.
                    DoOutputLine(line.substr(0, lastSpace));

                    // And redo the layout with the rest.
                    line = line.substr(lastSpace + 1);
                }
            }
        }

        wxString _wrapped;
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
                (TextTrimming)0);
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
            DrawTextCore(
                text,
                /*useBounds:*/true,
                bounds,
                font,
                brush,
                horizontalAlignment,
                verticalAlignment,
                trimming);
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
        void DrawTextCore(
            const string& text,
            bool useBounds,
            const Rect& bounds,
            Font* font,
            Brush* brush,
            TextHorizontalAlignment horizontalAlignment,
            TextVerticalAlignment verticalAlignment,
            TextTrimming trimming)
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
                TextWrapper wrapper(_dc, wxStr(text), fromDip(bounds.Width, window));
                _dc->DrawText(wrapper.GetWrapped(), fromDip(bounds.GetLocation() + _translation, window));
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
