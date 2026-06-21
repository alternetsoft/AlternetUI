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

    void Region::InitializeWithRect(const RectI& rect)
    {
        _region = wxRegion(rect);
    }

    void Region::Clear()
    {
        _region.Clear();
    }

    int Region::ContainsPoint(const PointI& pt)
    {
        return _region.Contains(pt);
    }

    int Region::ContainsRect(const RectI& rect)
    {
        return _region.Contains(rect);
    }

    bool Region::IsEmpty()
    {
        return _region.IsEmpty();
    }

    bool Region::IsOk()
    {
        return _region.IsOk();
    }

    void Region::InitializeWithPolygon(Point* points, int pointsCount, FillMode fillMode, float scaleFactor)
    {
        std::vector<wxPoint> wxPoints;
        wxPoints.reserve(pointsCount);
        for (int i = 0; i < pointsCount; i++)
        {
            wxPoints.push_back(fromDipSf(points[i], scaleFactor));
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

    void Region::IntersectWithRect(const RectI& rect)
    {
        _region.Intersect(rect);
    }

    void Region::IntersectWithRegion(Region* region)
    {
        _region.Intersect(region->GetRegion());
    }

    void Region::UnionWithRect(const RectI& rect)
    {
        _region.Union(rect);
    }

    void Region::UnionWithRegion(Region* region)
    {
        _region.Union(region->GetRegion());
    }

    void Region::XorWithRect(const RectI& rect)
    {
        _region.Xor(rect);
    }

    void Region::XorWithRegion(Region* region)
    {
        _region.Xor(region->GetRegion());
    }

    void Region::SubtractRect(const RectI& rect)
    {
        _region.Subtract(rect);
    }

    void Region::SubtractRegion(Region* region)
    {
        _region.Subtract(region->GetRegion());
    }

    void Region::Translate(int dx, int dy)
    {
        _region.Offset(dx, dy);
    }

    RectI Region::GetBounds()
    {
        return _region.GetBox();
    }

    bool Region::IsEqualTo(Region* other)
    {
        return _region.IsEqual(other->GetRegion());
    }

    int Region::GetRegionHashCode()
    {
        return (int)(int64_t)_region.GetRefData();
    }
}