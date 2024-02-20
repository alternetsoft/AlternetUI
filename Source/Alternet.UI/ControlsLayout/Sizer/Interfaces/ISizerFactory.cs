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
    internal interface ISizerFactory
    {
        /// <summary>
        /// Creates default <see cref="ISizerFlags"/> implementation.
        /// </summary>
        /// <param name="proportion"></param>
        ISizerFlags CreateSizerFlags(int proportion = 0);

        /// <summary>
        /// Creates default <see cref="IFlexGridSizer"/> implementation.
        /// </summary>
        /// <param name="cols">Number of columns.</param>
        /// <param name="vgap">The size of the padding between the rows, in pixels.</param>
        /// <param name="hgap">The size of the padding between the columns, in pixels.</param>
        /// <remarks>
        /// The number of rows will be deduced automatically depending
        /// on the number of the elements added to the sizer.
        /// </remarks>
        IFlexGridSizer CreateFlexGridSizer(int cols, int vgap = 0, int hgap = 0);

        /// <summary>
        /// Creates default <see cref="IFlexGridSizer"/> implementation.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="vgap">The size of the padding between the rows, in pixels.</param>
        /// <param name="hgap">The size of the padding between the columns, in pixels.</param>
        /// <remarks>
        /// If the value of rows argument is not zero the sizer will check that no more
        /// than cols * rows elements are added to it, i.e.that no more than the given
        /// number of rows is used.Adding less than maximally allowed number of items
        /// is not an error however.
        /// </remarks>
        /// <remarks>
        /// It is also possible to specify the number of rows and use 0 for cols.
        /// In this case, the sizer will use the given fixed number of rows and
        /// as many columns as necessary.
        /// </remarks>
        IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap = 0, int hgap = 0);

        /// <summary>
        /// Creates default <see cref="IGridBagSizer"/> implementation.
        /// </summary>
        /// <param name="vgap">The size of the padding between the rows, in pixels.</param>
        /// <param name="hgap">The size of the padding between the columns, in pixels.</param>
        IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0);

        /// <summary>
        /// Creates default <see cref="IBoxSizer"/> implementation.
        /// </summary>
        /// <param name="isVertical">Specifies whether sizer is vertical or horizontal.</param>
        IBoxSizer CreateBoxSizer(bool isVertical);

        /// <summary>
        /// Creates default <see cref="IGridSizer"/> implementation.
        /// </summary>
        /// <param name="cols">Number of columns.</param>
        /// <param name="vgap">The size of the padding between the rows, in pixels.</param>
        /// <param name="hgap">The size of the padding between the columns, in pixels.</param>
        /// <remarks>
        /// The number of rows will be deduced automatically depending
        /// on the number of the elements added to the sizer.
        /// </remarks>
        IGridSizer CreateGridSizer(int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IGridSizer"/> implementation.
        /// </summary>
        /// <param name="rows">Number of rows.</param>
        /// <param name="cols">Number of columns.</param>
        /// <param name="vgap">The size of the padding between the rows, in pixels.</param>
        /// <param name="hgap">The size of the padding between the columns, in pixels.</param>
        /// <remarks>
        /// If the value of rows argument is not zero the sizer will check that no more
        /// than cols * rows elements are added to it, i.e.that no more than the given
        /// number of rows is used.Adding less than maximally allowed number of items
        /// is not an error however.
        /// </remarks>
        /// <remarks>
        /// It is also possible to specify the number of rows and use 0 for cols.
        /// In this case, the sizer will use the given fixed number of rows and
        /// as many columns as necessary.
        /// </remarks>
        IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap);

        /// <summary>
        /// Creates default <see cref="IWrapSizer"/> implementation.
        /// </summary>
        /// <param name="isVertical">Specifies whether sizer is vertical or horizontal.</param>
        /// <param name="flags">Sizer flags.</param>
        IWrapSizer CreateWrapSizer(bool isVertical, WrapSizerFlag flags = WrapSizerFlag.Default);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a control.
        /// </summary>
        /// <param name="control">Control which will be affected by the item.</param>
        /// <param name="proportion"></param>
        /// <param name="flag">Sizer flags.</param>
        /// <param name="border"></param>
        ISizerItem CreateSizerItem(
            Control control,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a control.
        /// </summary>
        /// <param name="control">Control which will be affected by the item.</param>
        /// <param name="flags">Sizer flags.</param>
        ISizerItem CreateSizerItem(Control control, ISizerFlags flags);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a subsizer.
        /// </summary>
        /// <param name="sizer">Sub-sizer.</param>
        /// <param name="proportion"></param>
        /// <param name="flag">Sizer flags.</param>
        /// <param name="border"></param>
        ISizerItem CreateSizerItem(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a subsizer.
        /// </summary>
        /// <param name="sizer">Sub-sizer.</param>
        /// <param name="flags">Sizer flags.</param>
        ISizerItem CreateSizerItem(ISizer sizer, ISizerFlags flags);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a spacer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="proportion"></param>
        /// <param name="flags">Sizer flags.</param>
        /// <param name="border"></param>
        ISizerItem CreateSizerItem(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flags = 0,
            int border = 0);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation with the specified parameters.
        /// Constructs a sizer item for tracking a spacer.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sizerFlags"></param>
        ISizerItem CreateSizerItem(int width, int height, ISizerFlags sizerFlags);

        /// <summary>
        /// Creates default <see cref="ISizerItem"/> implementation.
        /// </summary>
        ISizerItem CreateSizerItem();
    }
}
