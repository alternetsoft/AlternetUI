using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal interface ISizerFactory
    {
        IFlexGridSizer CreateFlexGridSizer(int cols, int vgap, int hgap);

        IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap, int hgap);

        IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0);

        IBoxSizer CreateBoxSizer(bool isVertical);

        IGridSizer CreateGridSizer(int cols, int vgap, int hgap);

        IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap);

        IWrapSizer CreateWrapSizer(bool isVertical, WrapSizerFlags flags = WrapSizerFlags.Default);
    }
}
