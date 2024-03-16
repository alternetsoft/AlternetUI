#include "TransformMatrix.h"

namespace Alternet::UI
{
    TransformMatrix::TransformMatrix()
    {
    }

    TransformMatrix::TransformMatrix(const wxAffineMatrix2D& matrix) : _matrix(matrix)
    {
    }

    TransformMatrix::~TransformMatrix()
    {
    }

    void TransformMatrix::Initialize(double m11, double m12, double m21,
        double m22, double dx, double dy)
    {
        auto window = GetWindow();
        _matrix.Set(wxMatrix2D(m11, m12, m21, m22), wxPoint2DDouble(fromDip(dx, window),
            fromDip(dy, window)));
    }

    double TransformMatrix::GetM11()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return m.m_11;
    }

    void TransformMatrix::SetM11(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        m.m_11 = value;
        _matrix.Set(m, t);
    }

    double TransformMatrix::GetM12()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return m.m_12;
    }

    void TransformMatrix::SetM12(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        m.m_12 = value;
        _matrix.Set(m, t);
    }

    double TransformMatrix::GetM21()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return m.m_21;
    }

    void TransformMatrix::SetM21(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        m.m_21 = value;
        _matrix.Set(m, t);
    }

    double TransformMatrix::GetM22()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return m.m_22;
    }

    void TransformMatrix::SetM22(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        m.m_22 = value;
        _matrix.Set(m, t);
    }

    double TransformMatrix::GetDX()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return toDip(t.m_x, GetWindow());
    }

    void TransformMatrix::SetDX(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        t.m_x = fromDip(value, GetWindow());
        _matrix.Set(m, t);
    }

    double TransformMatrix::GetDY()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return toDip(t.m_y, GetWindow());
    }

    void TransformMatrix::SetDY(double value)
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        t.m_y = fromDip(value, GetWindow());
        _matrix.Set(m, t);
    }

    bool TransformMatrix::GetIsIdentity()
    {
        return _matrix.IsIdentity();
    }

    void TransformMatrix::Reset()
    {
        _matrix = wxAffineMatrix2D();
    }

    void TransformMatrix::Multiply(TransformMatrix* matrix)
    {
        _matrix.Concat(matrix->GetMatrix());
    }

    void TransformMatrix::Translate(double offsetX, double offsetY)
    {
        auto window = GetWindow();
        _matrix.Translate(fromDip(offsetX, window), fromDip(offsetY, window));
    }

    void TransformMatrix::Scale(double scaleX, double scaleY)
    {
        _matrix.Scale(scaleX, scaleY);
    }

    void TransformMatrix::Rotate(double angle)
    {
        _matrix.Rotate(angle * DegToRad);
    }

    void TransformMatrix::Invert()
    {
        _matrix.Invert();
    }

    Point TransformMatrix::TransformPoint(const Point& point)
    {
        auto window = GetWindow();
        return toDip(_matrix.TransformPoint(fromDip(point, window)), window);
    }

    Size TransformMatrix::TransformSize(const Size& size)
    {
        auto window = GetWindow();
        auto result = toDip(_matrix.TransformDistance(
            fromDip(Point(size.Width, size.Height), window)), window);
        return Size(result.X, result.Y);
    }

    bool TransformMatrix::IsEqualTo(TransformMatrix* other)
    {
        return _matrix.IsEqual(other->GetMatrix());
    }

    int TransformMatrix::GetHashCode_()
    {
        wxMatrix2D m;
        wxPoint2DDouble t;
        _matrix.Get(&m, &t);
        return CombineHashCodes(m.m_11, m.m_12, m.m_21, m.m_22, t.m_x, t.m_y);
    }

    wxAffineMatrix2D TransformMatrix::GetMatrix()
    {
        return _matrix;
    }

    int TransformMatrix::GetHashCode(double value)
    {
        if (value == 0.0)
            return 0;

        auto num2 = *((int64_t*)&value);
        return (((int)num2) ^ ((int)(num2 >> 32)));
    }

    int TransformMatrix::CombineHashCodes(double v1, double v2, double v3,
        double v4, double v5, double v6)
    {
        int hash = 17;
        hash = hash * 23 + GetHashCode(v1);
        hash = hash * 23 + GetHashCode(v2);
        hash = hash * 23 + GetHashCode(v3);
        hash = hash * 23 + GetHashCode(v4);
        hash = hash * 23 + GetHashCode(v5);
        hash = hash * 23 + GetHashCode(v6);
        return hash;
    }

    wxWindow* TransformMatrix::GetWindow()
    {
        return ParkingWindow::GetWindow();
    }
}
