using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a method to assign values from another instance.
    /// </summary>
    /// <typeparam name="T">The type of the object to assign from.</typeparam>
    public interface IAssignable<T>
    {
        /// <summary>
        /// Assigns values from the specified source instance to the current instance.
        /// </summary>
        /// <param name="source">The source instance to assign values from.</param>
        void AssignFrom(T source);
    }
}
