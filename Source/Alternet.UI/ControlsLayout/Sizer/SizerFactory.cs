using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class SizerFactory : ISizerFactory
    {
        private readonly bool disposeHandle;

        public SizerFactory(bool disposeHandle = false)
        {
            this.disposeHandle = disposeHandle;
        }

        public static ISizerFactory Default { get; set; } = new SizerFactory(false);

        public IFlexGridSizer CreateFlexGridSizer(int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(cols, vgap, hgap, disposeHandle);
        }

        public IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        public IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0)
        {
            return new GridBagSizer(vgap, hgap, disposeHandle);
        }

        public IBoxSizer CreateBoxSizer(bool isVertical)
        {
            return new BoxSizer(isVertical, disposeHandle);
        }

        public IGridSizer CreateGridSizer(int cols, int vgap, int hgap)
        {
            return new GridSizer(cols, vgap, hgap, disposeHandle);
        }

        public IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new GridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        public IWrapSizer CreateWrapSizer(
            bool isVertical,
            WrapSizerFlags flags = WrapSizerFlags.Default)
        {
            return new WrapSizer(isVertical, flags, disposeHandle);
        }
    }
}
