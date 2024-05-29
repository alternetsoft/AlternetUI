#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description: Contains converter for creating PropertyPath from string
//              and saving PropertyPath to string
//

using System;



#pragma warning disable 1634, 1691  // suppressing PreSharp warnings

namespace Alternet.UI.Port
{
    internal class ItemsControl
    {
        internal static bool EqualsEx(object o1, object o2)
        {
            try
            {
                return Object.Equals(o1, o2);
            }
            catch (System.InvalidCastException)
            {
                // A common programming error: the type of o1 overrides Equals(object o2)
                // but mistakenly assumes that o2 has the same type as o1:
                //     MyType x = (MyType)o2;
                // This throws InvalidCastException when o2 is a sentinel object,
                // e.g. UnsetValue, DisconnectedItem, NewItemPlaceholder, etc.
                // Rather than crash, just return false - the objects are clearly unequal.
                return false;
            }
        }
    }
}
