using System;

namespace Alternet.UI
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class DisableDpiAwarenessAttribute : BaseAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisableDpiAwarenessAttribute"/> class.
        /// </summary>
        public DisableDpiAwarenessAttribute()
        {
        }
    }
}
