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
using System.Collections.Generic;
using System.Drawing;
using Alternet.UI;

using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Represents a pop-up window for Code Completion controller.
    /// </summary>
    public interface ICodeCompletionPopup
    {
        /// <summary>
        /// When implemented by a class, indicates whether pop-up window is visible.
        /// </summary>
        bool Visible { get; }

        /// <summary>
        /// When implemented by a class, closes pop-up control.
        /// </summary>
        void Close();

        /// <summary>
        /// When implemented by a class, updates <c>Symbol</c> collection to the pop-up control.
        /// </summary>
        /// <param name="items">List of <c>Symbol</c> to be set.</param>
        void SetItems(IEnumerable<Symbol> items);

        /// <summary>
        /// When implemented by a class, represents list of items in a string form.
        /// </summary>
        /// <returns>List of items.</returns>
        IEnumerable<string> GetItems();

        /// <summary>
        /// When implemented by a class, finds out currently selected item in the pop-up window.
        /// </summary>
        /// <returns>Selected item in a string form.</returns>
        string GetSelectedItem();

        /// <summary>
        /// When implemented by a class, changes selection accordingly to specified command.
        /// </summary>
        /// <param name="command">Specifies selection command.</param>
        void ChangeSelection(ChangePopupSelectionCommand command);

        /// <summary>
        /// When implemented by a class, displays popup window with specified parameters.
        /// </summary>
        /// <param name="ownerControl">The control that is the reference point for the pop-up position.</param>
        /// <param name="position">The horizontal and vertical location of the reference control's upper-left corner, in pixels.</param>
        void Show(Control ownerControl, Point position);
    }
}