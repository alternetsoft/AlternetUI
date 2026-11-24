using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a sample client that interacts with a <see cref="SchedulerDocument"/> for scheduling purposes.
    /// This is a sample implementation that can be used as an initial reference
    /// or as a base for creating custom clients that work with scheduling documents.
    /// </summary>
    public class SampleSchedulerClient : DisposableObject
    {
        private readonly ListSourceListener<CalendarItem> itemsListener = new();
        private SchedulerDocument? document;

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleSchedulerClient"/> class.
        /// </summary>
        public SampleSchedulerClient()
        {
        }

        /// <summary>
        /// Gets the <see cref="ListSourceListener{CalendarItem}"/> associated with this client.
        /// </summary>
        public ListSourceListener<CalendarItem> ItemsListener
        {
            get
            {
                return itemsListener;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SchedulerDocument"/> associated with this client.
        /// </summary>
        public virtual SchedulerDocument? Document
        {

            get
            {
                return document;
            }

            set
            {
                if (document == value)
                    return;

                UnbindDocumentEvents(SafeDocument);
                document = value;
                BindDocumentEvents(SafeDocument);
            }
        }

        /// <summary>
        /// Gets the <see cref="SchedulerDocument"/> associated with this client.
        /// If the <see cref="Document"/> property is null,
        /// this property returns the singleton instance of <see cref="SchedulerDocument"/>
        /// using <see cref="SchedulerDocument.Instance"/> property.
        /// </summary>
        public virtual SchedulerDocument SafeDocument
        {
            get
            {
                return document ?? SchedulerDocument.Instance;
            }
        }

        /// <summary>
        /// Binds the necessary events to the specified <see cref="SchedulerDocument"/> instance.
        /// </summary>
        /// <param name="document">Scheduler document instance.</param>
        protected virtual void BindDocumentEvents(SchedulerDocument document)
        {
            document.Items.Subscribe(ItemsListener);
        }

        /// <summary>
        /// Unbinds the events from the specified <see cref="SchedulerDocument"/> instance.
        /// </summary>
        /// <param name="document">Scheduler document instance.</param>
        protected virtual void UnbindDocumentEvents(SchedulerDocument document)
        {
            document.Items.Unsubscribe(ItemsListener);
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            UnbindDocumentEvents(SafeDocument);
            base.DisposeManaged();
        }
    }
}
