using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public class GenericToolBarSet : VerticalStackPanel
    {
        public GenericToolBarSet()
        {
            ToolBarCount = 1;
        }

        public virtual int ToolBarCount
        {
            get
            {
                return Children.Count;
            }

            set
            {
                value = Math.Max(value, 1);
                if (value == ToolBarCount)
                    return;
                Children.SetCount(value, CreateToolBar);
            }
        }

        public virtual GenericToolBar GetToolBar(int index)
        {
            if (ToolBarCount <= index)
                ToolBarCount = index + 1;
            return (GenericToolBar)Children[index];
        }

        protected virtual GenericToolBar CreateToolBar()
        {
            GenericToolBar result = new();
            if(Children.Count > 0)
            {
                result.Margin = (0, 4, 0, 0);
            }

            return result;
        }
    }
}
