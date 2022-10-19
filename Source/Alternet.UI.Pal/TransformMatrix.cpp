#include "TransformMatrix.h"

namespace Alternet::UI
{
    TransformMatrix::TransformMatrix()
    {
    }

    TransformMatrix::~TransformMatrix()
    {
    }

    void TransformMatrix::Initialize(double m11, double m12, double m21, double m22, double dx, double dy)
    {
    }

    double TransformMatrix::GetM11()
    {
        return 0.0;
    }

    void TransformMatrix::SetM11(double value)
    {
    }

    double TransformMatrix::GetM12()
    {
        return 0.0;
    }

    void TransformMatrix::SetM12(double value)
    {
    }

    double TransformMatrix::GetM21()
    {
        return 0.0;
    }

    void TransformMatrix::SetM21(double value)
    {
    }

    double TransformMatrix::GetM22()
    {
        return 0.0;
    }

    void TransformMatrix::SetM22(double value)
    {
    }

    double TransformMatrix::GetDX()
    {
        return 0.0;
    }

    void TransformMatrix::SetDX(double value)
    {
    }

    double TransformMatrix::GetDY()
    {
        return 0.0;
    }

    void TransformMatrix::SetDY(double value)
    {
    }

    bool TransformMatrix::GetIsIdentity()
    {
        return false;
    }

    void TransformMatrix::Reset()
    {
    }

    void TransformMatrix::Multiply(TransformMatrix* matrix)
    {
    }

    void TransformMatrix::Translate(double offsetX, double offsetY)
    {
    }

    void TransformMatrix::Scale(double scaleX, double scaleY)
    {
    }

    void TransformMatrix::Rotate(double angle)
    {
    }

    void TransformMatrix::Invert()
    {
    }

    Point TransformMatrix::TransformPoint(const Point& point)
    {
        return Point();
    }

    Size TransformMatrix::TransformSize(const Size& size)
    {
        return Size();
    }

    bool TransformMatrix::IsEqualTo(TransformMatrix* other)
    {
        return false;
    }

    int TransformMatrix::GetHashCode_()
    {
        return 0;
    }
}
