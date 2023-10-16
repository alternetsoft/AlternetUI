using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the <see cref="ISizerFactory"/> interface.
    /// </summary>
    public class SizerFactory : ISizerFactory
    {
        private readonly bool disposeHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SizerFactory"/> class.
        /// </summary>
        /// <param name="disposeHandle">Whether to dispose created sizers automatically.</param>
        public SizerFactory(bool disposeHandle = false)
        {
            this.disposeHandle = disposeHandle;
        }

        /// <summary>
        /// Gets or sets default <see cref="ISizerFactory"/> provider.
        /// </summary>
        public static ISizerFactory Default { get; set; } = new SizerFactory(false);

        /// <inheritdoc cref="ISizerFactory.CreateSizerFlags"/>
        public virtual ISizerFlags CreateSizerFlags(int proportion = 0)
        {
            return new SizerFlags(proportion);
        }

        /// <inheritdoc cref="ISizerFactory.CreateFlexGridSizer(int, int, int)"/>
        public virtual IFlexGridSizer CreateFlexGridSizer(int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateFlexGridSizer(int, int, int, int)"/>
        public virtual IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridBagSizer"/>
        public virtual IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0)
        {
            return new GridBagSizer(vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateBoxSizer"/>
        public virtual IBoxSizer CreateBoxSizer(bool isVertical)
        {
            return new BoxSizer(isVertical, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridSizer(int, int, int)"/>
        public virtual IGridSizer CreateGridSizer(int cols, int vgap, int hgap)
        {
            return new GridSizer(cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridSizer(int, int, int, int)"/>
        public virtual IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new GridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateWrapSizer"/>
        public virtual IWrapSizer CreateWrapSizer(
            bool isVertical,
            WrapSizerFlag flags = WrapSizerFlag.Default)
        {
            return new WrapSizer(isVertical, flags, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(Control, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(window, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(Control, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(Control window, ISizerFlags sizerFlags)
        {
            return new SizerItem(window, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(ISizer, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(sizer, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(ISizer, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(ISizer sizer, ISizerFlags sizerFlags)
        {
            return new SizerItem(sizer, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(int, int, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(width, height, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(int, int, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(int width, int height, ISizerFlags sizerFlags)
        {
            return new SizerItem(width, height, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem()"/>
        public virtual ISizerItem CreateSizerItem()
        {
            return new SizerItem();
        }
    }
}