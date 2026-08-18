using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ColorListBox"/> control.
    /// </summary>
    public partial class PopupColorListBox : PopupListBox<ColorListBox>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupColorListBox"/> class.
        /// </summary>
        public PopupColorListBox()
            : this(defaultColors: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupColorListBox"/> class with specified default colors usage flag.
        /// </summary>
        /// <param name="defaultColors">A value indicating whether to use default colors.</param>
        public PopupColorListBox(bool defaultColors)
            : base(defaultColors)
        {
            Title = CommonStrings.Default.WindowTitleSelectColor;
        }

        /// <inheritdoc/>
        protected override ColorListBox CreateMainControl()
        {
            bool defaultColors = true;

            if (InitialSettings is bool initialSettingsBool)
                defaultColors = initialSettingsBool;

            return new ColorListBox(defaultColors)
            {
                HasBorder = false,
            };
        }

        /// <summary>
        /// Gets popup result as <see cref="Color"/>.
        /// </summary>
        public virtual Color? ResultValue
        {
            get
            {
                if (ResultIndex is null)
                    return null;

                var color = ColorListBox.GetItemValueOrDefault(
                    MainControl,
                    ResultIndex.Value,
                    Color.Empty);

                return color;
            }
        }

        /// <summary>
        /// Gets popup result as <see cref="DrawingResource"/>.
        /// </summary>
        public virtual DrawingResource? ResultAsDrawingResource
        {
            get
            {
                if (ResultIndex is null)
                    return null;
                var result = ColorListBox.GetItemValueAsDrawingResource(MainControl, ResultIndex.Value);
                return result;
            }
        }
    }
}
