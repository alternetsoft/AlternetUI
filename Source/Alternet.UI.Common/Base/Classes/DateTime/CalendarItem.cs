using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a calendar item that can be displayed in a calendar or scheduler control.
    /// </summary>
    public partial class CalendarItem : ImmutableObject, IEquatable<CalendarItem>
    {
        private string? title;
        private string? description;
        private object? location;
        private CalendarItemCategory category;
        private CalendarItemStatus status;
        private object? userData;
        private string? userName;
        private bool visible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItem"/> class.
        /// </summary>
        public CalendarItem()
        {
            category = KnownCalendarItemMarkers.CategoryNone;
            status = KnownCalendarItemMarkers.StatusUnknown;
        }

        /// <summary>
        /// Gets or sets the title of the calendar item.
        /// </summary>
        public virtual string? Title
        {
            get => title;
            set
            {
                this.SetProperty(ref title, value, nameof(Title));
            }
        }

        /// <summary>
        /// Gets or sets the description of the calendar item.
        /// </summary>
        public virtual string? Description
        {
            get => description;
            set
            {
                this.SetProperty(ref description, value, nameof(Description));
            }
        }

        /// <summary>
        /// Gets or sets the location property of the calendar item.
        /// </summary>
        public virtual object? Location
        {
            get => location;
            set
            {
                this.SetProperty(ref location, value, nameof(Location));
            }
        }

        /// <summary>
        /// Gets or sets the category property of the calendar item.
        /// </summary>
        public virtual CalendarItemCategory Category
        {
            get => category;
            set
            {
                this.SetProperty(ref category!, value ?? KnownCalendarItemMarkers.CategoryNone, nameof(Category));
            }
        }

        /// <summary>
        /// Gets or sets the status property of the calendar item.
        /// </summary>
        public virtual CalendarItemStatus Status
        {
            get => status;
            set
            {
                this.SetProperty(ref status!, value ?? KnownCalendarItemMarkers.StatusUnknown, nameof(Status));
            }
        }

        /// <summary>
        /// Gets or sets the user data of the calendar item.
        /// </summary>
        public virtual object? UserData
        {
            get => userData;
            set
            {
                this.SetProperty(ref userData, value, nameof(UserData));
            }
        }

        /// <summary>
        /// Gets or sets the user name of the calendar item.
        /// </summary>
        public virtual string? UserName
        {
            get => userName;
            set
            {
                this.SetProperty(ref userName, value, nameof(UserName));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the calendar item is visible.
        /// </summary>
        public virtual bool Visible
        {
            get => visible;
            set
            {
                this.SetProperty(ref visible, value, nameof(Visible));
            }
        }

        /// <summary>
        /// Loads the state of this instance from the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag from which to load the state.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
        public virtual bool LoadFrom(IPropertyBag? propertyBag)
        {
            if (propertyBag == null)
                return false;

            return DoInsideSuspendedPropertyChanged(() =>
            {
                Reset();

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="IPropertyBag"/>.
        /// </summary>
        /// <param name="propertyBag">The property bag to which the state will be saved.</param>
        public virtual void SaveTo(IPropertyBag propertyBag)
        {
        }

        /// <summary>
        /// Creates a deep copy of the current instance.
        /// </summary>
        /// <returns>A new instance that is a deep copy of the current instance.</returns>
        public virtual CalendarItem Clone()
        {
            var clone = new CalendarItem();
            clone.Assign(this);
            return clone;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current instance of <see cref="CalendarItem"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not CalendarItem other)
            {
                return false;
            }

            return Equals(other);
        }

        /// <inheritdoc/>
        public bool Equals(CalendarItem? other)
        {
            if (other == null)
            {
                return false;
            }

            return Title == other.Title &&
                   Description == other.Description &&
                   Location?.ToString() == other.Location?.ToString() &&
                   Category == other.Category &&
                   Status == other.Status &&
                   Equals(UserData, other.UserData) &&
                   UserName == other.UserName &&
                   Visible == other.Visible;
        }

        /// <summary>
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                SuspendPropertyChanged();

                Title = null;
                Description = null;
                Location = null;
                Category = KnownCalendarItemMarkers.CategoryNone;
                Status = KnownCalendarItemMarkers.StatusUnknown;
                UserData = null;
                UserName = null;
                Visible = true;

                ResumePropertyChanged();
                return;
            }

            if (other is CalendarItem otherItem)
            {
                if (Equals(other))
                    return;

                SuspendPropertyChanged();

                Title = otherItem.Title;
                Description = otherItem.Description;
                Location = otherItem.Location;
                Category = otherItem.Category;
                Status = otherItem.Status;
                UserData = otherItem.UserData;
                UserName = otherItem.UserName;
                Visible = otherItem.Visible;

                ResumePropertyChanged();
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Title, Description, Location?.ToString(), Category, Status, UserData, UserName, Visible).GetHashCode();
        }
    }
}
