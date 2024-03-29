#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
// This file was generated, please do not edit it directly.
//
// Please see MilCodeGen.html for more information.
//


using Alternet.UI.Markup;
using Alternet.Drawing;

#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace Alternet.UI
{
    /// <summary>
    /// RectValueSerializer - ValueSerializer class for converting instances of strings to and from Rect instances
    /// This is used by the MarkupWriter class.
    /// </summary>
    public class RectValueSerializer : ValueSerializer 
    {
        /// <summary>
        /// Returns true.
        /// </summary>
        public override bool CanConvertFromString(string value, IValueSerializerContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the given value can be converted into a string
        /// </summary>
        public override bool CanConvertToString(object value, IValueSerializerContext context)
        {
            // Validate the input type
            if (!(value is RectD))
            {
                return false;
            }

            return true;
}

        /// <summary>
        /// Converts a string into a Rect.
        /// </summary>
        public override object ConvertFromString(string value, IValueSerializerContext context)
        {
            if (value != null)
            {
                return RectD.Parse(value );
            }
            else
            {
                return base.ConvertFromString( value, context );
            }
}

        /// <summary>
        /// Converts the value into a string.
        /// </summary>
        public override string ConvertToString(object value, IValueSerializerContext context)
        {
            if (value is RectD)
            {
                RectD instance = (RectD) value;


                #pragma warning suppress 6506 // instance is obviously not null
                return instance.ConvertToString(null, TypeConverterHelper.InvariantEnglishUS);
            }

            return base.ConvertToString(value, context);
        }
    }
}
