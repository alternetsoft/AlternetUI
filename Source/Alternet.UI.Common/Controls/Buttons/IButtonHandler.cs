using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    public interface IButtonHandler : IControlHandler
    {
        bool HasBorder { get; set; }

        bool IsDefault { get; set; }

        bool ExactFit { get; set; }

        bool IsCancel { get; set; }

        ControlStateImages StateImages { get; set; }

        bool TextVisible { get; set; }

        GenericDirection TextAlign { get; set; }

        void SetImagePosition(GenericDirection dir);

        void SetImageMargins(double x, double y);
    }
}
