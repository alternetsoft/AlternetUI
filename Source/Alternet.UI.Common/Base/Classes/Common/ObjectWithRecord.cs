using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Extends <see cref="ImmutableObject"/> with <see cref="AsRecord"/> property
    /// and other features.
    /// </summary>
    /// <typeparam name="T">Type of record.</typeparam>
    public class ObjectWithRecord<T> : ImmutableObject
        where T : struct
    {
        internal T record;

        /// <summary>
        /// Gets or sets this object as record.
        /// </summary>
        public virtual T AsRecord
        {
            get
            {
                return record;
            }

            set
            {
                record = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Assigns this object with properties of other object.
        /// </summary>
        /// <param name="value">Source of the property values to assign.</param>
        public virtual void Assign(ObjectWithRecord<T> value)
        {
            record = value.record;
            RaisePropertyChanged();
        }
    }
}
