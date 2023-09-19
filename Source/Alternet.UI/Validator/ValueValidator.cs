using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    internal class ValueValidator : DisposableObject, IValueValidator
    {
        public ValueValidator(IntPtr handle = default, bool disposeHandle = true)
            : base(handle, disposeHandle)
        {
        }

        /// <inheritdoc cref="IValueValidator.Name"/>
        public string? Name { get; set; }

        /// <inheritdoc cref="IValueValidator.Tag"/>
        [Browsable(false)]
        public object? Tag { get; set; }
    }
}
