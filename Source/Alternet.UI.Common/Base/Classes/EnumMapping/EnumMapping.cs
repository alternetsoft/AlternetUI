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
        private readonly TDest?[] values;
        private readonly int maxValueAsInt;
        private readonly TSource maxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumMapping{TSource, TDest}"/> class.
        /// </summary>
        /// <param name="maxValue">Maximal value of the source enum.</param>
        public EnumMapping(TSource maxValue)
        {
            this.maxValue = maxValue;
            this.maxValueAsInt = System.Convert.ToInt32(maxValue);
            values = new TDest?[this.maxValueAsInt + 1];
        }

        /// <inheritdoc/>
        public override TSource SourceMaxValue => maxValue;

        /// <inheritdoc/>
        public override int SourceMaxValueAsInt => maxValueAsInt;

        /// <inheritdoc/>
        public override void LogToFile(string? logFileName = default)
        {
            var stype = typeof(TSource);
            var dtype = typeof(TDest);

            var prefix = $"Enum mapping from '{stype}' to '{dtype}'";

            App.Log($"{prefix} logged to file");

            LogUtils.LogActionToFile(
                () =>
                {
                    var enumValues = Enum.GetValues(stype).Cast<int>();
                    foreach (var source in enumValues)
                    {
                        var dest = values[source]?.ToString();
                        var sourceEnum = Enum.GetName(stype, source);

                        if(dest is not null)
                        {
                            dest = $"{dtype.Name}.{dest}";
                        }

                        LogUtils.LogToFile(
                            $"{stype.Name}.{sourceEnum} => {dest}",
                            logFileName);
                    }
                },
                prefix,
                logFileName);
        }

        /// <summary>
        /// Adds enum mapping.
        /// </summary>
        /// <param name="from">Source enum value.</param>
        /// <param name="to">Destination enum value.</param>
        public override void Add(TSource from, TDest? to)
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
            values[intValue] = null;
        }

        /// <inheritdoc/>
        public override bool HasMapping(TSource value)
        {
            if (ConvertOrNull(value) is null)
                return false;
            else
                return true;
        }

        /// <inheritdoc/>
        public override TDest? ConvertOrNull(TSource value)
        {
            var intValue = System.Convert.ToInt32(value);

            if (intValue < 0 || intValue > maxValueAsInt)
                return null;

            var result = values[intValue];
            return result;
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
            var result = ConvertOrNull(value);

            if (result is null)
                return defaultValue;
            else
                return result.Value;
        }
    }
}
