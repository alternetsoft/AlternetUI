#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// 
// Description:
//              This is a helper class to facilate the storage of Security critical data ( aka "Plutonium")
//
//              What is "critical data" ? This is any data created that required an Assert for it's creation. 
//              As an example - the creation of an HwndWrapper during Dispatcher.Attach. 
//
//
//
//

namespace Alternet.UI
{
    [FriendAccessAllowed]
    internal struct SecurityCriticalData<T>
    {
        internal SecurityCriticalData(T value) 
        { 
            _value = value; 
        }

        internal T Value 
        { 
            get 
            {     
                return _value; 
            } 
        }

        private T _value;
    }
}
