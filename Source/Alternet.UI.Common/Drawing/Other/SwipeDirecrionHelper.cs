using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides helper methods for working with <see cref="SwipeDirection"/> enumeration.
    /// </summary>
    public static class SwipeDirectionHelper
    {
        /// <summary>
        /// Determines the swipe direction based on the movement from an initial point to an end point.
        /// </summary>
        /// <param name="initialPoint">The starting point of the swipe gesture.</param>
        /// <param name="endPoint">The ending point of the swipe gesture.</param>
        /// <returns>A value of type <see cref="SwipeDirection"/> indicating the direction of the swipe, such as up, down, left,
        /// or right.</returns>
        public static SwipeDirection GetSwipeDirection(PointD initialPoint, PointD endPoint)
        {
            var angle = GetAngleFromPoints(initialPoint.X, initialPoint.Y, endPoint.X, endPoint.Y);
            return GetSwipeDirectionFromAngle(angle);
        }

        /// <summary>
        /// Calculates the angle, in degrees, between two points in a 2D plane, measured from the first point to the
        /// second point.
        /// </summary>
        /// <remarks>The angle is measured in a counterclockwise direction, with 0 degrees corresponding
        /// to the positive X-axis. This method is useful for determining direction or orientation between two points in
        /// graphical applications.</remarks>
        /// <param name="x1">The X-coordinate of the starting point.</param>
        /// <param name="y1">The Y-coordinate of the starting point.</param>
        /// <param name="x2">The X-coordinate of the ending point.</param>
        /// <param name="y2">The Y-coordinate of the ending point.</param>
        /// <returns>A floating-point value representing the angle in degrees from the starting point to the ending point, in the
        /// range [0, 360).</returns>
        public static float GetAngleFromPoints(float x1, float y1, float x2, float y2)
        {
            float rad = MathF.Atan2(y1 - y2, x2 - x1) + MathF.PI;
            return (rad * 180 / MathF.PI + 180) % 360;
        }

        /// <summary>
        /// Determines the swipe direction based on the specified angle in degrees.
        /// </summary>
        /// <remarks>The method interprets the angle as follows: 45–135 degrees as Up, 0–45 and 315–360
        /// degrees as Right, 225–315 degrees as Down, and all other angles as Left. This is useful for gesture
        /// recognition in touch or pointer input scenarios.</remarks>
        /// <param name="angle">The angle, in degrees, representing the direction of the swipe. Valid values are typically in the range 0 to
        /// 360.</param>
        /// <returns>A value of type SwipeDirection indicating the detected swipe direction: Up, Down, Left, or Right.</returns>
        public static SwipeDirection GetSwipeDirectionFromAngle(float angle)
        {
            if (IsAngleInRange(angle, 45, 135))
                return SwipeDirection.Up;

            if (IsAngleInRange(angle, 0, 45) || IsAngleInRange(angle, 315, 360))
                return SwipeDirection.Right;

            if (IsAngleInRange(angle, 225, 315))
                return SwipeDirection.Down;

            return SwipeDirection.Left;
        }

        /// <summary>
        /// Determines whether a specified angle is within the range defined by the initial and end boundaries.
        /// </summary>
        /// <remarks>The comparison is inclusive of the initial boundary and exclusive of the end
        /// boundary. This method does not normalize the angle; callers should ensure the values are in the expected
        /// range if wrap-around behavior is required.</remarks>
        /// <param name="angle">The angle to evaluate, in degrees.</param>
        /// <param name="init">The inclusive lower bound of the range, in degrees.</param>
        /// <param name="end">The exclusive upper bound of the range, in degrees.</param>
        /// <returns>true if the angle is greater than or equal to the initial boundary and less than the end boundary;
        /// otherwise, false.</returns>
        public static bool IsAngleInRange(float angle, float init, float end)
        {
            return (angle >= init) && (angle < end);
        }
    }
}
