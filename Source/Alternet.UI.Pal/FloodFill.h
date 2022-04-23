#pragma once
#include "Common.h"


namespace
{
    void FloodFillSlow(wxImage& image, wxPoint point, wxColor fillColor)
    {
        // See https://stackoverflow.com/a/1257195/71689

        int w = image.GetSize().x;
        int h = image.GetSize().y;

        if (point.y < 0 || point.y > h - 1 || point.x < 0 || point.x > w - 1)
            return;

        auto stack = std::make_unique<std::stack<wxPoint>>();

        wxColor seedColor(
            image.GetRed(point.x, point.y),
            image.GetGreen(point.x, point.y),
            image.GetBlue(point.x, point.y));

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

            wxColor val(image.GetRed(x, y), image.GetGreen(x, y), image.GetBlue(x, y));

            if (val == seedColor)
            {
                image.SetRGB(x, y, fillRed, fillGreen, fillBlue);
                stack->push(wxPoint(x + 1, y));
                stack->push(wxPoint(x - 1, y));
                stack->push(wxPoint(x, y + 1));
                stack->push(wxPoint(x, y - 1));
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
        wxBitmap dcBitmap(w, h, *dc);

        wxMemoryDC memDC(dcBitmap);
        memDC.Blit(wxPoint(), wxSize(w, h), dc, wxPoint());
        memDC.SelectObject(wxNullBitmap);

        auto image = dcBitmap.ConvertToImage();

        FloodFillSlow(image, point, fillColor);

        wxBitmap outputBitmap(image);
        dc->DrawBitmap(outputBitmap, wxPoint());
    }

}
