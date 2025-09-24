using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Provides utility methods and constants for working with spans.
    /// </summary>
    /// <remarks>This class contains helper methods and constants designed to facilitate
    /// operations involving spans.
    /// It is intended for use in scenarios where efficient memory management
    /// and performance are critical.</remarks>
    public static class SpanUtils
    {
        /// <summary>
        /// Represents the maximum number of spans that can be stored in a stack.
        /// </summary>
        /// <remarks>This constant defines the upper limit for the size of a span stack. It is used to
        /// prevent excessive memory usage or stack overflow scenarios when working with spans.</remarks>
        public static int SpanStackLimit = 256;
    }
}