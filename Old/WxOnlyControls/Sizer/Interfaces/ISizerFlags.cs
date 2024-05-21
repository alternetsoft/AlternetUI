using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to specify <see cref="SizerFlag"/> in the convenient way.
    /// </summary>
    internal interface ISizerFlags : IDisposableObject
    {
        /// <summary>
        /// Gets the border used by default in <see cref="Border(GenericDirection)"/> method.
        /// </summary>
        /// <remarks>
        /// This value is scaled appropriately for the current DPI on the systems where physical
        /// pixel values are used for the control positions and sizes, i.e.not with Linux or MacOs.
        /// </remarks>
        int GetDefaultBorder();

        /// <summary>
        /// Gets the border used by default in <see cref="Border(GenericDirection)"/> method,
        /// with fractional precision. This is useful when the border is scaled to
        /// a non-integer DPI.
        /// </summary>
        float GetDefaultBorderFractional();

        /// <summary>
        /// Gets current proportion value.
        /// </summary>
        int GetProportion();

        /// <summary>
        /// Gets current flags value.
        /// </summary>
        /// <returns></returns>
        SizerFlag GetFlags();

        /// <summary>
        /// Gets current border width value.
        /// </summary>
        /// <returns></returns>
        int GetBorderInPixels();

        /// <summary>
        /// Sets the proportion of this <see cref="ISizerFlags"/> to <paramref name="proportion"/>.
        /// </summary>
        /// <param name="proportion">New proportion value.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Proportion(int proportion);

        /// <summary>
        /// Sets the object of the <see cref="ISizerFlags"/> to expand to fill as much area as it can.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Expand();

        /// <summary>
        /// Sets the alignment.
        /// </summary>
        /// <param name="alignment">New alignment value.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Align(GenericAlignment alignment);

        /// <summary>
        /// Sets the object of the <see cref="ISizerFlags"/> to center itself in the area it is given.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Center();

        /// <summary>
        /// Center an item only in vertical direction.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// The remarks in <see cref="CenterHorizontal"/> documentation also apply to this function.
        /// </remarks>
        /// <remarks>
        /// Note that, unlike <see cref="Align"/>, this method doesn't change the vertical alignment.
        /// </remarks>
        ISizerFlags CenterVertical();

        /// <summary>
        /// Center an item only in horizontal direction.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// This is mostly useful for 2D sizers as for the 1D ones it is shorter to just
        /// use <see cref="Center"/> as the alignment is only used in one direction with
        /// them anyhow. For 2D sizers, centering an item in one direction is quite different
        /// from centering it in both directions however.
        /// </remarks>
        /// <remarks>
        /// Note that, unlike <see cref="Align(GenericAlignment)"/>, this method doesn't
        /// change the vertical alignment.
        /// </remarks>
        ISizerFlags CenterHorizontal();

        /// <summary>
        /// Aligns the object to the top, similar for <see cref="Align(GenericAlignment)"/> with
        /// <see cref="GenericAlignment.Top"/>.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// Unlike <see cref="Align(GenericAlignment)"/>, this method doesn't change the horizontal
        /// alignment of the item.
        /// </remarks>
        ISizerFlags Top();

        /// <summary>
        /// Aligns the object to the left, similar for <see cref="Align(GenericAlignment)"/> with
        /// <see cref="GenericAlignment.Left"/>.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="Align(GenericAlignment)"/>, this method doesn't change the vertical
        /// alignment of the item.
        /// </remarks>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Left();

        /// <summary>
        /// Aligns the object to the right, similar for <see cref="Align(GenericAlignment)"/> with
        /// <see cref="GenericAlignment.Right"/>.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="Align(GenericAlignment)"/>, this method doesn't change the vertical
        /// alignment of the item.
        /// </remarks>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Right();

        /// <summary>
        /// Aligns the object to the bottom, similar for <see cref="Align(GenericAlignment)"/> with
        /// <see cref="GenericAlignment.Bottom"/>.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// Unlike <see cref="Align(GenericAlignment)"/>, this method doesn't change the horizontal
        /// alignment of the item.
        /// </remarks>
        ISizerFlags Bottom();

        /// <summary>
        /// Sets the <see cref="ISizerFlags"/> to have a border of a number of pixels specified
        /// by <paramref name="borderInPixels"/> with the directions specified by
        /// <paramref name="direction"/>.
        /// </summary>
        /// <param name="direction">Sides to which border is applied.</param>
        /// <param name="borderInPixels">Border width in pixels.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// Prefer to use <see cref="Border(GenericDirection)"/> or <see cref="DoubleBorder"/>
        /// or <see cref="TripleBorder"/> versions instead of hard-coding the border value
        /// in pixels to avoid too small borders on devices with high DPI displays.
        /// </remarks>
        ISizerFlags Border(GenericDirection direction, int borderInPixels);

        /// <summary>
        /// Sets the <see cref="ISizerFlags"/> to have a border with size as returned by
        /// <see cref="GetDefaultBorder"/>.
        /// </summary>
        /// <param name="direction">Sides to which border is applied.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Border(GenericDirection direction = GenericDirection.All);

        /// <summary>
        /// Sets the border in the given direction having twice the default border size.
        /// </summary>
        /// <param name="direction">Sides to which border is applied.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags DoubleBorder(GenericDirection direction = GenericDirection.All);

        /// <summary>
        /// Sets the border in the given direction having 3X of the default border size.
        /// </summary>
        /// <param name="direction">Sides to which border is applied.</param>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags TripleBorder(GenericDirection direction = GenericDirection.All);

        /// <summary>
        /// Sets the border in left and right directions having the default border size.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags HorzBorder();

        /// <summary>
        /// Sets the border in left and right directions having twice the default border size.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags DoubleHorzBorder();

        /// <summary>
        /// Set the <see cref="SizerFlag.Shaped"/> flag which indicates that the elements should
        /// always keep the fixed width to height ratio equal to its original value.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags Shaped();

        /// <summary>
        /// Set the <see cref="SizerFlag.FixedMinSize"/> flag which indicates that the initial
        /// size of the control should be also set as its minimal size.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        ISizerFlags FixedMinSize();

        /// <summary>
        /// Set the <see cref="SizerFlag.ReserveSpaceEvenIfHidden"/> flag.
        /// Makes the item ignore control's visibility status.
        /// </summary>
        /// <returns>
        /// This <see cref="ISizerFlags"/> instance allowing chaining multiple methods calls.
        /// </returns>
        /// <remarks>
        /// Normally sizers don't allocate space for hidden controls or other items.
        /// This flag overrides this behaviour so that sufficient space is allocated for
        /// the control even if it isn't visible. This makes it possible to dynamically
        /// show and hide controls without resizing parent dialog, for example.
        /// </remarks>
        ISizerFlags ReserveSpaceEvenIfHidden();
    }
}