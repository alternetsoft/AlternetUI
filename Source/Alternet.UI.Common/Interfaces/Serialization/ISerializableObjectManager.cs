using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a manager for serializable objects.
    /// </summary>
    public interface ISerializableObjectManager
    {
        private static ISerializableObjectManager instance = new DefaultSerializableObjectManager();

        /// <summary>
        /// Gets or sets the singleton instance of the <see cref="ISerializableObjectManager"/> interface.
        /// </summary>
        public static ISerializableObjectManager Instance
        {
            get => instance;
            set
            {
                instance = value ?? new DefaultSerializableObjectManager();
            }
        }

        /// <summary>
        /// Gets the <see cref="IDateTimeSerialization"/> instance
        /// that manages the serialization of <see cref="DateTime"/> objects.
        /// </summary>
        IDateTimeSerialization DateTimeManager { get; }

        private class DefaultSerializableObjectManager : ISerializableObjectManager
        {
            public IDateTimeSerialization DateTimeManager => DateTimeSerialization.Instance;
        }
    }
}
