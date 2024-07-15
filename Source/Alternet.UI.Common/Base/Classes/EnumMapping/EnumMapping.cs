using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to map source enum values to the destination enum values.
    /// </summary>
    /// <typeparam name="TSource">Type of the source enum.</typeparam>
    /// <typeparam name="TDest">Type of the destination enum.</typeparam>
    public class EnumMapping<TSource, TDest> : AbstractEnumMapping<TSource, TDest>
        where TSource : struct, Enum
        where TDest : struct, Enum
    {
        private readonly TDest[] values;
        private readonly int maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMapping{TSource, TDest}"/> class.
        /// </summary>
        /// <param name="maxValue">Maximal value of the source enum. Optional. If not specified,
        /// <see cref="EnumUtils.GetMaxValueAsInt{T}()"/> is used.</param>
        public EnumMapping(TSource? maxValue = null)
        {
            if (maxValue is null)
                this.maxValue = EnumUtils.GetMaxValueAsInt<TSource>();
            else
                this.maxValue = System.Convert.ToInt32(maxValue.Value);

            values = new TDest[this.maxValue + 1];
        }

        /// <summary>
        /// Adds enum mapping.
        /// </summary>
        /// <param name="from">Source enum value.</param>
        /// <param name="to">Destination enum value.</param>
        public override void Add(TSource from, TDest to)
        {
            var intValue = System.Convert.ToInt32(from);
            values[intValue] = to;
        }

        /// <summary>
        /// Removes enum mapping.
        /// </summary>
        /// <param name="from">Source enum value.</param>
        public override void Remove(TSource from)
        {
            var intValue = System.Convert.ToInt32(from);
            values[intValue] = default;
        }

        /// <summary>
        /// Converts source enum value to the destination enum value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="defaultValue">Value used when source
        /// to destination mapping is not specified.</param>
        /// <returns></returns>
        public override TDest Convert(TSource value, TDest defaultValue = default)
        {
            var intValue = System.Convert.ToInt32(value);

            if (intValue < 0 || intValue > maxValue)
                return defaultValue;

            var result = values[intValue];

            if (result.Equals(default))
                return defaultValue;
            else
                return result;
        }
    }
}
