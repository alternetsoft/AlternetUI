using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements speed button control.
    /// </summary>
    public class SpeedButton : Border
    {
        private readonly PictureBox pictureBox = new()
        {
            Dock = DockStyle.Left,
        };

        private readonly GenericLabel label = new()
        {
            Dock = DockStyle.Left,
        };

        public SpeedButton()
        {
            AcceptsFocusAll = false;
            pictureBox.Parent = this;
            label.Parent = this;
        }

        [Browsable(false)]
        public PictureBox PictureBox => pictureBox;

        public Image? Image
        {
            get => pictureBox.Image;

            set
            {
                pictureBox.Image = value;
                PerformLayout();
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        public override string Text
        {
            get
            {
                return label.Text;
            }

            set
            {
                if (label.Text == value)
                    return;
                label.Text = value;
                PerformLayout();
                OnTextChanged(EventArgs.Empty);
            }
        }
    }
}
