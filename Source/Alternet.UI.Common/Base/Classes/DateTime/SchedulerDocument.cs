using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a document that contains a collection of <see cref="CalendarItem"/> and other
    /// objects for scheduling purposes.
    /// </summary>
    public partial class SchedulerDocument : SerializableObject
    {
        private static SchedulerDocument? instance;

        private readonly ListSource<CalendarItem> items = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulerDocument"/> class.
        /// </summary>
        public SchedulerDocument()
        {
            items.ItemInserted += OnItemInserted;
            items.ItemRemoved += OnItemRemoved;
        }

        /// <summary>
        /// Gets the singleton instance of <see cref="SchedulerDocument"/>.
        /// This instance can be used as a default document for scheduling purposes.
        /// </summary>
        public static SchedulerDocument Instance
        {
            get
            {
                return instance ??= new();
            }
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

        /// <summary>
        /// Gets the <see cref="CalendarItemsByDate"/> for the specified parameters.
        /// </summary>
        /// <param name="prm">The parameters for getting the items by date.</param>
        /// <returns>A <see cref="CalendarItemsByDate"/> containing the items for the specified date range.</returns>
        public virtual CalendarItemsByDate GetItemsByDate(PatternRuleGetDatesParams prm)
        {
            var rangeData = new CalendarItemsByDate(prm.MinDate, prm.MaxDate);

            foreach (var dates in GetItemDates(prm))
            {
                var item = dates.Item;

                foreach (var date in dates.Dates)
                {
                    rangeData.AddItem(date, item);
                }
            }

            return rangeData;
        }

        /// <summary>
        /// Gets the dates for each <see cref="CalendarItem"/> in
        /// the source that match the specified repeat pattern rule.
        /// </summary>
        /// <param name="prm">The parameters for getting the dates.</param>
        /// <returns>An enumerable of <see cref="CalendarItemDates"/> objects.</returns>
        public virtual IEnumerable<CalendarItemDates> GetItemDates(PatternRuleGetDatesParams prm)
        {
            foreach (var item in Items)
            {
                var dates = item.Rule.GetDates(prm);
                var d = dates.Dates.ToArray();
                if (d.Length > 0)
                    yield return new CalendarItemDates(item, d);
            }
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
        /// Called when an item is removed from the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index of the removed item.</param>
        /// <param name="item">The item that was removed.</param>
        protected virtual void OnItemRemoved(object? sender, int index, CalendarItem item)
        {
            if (item.Document == this)
            {
                item.Document = null;
            }
        }

        /// <summary>
        /// Called when an item is inserted into the <see cref="Items"/> collection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="index">The index of the inserted item.</param>
        /// <param name="item">The item that was inserted.</param>
        protected virtual void OnItemInserted(object? sender, int index, CalendarItem item)
        {
            if (item.Document is not null && item.Document != this)
            {
                throw new InvalidOperationException("The CalendarItem is already associated with a SchedulerDocument.");
            }

            item.Document = this;
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
