using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class GridSizer : Sizer, IGridSizer
    {
        public GridSizer(int cols, int vgap, int hgap, bool disposeHandle)
            : this(
                  Native.GridSizer.CreateGridSizer(cols, vgap, hgap),
                  disposeHandle)
        {
        }

        public GridSizer(int rows, int cols, int vgap, int hgap, bool disposeHandle)
            : this(
                  Native.GridSizer.CreateGridSizer2(rows, cols, vgap, hgap),
                  disposeHandle)
        {
        }

        internal GridSizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public int ColCount
        {
            get
            {
                return Native.GridSizer.GetCols(Handle);
            }

            set
            {
                Native.GridSizer.SetCols(Handle, value);
            }
        }

        public int RowCount
        {
            get
            {
                return Native.GridSizer.GetRows(Handle);
            }

            set
            {
                Native.GridSizer.SetRows(Handle, value);
            }
        }

        public int EffectiveColsCount
        {
            get
            {
                return Native.GridSizer.GetEffectiveColsCount(Handle);
            }
        }

        public int EffectiveRowsCount
        {
            get
            {
                return Native.GridSizer.GetEffectiveRowsCount(Handle);
            }
        }

        public int HGap
        {
            get
            {
                return Native.GridSizer.GetHGap(Handle);
            }

            set
            {
                Native.GridSizer.SetHGap(Handle, value);
            }
        }

        public int VGap
        {
            get
            {
                return Native.GridSizer.GetVGap(Handle);
            }

            set
            {
                Native.GridSizer.SetHGap(Handle, value);
            }
        }
    }
}
