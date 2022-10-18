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

    wxWindow* GraphicsPath::GetWindow()
    {
        return DrawingContext::GetWindow(_dc->GetDC());
    }

    void GraphicsPath::MoveToIfNeeded(wxPoint point)
    {
        if (_startFigure)
        {
            _path.MoveToPoint(point);
            _startFigure = false;
        }
    }

    void GraphicsPath::AddLines(Point* points, int pointsCount)
    {
        auto window = GetWindow();
        
        if (pointsCount > 0)
            MoveToIfNeeded(fromDip(points[0], window));
        
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
        MoveToIfNeeded(wxPt1);
        _path.AddLineToPoint(wxPt1);
        _path.AddLineToPoint(fromDip(pt2, window));
    }
    
    void GraphicsPath::AddLineTo(const Point& pt)
    {
        auto window = GetWindow();
        MoveToIfNeeded(wxPoint());
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
        MoveToIfNeeded(wxPoint());
        _path.AddCurveToPoint(fromDip(controlPoint1, window), fromDip(controlPoint2, window), fromDip(endPoint, window));
    }
    
    void GraphicsPath::AddArc(const Point& center, double radius, double startAngle, double sweepAngle)
    {
        const double DegToRad = M_PI / 180;

        auto window = GetWindow();
        
        auto startAngleRad = DegToRad * startAngle;
        auto sweepAngleRad = DegToRad * sweepAngle;

        auto wxCenter = fromDip(center, window);
        auto wxRadius = fromDip(radius, window);

        if (_startFigure)
        {
            auto counterClockwiseStartAngle = -startAngleRad + (M_PI / 2);
            wxPoint startPoint(
                wxCenter.x + (wxRadius * sin(counterClockwiseStartAngle)),
                wxCenter.y + (wxRadius * cos(counterClockwiseStartAngle)));
            MoveToIfNeeded(startPoint);
        }

        _path.AddArc(wxCenter, wxRadius, startAngleRad, startAngleRad + sweepAngleRad, true);
    }
    
    void GraphicsPath::AddRectangle(const Rect& rect)
    {
        auto window = GetWindow();
        auto wxRect = fromDip(rect, window);
        _path.AddRectangle(wxRect.x, wxRect.y, wxRect.width, wxRect.height);
    }
    
    void GraphicsPath::AddRoundedRectangle(const Rect& rect, double cornerRadius)
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
    
    void GraphicsPath::StartFigure()
    {
        _startFigure = true;
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

    wxPolygonFillMode GraphicsPath::GetWxFillMode()
    {
        switch (_fillMode)
        {
        case FillMode::Alternate:
            return wxPolygonFillMode::wxODDEVEN_RULE;
        case FillMode::Winding:
            return wxPolygonFillMode::wxWINDING_RULE;
        default:
            throwExNoInfo;
        }
    }
}
