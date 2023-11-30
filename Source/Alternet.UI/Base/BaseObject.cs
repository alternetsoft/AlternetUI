using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Encapsulates a method that has a single parameter and does not return a value.
    /// </summary>
    /// <typeparam name="T">The type of the parameter of the method that this delegate encapsulates.</typeparam>
    /// <param name="item">The parameter of the method that this delegate encapsulates.</param>
    /// <remarks>
    /// This delegate is different from <see cref="Action{T}"/> in the parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T>(ref T item);

    /// <summary>
    /// Encapsulates a method that has two parameters and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the method that this delegate encapsulates.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the method that this delegate encapsulates.</typeparam>
    /// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param>
    /// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param>
    /// <remarks>
    /// This delegate is different from <see cref="Action{T1,T2}"/> in the first parameter definition.
    /// Here it is defined as "ref" parameter.
    /// </remarks>
    public delegate void ActionRef<T1, T2>(ref T1 arg1, T2 arg2);

    /// <summary>
    /// Base class with properties and methods common to all Alternet.UI objects.
    /// </summary>
    public class BaseObject : IBaseObject
    {
        /// <inheritdoc/>
        public virtual void Required()
        {
        }
    }
}
