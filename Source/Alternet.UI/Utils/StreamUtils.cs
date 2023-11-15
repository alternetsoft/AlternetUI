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
        /// Reads <see cref="string"/> from <see cref="Stream"/> using <see cref="Encoding.UTF8"/>
        /// encoding.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        public static string StringFromStream(Stream stream)
        {
            var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Writes <see cref="string"/> to <see cref="Stream"/> using <see cref="Encoding.UTF8"/>
        /// encoding.
        /// </summary>
        /// <param name="stream">Stream with data.</param>
        /// <param name="s">String with data.</param>
        public static void StringToStream(Stream stream, string s)
        {
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(s);
        }
    }
}
