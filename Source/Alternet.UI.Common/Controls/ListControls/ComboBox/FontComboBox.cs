using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ComboBox"/> descendant for selecting font names.
    /// </summary>
    public partial class FontComboBox : ComboBox
    {
        /// <summary>
        /// Gets or sets method that initializes items in <see cref="FontListBox"/>.
        /// </summary>
        public static Action<FontComboBox>? InitFonts = InitDefaultFonts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontComboBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public FontComboBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontComboBox"/> class.
        /// </summary>
        public FontComboBox()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontComboBox"/> class
        /// with default list of the fonts.
        /// </summary>
        /// <param name="defaultFonts">Specifies whether to add default fonts
        /// to the control.</param>
        public FontComboBox(bool defaultFonts)
        {
            Initialize(defaultFonts);
        }

        /// <summary>
        /// Gets or sets font name.
        /// </summary>
        public virtual string? Value
        {
            get
            {
                return SelectedItem as string;
            }

            set
            {
                if (Value == value)
                    return;

                var found = FindStringExact(value);
                if (found != null)
                    SelectedIndex = found.Value;
            }
        }

        /// <summary>
        /// Adds font names to the <see cref="FontComboBox"/>. This is default
        /// implementation of the initialization method. It is assigned to
        /// <see cref="InitFonts"/> property by default.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        public static void InitDefaultFonts(FontComboBox control)
        {
            ListControlUtils.AddFontNames(control);
        }

        /// <summary>
        /// Initializes control with default font names and assigns item painter.
        /// This method is called from constructor.
        /// </summary>
        /// <param name="defaultFonts">Whether to add default fonts.</param>
        protected virtual void Initialize(bool defaultFonts = true)
        {
            if (defaultFonts)
            {
                if (InitFonts is not null)
                    InitFonts(this);
            }
        }
    }
}
