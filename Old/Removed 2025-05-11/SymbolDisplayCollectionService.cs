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

using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    public class SymbolDisplayCollectionService
    {
        public virtual IEnumerable<Symbol> GetFilteredItems(IEnumerable<Symbol> items, string searchString)
        {
            return items.Where(x => ItemFilter(x, searchString));
        }

        public virtual IEnumerable<Symbol> SortSymbols(IEnumerable<Symbol> symbols, string searchString, bool prioritizeCaseSensitiveMatch)
        {
            var result = new List<Symbol>(symbols);

            Action<int?> moveItemToTop = index =>
            {
                var item = result[index.Value];
                result.RemoveAt(index.Value);
                result.Insert(0, item);
            };

            // Move all matches to the top, ignoring the case.
            int startIndex = 0;
            while (true)
            {
                int? index = FindSymbolByName(result, startIndex, searchString, StringComparison.OrdinalIgnoreCase);
                if (index == null)
                    break;

                moveItemToTop(index);
                startIndex++;
            }

            if (prioritizeCaseSensitiveMatch)
            {
                // If required (e.g. for C#), move the exact match to the very top.
                var matchCaseIndex = FindSymbolByName(result, 0, searchString, StringComparison.Ordinal);
                if (matchCaseIndex != null)
                    moveItemToTop(matchCaseIndex);
            }

            return result;
        }

        protected virtual bool ItemFilter(Symbol item, string searchString)
        {
            return item.Name.StartsWith(searchString, StringComparison.OrdinalIgnoreCase);
        }

        protected virtual int? FindSymbolByName(IList<Symbol> symbols, int startIndex, string name, StringComparison comparison)
        {
            for (int i = startIndex; i < symbols.Count; i++)
            {
                if (symbols[i].Name.Equals(name, comparison))
                    return i;
            }

            return null;
        }
    }
}