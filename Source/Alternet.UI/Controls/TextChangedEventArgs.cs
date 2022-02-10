// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description: TextChanged event argument.
//

using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Alternet.UI
{
    #region TextChangedEvent

    public delegate void TextChangedEventHandler(object sender, TextChangedEventArgs e);

    public class TextChangedEventArgs : RoutedEventArgs
    {
        public TextChangedEventArgs(RoutedEvent id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            RoutedEvent = id;
        }
    }

    #endregion TextChangedEvent
}
