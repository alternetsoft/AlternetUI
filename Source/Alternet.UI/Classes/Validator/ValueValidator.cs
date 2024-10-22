using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for <see cref="IValueValidator"/> implementation.
    /// </summary>
    internal class ValueValidator : DisposableObject<IntPtr>, IValueValidator
    {
        internal ValueValidator(IntPtr handle = default, bool disposeHandle = true)
            : base(handle, disposeHandle)
        {
        }

        /// <inheritdoc cref="IValueValidator.Name"/>
        public string? Name { get; set; }
    }
}
