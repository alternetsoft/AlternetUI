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
    public class ImmutableWithRecord<T> : ImmutableObject
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
                if (Immutable)
                    return;
                record = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Conversion operator from <see cref="ImmutableWithRecord{T}"/> to it's record.
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator T(ImmutableWithRecord<T> value)
        {
            return value.AsRecord;
        }

        /// <summary>
        /// Assigns this object with properties of other object.
        /// </summary>
        /// <param name="value">Source of the property values to assign.</param>
        public virtual void Assign(ImmutableWithRecord<T> value)
        {
            if (Immutable)
                return;
            record = value.record;
            RaisePropertyChanged();
        }
    }
}
