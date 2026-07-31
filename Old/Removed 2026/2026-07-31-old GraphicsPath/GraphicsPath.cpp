#include "GraphicsPath.h"
#include "DrawingContext.h"

namespace Alternet::UI
{
    GraphicsPath::GraphicsPath()
    {
    }

    GraphicsPath::GraphicsPath(wxDC* dc, wxGraphicsContext* graphicsConext) :
        _dc(dc), _graphicsConext(graphicsConext)
    {
        InitializePath(_graphicsConext);
    }

    GraphicsPath::~GraphicsPath()
    {
    }

    void GraphicsPath::Initialize(DrawingContext* dc)
    {
        _dc = dc->GetDC();
        _graphicsConext = dc->GetGraphicsContext();
        InitializePath(_graphicsConext);
    }

    void GraphicsPath::InitializePath(wxGraphicsContext* graphicsConext)
    {
        _path = graphicsConext->CreatePath();
    }

    wxWindow* GraphicsPath::GetWindow()
    {
        return DrawingContext::GetWindow(_dc);
    }

    void GraphicsPath::AddLines(Point* points, int pointsCount)
    {
        auto window = GetWindow();
        
        for (int i = 0; i < pointsCount; i++)
        {
            auto point = points[i];
            auto wxPoint = fromDip(point, window);
            _path.AddLineToPoint(wxPoint);
        }
    }
    
    void GraphicsPath::AddLine(const Point& pt1, const Point& pt2)
    {
        auto window = GetWindow();
        auto wxPt1 = fromDip(pt1, window);
        _path.AddLineToPoint(wxPt1);
        _path.AddLineToPoint(fromDip(pt2, window));
    }
    
    void GraphicsPath::AddLineTo(const Point& pt)
    {
        auto window = GetWindow();
        _path.AddLineToPoint(fromDip(pt, window));
    }
    
    void GraphicsPath::AddEllipse(const Rect& rect)
    {
        auto window = GetWindow();
        auto wxRect = fromDip(rect, window);
        _path.AddEllipse(wxRect.x, wxRect.y, wxRect.width, wxRect.height);
    }
    
    void GraphicsPath::AddBezier(const Point& startPoint, const Point& controlPoint1, const Point& controlPoint2, const Point& endPoint)
    {
        auto window = GetWindow();
        _path.MoveToPoint(fromDip(startPoint, window));
        _path.AddCurveToPoint(fromDip(controlPoint1, window), fromDip(controlPoint2, window), fromDip(endPoint, window));
    }
    
    void GraphicsPath::AddBezierTo(const Point& controlPoint1, const Point& controlPoint2, const Point& endPoint)
    {
        auto window = GetWindow();
        _path.AddCurveToPoint(fromDip(controlPoint1, window), fromDip(controlPoint2, window), fromDip(endPoint, window));
    }
    
    void GraphicsPath::AddArc(const Point& center, Coord radius, Coord startAngle, Coord sweepAngle)
    {
        auto window = GetWindow();
        
        auto startAngleRad = DegToRad * startAngle;
        auto sweepAngleRad = DegToRad * sweepAngle;

        auto wxCenter = fromDip(center, window);
        auto wxRadius = fromDip(radius, window);

        //if (_startFigure)
        //{
        //    auto counterClockwiseStartAngle = -startAngleRad + (M_PI / 2);
        //    wxPoint startPoint(
        //        wxCenter.x + (wxRadius * sin(counterClockwiseStartAngle)),
        //        wxCenter.y + (wxRadius * cos(counterClockwiseStartAngle)));
        //    MoveToIfNeeded(startPoint);
        //}

        _path.AddArc(wxCenter, wxRadius, startAngleRad, startAngleRad + sweepAngleRad, true);
    }
    
    void GraphicsPath::AddRectangle(const Rect& rect)
    {
        auto window = GetWindow();
        auto wxRect = fromDip(rect, window);
        _path.AddRectangle(wxRect.x, wxRect.y, wxRect.width, wxRect.height);
    }

    void GraphicsPath::AddRoundedRectangle(const Rect& rect, Coord cornerRadius)
    {
        auto window = GetWindow();
        auto wxRect = fromDip(rect, window);
        _path.AddRoundedRectangle(wxRect.x, wxRect.y, wxRect.width, wxRect.height, fromDip(cornerRadius, window));
    }
    
    Rect GraphicsPath::GetBounds()
    {
        auto window = GetWindow();
        
        auto box = _path.GetBox();
        wxRect intBox((int)box.m_x, (int)box.m_y, (int)box.m_width, (int)box.m_height);

        return toDip(intBox, window);
    }
    
    void GraphicsPath::StartFigure(const Point& point)
    {
        _path.MoveToPoint(fromDip(point, GetWindow()));
    }
    
    void GraphicsPath::CloseFigure()
    {
        _path.CloseSubpath();
    }

    FillMode GraphicsPath::GetFillMode()
    {
        return _fillMode;
    }

    void GraphicsPath::SetFillMode(FillMode value)
    {
        _fillMode = value;
    }

    wxGraphicsPath GraphicsPath::GetPath()
    {
        return _path;
    }

    /*static*/ wxPolygonFillMode GraphicsPath::GetWxFillMode(FillMode value)
    {
        switch (value)
        {
        case FillMode::Alternate:
            return wxPolygonFillMode::wxODDEVEN_RULE;
        case FillMode::Winding:
            return wxPolygonFillMode::wxWINDING_RULE;
        default:
            throwExNoInfo;
        }
    }

    wxPolygonFillMode GraphicsPath::GetWxFillMode()
    {
        return GetWxFillMode(_fillMode);
    }
}
