using System;
using System.ComponentModel;

namespace Alternet.UI
{
    /// <summary>
    /// TODO
    /// </summary>
    [TypeConverter(typeof(GridLengthConverter))]
    public struct GridLength
    {
        public GridLength(float value, GridUnitType gridUnitType)
        {
            Value = value;
            GridUnitType = gridUnitType;
        }

        public float Value { get; }

        public GridUnitType GridUnitType { get; }
    }
}