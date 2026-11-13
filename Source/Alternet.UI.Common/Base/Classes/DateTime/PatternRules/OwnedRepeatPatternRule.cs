using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for an owner of a repeat pattern rule,
    /// providing access to the start date, duration, start time, end date, occurrence count,
    /// and end condition of the repeat pattern.
    /// </summary>
    public interface IRepeatPatternRuleOwner
    {
        /// <summary>
        /// Gets or sets the start date of the repeat pattern.
        /// </summary>
        DateOnly ValidFromDate { get; set; }

        /// <summary>
        /// Gets or sets the duration of the repeat pattern.
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        /// Gets or sets the start time of the repeat pattern.
        /// </summary>
        TimeOnly StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end date of the repeat pattern.
        /// </summary>
        DateOnly ValidToDate { get; set; }

        /// <summary>
        /// Gets or sets the occurrence count of the repeat pattern.
        /// </summary>
        int OccurrenceCount { get; set; }

        /// <summary>
        /// Gets or sets the end condition of the repeat pattern,
        /// which determines how the repeat pattern ends
        /// (never, after a number of occurrences, or on a specific date).
        /// </summary>
        RepeatPatternEndCondition EndCondition { get; set; }

        /// <summary>
        /// Gets the <see cref="IRepeatPatternRuleOwner"/> associated with this repeat pattern rule owner.
        /// </summary>
        IRepeatPatternRuleOwner? Owner { get; }
    }

    /// <summary>
    /// Represents a struct that implements the <see cref="IRepeatPatternRuleOwner"/> interface,
    /// </summary>
    public struct RepeatPatternRuleOwnerStruct : IRepeatPatternRuleOwner
    {
        /// <summary>
        /// Gets or sets the start date of the repeat pattern.
        /// </summary>
        public DateOnly ValidFromDate;

        /// <summary>
        /// Gets or sets the duration of the repeat pattern.
        /// </summary>
        public TimeSpan Duration;

        /// <summary>
        /// Gets or sets the start time of the repeat pattern.
        /// </summary>
        public TimeOnly StartTime;

        /// <summary>
        /// Gets or sets the end date of the repeat pattern.
        /// </summary>
        public DateOnly ValidToDate;

        /// <summary>
        /// Gets or sets the occurrence count of the repeat pattern.
        /// </summary>
        public int OccurrenceCount;

        /// <summary>
        /// Gets or sets the end condition of the repeat pattern,
        /// which determines how the repeat pattern ends
        /// (never, after a number of occurrences, or on a specific date).
        /// </summary>
        public RepeatPatternEndCondition EndCondition;

        /// <summary>
        /// Gets the <see cref="IRepeatPatternRuleOwner"/> associated with this repeat pattern rule owner.
        /// </summary>
        public readonly IRepeatPatternRuleOwner? Owner => null;

        /// <inheritdoc/>
        DateOnly IRepeatPatternRuleOwner.ValidFromDate
        {
            readonly get => ValidFromDate;
            set => ValidFromDate = value;
        }

        /// <inheritdoc/>
        TimeSpan IRepeatPatternRuleOwner.Duration
        {
            readonly get => Duration;
            set => Duration = value;
        }

        /// <inheritdoc/>
        TimeOnly IRepeatPatternRuleOwner.StartTime
        {
            readonly get => StartTime;
            set => StartTime = value;
        }

        /// <inheritdoc/>
        DateOnly IRepeatPatternRuleOwner.ValidToDate
        {
            readonly get => ValidToDate;
            set => ValidToDate = value;
        }

        /// <inheritdoc/>
        int IRepeatPatternRuleOwner.OccurrenceCount
        {
            readonly get => OccurrenceCount;
            set => OccurrenceCount = value;
        }

        /// <inheritdoc/>
        RepeatPatternEndCondition IRepeatPatternRuleOwner.EndCondition
        {
            readonly get => EndCondition;
            set => EndCondition = value;
        }
    }

    /// <summary>
    /// Represents a repeat pattern rule that is owned by another <see cref="BaseRepeatPatternRule"/>.
    /// </summary>
    public abstract partial class OwnedRepeatPatternRule : BaseRepeatPatternRule
    {
        private readonly IRepeatPatternRuleOwner? owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnedRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="IRepeatPatternRuleOwner"/> of this rule.</param>
        public OwnedRepeatPatternRule(IRepeatPatternRuleOwner? owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Gets the owner <see cref="IRepeatPatternRuleOwner"/> of this rule.
        /// </summary>
        public IRepeatPatternRuleOwner? Owner => owner;

        /// <summary>
        /// Gets a root owner <see cref="IRepeatPatternRuleOwner"/> of this rule,
        /// which is the top-level owner in the ownership hierarchy.
        /// </summary>
        public IRepeatPatternRuleOwner? RootOwner => owner?.Owner;

        /// <summary>
        /// Gets the <see cref="CalendarItem"/> associated with this repeat pattern rule, if any.
        /// </summary>
        public CalendarItem? CalendarItem => RootOwner as CalendarItem;

        /// <summary>
        /// Gets the <see cref="SchedulerDocument"/> associated with this repeat pattern rule, if any.
        /// </summary>
        public SchedulerDocument? Document => CalendarItem?.Document;

        /// <inheritdoc/>
        public override DateOnly ValidFromDate
        {
            get => owner?.ValidFromDate ?? base.ValidFromDate;
            set
            {
                if (owner is null)
                    base.ValidFromDate = value;
                else
                    owner.ValidFromDate = value;
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get => owner?.Duration ?? base.Duration;
            set
            {
                if (EffectiveClearSeconds())
                    value = DateUtils.ClearSeconds(value);

                if (owner is null)
                    base.Duration = value;
                else
                    owner.Duration = value;
            }
        }

        /// <inheritdoc/>
        public override TimeOnly StartTime
        {
            get => owner?.StartTime ?? base.StartTime;
            set
            {
                if (EffectiveClearSeconds())
                    value = DateUtils.ClearSeconds(value);

                if (owner is null)
                    base.StartTime = value;
                else
                    owner.StartTime = value;
            }
        }

        /// <inheritdoc/>
        public override DateOnly ValidToDate
        {
            get => owner?.ValidToDate ?? base.ValidToDate;
            set
            {
                if (owner is null)
                    base.ValidToDate = value;
                else
                    owner.ValidToDate = value;
            }
        }

        /// <inheritdoc/>
        public override int OccurrenceCount
        {
            get => owner?.OccurrenceCount ?? base.OccurrenceCount;
            set
            {
                if (owner is null)
                    base.OccurrenceCount = value;
                else
                    owner.OccurrenceCount = value;
            }
        }

        /// <inheritdoc/>
        public override RepeatPatternEndCondition EndCondition
        {
            get => owner?.EndCondition ?? base.EndCondition;
            set
            {
                if (owner is null)
                    base.EndCondition = value;
                else
                    owner.EndCondition = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the seconds component of the time should be cleared.
        /// </summary>
        /// <returns><c>true</c> if the seconds component should be cleared; otherwise, <c>false</c>.</returns>
        protected virtual bool EffectiveClearSeconds()
        {
            return GetDocumentOrDefault().EffectiveClearSeconds();
        }

        /// <summary>
        /// Gets the <see cref="SchedulerDocument"/> associated with this repeat pattern rule,
        /// or returns the default document instance if none is set.
        /// </summary>
        /// <returns>The associated <see cref="SchedulerDocument"/> or the default document instance.</returns>
        public SchedulerDocument GetDocumentOrDefault()
        {
            return Document ?? SchedulerDocument.Instance;
        }
    }
}
