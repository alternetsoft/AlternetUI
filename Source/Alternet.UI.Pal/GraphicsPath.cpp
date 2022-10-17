#include "GraphicsPath.h"
#include "DrawingContext.h"

namespace Alternet::UI
{
    GraphicsPath::GraphicsPath()
    {
    }

    GraphicsPath::~GraphicsPath()
    {
        if (_dc != nullptr)
        {
            _dc->Release();
            _dc = nullptr;
        }
    }

    void GraphicsPath::Initialize(DrawingContext* dc)
    {
        _dc = dc;
        _dc->AddRef();
        _path = dc->GetGraphicsContext()->CreatePath();
    }

    void GraphicsPath::AddLines(Point* points, int pointsCount)
    {
        auto window = DrawingContext::GetWindow(_dc->GetDC());
        
        for (int i = 0; i < pointsCount; i++)
        {
            auto point = points[i];
            auto wxPoint = fromDip(point, window);
            _path.AddLineToPoint(wxPoint);
        }
    }
    
    void GraphicsPath::AddLine(const Point& pt1, const Point& pt2)
    {
    }
    
    void GraphicsPath::AddLineTo(const Point& pt)
    {
    }
    
    void GraphicsPath::AddEllipse(const Rect& rect)
    {
    }
    
    void GraphicsPath::AddBezier(const Point& startPoint, const Point& controlPoint1, const Point& controlPoint2, const Point& endPoint)
    {
    }
    
    void GraphicsPath::AddBezierTo(const Point& controlPoint1, const Point& controlPoint2, const Point& endPoint)
    {
    }
    
    void GraphicsPath::AddArc(const Point& center, double radius, double startAngle, double sweepAngle)
    {
    }
    
    void GraphicsPath::AddRectangle(const Rect& rect)
    {
    }
    
    void GraphicsPath::AddRoundedRectangle(const Rect& rect, double cornerRadius)
    {
    }
    
    Rect GraphicsPath::GetBounds()
    {
        return Rect();
    }
    
    void GraphicsPath::StartFigure()
    {
    }
    
    void GraphicsPath::CloseFigure()
    {
    }

    wxGraphicsPath GraphicsPath::GetPath()
    {
        return _path;
    }
}
