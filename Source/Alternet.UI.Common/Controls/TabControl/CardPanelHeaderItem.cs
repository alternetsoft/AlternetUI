using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Individual page of the <see cref="CardPanelHeader"/>
    /// </summary>
    public class CardPanelHeaderItem : BaseControlItem
    {
        private SpeedButton headerControl;
        private AbstractControl? cardControl;
        private ObjectUniqueId? cardUniqueId;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelHeaderItem"/> class.
        /// </summary>
        /// <param name="headerControl">Control to use as <see cref="HeaderButton"/>
        /// property value.</param>
        public CardPanelHeaderItem(SpeedButton headerControl)
        {
            this.headerControl = headerControl;
        }

        /// <summary>
        /// Gets associated header control.
        /// </summary>
        public virtual SpeedButton HeaderButton
        {
            get
            {
                return headerControl;
            }

            internal set
            {
                headerControl = value;
            }
        }

        /// <summary>
        /// Gets or sets associated card control.
        /// </summary>
        public virtual AbstractControl? CardControl
        {
            get => cardControl;
            set => cardControl = value;
        }

        /// <summary>
        /// Gets or sets disabled tab image.
        /// </summary>
        public virtual Image? DisabledImage
        {
            get => HeaderButton?.DisabledImage;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.DisabledImage = value;
            }
        }

        /// <summary>
        /// Gets or sets disabled tab image.
        /// </summary>
        public virtual ImageSet? DisabledImageSet
        {
            get => HeaderButton?.DisabledImageSet;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.DisabledImageSet = value;
            }
        }

        /// <summary>
        /// Gets or sets tab svg color.
        /// </summary>
        public virtual Color? SvgColor
        {
            get => HeaderButton?.SvgColor;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.SvgColor = value;
            }
        }

        /// <summary>
        /// Gets or sets tab svg size.
        /// </summary>
        public virtual int? SvgSize
        {
            get => HeaderButton?.SvgSize;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.SvgSize = value;
            }
        }

        /// <summary>
        /// Gets or sets tab svg image.
        /// </summary>
        public virtual SvgImage? SvgImage
        {
            get => HeaderButton?.SvgImage;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.SvgImage = value;
            }
        }

        /// <summary>
        /// Gets or sets tab image.
        /// </summary>
        public virtual Image? Image
        {
            get => HeaderButton?.Image;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.Image = value;
            }
        }

        /// <summary>
        /// Gets or sets tab image.
        /// </summary>
        public virtual ImageSet? ImageSet
        {
            get => HeaderButton?.ImageSet;
            set
            {
                if (HeaderButton is null)
                    return;
                HeaderButton.ImageSet = value;
            }
        }

        /// <summary>
        /// Gets or sets associated <see cref="CardPanelItem"/> unique id.
        /// </summary>
        public virtual ObjectUniqueId? CardUniqueId
        {
            get => cardUniqueId;
            set => cardUniqueId = value;
        }

        /// <summary>
        /// Gets or sets text property of the <see cref="HeaderButton"/>.
        /// </summary>
        public virtual string? Text
        {
            get
            {
                return HeaderButton?.Text;
            }

            set
            {
                HeaderButton?.SetText(value);
            }
        }
    }
}
