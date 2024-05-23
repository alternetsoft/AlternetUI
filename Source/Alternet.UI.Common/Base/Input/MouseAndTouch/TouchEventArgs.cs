using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public class TouchEventArgs : HandledEventArgs
    {
        public TouchEventArgs()
        {
        }

        public TouchEventArgs(long id, TouchAction type, PointD location, bool inContact)
            : this(id, type, MouseButton.Left, TouchDeviceType.Touch, location, inContact, 0, 1f)
        {
        }

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

        public long Id { get; set; }

        public TouchAction ActionType { get; set; }

        public TouchDeviceType DeviceType { get; set; }

        public MouseButton MouseButton { get; set; }

        public PointD Location { get; set; }

        public bool InContact { get; set; }

        public int WheelDelta { get; set; }

        public float Pressure { get; set; }

        public override string ToString()
        {
            return $"{{ActionType={ActionType}, DeviceType={DeviceType}, Handled={Handled}, Id={Id}, InContact={InContact}, Location={Location}, MouseButton={MouseButton}, WheelDelta={WheelDelta}, Pressure={Pressure}}}";
        }
    }
}
