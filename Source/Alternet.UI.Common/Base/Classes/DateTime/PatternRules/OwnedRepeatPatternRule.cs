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
        DateOnly StartDate { get; set; }

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
        DateOnly EndDate { get; set; }

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
    }

    /// <summary>
    /// Represents a struct that implements the <see cref="IRepeatPatternRuleOwner"/> interface,
    /// </summary>
    public struct RepeatPatternRuleOwnerStruct : IRepeatPatternRuleOwner
    {
        /// <summary>
        /// Gets or sets the start date of the repeat pattern.
        /// </summary>
        public DateOnly StartDate;

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
        public DateOnly EndDate;

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

        /// <inheritdoc/>
        DateOnly IRepeatPatternRuleOwner.StartDate
        {
            readonly get => StartDate;
            set => StartDate = value;
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
        DateOnly IRepeatPatternRuleOwner.EndDate
        {
            readonly get => EndDate;
            set => EndDate = value;
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

        /// <inheritdoc/>
        public override DateOnly StartDate
        {
            get => owner?.StartDate ?? base.StartDate;
            set
            {
                if (owner is null)
                    base.StartDate = value;
                else
                    owner.StartDate = value;
            }
        }

        /// <inheritdoc/>
        public override TimeSpan Duration
        {
            get => owner?.Duration ?? base.Duration;
            set
            {
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
                if (owner is null)
                    base.StartTime = value;
                else
                    owner.StartTime = value;
            }
        }

        /// <inheritdoc/>
        public override DateOnly EndDate
        {
            get => owner?.EndDate ?? base.EndDate;
            set
            {
                if (owner is null)
                    base.EndDate = value;
                else
                    owner.EndDate = value;
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
    }
}
