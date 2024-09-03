using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for Alternet.UI components.
    /// </summary>
    public class BaseComponent : DisposableObject, IComponent
    {
        /// <summary>
        /// Gets or sets the <see cref="ISite"/> associated with the object; or <c>null</c>,
        /// if the object does not have a site.
        /// </summary>
        public virtual ISite? Site { get; set; }

        /// <summary>
        /// Gets or sets an object that contains some data about the component.
        /// </summary>
        /// <returns>
        /// An <see cref="object" /> that contains data about the component.
        /// The default is <see langword="null" />.
        /// </returns>
        [SRCategory("Data")]
        [Localizable(false)]
        [Bindable(true)]
        [DefaultValue(null)]
        [TypeConverter(typeof(StringConverter))]
        public virtual object? Tag { get; set; }
    }
}
