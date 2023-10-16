using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class FlexGridSizer : GridSizer, IFlexGridSizer
    {
        public FlexGridSizer(int cols, int vgap, int hgap, bool disposeHandle)
            : this(
                  Native.FlexGridSizer.CreateFlexGridSizer(cols, vgap, hgap),
                  disposeHandle)
        {
        }

        public FlexGridSizer(int rows, int cols, int vgap, int hgap, bool disposeHandle)
            : this(
                  Native.FlexGridSizer.CreateFlexGridSizer2(rows, cols, vgap, hgap),
                  disposeHandle)
        {
        }

        internal FlexGridSizer(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public GenericOrientation FlexibleDirection
        {
            get
            {
                return (GenericOrientation)Native.FlexGridSizer.GetFlexibleDirection(Handle);
            }

            set
            {
                Native.FlexGridSizer.SetFlexibleDirection(Handle, (int)value);
            }
        }

        public FlexSizerGrowMode NonFlexibleGrowMode
        {
            get
            {
                return (FlexSizerGrowMode)Native.FlexGridSizer.GetNonFlexibleGrowMode(Handle);
            }

            set
            {
                Native.FlexGridSizer.SetNonFlexibleGrowMode(Handle, (int)value);
            }
        }

        public void AddGrowableCol(int idx, int proportion = 0)
        {
            Native.FlexGridSizer.AddGrowableCol(Handle, idx, proportion);
        }

        public void AddGrowableRow(int idx, int proportion = 0)
        {
            Native.FlexGridSizer.AddGrowableRow(Handle, idx, proportion);
        }

        public bool IsColGrowable(int idx)
        {
            return Native.FlexGridSizer.IsColGrowable(Handle, idx);
        }

        public bool IsRowGrowable(int idx)
        {
            return Native.FlexGridSizer.IsRowGrowable(Handle, idx);
        }

        public void RemoveGrowableCol(int idx)
        {
            Native.FlexGridSizer.RemoveGrowableCol(Handle, idx);
        }

        public void RemoveGrowableRow(int idx)
        {
            Native.FlexGridSizer.RemoveGrowableRow(Handle, idx);
        }
    }
}
