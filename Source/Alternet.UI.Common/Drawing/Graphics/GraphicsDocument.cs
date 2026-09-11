using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class GraphicsDocument : GenericControl
    {
        private GenericWrappedTextControl? wrappedText;
        private readonly LightDarkColor drawTextColor = new(Color.Black);

        public GraphicsDocument()
        {
        }

        public LightDarkColor DrawTextColor => drawTextColor;

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
