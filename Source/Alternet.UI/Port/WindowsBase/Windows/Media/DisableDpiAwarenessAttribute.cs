#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
//
//
//  By default, Alternet UI application is Dpi-Aware when the UI layout is calculated.
//  But if in any case, an application wants to host Alternet UI control and doesn't 
//  want to support Dpi aware,  the way to achieve it is to add below attribute
//  value in its application assembly.
//
//     [assembly:System.Windows.Media.DisableDpiAwareness]
// 
//

using System;

namespace Alternet.UI.Media
{
    /// <summary>
    /// DisableDpiAwarenessAttribute tells to disable DpiAwareness in this 
    /// application for Alternet UI UI elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class DisableDpiAwarenessAttribute: Attribute
    {
        /// <summary>
        /// Ctor of DisableDpiAwareness
        /// </summary>
        public DisableDpiAwarenessAttribute( )
        {
        }
    }
}
