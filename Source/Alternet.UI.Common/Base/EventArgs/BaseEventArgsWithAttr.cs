using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="BaseEventArgs"/> with custom attributes.
    /// This includes <see cref="Tag"/>, <see cref="CustomFlags"/>, <see cref="CustomAttr"/>,
    /// <see cref="FlagsAndAttributes"/> properties and other related
    /// to custom attributes features.
    /// </summary>
    public partial class BaseEventArgsWithAttr : BaseEventArgs
    {
        private FlagsAndAttributesStruct attr = new();

        /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
        [Browsable(false)]
        public virtual object? Tag { get; set; }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlags"/>
        [Browsable(false)]
        public ICustomIntFlags IntFlags
        {
            get
            {
                return IntFlagsAndAttributes;
            }
        }

        /// <inheritdoc cref="BaseObjectWithAttr.IntFlagsAndAttributes"/>
        [Browsable(false)]
        public virtual IIntFlagsAndAttributes IntFlagsAndAttributes
        {
            get
            {
                return attr.IntFlagsAndAttributes;
            }

            set
            {
                attr.IntFlagsAndAttributes = value;
            }
        }

        /// <inheritdoc cref="BaseObjectWithAttr.FlagsAndAttributes"/>
        [Browsable(false)]
        public virtual IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return attr.FlagsAndAttributes;
            }

            set
            {
                attr.FlagsAndAttributes = value;
            }
        }

        /// <summary>
        /// <inheritdoc cref="BaseObjectWithAttr.CustomFlags"/>
        /// </summary>
        [Browsable(false)]
        public ICustomFlags<string> CustomFlags => FlagsAndAttributes.Flags;

        /// <inheritdoc cref="BaseObjectWithAttr.CustomAttr"/>
        [Browsable(false)]
        public ICustomAttributes<string, object> CustomAttr => FlagsAndAttributes.Attr;
    }
}
