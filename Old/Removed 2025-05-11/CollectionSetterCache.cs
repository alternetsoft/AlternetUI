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
using Alternet.Common;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Helps to avoid redundant SetItems operations when the items set previously are the same.
    /// </summary>
    public class CollectionSetterCache<T>
    {
        private int? lastCompletionPopupItemsHashCode;
        private int? lastCompletionPopupItemsCount;
        private Func<T, int> getItemHashCodeFunc;

        private Action<IEnumerable<T>> setItemsCore;

        /// <summary>
        /// Initializes a new instance of the <c>CollectionSetterCache</c> class with specified parameters.
        /// </summary>
        /// <param name="setItemsCore">Specifies core method to set new item.</param>
        /// <param name="getItemHashCodeFunc">Specifies method to handle items hash.</param>
        public CollectionSetterCache(Action<IEnumerable<T>> setItemsCore, Func<T, int> getItemHashCodeFunc)
        {
            this.setItemsCore = setItemsCore;
            this.getItemHashCodeFunc = getItemHashCodeFunc;
        }

        /// <summary>
        /// Sets new item collection.
        /// </summary>
        /// <param name="items">List of items to set.</param>
        public void SetItems(ICollection<T> items)
        {
            int hashCode = 0;
            if (lastCompletionPopupItemsCount != null && lastCompletionPopupItemsCount == items.Count)
            {
                hashCode = EnumerableExtensions.GetItemsHashCode(items, getItemHashCodeFunc);
                if (lastCompletionPopupItemsHashCode != null || lastCompletionPopupItemsHashCode == hashCode)
                    return;
            }

            setItemsCore(items);

            lastCompletionPopupItemsHashCode = hashCode;
            lastCompletionPopupItemsCount = items.Count;
        }

        /// <summary>
        /// Clears items hash.
        /// </summary>
        public void Clear()
        {
            lastCompletionPopupItemsHashCode = null;
            lastCompletionPopupItemsCount = null;
        }
    }
}