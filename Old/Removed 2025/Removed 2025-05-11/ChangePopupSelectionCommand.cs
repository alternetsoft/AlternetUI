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

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Specifies the possible kinds of popup selection command.
    /// </summary>
    public enum ChangePopupSelectionCommand
    {
        /// <summary>
        /// Select up.
        /// </summary>
        Up,

        /// <summary>
        /// Select down.
        /// </summary>
        Down,

        /// <summary>
        /// Select page up.
        /// </summary>
        PageUp,

        /// <summary>
        /// Select page down.
        /// </summary>
        PageDown,
    }
}