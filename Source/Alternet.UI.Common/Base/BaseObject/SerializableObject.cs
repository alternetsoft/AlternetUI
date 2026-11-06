using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an object that can be serialized to and deserialized from a
    /// property bag or JSON/XML string representation.
    /// This is an abstract base class that provides a common interface for serialization and deserialization.
    /// You need to implement the LoadFrom and SaveTo methods in derived classes to define how the
    /// object's state is saved and loaded.
    /// </summary>
    public abstract partial class SerializableObject : ImmutableObject, IPropertyBagSerialize
    {
        /// <summary>
        /// Gets or sets a JSON string representation of the current state of this instance.
        /// </summary>
        [Browsable(false)]
        public virtual string AsJsonString
        {
            get
            {
                return AsPropertyBag.AsJsonString;
            }

            set
            {
                IPropertyBag propertyBag = PropertyBag.Create();
                propertyBag.AsJsonString = value;
                LoadFrom(new SerializableObjectContext(propertyBag));
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IPropertyBag"/> representation of the current state of this instance.
        /// </summary>
        [Browsable(false)]
        public virtual IPropertyBag AsPropertyBag
        {
            get
            {
                IPropertyBag propertyBag = PropertyBag.Create();
                SaveTo(new SerializableObjectContext(propertyBag));
                return propertyBag;
            }

            set
            {
                LoadFrom(new SerializableObjectContext(value));
            }
        }

        /// <summary>
        /// Gets or sets a XML string representation of the current state of this instance.
        /// </summary>
        [Browsable(false)]
        public virtual string AsXmlString
        {
            get
            {
                return AsPropertyBag.AsXmlString;
            }

            /*
            set
            {
                IPropertyBag propertyBag = PropertyBag.Create();
                propertyBag.AsXmlString = value;
                AsPropertyBag = propertyBag;
            }
            */
        }

        /// <inheritdoc/>
        public abstract bool LoadFrom(ISerializableObjectContext? context);

        /// <inheritdoc/>
        public abstract void SaveTo(ISerializableObjectContext context);
    }
}
