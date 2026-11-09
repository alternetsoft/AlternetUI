using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a document that contains a collection of <see cref="CalendarItem"/> and other
    /// objects for scheduling purposes.
    /// </summary>
    public partial class SchedulerDocument : SerializableObject
    {
        private readonly ListSource<CalendarItem> items = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDocument"/> class.
        /// </summary>
        public SchedulerDocument()
        {
        }

        /// <summary>
        /// Gets a collection of <see cref="CalendarItem"/> objects.
        /// </summary>
        public ListSource<CalendarItem> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="CalendarItem"/> and returns it.
        /// This method can be overridden in derived classes to create custom calendar items.
        /// This method does not add the created item to the <see cref="Items"/> collection;
        /// it only creates and returns a new instance. Use the <see cref="Add(CalendarItem)"/> method
        /// to add the created item to the source.
        /// </summary>
        /// <returns>A new instance of <see cref="CalendarItem"/>.</returns>
        public virtual CalendarItem CreateItem()
        {
            var item = new CalendarItem();
            return item;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CalendarItem"/> with the specified unique id and returns it.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the new <see cref="CalendarItem"/>.</param>
        /// <returns>A new instance of <see cref="CalendarItem"/> with the specified unique id.</returns>
        public virtual CalendarItem CreateItem(ObjectUniqueId uniqueId)
        {
            var item = new CalendarItem(uniqueId);
            return item;
        }

        /// <summary>
        /// Adds a <see cref="CalendarItem"/> to the source.
        /// </summary>
        /// <param name="item">The <see cref="CalendarItem"/> to add.</param>
        public virtual void Add(CalendarItem item)
        {
            items.Add(item);
        }

        /// <inheritdoc/>
        public override bool LoadFrom(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;

            var propertyBag = context.PropertyBag;

            if (propertyBag == null)
                return false;

            var prefix = context.Prefix;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            return DoInsideSuspendedPropertyChanged(() =>
            {
                Items.DoInsideUpdate(() =>
                {
                    Items.Clear();

                    propertyBag.TryGetCollectionProperty(prefix + nameof(Items), out var itemsCollection);

                    if (itemsCollection != null)
                    {
                        foreach (var pb in itemsCollection)
                        {
                            CalendarItem calendarItem;

                            if (pb.TryGetUniqueIdProperty(prefix + nameof(UniqueId), out var uniqueId))
                            {
                                calendarItem = CreateItem(uniqueId);
                            }
                            else
                            {
                                calendarItem = CreateItem();
                            }

                            calendarItem.LoadFrom(new SerializableObjectContext(pb));
                            Add(calendarItem);
                        }
                    }
                });

                return true;
            });
        }

        /// <inheritdoc/>
        public override bool SaveTo(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;
    
            var propertyBag = context.PropertyBag;
            var prefix = context.Prefix;

            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.ElementName ??= GetElementName();
            propertyBag.IsRoot = true;

            propertyBag.SetCollectionProperty(prefix + nameof(Items), Items);

            return true;
        }

        /// <summary>
        /// Gets the name of the root element that represents this object when serialized.
        /// </summary>
        /// <returns>The name of the root element.</returns>
        protected virtual string GetElementName()
        {
            return "SchedulerDocument";
        }
    }
}
