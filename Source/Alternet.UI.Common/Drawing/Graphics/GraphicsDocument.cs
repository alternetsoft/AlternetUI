using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class GraphicsDocument : GenericControl
    {
        private GenericWrappedTextControl? wrappedText;

        public GraphicsDocument()
        {
        }

        public virtual GenericWrappedTextControl WrappedText
        {
            get
            {
                if (wrappedText is null)
                {
                    wrappedText = new(this);
                    wrappedText.VerticalAlignment = Alternet.UI.VerticalAlignment.Top;
                    wrappedText.HorizontalAlignment = Alternet.UI.HorizontalAlignment.Left;
                }

                return wrappedText;
            }
        }
    }
}
