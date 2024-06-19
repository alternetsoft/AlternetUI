using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public interface IButtonHandler : IControlHandler
    {
        Image? NormalImage { set; }

        Image? HoveredImage { set; }

        Image? PressedImage { set; }

        Image? DisabledImage { set; }

        Image? FocusedImage { set; }

        Action? Click { get; set; }

        bool HasBorder { get; set; }

        bool IsDefault { get; set; }

        bool ExactFit { get; set; }

        bool IsCancel { get; set; }

        bool TextVisible { get; set; }

        GenericDirection TextAlign { get; set; }

        void SetImagePosition(GenericDirection dir);

        void SetImageMargins(double x, double y);
    }
}
