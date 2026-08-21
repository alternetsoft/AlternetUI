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
    public partial class CustomEventArgs : BaseEventArgs
    {
        private FlagsAndAttributesStruct attr = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEventArgs"/> class.
        /// </summary>
        public CustomEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEventArgs"/> class
        /// with the specified custom flags turned on.
        /// </summary>
        public CustomEventArgs(params string[] flags)
        {
            foreach(var flag in flags)
                CustomFlags[flag] = true;
        }

        /// <inheritdoc cref="BaseObjectWithAttr.Tag"/>
        [Browsable(false)]
        public virtual object? Tag { get; set; }

        /// <summary>
        /// Sets custom attribute with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the custom attribute.</param>
        /// <param name="value">The value of the custom attribute.</param>
        /// <returns>The current <see cref="CustomEventArgs"/> instance.</returns>
        public CustomEventArgs Attr(string name, object? value)
        {
            CustomAttr[name] = value;
            return this;
        }

        /// <summary>
        /// Sets custom flag with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the custom flag.</param>
        /// <param name="value">The value of the custom flag. Optional.
        /// If not specified, the default value is <c>true</c>.</param>
        /// <returns>The current <see cref="CustomEventArgs"/> instance.</returns>
        public CustomEventArgs Flag(string name, bool value = true)
        {
            CustomFlags[name] = value;
            return this;
        }

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

        /// <summary>
        /// Creates <see cref="CustomEventArgs"/> instance with the specified flag turned on.
        /// </summary>
        /// <param name="flagName">Custom flag name.</param>
        /// <returns></returns>
        public static CustomEventArgs CreateWithFlag(string flagName)
        {
            CustomEventArgs result = new();
            result.CustomFlags[flagName] = true;
            return result;
        }

        /// <summary>
        /// Creates <see cref="CustomEventArgs"/> instance with the specified flags turned on.
        /// </summary>
        /// <param name="flagNames">Collection of custom flag names.</param>
        /// <returns></returns>
        public static CustomEventArgs CreateWithFlags(params string[] flagNames)
        {
            CustomEventArgs result = new();

            foreach(var flagName in flagNames)
                result.CustomFlags[flagName] = true;

            return result;
        }
    }
}
