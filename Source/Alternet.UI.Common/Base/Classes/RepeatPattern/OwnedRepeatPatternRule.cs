using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a repeat pattern rule that is owned by another <see cref="DateRepeatPatternRule"/>.
    /// </summary>
    public partial class OwnedRepeatPatternRule : DateRepeatPatternRule
    {
        private readonly DateRepeatPatternRule owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnedRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner <see cref="DateRepeatPatternRule"/> of this rule.</param>
        public OwnedRepeatPatternRule(DateRepeatPatternRule owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Gets the owner <see cref="DateRepeatPatternRule"/> of this rule.
        /// </summary>
        public DateRepeatPatternRule Owner => owner;

        /// <inheritdoc/>
        public override DateOnly StartDate
        {
            get => owner.StartDate;
            set
            {
            }
        }

        /// <inheritdoc/>
        public override DateOnly? EndDate
        {
            get => owner.EndDate;
            set
            {
            }
        }

        /// <inheritdoc/>
        public override int OccurrenceCount
        {
            get => owner.OccurrenceCount;
            set
            {
            }
        }
    }
}
