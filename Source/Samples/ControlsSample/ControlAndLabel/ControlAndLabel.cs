using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements labeled control.
    /// </summary>
    /// <remarks> This is an abstract class. You can use <see cref="TextBoxAndLabel"/>,
    /// <see cref="ComboBoxAndLabel"/> or derive from <see cref="ControlAndLabel"/>
    /// in order to implement your own custom labeled control.</remarks>
    public abstract class ControlAndLabel : StackPanel
    {
        private readonly Label label = new()
        {

        };

        private readonly PictureBox errorPicture = new()
        {

        };

        private Control mainControl;

        public ControlAndLabel()
            : base()
        {
            label.Parent = this;
            mainControl = CreateControl();
            mainControl.Parent = this;
            errorPicture.Parent = this;
        }

        public Label Label => label;

        public PictureBox ErrorPicture => errorPicture;

        protected abstract Control CreateControl();
    }
}
