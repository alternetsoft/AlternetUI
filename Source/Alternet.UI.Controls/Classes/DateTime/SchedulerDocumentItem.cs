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
        private ObjectUniqueId changeToken;
        private SchedulerDocument? document;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDocumentItem"/> class.
        /// </summary>
        public SchedulerDocumentItem()
        {
            RenewChangeToken();
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

        /// <summary>
        /// Gets the <see cref="SchedulerDocument"/> associated with this calendar item,
        /// or returns the default instance if none is set.
        /// </summary>
        /// <returns>The associated <see cref="SchedulerDocument"/> or the default instance.</returns>
        public SchedulerDocument GetDocumentOrDefault()
        {
            return Document ?? SchedulerDocument.Instance;
        }

        /// <summary>
        /// Gets the change token of the item, which is updated
        /// whenever a property of the item changes.
        /// </summary>
        public ObjectUniqueId ChangeToken => changeToken;

        /// <summary>
        /// Renews the change token of the item.
        /// </summary>
        public virtual void RenewChangeToken()
        {
            changeToken = new(useGuid: true);
        }

        /// <inheritdoc/>
        protected override void OnTransactionEnded()
        {
            base.OnTransactionEnded();
            RenewChangeToken();
        }

        /// <inheritdoc/>
        protected override bool UseGuidForUniqueId => true;
    }
}
