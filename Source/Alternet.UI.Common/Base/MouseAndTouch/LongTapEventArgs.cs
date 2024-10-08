using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides event data for the long tap events.
    /// </summary>
    public class LongTapEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LongTapEventArgs"/> class.
        /// </summary>
        public LongTapEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class with the specified
        /// parameters.
        /// </summary>
        /// <param name="deviceType">The touch device used to raise the touch event.</param>
        /// <param name="location">The location of the touch.</param>
        public LongTapEventArgs(
            TouchDeviceType deviceType,
            PointD location)
        {
            DeviceType = deviceType;
            Location = location;
        }

        /// <summary>
        /// Gets or sets the touch device used to raise the touch event.
        /// </summary>
        public TouchDeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the location of the touch.
        /// </summary>
        public PointD Location { get; set; }

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string[] names = { nameof(DeviceType), nameof(Location) };
            object[] values = { DeviceType, Location };

            return StringUtils.ToStringWithOrWithoutNames(names, values);
        }
    }
}
