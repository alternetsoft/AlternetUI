using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a repeat pattern rule for events or tasks that occur only once, without any repetition.
    /// </summary>
    public partial class OnceRepeatPatternRule : OwnedRepeatPatternRule, IPropertyBagSerialize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnceRepeatPatternRule"/> class with the specified owner.
        /// </summary>
        /// <param name="owner">The owner of the repeat pattern rule.</param>
        public OnceRepeatPatternRule(IRepeatPatternRuleOwner owner) : base(owner)
        {
        }

        /// <inheritdoc/>
        public override PatternRuleGetDatesResult GetDates(PatternRuleGetDatesParams prm)
        {
            return PatternRuleGetDatesResult.Empty;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
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

            return true;
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
                DoInsideSuspendedPropertyChanged(() =>
                {
                });

                return true;
            }

            if (other is OnceRepeatPatternRule otherRule)
            {
                if (Equals(other))
                    return false;
                DoInsideSuspendedPropertyChanged(() =>
                {
                });

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool LoadFrom(ISerializableObjectContext? context)
        {
            return false;
        }

        /// <inheritdoc/>
        public override bool SaveTo(ISerializableObjectContext? context)
        {
            return false;
        }
    }
}
