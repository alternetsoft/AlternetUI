using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    internal class SchedulerDocumentListEditSource : ListEditSource
    {
        private readonly SchedulerDocument document;

        public SchedulerDocumentListEditSource(SchedulerDocument instance)
        {
            document = instance;
            Instance = document;
        }        

        /// <inheritdoc/>
        public override IEnumerable? RootItems
        {
            get
            {
                return document.Items;
            }
        }

        /// <inheritdoc/>
        public override object? CreateNewItem()
        {
            var result = new CalendarItem();
            result.Text = CommonStrings.Default.UntitledItemText;
            return result;
        }

        /// <inheritdoc/>
        public override void ApplyData(IEnumerableTree tree)
        {
            var items = EnumerableUtils.GetItems<CalendarItem>(tree);

            document.Items.BeginUpdate();
            try
            {
                document.Items.Clear();
                document.Items.AddRange(items);
            }
            finally
            {
                document.Items.EndUpdate();
            }
        }

        /// <inheritdoc/>
        public override object CloneItem(object item)
        {
            return (item as CalendarItem)?.Clone() ?? item;
        }
    }
}