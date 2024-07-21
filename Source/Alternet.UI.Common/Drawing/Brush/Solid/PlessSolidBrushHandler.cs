using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Platformless solid brush handler.
    /// </summary>
    public class PlessSolidBrushHandler : PlessBrushHandler, ISolidBrushHandler
    {
        private Color color = Color.Black;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessSolidBrushHandler"/> class.
        /// </summary>
        /// <param name="brush"></param>
        public PlessSolidBrushHandler(SolidBrush brush)
            : base(brush)
        {
        }

        /// <summary>
        /// Gets brush color.
        /// </summary>
        public Color Color => color;

        /// <inheritdoc/>
        public virtual void Update(SolidBrush brush)
        {
            this.color = brush?.Color ?? Color.Black;
        }
    }
}
