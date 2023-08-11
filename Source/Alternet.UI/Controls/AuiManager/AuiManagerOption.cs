using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public enum AuiManagerOption
    {
        AllowFloating = 1 << 0,

        AllowActivePane = 1 << 1,

        TransparentDrag = 1 << 2,

        TransparentHint = 1 << 3,

        VenetianBlindsHint = 1 << 4,

        RectangleHint = 1 << 5,

        HintFade = 1 << 6,

        NoVenetianBlindsFade = 1 << 7,

        LiveResize = 1 << 8,

        Default = AllowFloating | TransparentHint | HintFade | NoVenetianBlindsFade,
    };

}
