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
    internal class CardPanelHeaderItem : BaseControlItem
    {
        private readonly SpeedButton headerControl;
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
        public virtual SpeedButton HeaderButton => headerControl;

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
        /// Gets or sets tab image.
        /// </summary>
        public virtual Image? Image
        {
            get => HeaderButton?.Image;
            set
            {
                if(HeaderButton is null)
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
