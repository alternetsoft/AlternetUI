using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control overlay that includes a tooltip with customizable parameters.
    /// </summary>
    /// <remarks>This class extends <see cref="ControlOverlayWithImage"/> by adding
    /// support for a rich tooltip. The tooltip can be customized using
    /// the <see cref="ToolTip"/> property. Use the <see cref="UpdateImage"/> method
    /// in order to update the visual representation of the tooltip.</remarks>
    public class ControlOverlayWithToolTip : ControlOverlayWithImage
    {
        private RichToolTipParams? toolTipParams;
        private TemplateControls.RichToolTipTemplate? template;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlOverlayWithToolTip"/> class.
        /// </summary>
        /// <remarks>This constructor sets up a new control overlay with tooltip functionality. 
        /// Additional configuration may be required after initialization to fully
        /// utilize the overlay.</remarks>
        public ControlOverlayWithToolTip()
        {
            Flags = ControlOverlayFlags.RemoveOnEscape | ControlOverlayFlags.RemoveOnEnter;
        }

        /// <summary>
        /// Gets or sets the template used to define the content and layout of the rich tooltip.
        /// </summary>
        /// <remarks>Use this property to customize the visual presentation of the
        /// rich tooltip by providing a specific template.
        /// If no template is explicitly set, a default instance will be used.</remarks>
        public virtual TemplateControls.RichToolTipTemplate Template
        {
            get
            {
                return template ??= new();
            }

            set => template = value;
        }

        /// <summary>
        /// Gets or sets the tooltip parameters.
        /// </summary>
        public virtual RichToolTipParams ToolTip
        {
            get
            {
                return toolTipParams ??= new();
            }

            set => toolTipParams = value;
        }

        /// <summary>
        /// Updates the <see cref="ControlOverlayWithImage.Image"/> property
        /// with a new image generated
        /// from the current template and tooltip parameters.
        /// </summary>
        /// <remarks>This method ensures that the <see cref="ControlOverlayWithImage.Image"/>
        /// property is updated using the
        /// current state of the template and tooltip parameters.
        /// If the template is null, a new instance is created
        /// before generating the image.</remarks>
        public virtual void UpdateImage()
        {
            var image = RichToolTip.CreateToolTipImage(Template, ToolTip);
            Image = image;
        }
    }
}
