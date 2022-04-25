#pragma once
#include "Common.h"


namespace
{
    inline wxColor GetPixel(wxImage& image, int x, int y)
    {
        return wxColor(image.GetRed(x, y), image.GetGreen(x, y), image.GetBlue(x, y));
    }

    void FloodFillSlow(wxImage& image, wxPoint point, wxColor fillColor)
    {
        // See https://stackoverflow.com/a/1257195/71689

        int w = image.GetSize().x;
        int h = image.GetSize().y;

        if (point.y < 0 || point.y > h - 1 || point.x < 0 || point.x > w - 1)
            return;

        auto stack = std::make_unique<std::stack<wxPoint>>();

        auto seedColor = GetPixel(image, point.x, point.y);

        auto fillRed = fillColor.Red();
        auto fillGreen = fillColor.Green();
        auto fillBlue = fillColor.Blue();

        stack->push(point);
        while (!stack->empty())
        {
            auto p = stack->top();
            stack->pop();

            int x = p.x;
            int y = p.y;
            if (y < 0 || y > h - 1 || x < 0 || x > w - 1)
                continue;

            if (GetPixel(image, x, y) == seedColor)
            {
                image.SetRGB(x, y, fillRed, fillGreen, fillBlue);
                stack->push(wxPoint(x + 1, y));
                stack->push(wxPoint(x - 1, y));
                stack->push(wxPoint(x, y + 1));
                stack->push(wxPoint(x, y - 1));
            }
        }
    }

    void FloodFillFast(wxImage& image, wxPoint point, wxColor fillColor)
    {
        // https://lodev.org/cgtutor/floodfill.html#Scanline_Floodfill_Algorithm_With_Stack

        int w = image.GetSize().x;
        int h = image.GetSize().y;

        if (point.y < 0 || point.y > h - 1 || point.x < 0 || point.x > w - 1)
            return;

        auto stack = std::make_unique<std::stack<wxPoint>>();

        auto seedColor = GetPixel(image, point.x, point.y);

        auto fillRed = fillColor.Red();
        auto fillGreen = fillColor.Green();
        auto fillBlue = fillColor.Blue();

        if (seedColor == fillColor) return;

        int x = point.x;
        int y = point.y;

        int x1;
        bool spanAbove, spanBelow;

        auto p = point;

        stack->push(p);
        while (!stack->empty())
        {
            auto p = stack->top();
            stack->pop();
            
            x = p.x;
            y = p.y;

            x1 = x;
            while (x1 >= 0 && GetPixel(image, x1, y) == seedColor) x1--;
            x1++;
            spanAbove = spanBelow = 0;
            while (x1 < w && GetPixel(image, x1, y) == seedColor)
            {
                image.SetRGB(x1, y, fillRed, fillGreen, fillBlue);
                if (!spanAbove && y > 0 && GetPixel(image, x1, y - 1) == seedColor)
                {
                    stack->push(wxPoint(x1, y - 1));
                    spanAbove = 1;
                }
                else if (spanAbove && y > 0 && GetPixel(image, x1, y - 1) != seedColor)
                {
                    spanAbove = 0;
                }
                if (!spanBelow && y < h - 1 && GetPixel(image, x1, y + 1) == seedColor)
                {
                    stack->push(wxPoint(x1, y + 1));
                    spanBelow = 1;
                }
                else if (spanBelow && y < h - 1 && GetPixel(image, x1, y + 1) != seedColor)
                {
                    spanBelow = 0;
                }
                x1++;
            }
        }
    }
}

namespace Alternet::UI
{
    void FloodFill(wxDC* dc, wxPoint point, wxColor fillColor)
    {
        int w, h;
        dc->GetSize(&w, &h);

        if (point.y < 0 || point.y > h - 1 || point.x < 0 || point.x > w - 1)
            return;

        auto stack = std::make_unique<std::stack<wxPoint>>();
#ifdef __WXGTK__
        wxBitmap dcBitmap(w, h);
#else
        wxBitmap dcBitmap(w, h, *dc);
#endif

        wxMemoryDC memDC(dcBitmap);
        memDC.Blit(wxPoint(), wxSize(w, h), dc, wxPoint());
        memDC.SelectObject(wxNullBitmap);

        auto image = dcBitmap.ConvertToImage();

        //FloodFillSlow(image, point, fillColor);
        FloodFillFast(image, point, fillColor);

        wxBitmap outputBitmap(image);
        dc->DrawBitmap(outputBitmap, wxPoint());
    }
}
