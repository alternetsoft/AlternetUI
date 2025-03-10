using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Interface representing an object with attributes.
    /// </summary>
    public interface IBaseObjectWithAttr : IBaseObjectWithId
    {
        /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
        object? Tag { get; set; }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlags"/>
        ICustomIntFlags IntFlags { get; }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlagsAndAttributes"/>
        IIntFlagsAndAttributes IntFlagsAndAttributes { get; set; }

        /// <inheritdoc cref="BaseObjectWithAttr.FlagsAndAttributes"/>
        IFlagsAndAttributes FlagsAndAttributes { get; set; }

        /// <inheritdoc cref="BaseObjectWithAttr.CustomFlags"/>
        ICustomFlags<string> CustomFlags { get; }

        /// <inheritdoc cref="BaseObjectWithAttr.CustomAttr"/>
        ICustomAttributes<string, object> CustomAttr { get; }
    }
}