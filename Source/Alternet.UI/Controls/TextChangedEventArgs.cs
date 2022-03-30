// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description: TextChanged event argument.
//

using System;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle the text change routed events.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void TextChangedEventHandler(object sender, TextChangedEventArgs e);

    /// <summary>
    /// Provides data for the text change routed events.
    /// </summary>
    public class TextChangedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextChangedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The event identifier (ID).</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TextChangedEventArgs(RoutedEvent id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

            RoutedEvent = id;
        }
    }
}
