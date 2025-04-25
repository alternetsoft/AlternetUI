using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class ScrollBarSettings : BaseObjectWithNotify
    {
        private HiddenOrVisible visibility;

        public HiddenOrVisible SuggestedVisibility
        {
            get
            {
                return visibility;
            }

            set
            {
                SetProperty(ref visibility, value);
            }
        }
    }
}