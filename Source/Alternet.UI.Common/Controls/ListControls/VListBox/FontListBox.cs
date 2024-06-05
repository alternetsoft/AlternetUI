﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ListBox"/> descendant for selecting font names.
    /// </summary>
    public class FontListBox : VListBox
    {
        /// <summary>
        /// Gets or sets method that initializes items in <see cref="FontListBox"/>.
        /// </summary>
        public static Action<FontListBox>? InitFonts = InitDefaultFonts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontListBox"/> class.
        /// </summary>
        public FontListBox()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontListBox"/> class.
        /// </summary>
        /// <param name="defaultFonts">Specifies whether to add default fonts
        /// to the control.</param>
        public FontListBox(bool defaultFonts)
        {
            Initialize(defaultFonts);
        }

        /// <summary>
        /// Adds font names to the <see cref="FontListBox"/>. This is default
        /// implementation of the initialization method. It is assigned to
        /// <see cref="InitFonts"/> property by default.
        /// </summary>
        /// <param name="control">Control to initialize.</param>
        public static void InitDefaultFonts(FontListBox control)
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
