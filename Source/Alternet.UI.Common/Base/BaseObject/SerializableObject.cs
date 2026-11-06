using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        /// <summary>
        /// Saves the current state of this instance to a string representation in the specified format.
        /// </summary>
        /// <param name="s">The output string representation of the current state.</param>
        /// <param name="format">The format in which to save the current state.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool SaveToString(out string s, SerializableSaveFormat format)
        {
            switch (format)
            {
                case SerializableSaveFormat.Json:
                    s = AsJsonString;
                    return true;
                case SerializableSaveFormat.Xml:
                    s = AsXmlString;
                    return true;
                default:
                    s = string.Empty;
                    return false;
            }
        }

        /// <summary>
        /// Loads the state of this instance from a string representation in the specified format.
        /// </summary>
        /// <param name="s">The input string representation of the state.</param>
        /// <param name="format">The format in which the state is represented.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool LoadFromString(string s, SerializableOpenFormat format)
        {
            switch (format)
            {
                case SerializableOpenFormat.Json:
                    AsJsonString = s;
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Saves the current state of this instance to a file in the specified format.
        /// <see cref="FileSystem.Create(string)"/> is used to create the file.
        /// </summary>
        /// <param name="fileName">The name of the file to which the state will be saved.</param>
        /// <param name="format">The format in which to save the current state.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool SaveToFile(string fileName, SerializableSaveFormat format)
        {
            using var fileStream = FileSystem.Create(fileName);
            return SaveToStream(fileStream, format);
        }

        /// <summary>
        /// Loads the state of this instance from a file in the specified format.
        /// <see cref="FileSystem.OpenRead(string)"/> is used to open the file.
        /// </summary>
        /// <param name="fileName">The name of the file from which the state will be loaded.</param>
        /// <param name="format">The format in which the state is represented.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool LoadFromFile(string fileName, SerializableOpenFormat format)
        {
            using var fileStream = FileSystem.OpenRead(fileName);
            return LoadFromStream(fileStream, format);
        }

        /// <summary>
        /// Saves the current state of this instance to a stream in the specified format.
        /// </summary>
        /// <param name="stream">The output stream to which the serialized data will be written.</param>
        /// <param name="format">The format in which to save the current state.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool SaveToStream(Stream stream, SerializableSaveFormat format)
        {
            SaveToString(out var s, format);
            return StreamUtils.StringToStreamSafe(stream, s, Encoding.UTF8);
        }

        /// <summary>
        /// Loads the state of this instance from a stream in the specified format.
        /// </summary>
        /// <param name="stream">The input stream containing the serialized data.</param>
        /// <param name="format">The format in which the data is serialized.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public virtual bool LoadFromStream(Stream stream, SerializableOpenFormat format)
        {
            var s = StreamUtils.StringFromStreamOrNull(stream, Encoding.UTF8);

            if (s == null)
                return false;

            return LoadFromString(s, format);
        }
    }
}
