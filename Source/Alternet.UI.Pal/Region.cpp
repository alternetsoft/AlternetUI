#include "Region.h"
#include "GraphicsPath.h"
#include "Control.h"

namespace Alternet::UI
{
    Region::Region()
    {
    }

    Region::~Region()
    {
    }

    void Region::InitializeWithRegion(Region* region)
    {
        _region = wxRegion(region->_region);
    }

    void Region::InitializeWithRect(const Rect& rect)
    {
        _region = wxRegion(fromDip(rect, GetWindow()));
    }

    void Region::Clear()
    {
        _region.Clear();
    }

    int Region::ContainsPoint(const Point& pt)
    {
        auto point = fromDip(pt, GetWindow());
        return _region.Contains(point);
    }

    int Region::ContainsRect(const Rect& rect)
    {
        auto r = fromDip(rect, GetWindow());
        return _region.Contains(r);
    }

    bool Region::IsEmpty()
    {
        return _region.IsEmpty();
    }

    bool Region::IsOk()
    {
        return _region.IsOk();
    }

    void Region::InitializeWithPolygon(Point* points, int pointsCount, FillMode fillMode)
    {
        std::vector<wxPoint> wxPoints;
        wxPoints.reserve(pointsCount);
        for (int i = 0; i < pointsCount; i++)
        {
            wxPoints.push_back(fromDip(points[i], GetWindow()));
        }

        _region = wxRegion(pointsCount, &wxPoints[0], GraphicsPath::GetWxFillMode(fillMode));
    }

    wxRegion Region::GetRegion()
    {
        return _region;
    }

    wxWindow* Region::GetWindow()
    {
        return ParkingWindow::GetWindow();
    }

    void Region::IntersectWithRect(const Rect& rect)
    {
        _region.Intersect(fromDip(rect, GetWindow()));
    }

    void Region::IntersectWithRegion(Region* region)
    {
        _region.Intersect(region->GetRegion());
    }

    void Region::UnionWithRect(const Rect& rect)
    {
        _region.Union(fromDip(rect, GetWindow()));
    }

    void Region::UnionWithRegion(Region* region)
    {
        _region.Union(region->GetRegion());
    }

    void Region::XorWithRect(const Rect& rect)
    {
        _region.Xor(fromDip(rect, GetWindow()));
    }

    void Region::XorWithRegion(Region* region)
    {
        _region.Xor(region->GetRegion());
    }

    void Region::SubtractRect(const Rect& rect)
    {
        _region.Subtract(fromDip(rect, GetWindow()));
    }

    void Region::SubtractRegion(Region* region)
    {
        _region.Subtract(region->GetRegion());
    }

    void Region::Translate(Coord dx, Coord dy)
    {
        _region.Offset(fromDip(dx, GetWindow()), fromDip(dy, GetWindow()));
    }

    Rect Region::GetBounds()
    {
        return toDip(_region.GetBox(), GetWindow());
    }

    bool Region::IsEqualTo(Region* other)
    {
        return _region.IsEqual(other->GetRegion());
    }

    int Region::GetHashCode_()
    {
        return (int)(int64_t)_region.GetRefData();
    }
}