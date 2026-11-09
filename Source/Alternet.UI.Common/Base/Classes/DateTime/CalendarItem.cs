using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a calendar item that can be displayed in a calendar or scheduler control.
    /// </summary>
    public partial class CalendarItem : SerializableObject, IEquatable<CalendarItem>, IPropertyBagSerialize
    {
        private RepeatPatternRule rule;
        private CalendarAlarmInfo alarmInfo;
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
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItem"/> class
        /// with the specified unique id.
        /// </summary>
        /// <param name="uniqueId">The unique identifier for the new instance.</param>
        public CalendarItem(ObjectUniqueId uniqueId)
            : base(uniqueId)
        {
            Initialize();
        }

        /// <summary>
        /// Gets the <see cref="CalendarAlarmInfo"/> associated with this calendar item.
        /// </summary>
        public virtual CalendarAlarmInfo AlarmInfo
        {
            get
            {
                return alarmInfo;
            }

            set
            {
                alarmInfo.Assign(value);
            }
        }

        /// <summary>
        /// Gets or sets the end date and time of the calendar item.
        /// </summary>
        public DateTime End
        {
            get => rule.End;
            set
            {
                rule.End = value;
            }
        }

        /// <summary>
        /// Gets or sets the start date and time of the calendar item.
        /// </summary>
        public DateTime Start
        {
            get => rule.Start;
            set
            {
                rule.Start = value;
            }
        }

        /// <summary>
        /// Gets or sets the repeat pattern rule associated with this calendar item.
        /// When set, the rule is assigned to the existing <see cref="RepeatPatternRule"/> instance.
        /// </summary>
        public RepeatPatternRule Rule
        {
            get => rule;

            set
            {
                rule.Assign(value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the calendar item.
        /// </summary>
        public virtual string? Text
        {
            get => title;
            set
            {
                this.SetProperty(ref title, value, nameof(Text));
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
        /// Loads the state of this instance from the specified <see cref="ISerializableObjectContext"/>.
        /// </summary>
        /// <param name="context">The context containing the property bag and optional prefix for property names.</param>
        /// <returns><c>true</c> if the state was successfully loaded; otherwise, <c>false</c>.</returns>
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
                Reset();

                if (propertyBag.TryGetStringProperty(prefix + nameof(Text), out var textString))
                    Text = textString;
                if (propertyBag.TryGetStringProperty(prefix + nameof(Description), out var descriptionString))
                    Description = descriptionString;
                if (propertyBag.TryGetStringProperty(prefix + nameof(Location), out var locationString))
                    Location = locationString;
                if (propertyBag.TryGetBoolProperty(prefix + nameof(Visible), out var visibleBool))
                    Visible = visibleBool;
                if (propertyBag.TryGetStringProperty(prefix + nameof(UserName), out var userNameString))
                    UserName = userNameString;
                if (propertyBag.TryGetStringProperty(prefix + nameof(Status), out var statusString))
                    Status = KnownCalendarItemMarkers.FindStatus(statusString);
                if (propertyBag.TryGetStringProperty(prefix + nameof(Category), out var categoryString))
                    Category = KnownCalendarItemMarkers.FindCategory(categoryString);
                
                Rule.LoadFrom(new SerializableObjectContext(propertyBag, prefix + nameof(Rule)));
                AlarmInfo.LoadFrom(new SerializableObjectContext(propertyBag, prefix + nameof(AlarmInfo)));

                return true;
            });
        }

        /// <summary>
        /// Saves the current state of this instance
        /// to the specified <see cref="SerializableObjectContext"/>.
        /// </summary>
        /// <param name="context">The context to which the state will be saved.</param>
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

            propertyBag.SetUniqueIdProperty(prefix + nameof(UniqueId), UniqueId);
            propertyBag.SetStringProperty(prefix + nameof(Text), Text ?? string.Empty);
            propertyBag.SetStringProperty(prefix + nameof(Description), Description ?? string.Empty);
            propertyBag.SetStringProperty(prefix + nameof(Location), Location?.ToString() ?? string.Empty);
            propertyBag.SetStringProperty(prefix + nameof(Status), Status.Name ?? string.Empty);
            propertyBag.SetStringProperty(prefix + nameof(Category), Category.Name ?? string.Empty);
            propertyBag.SetBoolProperty(prefix + nameof(Visible), Visible);
            propertyBag.SetStringProperty(prefix + nameof(UserName), UserName ?? string.Empty);

            Rule.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(Rule)));
            AlarmInfo.SaveTo(new SerializableObjectContext(propertyBag, prefix + nameof(AlarmInfo)));

            return true;
        }

        /// <summary>
        /// Creates a deep copy of the current instance.
        /// </summary>
        /// <returns>A new instance that is a deep copy of the current instance.</returns>
        public virtual CalendarItem Clone()
        {
            var clone = Activator.CreateInstance(GetType()) as CalendarItem;
            clone ??= new CalendarItem();
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

            return Text == other.Text &&
                   Description == other.Description &&
                   Location?.ToString() == other.Location?.ToString() &&
                   Category == other.Category &&
                   Status == other.Status &&
                   Equals(UserData, other.UserData) &&
                   AlarmInfo.Equals(other.AlarmInfo) &&
                   UserName == other.UserName &&
                   Visible == other.Visible &&
                   Rule.Equals(other.Rule);
        }

        /// <summary>
        /// Resets the current instance to its default state which was set when the instance was created.
        /// </summary>
        public virtual void Reset()
        {
            Assign(null);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return UniqueId.GetHashCode();
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        public virtual void Assign(object? other)
        {
            if (other == null)
            {
                DoInsideSuspendedPropertyChanged(() =>
                {
                    Text = null;
                    Description = null;
                    AlarmInfo.Assign(null);
                    Location = null;
                    Category = KnownCalendarItemMarkers.CategoryNone;
                    Status = KnownCalendarItemMarkers.StatusUnknown;
                    UserData = null;
                    UserName = null;
                    Visible = true;
                    Rule.Assign(null);
                });

                return;
            }

            if (other is CalendarItem otherItem)
            {
                Assign(otherItem);
            }
        }

        /// <summary>
        /// Assigns the values from another <see cref="CalendarItem"/> instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values.</param>
        public virtual void Assign(CalendarItem? other)
        {
            if (other == null)
            {
                DoInsideSuspendedPropertyChanged(() =>
                {
                    Category = KnownCalendarItemMarkers.CategoryNone;
                    Status = KnownCalendarItemMarkers.StatusUnknown;
                    Text = null;
                    Description = null;
                    Location = null;
                    UserData = null;
                    UserName = null;
                    Visible = true;
                    Rule.Reset();
                    AlarmInfo.Reset();
                });

                return;
            }

            if (Equals(other))
                return;

            DoInsideSuspendedPropertyChanged(() =>
            {
                Text = other.Text;
                Description = other.Description;
                Location = other.Location;
                Category = other.Category;
                Status = other.Status;
                UserData = other.UserData;
                UserName = other.UserName;
                Visible = other.Visible;
                Rule = other.Rule;
                AlarmInfo = other.AlarmInfo;
            });
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Called when a property of the <see cref="RepeatPatternRule"/> associated with this calendar item changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnRuleChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(Rule));
        }

        /// <summary>
        /// Called when a property of the <see cref="CalendarAlarmInfo"/> associated with this calendar item changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnAlarmInfoChanged(object? sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(AlarmInfo));
        }

        /// <inheritdoc/>
        protected override bool UseGuidForUniqueId => true;

        /// <summary>
        /// Creates a new instance of <see cref="RepeatPatternRule"/>. This method can be overridden
        /// in derived classes to provide a custom implementation of <see cref="RepeatPatternRule"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="RepeatPatternRule"/>.</returns>
        protected virtual RepeatPatternRule CreateRule()
        {
            return new RepeatPatternRule();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItem"/> class
        /// </summary>
        [MemberNotNull(nameof(rule)), MemberNotNull(nameof(alarmInfo))]
        [MemberNotNull(nameof(category)), MemberNotNull(nameof(status))]
        protected virtual void Initialize()
        {
            category = KnownCalendarItemMarkers.CategoryNone;
            status = KnownCalendarItemMarkers.StatusUnknown;
            rule = CreateRule();
            alarmInfo = CreateAlarmInfo();
            rule.PropertyChanged += OnRuleChanged;
            alarmInfo.PropertyChanged += OnAlarmInfoChanged;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CalendarAlarmInfo"/>. This method can be overridden
        /// in derived classes to provide a custom implementation of <see cref="CalendarAlarmInfo"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="CalendarAlarmInfo"/>.</returns>
        protected virtual CalendarAlarmInfo CreateAlarmInfo()
        {
            return new CalendarAlarmInfo();
        }
    }
}
