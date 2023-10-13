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
            WrapSizerFlags flags = WrapSizerFlags.Default)
        {
            return new WrapSizer(isVertical, flags, disposeHandle);
        }
    }
}
