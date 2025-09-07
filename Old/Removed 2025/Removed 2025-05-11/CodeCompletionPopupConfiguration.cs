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
using Alternet.Common;
using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;
using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Provides properties and methods to setup Code completion pop-up window behavior.
    /// </summary>
    public class CodeCompletionPopupConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <c>CodeCompletionPopupConfiguration</c>
        /// class with specified settings.
        /// </summary>
        /// <param name="imageList">ImageList related to pop-up window.</param>
        /// <param name="alphaImageList">AlphaImageList related to a pop-up window.</param>
        /// <param name="getImageIndexFunc">Method to get image index
        /// accordingly to specified SymbolKind.</param>
        public CodeCompletionPopupConfiguration()
        {
        }

        /// <summary>
        /// Represents the AlphaImageList that contains the images to display in
        /// Code completion pop-up window.
        /// </summary>
        public AlphaImageList? AlphaImageList { get; private set; }

        /// <summary>
        /// Represents a method to get image index accordingly to specified SymbolKind.
        /// </summary>
        public Func<SymbolKind, int>? GetImageListIndexFunc { get; private set; }
    }
}