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
        /// Gets or sets the identifying name of the object.
        /// The name provides a reference so that code-behind, such as event handler code,
        /// can refer to a markup object after it is constructed during processing by a
        /// UIXML processor.
        /// </summary>
        /// <value>The name of the object. The default is <c>null</c>.</value>
        public virtual string? Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> associated with the object.
        /// </summary>
        /// <returns>
        /// The <see cref="ISite" /> associated with the object, if any.
        /// </returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
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