using System;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides data for the <see cref="AbstractControl.DragDrop"/>, <see cref="AbstractControl.DragEnter"/>,
    /// or <see cref="AbstractControl.DragOver"/> events.
    /// </summary>
    public class DragEventArgs : BaseEventArgs
    {
        private PointD mouseClientLocation;
        private PointD? mouseScreenLocation;
        private AbstractControl originalTarget;

        /// <summary>
        /// Initializes a new instance of the <see cref="DragEventArgs"/> class.
        /// </summary>
        /// <param name="originalTarget">The original target control of the drag event.</param>
        /// <param name="data">The data associated with this event.</param>
        /// <param name="mouseClientLocation">The client coordinates of the mouse pointer
        /// in device-independent units.</param>
        /// <param name="effect">One of the <see cref="DragDropEffects"/> values.</param>
        public DragEventArgs(
            AbstractControl originalTarget,
            IDataObject data,
            PointD mouseClientLocation,
            DragDropEffects effect)
        {
            this.originalTarget = originalTarget;
            Data = data;
            this.mouseClientLocation = mouseClientLocation;
            Effect = effect;
        }

        /// <summary>
        /// Gets or sets the <see cref="IDataObject"/> that contains the data associated
        /// with this event.
        /// </summary>
        public virtual IDataObject Data { get; set; }

        /// <summary>
        /// Gets or sets the original target control of the drag event.
        /// </summary>  
        public virtual AbstractControl OriginalTarget
        {
            get => originalTarget;
            set
            {
                if (originalTarget == value)
                    return;
                originalTarget = value;
                mouseScreenLocation = null;
            }
        }

        /// <summary>
        /// Gets or sets the client coordinates of the mouse pointer in device-independent units.
        /// </summary>
        public virtual PointD MouseClientLocation
        {
            get => mouseClientLocation;
            set
            {
                if (mouseClientLocation == value)
                    return;
                mouseClientLocation = value;
                mouseScreenLocation = null;
            }
        }

        /// <summary>
        /// Gets the X coordinate of the mouse pointer in screen coordinates in device-independent units.
        /// </summary>
        public float X
        {
            get
            {
                return MouseScreenLocation.X;
            }
        }

        /// <summary>
        /// Gets the Y coordinate of the mouse pointer in screen coordinates in device-independent units.
        /// </summary>
        public float Y
        {
            get
            {
                return MouseScreenLocation.Y;
            }
        }
    
        /// <summary>
        /// Gets the screen coordinates of the mouse pointer in device-independent units.
        /// </summary>
        public virtual PointD MouseScreenLocation
        {
            get
            {
                if (mouseScreenLocation == null)
                    mouseScreenLocation = OriginalTarget.PointToScreen(mouseClientLocation);
                return mouseScreenLocation.Value;
            }
        }

        /// <summary>
        /// Gets or sets the target drop effect in a drag-and-drop operation.
        /// </summary>
        public virtual DragDropEffects Effect { get; set; }
    }
}