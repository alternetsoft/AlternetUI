#include "GraphicsPath.h"

namespace Alternet::UI
{
    GraphicsPath::GraphicsPath()
    {
    }

    GraphicsPath::~GraphicsPath()
    {
    }
    
    void GraphicsPath::AddLines(Point* points, int pointsCount)
    {
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
}
