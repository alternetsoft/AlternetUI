#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System.Collections.Generic;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Represents data for the parameter info.
    /// </summary>
    public class ParameterInfoSymbol
    {
        /// <summary>
        /// Initializes a new instance of the <c>ParameterInfoSymbol</c> class with specified parameters.
        /// </summary>
        /// <param name="displayText">Formatted string contained parameter data.</param>
        /// <param name="symbolDescription">Description of the current symbol.</param>
        /// <param name="parameterDescription">Description of the parameter</param>
        /// <param name="currentSymbolIndex">Index of current symbol.</param>
        /// <param name="symbolsCount">Number of the symbols in the parameter info.</param>
        /// <param name="symbols">Symbols in the parameter info.</param>
        public ParameterInfoSymbol(
            string displayText,
            string symbolDescription,
            string parameterDescription,
            int currentSymbolIndex,
            int symbolsCount,
            IEnumerable<ParameterInfoSymbol> symbols)
        {
            DisplayText = displayText;
            SymbolDescription = symbolDescription;
            ParameterDescription = parameterDescription;
            CurrentSymbolIndex = currentSymbolIndex;
            SymbolsCount = symbolsCount;
            Symbols = symbols;
        }

        /// <summary>
        /// Gets formatted string contained parameter data.
        /// </summary>
        public string DisplayText { get; private set; }

        /// <summary>
        /// Gets description of the current symbol.
        /// </summary>
        public string SymbolDescription { get; private set; }

        /// <summary>
        /// Gets description of the parameter.
        /// </summary>
        public string ParameterDescription { get; private set; }

        /// <summary>
        /// Gets index of current symbol.
        /// </summary>
        public int CurrentSymbolIndex { get; private set; }

        /// <summary>
        /// Gets number of the symbols in the parameter info.
        /// </summary>
        public int SymbolsCount { get; private set; }

        /// <summary>
        /// Gets symbols in the parameter info.
        /// </summary>
        public IEnumerable<ParameterInfoSymbol> Symbols { get; private set; }
    }
}