using System;

namespace Alternet.UI
{
    /// <summary>
    /// DisableDpiAwarenessAttribute tells to disable DpiAwareness in this
    /// application for controls.
    /// </summary>
    /// <remarks>
    ///  By default, Alternet UI application is Dpi-Aware when the UI layout is calculated.
    ///  But if in any case, an application wants to host Alternet UI control and doesn't
    ///  want to support Dpi aware, the way to achieve it is to add this attribute
    ///  value in its application assembly.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class DisableDpiAwarenessAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableDpiAwarenessAttribute"/> class.
        /// </summary>
        public DisableDpiAwarenessAttribute()
        {
        }
    }
}
