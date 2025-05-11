#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Types of animation of the pop-up window.
    /// </summary>
    [Flags]
    public enum PopupAnimations
    {
        /// <summary>
        /// Uses no animation.
        /// </summary>
        None = 0,

        /// <summary>
        /// Animates the window from left to right. This flag can be used with roll or slide animation.
        /// </summary>
        LeftToRight = 0x00001,

        /// <summary>
        /// Animates the window from right to left. This flag can be used with roll or slide animation.
        /// </summary>
        RightToLeft = 0x00002,

        /// <summary>
        /// Animates the window from top to bottom. This flag can be used with roll or slide animation.
        /// </summary>
        TopToBottom = 0x00004,

        /// <summary>
        /// Animates the window from bottom to top. This flag can be used with roll or slide animation.
        /// </summary>
        BottomToTop = 0x00008,

        /// <summary>
        /// Makes the window appear to collapse inward if it is hiding or expand outward
        /// if the window is showing.
        /// </summary>
        Center = 0x00010,

        /// <summary>
        /// Uses a slide animation.
        /// </summary>
        Slide = 0x40000,

        /// <summary>
        /// Uses a fade effect.
        /// </summary>
        Blend = 0x80000,

        /// <summary>
        /// Uses a roll animation.
        /// </summary>
        Roll = 0x100000,

        /// <summary>
        /// Uses a default animation.
        /// </summary>
        SystemDefault = 0x200000,
    }

    /// <summary>
    /// Represents a pop-up window.
    /// </summary>
    [ToolboxItem(false)]
    public partial class Popup : PopupListBox<VirtualListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Popup"/> class.
        /// </summary>
        /// <param name="content">The content of the pop-up.</param>
        /// <remarks>
        /// Pop-up will be disposed immediately after disposal of the content control.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="content" />
        /// is <code>null</code>.</exception>
        public Popup()
        {
            FocusOnOpen = true;
            AcceptAlt = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the content should receive the focus after the pop-up has been opened.
        /// </summary>
        /// <value><c>true</c> if the content should be focused after the pop-up has been opened; otherwise, <c>false</c>.</value>
        /// <remarks>If the FocusOnOpen property is set to <c>false</c>, then pop-up cannot use the fade effect.</remarks>
        public bool FocusOnOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether pressing the alt key should close the pop-up.
        /// </summary>
        /// <value><c>true</c> if pressing the alt key does not close the pop-up; otherwise, <c>false</c>.</value>
        public bool AcceptAlt { get; set; }
    }
}