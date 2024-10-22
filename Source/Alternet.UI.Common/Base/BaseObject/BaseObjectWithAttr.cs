using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseObjectWithId"/> with custom attributes.
    /// This includes <see cref="Tag"/>, <see cref="CustomFlags"/>, <see cref="CustomAttr"/>,
    /// <see cref="FlagsAndAttributes"/> properties and other related to custom attributes features.
    /// </summary>
    public partial class BaseObjectWithAttr : BaseObjectWithId
    {
        private IFlagsAndAttributes? flagsAndAttributes;

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
                return flagsAndAttributes ??= FlagsAndAttributesFactory.Create();
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
        public ICustomFlags<string> CustomFlags => FlagsAndAttributes.Flags;

        /// <summary>
        /// Gets custom attributes provider associated with the item.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomAttributes<string, object> CustomAttr => FlagsAndAttributes.Attr;
    }
}
