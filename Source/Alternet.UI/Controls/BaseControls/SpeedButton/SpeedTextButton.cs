using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="SpeedButton"/> descendant which by default shows text and no image.
    /// </summary>
    public class SpeedTextButton : SpeedButton
    {
        private static SpeedTextButton? defaults;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedTextButton"/> class.
        /// </summary>
        public SpeedTextButton()
        {
            ImageVisible = false;
            TextVisible = true;
        }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static new IReadOnlyFontAndColor? DefaultHoveredColors { get; set; }

        /// <summary>
        /// Gets or sets default pressed state colors.
        /// </summary>
        public static new IReadOnlyFontAndColor? DefaultPressedColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static new IReadOnlyFontAndColor? DefaultNormalColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static new IReadOnlyFontAndColor? DefaultDisabledColors { get; set; }

        /// <summary>
        /// Gets or sets default hovered state colors.
        /// </summary>
        public static new IReadOnlyFontAndColor? DefaultFocusedColors { get; set; }

        /// <summary>
        /// Gets or sets default settings for the <see cref="SpeedTextButton"/>.
        /// </summary>
        /// <remarks>
        /// Create instance of the <see cref="SpeedTextButton"/> and assign to this property.
        /// You can specify border and background settings and all new <see cref="SpeedTextButton"/>
        /// controls will inherit them.
        /// </remarks>
        public static new SpeedTextButton? Defaults
        {
            get
            {
                return defaults;
            }

            set
            {
                defaults = value;
            }
        }

        /// <inheritdoc/>
        protected override void InitBorderAndColors()
        {
            base.InitBorderAndColors();

            Borders!.Normal = Borders.Hovered;
        }

        /// <inheritdoc/>
        protected override IReadOnlyFontAndColor? GetPressedColors()
        {
            return DefaultPressedColors ?? FontAndColor.SystemColorActiveCaption;
        }

        /// <inheritdoc/>
        protected override IReadOnlyFontAndColor? GetHoveredColors()
        {
            return DefaultHoveredColors ?? FontAndColor.SystemColorHighlight;
        }

        /// <inheritdoc/>
        protected override IReadOnlyFontAndColor? GetFocusedColors()
        {
            return DefaultFocusedColors;
        }

        /// <inheritdoc/>
        protected override IReadOnlyFontAndColor? GetDisabledColors()
        {
            return DefaultDisabledColors;
        }

        /// <inheritdoc/>
        protected override IReadOnlyFontAndColor? GetNormalColors()
        {
            return DefaultNormalColors;
        }

        /// <inheritdoc/>
        protected override SpeedButton? GetButtonDefaults()
        {
            return defaults;
        }
    }
}
