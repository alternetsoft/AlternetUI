using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ControlView"/> descendant with the owned control of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of the owned control.</typeparam>
    public partial class ControlView<T> : ControlView
        where T : AbstractControl, new()
    {
        private readonly T ownedControl;

        static ControlView()
        {
            ControlView.InitMauiHandler();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlView{T}"/> class.
        /// </summary>
        public ControlView()
        {
            ownedControl = CreateControl();

            if (ownedControl is ScrollableUserControl scrollable)
            {
                scrollable.UseInternalScrollBars = false;
                scrollable.SetInternalScrollBarsImmutable();
            }

            base.Control = ownedControl;
        }

        /// <summary>
        /// Gets owned control.
        /// </summary>
        public new T Control
        {
            get => ownedControl;
            set
            {
            }
        }

        /// <summary>
        /// Creates owned control.
        /// </summary>
        /// <returns></returns>
        protected virtual T CreateControl()
        {
            return new T();
        }
    }
}
