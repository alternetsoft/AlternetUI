using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    }
}
