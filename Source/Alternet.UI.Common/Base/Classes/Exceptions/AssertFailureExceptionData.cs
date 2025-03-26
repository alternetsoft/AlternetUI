using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties that describe assert failure.
    /// </summary>
    public class AssertFailureExceptionData : BaseObject
    {
        /// <summary>
        /// Gets or sets assert failure message.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets source code file name.
        /// </summary>
        public string? File { get; set; }

        /// <summary>
        /// Gets or sets source code line number in the file.
        /// </summary>
        public string? Line { get; set; }

        /// <summary>
        /// Gets or sets function name where error occured.
        /// </summary>
        public string? Function { get; set; }

        /// <summary>
        /// Gets or sets assert condition.
        /// </summary>
        public string? Condition { get; set; }

        internal static void TestSerialize()
        {
            AssertFailureExceptionData e = new()
            {
                Message = "value",
                Condition = "value",
                File = "value",
                Function = "value",
                Line = "value",
            };

            XmlUtils.SerializeToFile(@"e:\sample.xml", e);
        }
    }
}
