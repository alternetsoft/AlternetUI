using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="Exception"/> with additional properties related to the
    /// assert failure error details.
    /// </summary>
    public class AssertFailureException : BaseException
    {
        private AssertFailureExceptionData? assertData;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailureException"/> class.
        /// </summary>
        public AssertFailureException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertFailureException"/> class.
        /// </summary>
        public AssertFailureException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Gets or sets assert failure data.
        /// </summary>
        public AssertFailureExceptionData? AssertData
        {
            get => assertData;
            set
            {
                assertData = value;

                if (AssertData is not null)
                {
                    AdditionalInformation =
                        "Assert.File: " + AssertData.File + Environment.NewLine +
                        "Assert.Line: " + AssertData.Line + Environment.NewLine +
                        "Assert.Function: " + AssertData.Function + Environment.NewLine +
                        "Assert.Condition: " + AssertData.Condition + Environment.NewLine;
                }
            }
        }
    }
}
