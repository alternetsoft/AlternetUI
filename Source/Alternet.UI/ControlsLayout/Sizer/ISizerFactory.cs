using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Creates different <see cref="ISizer"/> implementations.
    /// </summary>
    public interface ISizerFactory
    {
        /// <summary>
        /// Creates default <see cref="IFlexGridSizer"/> implementation.
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="vgap"></param>
        /// <param name="hgap"></param>
        /// <returns></returns>
        IFlexGridSizer CreateFlexGridSizer(int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IFlexGridSizer"/> implementation.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="vgap"></param>
        /// <param name="hgap"></param>
        /// <returns></returns>
        IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IGridBagSizer"/> implementation.
        /// </summary>
        /// <param name="vgap"></param>
        /// <param name="hgap"></param>
        /// <returns></returns>
        IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0);

        /// <summary>
        /// Creates default <see cref="IBoxSizer"/> implementation.
        /// </summary>
        /// <param name="isVertical"></param>
        /// <returns></returns>
        IBoxSizer CreateBoxSizer(bool isVertical);

        /// <summary>
        /// Creates default <see cref="IGridSizer"/> implementation.
        /// </summary>
        /// <param name="cols"></param>
        /// <param name="vgap"></param>
        /// <param name="hgap"></param>
        /// <returns></returns>
        IGridSizer CreateGridSizer(int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IGridSizer"/> implementation.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
        /// <param name="vgap"></param>
        /// <param name="hgap"></param>
        /// <returns></returns>
        IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IWrapSizer"/> implementation.
        /// </summary>
        /// <param name="isVertical"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        IWrapSizer CreateWrapSizer(bool isVertical, WrapSizerFlags flags = WrapSizerFlags.Default);
    }
}
