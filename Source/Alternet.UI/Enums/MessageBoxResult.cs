using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// The <see cref="MessageBox"/> box return value is OK.
        /// </summary>
        OK,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is Cancel.
        /// </summary>
        Cancel,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is Yes.
        /// </summary>
        Yes,

        /// <summary>
        /// The <see cref="MessageBox"/> box return value is No.
        /// </summary>
        No,
    }
}
