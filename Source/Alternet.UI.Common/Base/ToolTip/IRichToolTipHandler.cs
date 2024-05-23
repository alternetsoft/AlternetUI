using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IRichToolTipHandler : IDisposable
    {
        void SetBackgroundColor(Color color, Color endColor);

        void SetIcon(MessageBoxIcon icon);

        void SetForegroundColor(Color color);

        void SetTitleForegroundColor(Color color);

        void SetTimeout(uint milliseconds, uint millisecondsShowdelay = 0);

        void SetIcon(ImageSet? bitmap);

        void SetTitleFont(Font? font);

        void SetTipKind(RichToolTipKind tipKind);

        void Show(Control control, RectI? rect = null);
    }
}
