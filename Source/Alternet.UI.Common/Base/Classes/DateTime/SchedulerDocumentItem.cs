using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a base class for items used in a scheduler document.
    /// </summary>
    public abstract class SchedulerDocumentItem : SerializableObject
    {
        private SchedulerDocument? document;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDocumentItem"/> class.
        /// </summary>
        public SchedulerDocumentItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDocumentItem"/> class
        /// with the specified unique id.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the new instance.</param>
        public SchedulerDocumentItem(ObjectUniqueId uniqueId)
            : base(uniqueId)
        {
        }

        /// <inheritdoc/>
        public override ISerializableObjectManager? SerializableObjectManager
        {
            get
            {
                return base.SerializableObjectManager ?? Document?.SerializableObjectManager;
            }

            set
            {
                base.SerializableObjectManager = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SchedulerDocument"/> associated with this calendar item.
        /// </summary>
        public virtual SchedulerDocument? Document
        {
            get => document;
            internal set
            {
                document = value;
            }
        }

        /// <inheritdoc/>
        protected override bool UseGuidForUniqueId => true;
    }
}
