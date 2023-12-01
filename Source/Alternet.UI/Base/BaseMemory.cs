using System;
using System.Runtime.CompilerServices;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods to work with native memory buffers.
    /// </summary>
    public static class BaseMemory
    {
        /// <summary>
        /// Allocates size bytes of uninitialized storage.
        /// </summary>
        /// <param name="size">Number of bytes to allocate.</param>
        /// <returns>
        /// On success, returns the pointer to the beginning of newly allocated memory. To avoid a memory leak,
        /// the returned pointer must be deallocated with <see cref="FreeMem"/> or <see cref="Realloc"/>.
        /// On failure, returns a null pointer.
        /// </returns>
        /// <remarks>
        /// If allocation succeeds, returns a pointer to the lowest (first) byte in the allocated memory block
        /// that is suitably aligned for any scalar type
        /// (implicitly creating objects in the destination area).
        /// </remarks>
        /// <remarks>
        /// If <paramref name="size"/> is zero, the behavior is implementation defined
        /// (null pointer may be returned, or some non-null pointer may be returned that may not be
        /// used to access storage, but has to be passed to <see cref="FreeMem"/>).
        /// </remarks>
        /// <remarks>
        /// This function does not call constructors or initialize memory in any way.
        /// There are no ready-to-use smart pointers that could guarantee that the matching deallocation
        /// function is called.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Alloc(int size)
        {
            return Native.WxOtherFactory.MemoryAlloc((ulong)size);
        }

        /// <summary>
        /// Reallocates the given area of memory (implicitly creating objects in the destination area).
        /// It must be previously allocated by <see cref="Alloc"/> and not yet freed with <see cref="FreeMem"/>,
        /// otherwise, the results are undefined.
        /// </summary>
        /// <param name="ptr">Pointer to the object to realloc.</param>
        /// <param name="newSize">Number of bytes to allocate.</param>
        /// <returns>
        /// On success, returns a pointer to the beginning of newly allocated memory. To avoid a memory leak,
        /// the returned pointer must be deallocated with <see cref="FreeMem"/> or <see cref="Realloc"/>.
        /// The original pointer <paramref name="ptr"/> is invalidated and any access to it is undefined behavior
        /// (even if reallocation was in-place). On failure, returns a null pointer. The original pointer <paramref name="ptr"/>
        /// remains valid and may need to be deallocated with <see cref="FreeMem"/>.
        /// </returns>
        /// <remarks>
        /// The reallocation is done by either: Method 1. Expanding or contracting the existing area pointed to by
        /// <paramref name="ptr"/>, if possible. The contents of the area remain unchanged up to the lesser
        /// of the new and old sizes. If the area is expanded, the contents of the new part of the array are undefined.
        /// Method 2. Allocating a new memory block of size <paramref name="newSize"/> bytes, copying memory area with
        /// size equal the lesser of the new and the old sizes, and freeing the old block.
        /// If there is not enough memory, the old memory block is not freed and null pointer is returned.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="ptr"/> is a null pointer, the behavior is the same as calling <see cref="Alloc"/> with the
        /// <paramref name="newSize"/> parameter value.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="newSize"/> is zero, the behavior is implementation defined: null pointer may be
        /// returned (in which case the old memory block may or may not be freed) or some non-null pointer may
        /// be returned that may not be used to access storage. Such usage is deprecated.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Realloc(IntPtr ptr, int newSize) => Native.WxOtherFactory.MemoryRealloc(ptr, (ulong)newSize);

        /// <summary>
        /// Deallocates the space previously allocated by <see cref="Alloc"/> or <see cref="Realloc"/>.
        /// </summary>
        /// <param name="ptr">Pointer to the memory to deallocate.</param>
        /// <remarks>
        /// If <paramref name="ptr"/> is a null pointer, the function does nothing.
        /// The behavior is undefined if the value of ptr does not equal a value returned earlier.
        /// by <see cref="Alloc"/> or <see cref="Realloc"/>.
        /// The behavior is undefined if the memory area referred to by <paramref name="ptr"/> has already been deallocated.
        /// The behavior is undefined if after <see cref="FreeMem"/> returns, an access is made through the pointer
        /// <paramref name="ptr"/> (unless another allocation function happened to result in a pointer value equal to ptr).
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FreeMem(IntPtr ptr) => Native.WxOtherFactory.MemoryFree(ptr);

        /// <summary>
        /// Copies <paramref name="count"/> bytes from the object pointed to by <paramref name="src"/> to the
        /// object pointed to by <paramref name="dest"/>. Both objects are reinterpreted as arrays of byte.
        /// </summary>
        /// <param name="dest">Pointer to the memory location to copy to.</param>
        /// <param name="src">Pointer to the memory location to copy from.</param>
        /// <param name="count">Number of bytes to copy.</param>
        /// <returns><paramref name="dest"/></returns>
        /// <remarks>
        /// If the objects overlap, the behavior is undefined.
        /// If either <paramref name="dest"/> or <paramref name="src"/> is an invalid or null pointer,
        /// the behavior is undefined, even if <paramref name="count"/> is zero.
        /// If the objects are potentially-overlapping or not trivially copyable, the behavior of <see cref="Copy"/>
        /// is not specified and may be undefined.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Copy(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, (ulong)count);

        /// <summary>
        /// Copies <paramref name="count"/> bytes from the object pointed to by <paramref name="src"/> to the object
        /// pointed to by <paramref name="dest"/>. Both objects are reinterpreted as arrays of byte.
        /// </summary>
        /// <param name="dest">Pointer to the memory location to copy to.</param>
        /// <param name="src">Pointer to the memory location to copy from.</param>
        /// <param name="count">Number of bytes to copy</param>
        /// <returns></returns>
        /// <remarks>
        /// The objects may overlap: copying takes place as if the bytes were copied to a temporary byte array and
        /// then the bytes were copied from the array to <paramref name="dest"/>.
        /// If either <paramref name="dest"/> or <paramref name="src"/> is an invalid or null pointer, the behavior
        /// is undefined, even if <paramref name="count"/> is zero.
        /// If the objects are potentially-overlapping or not trivially copyable, the behavior is not specified and
        /// may be undefined.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Move(IntPtr dest, IntPtr src, int count)
            => Native.WxOtherFactory.MemoryMove(dest, src, (ulong)count);

        /// <summary>
        /// Copies the <paramref name="fillByte"/> into each of the first count bytes of the object
        /// pointed to by <paramref name="dest"/>.
        /// </summary>
        /// <param name="dest">Pointer to the object to fill.</param>
        /// <param name="fillByte">Fill byte (0..255).</param>
        /// <param name="count">Number of bytes to fill.</param>
        /// <returns><paramref name="dest"/></returns>
        /// <remarks>
        /// If the object is a potentially-overlapping subobject
        /// or is not trivially copyable, the behavior is undefined. If <paramref name="count"/> is greater
        /// than the size of the object pointed to by dest, the behavior is undefined.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr Fill(IntPtr dest, byte fillByte, int count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, (ulong)count);

        /// <inheritdoc cref="AllocLong"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr AllocLong(ulong size)
        {
            return Native.WxOtherFactory.MemoryAlloc(size);
        }

        /// <inheritdoc cref="Realloc"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr ReallocLong(IntPtr ptr, ulong newSize) => Native.WxOtherFactory.MemoryRealloc(ptr, newSize);

        /// <inheritdoc cref="Copy"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr CopyLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryCopy(dest, src, count);

        /// <inheritdoc cref="Move"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr MoveLong(IntPtr dest, IntPtr src, ulong count)
            => Native.WxOtherFactory.MemoryMove(dest, src, count);

        /// <inheritdoc cref="Fill"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr FillLong(IntPtr dest, byte fillByte, ulong count) =>
            Native.WxOtherFactory.MemorySet(dest, fillByte, count);
    }
}
