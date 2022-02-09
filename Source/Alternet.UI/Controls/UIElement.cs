using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Alternet.UI
{
    public class UIElement : DependencyObject
    {
        public event EventHandler LayoutUpdated;

        internal void RaiseEvent(RoutedEventArgs args)
        {
            throw new NotImplementedException();
        }

        internal DependencyObject GetUIParentCore()
        {
            throw new NotImplementedException();
        }

        internal DependencyObject GetUIParent(bool v)
        {
            throw new NotImplementedException();
        }
    }
}