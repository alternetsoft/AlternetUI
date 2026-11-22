using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Extensions;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a repeat pattern rule for events or tasks that occur only once, without any repetition.
    /// </summary>
    public partial class OnceRepeatPatternRule : OwnedRepeatPatternRule, IPropertyBagSerialize
    {
        private DateOnly startDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnceRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner of the repeat pattern rule.</param>
        public OnceRepeatPatternRule(IRepeatPatternRuleOwner owner) : base(owner)
        {
            startDate = DateUtils.Today;
        }

        /// <inheritdoc/>
        public override DateOnly ValidFromDate
        {
            get => startDate;
            set
            {
                // The ValidFromDate property is read-only for OnceRepeatPatternRule, so we do not allow setting it.
            }
        }

        /// <inheritdoc/>
        public override DateOnly ValidToDate
        {
            get
            {
                return EndDate;
            }

            set
            {
                // The ValidToDate property is read-only for OnceRepeatPatternRule, so we do not allow setting it.
            }
        }

        /// <summary>
        /// Gets the end date of the event or task that occurs only once.
        /// </summary>
        public virtual DateOnly EndDate
        {
            get
            {
                return DateOnly.FromDateTime(Ends);
            }
        }

        /// <summary>
        /// Gets or sets the interval of the event or task that occurs only once,
        /// represented by a <see cref="DateTimeInterval"/> object.
        /// </summary>
        public virtual DateTimeInterval Interval
        {
            get
            {
                return new DateTimeInterval(Starts, Duration, TimeZone);
            }

            set
            {
                if (Interval == value)
                    return;
                WithoutPropertyChanged(() =>
                {
                    StartDate = DateOnly.FromDateTime(value.Start);
                    StartTime = TimeOnly.FromDateTime(value.Start);
                    Duration = value.Duration;
                });
            }
        }


        /// <summary>
        /// Gets the start date and time of the event or task that occurs only once.
        /// </summary>
        public virtual DateTime Starts
        {
            get
            {
                return StartDate.ToDateTime(StartTime);
            }

            set
            {
                if (Starts == value)
                    return;

                WithoutPropertyChanged(() =>
                {
                    StartDate = DateOnly.FromDateTime(value);
                    StartTime = TimeOnly.FromDateTime(value);
                });
            }
        }

        /// <summary>
        /// Gets the end date and time of the event or task that occurs only once.
        /// </summary>
        public virtual DateTime Ends
        {
            get
            {
                var result = StartDate.ToDateTime(StartTime).Add(Duration);
                return result;
            }

            set
            {
                if (Ends == value)
                    return;
                WithoutPropertyChanged(() =>
                {
                    Duration = value - StartDate.ToDateTime(StartTime);
                });
            }
        }

        /// <summary>
        /// Gets or sets the start date of the event or task that occurs only once.
        /// This property is used to specify the date on which the event or task is scheduled to take place.
        /// </summary>
        public virtual DateOnly StartDate
        {
            get => startDate;
            set
            {
                SetProperty(ref startDate, value, nameof(StartDate));
            }
        }

        /// <inheritdoc/>
        public override PatternRuleGetDatesResult GetStartDates(PatternRuleGetDatesParams prm)
        {
            var startDate = StartDate;
            var endDate = EndDate;

            var startDateInRange = startDate >= prm.MinDate && startDate <= prm.MaxDate;

            if (startDateInRange)
            {
                return new(startDate);
            }

            var endDateInRange = endDate >= prm.MinDate && endDate <= prm.MaxDate;

            if (endDateInRange)
            {
                return new(startDate);
            }

            return new();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return StartDate.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current
        /// instance of <see cref="OnceRepeatPatternRule"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><c>true</c> if the specified object is equal to the current instance;
        /// otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is not OnceRepeatPatternRule other)
            {
                return false;
            }

            return StartDate == other.StartDate;
        }

        /// <summary>
        /// Assigns the values from another instance to the current instance.
        /// </summary>
        /// <param name="other">The instance from which to copy values. If <c>null</c>, default values are assigned.
        /// If the instance is not of type <see cref="OnceRepeatPatternRule"/>, no values are copied.</param>
        public override bool Assign(object? other)
        {
            if (other == null)
            {
                DoInsideTransaction(() =>
                {
                    StartDate = DateUtils.Today;
                });

                return true;
            }

            if (other is OnceRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return false;
                DoInsideTransaction(() =>
                {
                    StartDate = otherRule.StartDate;
                });

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool LoadFrom(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;

            var prefix = context.Prefix;
            var propertyBag = context.PropertyBag;

            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            return DoInsideTransaction<bool>(() =>
            {
                Reset();

                if (propertyBag.TryGetDateProperty(prefix + nameof(StartDate), out var startDate))
                {
                    StartDate = startDate;
                }

                return true;
            });
        }

        /// <inheritdoc/>
        public override bool SaveTo(ISerializableObjectContext? context)
        {
            if (context == null)
                return false;

            var prefix = context.Prefix;
            var propertyBag = context.PropertyBag;

            if (propertyBag == null)
                return false;

            if (prefix is not null)
                prefix += ".";
            else
                prefix = "";

            propertyBag.SetDateProperty(prefix + nameof(StartDate), StartDate);
            return true;
        }
    }
}
