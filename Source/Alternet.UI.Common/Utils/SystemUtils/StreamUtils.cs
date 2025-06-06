﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Stream"/> related static methods.
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Gets an empty stream.
        /// </summary>
        public static readonly Stream Empty = new MemoryStream();

        private static volatile Encoding? uTF8NoBOM;

        internal static Encoding UTF8NoBOM
        {
            get
            {
                if (uTF8NoBOM == null)
                {
                    UTF8Encoding noBOM = new(false, true);
                    Thread.MemoryBarrier();
                    uTF8NoBOM = noBOM;
                }

                return uTF8NoBOM;
            }
        }

        /// <summary>
        /// Creates <see cref="MemoryStream"/> with data copied from the specified stream.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        /// <returns></returns>
        public static MemoryStream CreateMemoryStream(Stream? stream)
        {
            MemoryStream memoryStream = new();

            if (stream is null)
                return memoryStream;

            stream.CopyTo(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        /// <summary>
        /// Compares two streams using byte to byte comparison.
        /// </summary>
        /// <param name="stream1">First stream to compare.</param>
        /// <param name="stream2">Second stream to compare.</param>
        /// <returns></returns>
        public static bool AreEqual(Stream stream1, Stream stream2)
        {
            if (stream1.Length != stream2.Length)
                return false;

            stream1.Position = 0;
            stream2.Position = 0;

            var bufferSize = 4096;

            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                var numRead1 = stream1.Read(buffer1, 0, bufferSize);
                var numRead2 = stream2.Read(buffer2, 0, bufferSize);

                if (numRead1 != numRead2)
                    return false;

                if(numRead1 == bufferSize)
                {
                    if (!ArrayUtils.AreEqual(buffer1, buffer2))
                        return false;
                }
                else
                {
                    if(ArrayUtils.AreEqual(buffer1, buffer2, 0, numRead1))
                        return true;
                    return false;
                }
            }
        }

        /// <summary>
        /// Converts a Base64 encoded string to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="base64String">The Base64 encoded string to convert.</param>
        /// <returns>A <see cref="MemoryStream"/> containing the decoded data.</returns>
        public static MemoryStream ConvertBase64ToStream(string base64String)
        {
            byte[] byteArray = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new(byteArray);
            return memoryStream;
        }

        /// <summary>
        /// Converts a <see cref="MemoryStream"/> to a Base64 encoded string.
        /// </summary>
        /// <param name="stream">The <see cref="MemoryStream"/> to convert.</param>
        /// <returns>A Base64 encoded string representing the content of the stream.</returns>
        public static string ConvertStreamToBase64(MemoryStream stream)
        {
            return Convert.ToBase64String(stream.ToArray());
        }

        /// <summary>
        /// Compares two memory streams using byte to byte comparison.
        /// </summary>
        /// <param name="stream1">First stream to compare.</param>
        /// <param name="stream2">Second stream to compare.</param>
        /// <returns></returns>
        public static bool AreEqual(MemoryStream stream1, MemoryStream stream2)
        {
            if (stream1.Length != stream2.Length)
                return false;
            stream1.Position = 0;
            stream2.Position = 0;

            var buffer1 = stream1.ToArray();
            var buffer2 = stream2.ToArray();

            return ArrayUtils.AreEqual(buffer1, buffer2);
        }

        /// <summary>
        /// Reads <see cref="string"/> from <see cref="Stream"/> using the specified encoding.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        /// <param name="encoding">Stream encoding. Optional. If not specified,
        /// <see cref="Encoding.UTF8"/> is used.</param>
        public static string StringFromStream(Stream stream, Encoding? encoding = null)
        {
            var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Reads a string from the specified stream using the specified encoding.
        /// Returns null if the stream is null or if an error occurs during reading.
        /// </summary>
        /// <param name="stream">The stream to read from. If null, the method returns null.</param>
        /// <param name="encoding">The encoding to use for reading the stream.
        /// Optional. If not specified, <see cref="Encoding.UTF8"/> is used.</param>
        /// <returns>The string read from the stream, or null if the stream
        /// is null or an error occurs.</returns>
        public static string? StringFromStreamOrNull(Stream? stream, Encoding? encoding = null)
        {
            if (stream is null)
                return null;
            try
            {
                var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);
                var result = reader.ReadToEnd();
                return result;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Writes <see cref="string"/> to <see cref="Stream"/> using the specified encoding.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        /// <param name="s">String with data.</param>
        /// <param name="encoding">Encoding to use for saving the string. Optional. If not specified,
        /// <see cref="Encoding.UTF8"/> is used.</param>
        public static void StringToStream(Stream stream, string s, Encoding? encoding = null)
        {
            var writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            writer.Write(s);
            writer.Flush();
            stream.Flush();
        }

        /// <summary>
        /// Copies stream to the file with the specified path.
        /// </summary>
        /// <param name="stream">Input stream to copy.</param>
        /// <param name="destPath">Destination file path.</param>
        public static void CopyStream(Stream stream, string destPath)
        {
            using var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write);
            stream.CopyTo(fileStream);
            fileStream.Flush();
            fileStream.Close();
        }

        /// <summary>
        /// Safely copies the content of a stream to a file at the specified destination path.
        /// Returns true if the operation succeeds, otherwise false.
        /// </summary>
        /// <param name="stream">The input stream to copy from.</param>
        /// <param name="destPath">The destination file path to copy to.</param>
        /// <returns>True if the copy operation succeeds, otherwise false.</returns>
        public static bool CopyStreamSafe(Stream stream, string destPath)
        {
            try
            {
                if (!FileUtils.CreateFilePathSafe(destPath))
                    return false;
                if(!FileUtils.DeleteIfExistsSafe(destPath))
                    return false;
                CopyStream(stream, destPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}