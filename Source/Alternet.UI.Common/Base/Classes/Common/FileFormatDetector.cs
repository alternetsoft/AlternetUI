using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides methods to detect file formats based on their signatures.
    /// </summary>
    public static class FileFormatDetector
    {
        /// <summary>
        /// Detects if the given byte array represents an '.ani' data (animated cursor data).
        /// </summary>
        /// <param name="data">The byte array to check.</param>
        /// <returns><c>true</c> if the byte array represents an '.ani' data (animated cursor data); otherwise, <c>false</c>.</returns>
        public static bool IsAni(byte[] data)
        {
            // ANI files are RIFF containers with "ACON" at offset 8
            if (data == null || data.Length < 12)
                return false;

            // RIFF header
            bool riff = data[0] == (byte)'R' &&
                        data[1] == (byte)'I' &&
                        data[2] == (byte)'F' &&
                        data[3] == (byte)'F';

            // "ACON" signature at offset 8
            bool acon = data[8] == (byte)'A' &&
                        data[9] == (byte)'C' &&
                        data[10] == (byte)'O' &&
                        data[11] == (byte)'N';

            return riff && acon;
        }

        /// <summary>
        /// Determines if the given byte array represents an '.ico' data (icon data).
        /// </summary>
        /// <param name="data">The byte array to check.</param>
        /// <returns><c>true</c> if the byte array represents an '.ico' data (icon data); otherwise, <c>false</c>.</returns>
        public static bool IsIco(byte[] data)
        {
            return CheckIcoHeader(data, expectedType: 1);
        }

        /// <summary>
        /// Determines if the given byte array represents a '.cur' data (cursor data).
        /// </summary>
        /// <param name="data">The byte array to check.</param>
        /// <returns><c>true</c> if the byte array represents a '.cur' data (cursor data); otherwise, <c>false</c>.</returns>
        public static bool IsCursor(byte[] data)
        {
            return CheckIcoHeader(data, expectedType: 2);
        }

        /// <summary>
        /// Checks if the provided byte array represents an ICO or CURSOR image by verifying the ICO/CURSOR header.
        /// </summary>
        /// <param name="data">The byte array to check.</param>
        /// <returns><c>true</c> if the byte array represents an ICO or CURSOR image; otherwise, <c>false</c>.</returns>
        public static bool IsIcoOrCursor(byte[] data)
        {
            return CheckIcoHeader(data, expectedType: null);
        }

        /// <summary>
        /// Checks if the provided byte array represents a PNG image by verifying the PNG signature.
        /// </summary>
        /// <param name="data">The byte array to check.</param>
        /// <returns><c>true</c> if the byte array represents a PNG image; otherwise, <c>false</c>.</returns>
        public static bool IsPng(byte[] data)
        {
            // PNG signature: 89 50 4E 47 0D 0A 1A 0A
            return data.Length > 8 &&
                   data[0] == 0x89 &&
                   data[1] == 0x50 &&
                   data[2] == 0x4E &&
                   data[3] == 0x47 &&
                   data[4] == 0x0D &&
                   data[5] == 0x0A &&
                   data[6] == 0x1A &&
                   data[7] == 0x0A;
        }

        private static bool CheckIcoHeader(byte[] data, int? expectedType)
        {
            if (data == null || data.Length < 6)
                return false;

            ushort reserved = BitConverter.ToUInt16(data, 0);
            ushort type = BitConverter.ToUInt16(data, 2);
            ushort count = BitConverter.ToUInt16(data, 4);

            if (reserved != 0 || count == 0)
                return false;

            if (expectedType.HasValue)
                return type == expectedType.Value;
            else
                return type == 1 || type == 2;
        }
    }
}
