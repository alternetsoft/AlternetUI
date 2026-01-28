using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents an immutable snapshot of mouse event data at a specific point in time.
    /// </summary>
    /// <remarks>This structure captures key properties of a mouse event, such as the button pressed, cursor
    /// location, click count, and the unique identifier of the current target control, if available. It is intended for
    /// scenarios where mouse event information needs to be stored or processed independently of the original event
    /// lifecycle.</remarks>
    public struct MouseEventArgsSnapshot
    {
        /// <summary>
        /// Represents the timestamp associated with the event or data, typically expressed as the number of ticks or
        /// milliseconds since a defined epoch.
        /// </summary>
        public long Timestamp;

        /// <summary>
        /// Specifies the mouse button associated with the event or action.
        /// </summary>
        public MouseButton Button;

        /// <summary>
        /// Specifies the location of the object in two-dimensional space.
        /// </summary>
        public PointD Location;

        /// <summary>
        /// Represents the number of times the associated element has been clicked.
        /// </summary>
        public int ClickCount;

        /// <summary>
        /// Gets or sets the unique identifier of the current target object, if available.
        /// </summary>
        public ObjectUniqueId? CurrentTargetUniqueId;

        /// <summary>
        /// Initializes a new instance of the MouseEventArgsSnapshot class using the specified mouse event arguments.
        /// </summary>
        /// <param name="e">The MouseEventArgs instance containing the details of the mouse event to snapshot. Cannot be null.</param>
        public MouseEventArgsSnapshot(MouseEventArgs e)
        {
            Timestamp = e.Timestamp;
            Button = e.ChangedButton;
            Location = e.Location;
            ClickCount = e.ClickCount;
            CurrentTargetUniqueId = (e.CurrentTarget as AbstractControl)?.UniqueId;
        }

        /// <summary>
        /// Determines whether the current mouse event is a continuation of a double-click sequence that began with the
        /// specified previous mouse event on the given control.
        /// </summary>
        /// <remarks>This method checks that the previous event occurred on the same control, involved the
        /// same mouse button, occurred within the system-defined double-click time and spatial threshold, and that the
        /// current event satisfies all criteria for being part of a double-click sequence.</remarks>
        /// <param name="previous">A snapshot of the previous mouse event to compare against the current event.</param>
        /// <param name="control">The control on which the mouse events occurred. Used to determine if the double-click sequence is relevant
        /// to this control.</param>
        /// <returns>true if the current mouse event is considered a continuation of a double-click sequence that started with
        /// the previous event on the specified control; otherwise, false.</returns>
        public readonly bool IsDoubleClickContinuationOf(in MouseEventArgsSnapshot previous, AbstractControl control)
        {
            if (!previous.CurrentTargetEquals(control))
                return false;
            if (previous.Button != Button)
                return false;

            var locationWithin = previous.LocationWithinDoubleClickSize(Location, control);
            var timestampWithin = previous.TimestampWithinDoubleClickTime(Timestamp);

            if (!locationWithin)
                return false;
            if (!timestampWithin)
                return false;
            return true;
        }

        /// <summary>
        /// Determines whether the specified location is within the system-defined double-click area relative to the
        /// current location.
        /// </summary>
        /// <param name="newLocation">The location to compare, typically representing the position of a pointer or user interaction.</param>
        /// <param name="control">The control that provides context for the double-click area calculation. Cannot be null.</param>
        /// <returns>true if the specified location is within the double-click area of the current location; otherwise, false.</returns>
        public readonly bool LocationWithinDoubleClickSize(PointD newLocation, AbstractControl control)
        {
            var result = SystemInformation.IsWithinDoubleClickSize(Location, newLocation, control);
            return result;
        }

        /// <summary>
        /// Determines whether the specified timestamp occurs within the system-defined double-click time interval of
        /// the current timestamp.
        /// </summary>
        /// <remarks>The double-click time threshold is determined by the system setting for double-click
        /// speed. This method is typically used to detect whether two input events should be considered part of a
        /// double-click sequence.</remarks>
        /// <param name="newTimestamp">The timestamp, in milliseconds, to compare against the current timestamp.</param>
        /// <returns>true if the specified timestamp is within the double-click time threshold; otherwise, false.</returns>
        public readonly bool TimestampWithinDoubleClickTime(long newTimestamp)
        {
            var distance = Math.Abs(newTimestamp - Timestamp);
            var result = distance < SystemInformation.DoubleClickTimeInTicks;
            return result;
        }

        /// <summary>
        /// Determines whether the current target is equal to the specified control.
        /// </summary>
        /// <param name="control">The control to compare with the current target. Can be null to check if there is no current target.</param>
        /// <returns>true if the current target is equal to the specified control; otherwise, false.</returns>
        public readonly bool CurrentTargetEquals(AbstractControl? control)
        {
            if (control == null)
                return !CurrentTargetUniqueId.HasValue;
            return CurrentTargetUniqueId.HasValue && CurrentTargetUniqueId.Value == control.UniqueId;
        }

        [Conditional("DEBUG")]
        internal static void LogSnapshotDifference(in MouseEventArgsSnapshot previous, MouseEventArgs e)
        {
            var s = new MouseEventArgsSnapshot(e);
            MouseEventArgsSnapshot.LogSnapshotDifference(in previous, in s, (AbstractControl)e.CurrentTarget);
        }

        [Conditional("DEBUG")]
        internal static void LogSnapshotDifference(
            in MouseEventArgsSnapshot previous,
            in MouseEventArgsSnapshot current,
            AbstractControl control)
        {
            var locationWithin = previous.LocationWithinDoubleClickSize(current.Location, control);
            var timestampWithin = previous.TimestampWithinDoubleClickTime(current.Timestamp);
            var timestampMsec = DateUtils.TicksToMilliseconds(current.Timestamp);
            var previousTimestampMsec = DateUtils.TicksToMilliseconds(previous.Timestamp);

            var isContinuation = current.IsDoubleClickContinuationOf(in previous, control);

            App.LogReplace(
                $"Snapshot {isContinuation} L: {current.Location} {previous.Location} {locationWithin} T: {timestampMsec} {previousTimestampMsec} {timestampMsec - previousTimestampMsec} {timestampWithin}");
        }
    }
}
