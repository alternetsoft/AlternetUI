#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Description: Defines the basic Name Scoping interface for root classes

namespace Alternet.UI
{
    /// <summary>
    /// INameScope- Defines the basic Name Scoping interface for root classes
    /// </summary>
    /// 
    public interface INameScope
    {
        /// <summary>
        /// Registers the name - element combination
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="scopedElement">Element where name is defined</param>
        void RegisterName(string name, object scopedElement);

        /// <summary>
        /// Unregisters the name - element combination
        /// </summary>
        /// <param name="name">Name of the element</param>
        void UnregisterName(string name);

        /// <summary>
        /// Find the element given name
        /// </summary>
        object FindName(string name);
    }
}

