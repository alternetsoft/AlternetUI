#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class TransformMatrix : public Object
    {
#include "Api/TransformMatrix.inc"
    public:
        TransformMatrix(const wxAffineMatrix2D& matrix);

        wxAffineMatrix2D GetMatrix();

    private:

        int GetHashCode(double value);
        int CombineHashCodes(double v1, double v2, double v3, double v4, double v5, double v6);

        wxWindow* GetWindow();

        wxAffineMatrix2D _matrix;
    };
}
