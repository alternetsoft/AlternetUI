using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class with properties and methods common to all control items like
    /// <see cref="TreeViewItem"/>, <see cref="ListViewItem"/> and <see cref="ListControlItem"/>.
    /// </summary>
    public partial class BaseControlItem : BaseObject, IBaseControlItem
    {
        private IFlagsAndAttributes? flagsAndAttributes;
        private ObjectUniqueId? uniqueId;

        /// <inheritdoc/>
        [Browsable(false)]
        public ObjectUniqueId UniqueId
        {
            get
            {
                return uniqueId ??= new();
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the item.
        /// </summary>
        /// <value>An <see cref="object"/> that contains data about the item.
        /// The default is <c>null</c>.</value>
        /// <remarks>
        /// Any type derived from the <see cref="object"/> class can be assigned
        /// to this property.
        /// A common use for the <see cref="Tag"/> property is to store data that
        /// is closely associated with the item.
        /// </remarks>
        [Browsable(false)]
        public object? Tag { get; set; }

        /// <summary>
        /// Gets custom flags and attributes provider associated with the item.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public virtual IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return flagsAndAttributes ??= Factory.CreateFlagsAndAttributes();
            }

            set
            {
                flagsAndAttributes = value;
            }
        }

        /// <summary>
        /// Gets custom flags provider associated with the item.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomFlags CustomFlags => FlagsAndAttributes.Flags;

        /// <summary>
        /// Gets custom attributes provider associated with the item.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomAttributes CustomAttr => FlagsAndAttributes.Attr;
    }
}
