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
using System.Linq;
using System.Threading.Tasks;

using Alternet.Common;

using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Provides a base class for evaluation controller. This class is abstract.
    /// </summary>
    public abstract class EvaluationCodeCompletionControllerBase : IDisposable
    {
        private ParameterInfo parameterInfo;

        private int? parameterInfoCurrentSymbolIndex;

        private CollectionSetterCache<Symbol> completionPopupItemsSetterCache;

        /// <summary>
        /// Initializes a new instance of the <c>EvaluationCodeCompletionControllerBase</c>
        /// class with specified parameters.
        /// </summary>
        /// <param name="site"><c>ISite</c> corresponding to this new instance.</param>
        /// <param name="debugger"><c>IScriptDebuggerBase</c> corresponding to this new
        /// instance.</param>
        public EvaluationCodeCompletionControllerBase(ISite site, IScriptDebuggerBase debugger)
        {
            this.Site = site;
            this.Debugger = debugger;

            InitializeCodeCompletion(debugger);
        }

        /// <summary>
        /// Represents methods declaration to support evaluation functionality.
        /// </summary>
        public interface ISite
        {
            /// <summary>
            /// When implemented by a class, closes evaluation controller.
            /// </summary>
            void Close();

            /// <summary>
            /// When implemented by a class, evaluates current expression.
            /// </summary>
            Task EvaluateCurrentExpressionAsync();
        }

        protected IScriptDebuggerBase Debugger { get; private set; }

        protected ISymbolRecommender SymbolRecommender { get; private set; }

        protected IParameterInfoProvider ParameterInfoProvider { get; private set; }

        protected SymbolDisplayCollectionService SymbolDisplayCollectionService
        {
            get;
            private set;
        }

        protected bool DisableCodeCompletion { get; set; }

        protected ISite Site { get; private set; }

        protected KeyConfiguration KeyConfiguration { get; private set; }

        protected virtual TimeSpan CompletionDelay
        {
            get
            {
                return TimeSpan.FromMilliseconds(100);
            }
        }

        protected abstract bool IsCodeCompletionPopupVisible { get; }

        protected abstract bool IsExpressionTextBoxFocused { get; }

        protected abstract string ExpressionText { get; set; }

        protected abstract int CursorPositionInExpression { get; set; }

        private CollectionSetterCache<Symbol> CompletionPopupItemsSetterCache
        {
            get
            {
                if (completionPopupItemsSetterCache == null)
                {
                    completionPopupItemsSetterCache = new CollectionSetterCache<Symbol>(
                        SetCodeCompletionPopupItemsCore,
                        x => x.Name.GetHashCode());
                }

                return completionPopupItemsSetterCache;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected virtual void InitializeCodeCompletion(IScriptDebuggerBase debugger)
        {
            var position = Task.Run(async () => await debugger.GetExecutionPositionAsync()).Result;
            if (position == null)
                return;

            SymbolDisplayCollectionService = CreateSymbolDisplayCollectionService();

            var completionService
                = debugger.ExpressionEvaluator?.GetCodeCompletionService(position);

            if (completionService != null)
            {
                KeyConfiguration = completionService.KeyConfiguration;
                SymbolRecommender = completionService.SymbolRecommender;
                ParameterInfoProvider = completionService.ParameterInfoProvider;
            }
            else
                KeyConfiguration = new KeyConfiguration();
        }

        protected virtual SymbolDisplayCollectionService CreateSymbolDisplayCollectionService()
        {
            return new SymbolDisplayCollectionService();
        }

        protected virtual void DoCodeCompletion()
        {
            TryShowParameterInfo();
            TryShowCodeCompletionDependingOnPressedKey();
        }

        protected abstract void TryShowCodeCompletionDependingOnPressedKey();

        protected virtual void TryShowCodeCompletionDependingOnPressedKey(char pressedKey)
        {
            bool showCodeCompletion = KeyConfiguration != null
                && KeyConfiguration.CodeCompletionKeys.Contains(pressedKey);

            if (pressedKey == '\b')
            {
                // Backspace was pressed.
                showCodeCompletion
                    = IsCodeCompletionPopupVisible && !string.IsNullOrEmpty(ExpressionText);
            }
            else if (ExpressionText.Length == 1)
                HideCodeCompletionPopup();

            if (showCodeCompletion)
                TryShowCodeCompletion(autocompleteOnSingleSuggestion: false);
            else
                HideCodeCompletionPopup();
        }

        protected virtual string GetSearchString(out char? lastExpressionSeparator)
        {
            string expressionToEvaluate = ExpressionText;
            int positionInExpression = CursorPositionInExpression;
            var substring = expressionToEvaluate.Substring(0, positionInExpression);

            var lastExpressionSeparatorIndex
                = substring.LastIndexOfAny(KeyConfiguration.ExpressionSeparators);
            lastExpressionSeparator =
                lastExpressionSeparatorIndex == -1
                ? (char?)null : substring[lastExpressionSeparatorIndex];

            return substring.Split(KeyConfiguration.ExpressionSeparators).Last();
        }

        protected virtual bool TryShowParameterInfo()
        {
            if (ParameterInfoProvider == null || !IsExpressionTextBoxFocused)
            {
                HideParameterInfoTooltip();
                return false;
            }

            parameterInfo = ParameterInfoProvider.GetParameterInfo(
                ExpressionText,
                CursorPositionInExpression);
            if (parameterInfo == null || !parameterInfo.Symbols.Any())
            {
                HideParameterInfoTooltip();
                parameterInfoCurrentSymbolIndex = null;
                return false;
            }

            if (parameterInfoCurrentSymbolIndex == null)
            {
                if (parameterInfo.DeducedCurrentSymbolIndex >= 0)
                    parameterInfoCurrentSymbolIndex = parameterInfo.DeducedCurrentSymbolIndex;
                else
                    parameterInfoCurrentSymbolIndex = 0;
            }
            else
            {
                parameterInfoCurrentSymbolIndex =
                    MathUtilities.Clamp(parameterInfoCurrentSymbolIndex.Value, 0, parameterInfo.Symbols.Count - 1);
            }

            ShowParameterInfoTooltip();
            return true;
        }

        protected virtual void ShowParameterInfoTooltip()
        {
            var text = ParameterInfoProvider.GetFormattedParameterInfoSymbolString(
                parameterInfo,
                parameterInfoCurrentSymbolIndex.Value);

            var currentSymbol = parameterInfo.Symbols[parameterInfoCurrentSymbolIndex.Value];
            var symbolDescription = currentSymbol.Description;

            var parameter = currentSymbol.Parameters.ElementAtOrDefault(currentSymbol.CurrentParameterIndex);
            var parameterDescription = parameter == null ? null : parameter.Description;

            var symbol = new ParameterInfoSymbol(
                text,
                symbolDescription,
                parameterDescription,
                currentSymbolIndex: parameterInfoCurrentSymbolIndex.Value,
                symbolsCount: parameterInfo.Symbols.Count,
                symbols: CreateParameterInfoSymbols());

            ShowParameterInfoTooltip(symbol);
        }

        protected virtual IList<ParameterInfoSymbol>? CreateParameterInfoSymbols()
        {
            if (parameterInfo?.Symbols == null)
                return null;

            var result = new List<ParameterInfoSymbol>();
            for (int i = 0; i < parameterInfo.Symbols.Count; i++)
            {
                var symbol = parameterInfo.Symbols[i];
                var text = ParameterInfoProvider.GetFormattedParameterInfoSymbolString(
                    parameterInfo,
                    i);

                var parameter = symbol.Parameters.ElementAtOrDefault(symbol.CurrentParameterIndex);
                var parameterDescription = parameter == null ? null : parameter.Description;

                var paramSymbol = new ParameterInfoSymbol(
                    text,
                    symbol.Description,
                    parameterDescription,
                    currentSymbolIndex: i,
                    symbolsCount: 0,
                    symbols: null);
                result.Add(paramSymbol);
            }

            return result;
        }

        protected abstract void ShowParameterInfoTooltip(ParameterInfoSymbol symbol);

        protected abstract void HideParameterInfoTooltip();

        protected virtual void HideCodeCompletionPopup()
        {
            CompletionPopupItemsSetterCache.Clear();
        }

        protected virtual void ChangeParameterInfoCurrentSymbol(int difference)
        {
            var symbolsCount = parameterInfo.Symbols.Count;
            if (symbolsCount == 1)
                return;

            parameterInfoCurrentSymbolIndex = MathUtilities.Wrap(
                parameterInfoCurrentSymbolIndex.Value + difference,
                0,
                symbolsCount - 1);

            ShowParameterInfoTooltip();
        }

        protected virtual void TryShowCodeCompletion(bool autocompleteOnSingleSuggestion)
        {
            if (DisableCodeCompletion)
                return;

            if (SymbolRecommender == null || !IsExpressionTextBoxFocused
                || KeyConfiguration == null)
                return;

            char? lastExpressionSeparator;
            var searchString = GetSearchString(out lastExpressionSeparator);

            var excludeKeywords =
                lastExpressionSeparator != null &&
                KeyConfiguration.ExcludeKeywordsCodeCompletionKeys.Contains(lastExpressionSeparator.Value);

            var symbols = SymbolRecommender.GetRecommendedSymbols(
                ExpressionText,
                CursorPositionInExpression,
                excludeKeywords);

            var sortedSymbols = SymbolDisplayCollectionService.SortSymbols(symbols, searchString, KeyConfiguration.PrioritizeCaseSensitiveMatch);

            ShowCodeCompletionPopup(autocompleteOnSingleSuggestion, searchString, sortedSymbols);
        }

        protected void SetCodeCompletionPopupItems(ICollection<Symbol> symbols)
        {
            CompletionPopupItemsSetterCache.SetItems(symbols);
        }

        protected abstract void SetCodeCompletionPopupItemsCore(IEnumerable<Symbol> symbols);

        protected abstract IEnumerable<string> GetCodeCompletionPopupItems();

        protected abstract void ShowCodeCompletionPopup();

        protected virtual void ShowCodeCompletionPopup(
            bool autocompleteOnSingleSuggestion,
            string searchString,
            IEnumerable<Symbol> sortedSymbols)
        {
            if (SymbolDisplayCollectionService == null)
                return;

            var filteredSymbols = SymbolDisplayCollectionService.GetFilteredItems(sortedSymbols, searchString).ToArray();

            if (!filteredSymbols.Any())
            {
                // If everything is filtered out, display without filtering.
                filteredSymbols = sortedSymbols.ToArray();
            }

            SetCodeCompletionPopupItems(filteredSymbols);

            bool needToShow = filteredSymbols.Length > 1 ||
                (filteredSymbols.Length == 1 && !filteredSymbols[0].Name.Equals(searchString, StringComparison.Ordinal));

            if (needToShow)
            {
                var items = GetCodeCompletionPopupItems();
                if (items.Count() == 1)
                {
                    if (autocompleteOnSingleSuggestion)
                    {
                        CompleteExpression(items.Single());
                        HideCodeCompletionPopup();
                        return;
                    }
                }

                ShowCodeCompletionPopup();
            }
            else
                HideCodeCompletionPopup();
        }

        protected virtual void CompleteExpression(string item)
        {
            char? lastExpressionSeparator;
            var searchString = GetSearchString(out lastExpressionSeparator);

            var expression = ExpressionText;
            int positionInExpression = CursorPositionInExpression;
            int searchStringStart = positionInExpression - searchString.Length;

            var pre = expression.Substring(0, searchStringStart);
            var post = expression.Substring(
                positionInExpression,
                expression.Length - positionInExpression);

            var newExpression = pre + item + post;
            var newPosition = positionInExpression + (newExpression.Length - expression.Length);

            ExpressionText = newExpression;
            CursorPositionInExpression = newPosition;
        }
    }
}