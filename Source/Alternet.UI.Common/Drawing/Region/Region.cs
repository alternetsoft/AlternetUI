using System;
using System.Runtime.CompilerServices;

namespace Alternet.Drawing
{
    /// <summary>
    /// Describes the interior of a graphics shape composed of rectangles and paths.
    /// </summary>
    public class Region : IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// This constructor creates an invalid (empty, null) <see cref="Region"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Region()
        {
            NativeObject = NativeDrawing.Default.CreateRegion();
        }

        /// <summary>
        /// Initializes a new <see cref="Region"/> from the specified <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="rect">A <see cref="RectD"/> structure that defines the interior
        /// of the new <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Region(RectD rect)
        {
            NativeObject = NativeDrawing.Default.CreateRegion(rect);
        }

        /// <summary>
        /// Initializes a new <see cref="Region"/> from the specified <see cref="Region"/>.
        /// </summary>
        /// <param name="region">A <see cref="Region"/> that defines the interior
        /// of the new <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Region(Region region)
        {
            NativeObject = NativeDrawing.Default.CreateRegion(region);
        }

        /// <summary>
        /// Initializes a new <see cref="Region"/> from the polygon specified by the
        /// <paramref name="points"/> array.
        /// </summary>
        /// <param name="points">A <see cref="PointD"/> structures array
        /// describing the polygon.</param>
        /// <param name="fillMode">The polygon fill mode.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Region(PointD[] points, FillMode fillMode = FillMode.Alternate)
        {
            NativeObject = NativeDrawing.Default.CreateRegion(points, fillMode);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="region">Native region instance.</param>
        public Region(object region)
        {
            NativeObject = region;
        }

        /// <summary>
        /// Returns true if the region is empty, false otherwise.
        /// </summary>
        public bool IsEmpty => isDisposed || NativeDrawing.Default.RegionIsEmpty(this);

        /// <summary>
        /// Returns true if the region is ok, false otherwise.
        /// </summary>
        public bool IsOk => !isDisposed && NativeDrawing.Default.RegionIsOk(this);

        /// <summary>
        /// Gets native region.
        /// </summary>
        public object NativeObject { get; private set; }

        /// <summary>
        /// Clears the region.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            CheckDisposed();
            NativeDrawing.Default.RegionClear(this);
        }

        /// <summary>
        /// Returns a value indicating whether the given point is contained within the region.
        /// </summary>
        /// <param name="pt">Point to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RegionContain Contains(PointD pt)
        {
            CheckDisposed();
            return NativeDrawing.Default.RegionContains(this, pt);
        }

        /// <summary>
        /// Returns a value indicating whether the given rectangle is contained within the region.
        /// </summary>
        /// <param name="rect">Rectangle to check.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RegionContain Contains(RectD rect)
        {
            CheckDisposed();
            return NativeDrawing.Default.RegionContains(this, rect);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the intersection of itself with the
        /// specified <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="rect">The <see cref="RectD"/> structure to intersect with this
        /// <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersect(RectD rect)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionIntersect(this, rect);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the intersection of itself with the
        /// specified <see cref="Region"/>.
        /// </summary>
        /// <param name="region">The <see cref="Region"/> to intersect
        /// with this <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Intersect(Region region)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionIntersect(this, region);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the union of itself with the
        /// specified <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="rect">The <see cref="RectD"/> structure to union with
        /// this <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Union(RectD rect)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionUnion(this, rect);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the union of itself with the
        /// specified <see cref="Region"/>.
        /// </summary>
        /// <param name="region">The <see cref="Region"/> to union with this
        /// <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Union(Region region)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionUnion(this, region);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the xor of itself with the
        /// specified <see cref="Region"/>.
        /// </summary>
        /// <param name="region">The <see cref="Region"/> to xor with this
        /// <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Xor(Region region)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionXor(this, region);
        }

        /// <summary>
        /// Updates this <see cref="Region"/> to the xor of itself with the
        /// specified <see cref="RectD"/> structure.
        /// </summary>
        /// <param name="rect">The <see cref="RectD"/> structure to xor with
        /// this <see cref="Region"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Xor(RectD rect)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionXor(this, rect);
        }

        /// <summary>
        /// Subtracts a rectangular region from this region.
        /// </summary>
        /// <param name="rect">Rectangular region to subtract.</param>
        /// <remarks>
        /// This operation combines the parts of 'this' region that are not part of
        /// the second region. The result is stored in this region.
        /// </remarks>
        /// <remarks>
        /// This method always fails, i.e.returns false, if this region is invalid but
        /// may nevertheless be safely used even in this case.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Subtract(RectD rect)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionSubtract(this, rect);
        }

        /// <summary>
        /// Subtracts a region from this region.
        /// </summary>
        /// <param name="region">Region to subtract.</param>
        /// <remarks>
        /// This operation combines the parts of 'this' region that are not part of
        /// the second region. The result is stored in this region.
        /// </remarks>
        /// <remarks>
        /// This method always fails, i.e.returns false, if this region is invalid but
        /// may nevertheless be safely used even in this case.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Subtract(Region region)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionSubtract(this, region);
        }

        /// <summary>
        /// Offsets the coordinates of this <see cref="Region"/> by the specified amount.
        /// </summary>
        /// <param name="dx">The amount to offset this <see cref="Region"/> horizontally.</param>
        /// <param name="dy">The amount to offset this <see cref="Region"/> vertically.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Translate(double dx, double dy)
        {
            CheckDisposed();
            NativeDrawing.Default.RegionTranslate(this, dx, dy);
        }

        /// <summary>
        /// Gets a <see cref="RectD"/> structure that represents a rectangle that
        /// bounds this <see cref="Region"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RectD GetBounds()
        {
            CheckDisposed();
            return NativeDrawing.Default.RegionGetBounds(this);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            CheckDisposed();

            if (obj is not Region region)
                return false;

            return NativeDrawing.Default.RegionEquals(this, region);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return NativeDrawing.Default.RegionGetHashCode(this);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="TransformMatrix"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the object has been disposed.
        /// </summary>
        private void CheckDisposed()
        {
            if (isDisposed)
                throw new ObjectDisposedException(null);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    ((IDisposable)NativeObject).Dispose();
                    NativeObject = null!;
                }

                isDisposed = true;
            }
        }
    }
}