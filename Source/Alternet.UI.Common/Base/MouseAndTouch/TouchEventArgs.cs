using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides event data for the touch events.
    /// </summary>
    public class TouchEventArgs : HandledEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class.
        /// </summary>
        public TouchEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class
        /// with the specified paramaters.
        /// </summary>
        /// <param name="id">The ID used to track the touch event.</param>
        /// <param name="type">The type of touch action that initiated this event.</param>
        /// <param name="location">The location of the touch.</param>
        /// <param name="inContact">Whether or not the touch device is in contact with the screen.</param>
        public TouchEventArgs(long id, TouchAction type, PointD location, bool inContact)
            : this(id, type, MouseButton.Left, TouchDeviceType.Touch, location, inContact, 0, 1f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class with the specified
        /// parameters.
        /// </summary>
        /// <param name="mouseButton">The mouse button used to raise the touch event.</param>
        /// <param name="deviceType">The touch device used to raise the touch event.</param>
        /// <param name="id">The ID used to track the touch event.</param>
        /// <param name="type">The type of touch action that initiated this event.</param>
        /// <param name="location">The location of the touch.</param>
        /// <param name="inContact">Whether or not the touch device is in contact with the screen.</param>
        public TouchEventArgs(
            long id,
            TouchAction type,
            MouseButton mouseButton,
            TouchDeviceType deviceType,
            PointD location,
            bool inContact)
            : this(id, type, mouseButton, deviceType, location, inContact, 0, 1f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class with the specified
        /// parameters.
        /// </summary>
        /// <param name="mouseButton">The mouse button used to raise the touch event.</param>
        /// <param name="deviceType">The touch device used to raise the touch event.</param>
        /// <param name="wheelDelta">The amount the wheel was scrolled.</param>
        /// <param name="id">The ID used to track the touch event.</param>
        /// <param name="type">The type of touch action that initiated this event.</param>
        /// <param name="location">The location of the touch.</param>
        /// <param name="inContact">Whether or not the touch device is in contact with the screen.</param>
        public TouchEventArgs(
            long id,
            TouchAction type,
            MouseButton mouseButton,
            TouchDeviceType deviceType,
            PointD location,
            bool inContact,
            int wheelDelta)
            : this(id, type, mouseButton, deviceType, location, inContact, wheelDelta, 1f)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TouchEventArgs"/> class with the specified
        /// parameters.
        /// </summary>
        /// <param name="mouseButton">The mouse button used to raise the touch event.</param>
        /// <param name="deviceType">The touch device used to raise the touch event.</param>
        /// <param name="wheelDelta">The amount the wheel was scrolled.</param>
        /// <param name="pressure">The pressure of the touch event.</param>
        /// <param name="id">The ID used to track the touch event.</param>
        /// <param name="type">The type of touch action that initiated this event.</param>
        /// <param name="location">The location of the touch.</param>
        /// <param name="inContact">Whether or not the touch device is in contact with the screen.</param>
        public TouchEventArgs(
            long id,
            TouchAction type,
            MouseButton mouseButton,
            TouchDeviceType deviceType,
            PointD location,
            bool inContact,
            int wheelDelta,
            float pressure)
        {
            Id = id;
            ActionType = type;
            DeviceType = deviceType;
            MouseButton = mouseButton;
            Location = location;
            InContact = inContact;
            WheelDelta = wheelDelta;
            Pressure = pressure;
        }

        /// <summary>
        /// Gets or sets the ID used to track the touch event.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the type of touch action that initiated this event.
        /// </summary>
        public TouchAction ActionType { get; set; }

        /// <summary>
        /// Gets or sets the touch device used to raise the touch event.
        /// </summary>
        public TouchDeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the mouse button used to raise the touch event.
        /// </summary>
        public MouseButton MouseButton { get; set; }

        /// <summary>
        /// Gets or sets the location of the touch.
        /// </summary>
        public PointD Location { get; set; }

        /// <summary>
        /// Gets or sets whether or not the touch device is in contact with the screen.
        /// </summary>
        public bool InContact { get; set; }

        /// <summary>
        /// Gets or sets the amount the wheel was scrolled.
        /// </summary>
        public int WheelDelta { get; set; }

        /// <summary>
        /// Gets or sets the pressure of the touch event.
        /// </summary>
        public float Pressure { get; set; }

        /// <summary>
        /// Gets string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string[] names =
            {
                nameof(ActionType),
                nameof(DeviceType),
                nameof(Location),
                nameof(Handled),
                nameof(MouseButton),
                nameof(WheelDelta),
                nameof(InContact),
                nameof(Pressure),
                nameof(Id),
            };

            object[] values =
            {
                ActionType,
                DeviceType,
                Location,
                Handled,
                MouseButton,
                WheelDelta,
                InContact,
                Pressure,
                Id,
            };

            return StringUtils.ToStringWithOrWithoutNames(names, values);
        }
    }
}
