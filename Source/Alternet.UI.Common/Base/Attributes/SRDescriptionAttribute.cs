using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="DescriptionAttribute"/> with the <see cref="Replaced"/> property.
    /// Allows to specify description by its id and description id replacement to text.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class SRDescriptionAttribute : DescriptionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SRDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">Description id.</param>
        public SRDescriptionAttribute(string description)
            : base(description)
        {
        }

        /// <summary>
        /// Gets or sets whether description id is replaced with the description text.
        /// </summary>
        public bool Replaced { get; set; }
    }
}
