using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IListEditSource
    {
        bool AllowSubItems { get; }
        bool AllowAdd { get; }
        bool AllowDelete { get; }

        object? Instance { get; set; }

        public PropertyInfo? PropInfo { get; set; }

        IEnumerable? RootItems { get; }

        IEnumerable? GetChildren(object item);

        string? GetItemTitle(object item);

        object? GetProperties(object item);

        ImageList? ImageList { get; }

        int? GetItemImageIndex(object item);

        object? CreateNewItem();
    }
}
