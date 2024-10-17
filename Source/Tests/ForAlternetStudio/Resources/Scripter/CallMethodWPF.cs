using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ScriptSpace
{
    public class ScriptClass
    {
        static bool initialized;
        static SolidColorBrush arenaBackgroundBrush;
        static SolidColorBrush dotBrush;

        static double currentAngle;
        static double radius;
        static double dotRadius;
        static Point center;

        static double DegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return radians;
        }

        static void InitializeIfNeeded(Rect bounds)
        {
            if (initialized)
                return;

            arenaBackgroundBrush = new SolidColorBrush(Colors.DarkBlue);
            dotBrush = new SolidColorBrush(Colors.White);

            double maxSide = Math.Max(bounds.Width, bounds.Height);
            radius = (maxSide - (maxSide / 3)) / 2;
            dotRadius = maxSide / 20;


            center = new Point(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2);

            initialized = true;
        }

        public static void OnRender(DrawingContext dc, Rect bounds)
        {
            InitializeIfNeeded(bounds);

            var arenaBounds = bounds;
            arenaBounds.Inflate(-2, -2);
            Point center = new Point((arenaBounds.Left + arenaBounds.Right) / 2, (arenaBounds.Top + arenaBounds.Bottom) / 2);
            dc.DrawEllipse(arenaBackgroundBrush, null, center, arenaBounds.Width / 2, arenaBounds.Height / 2);

            var radians = DegreesToRadians(currentAngle);

            var dotCenter = new Point(
                center.X + (int)(Math.Cos(radians) * radius),
                center.Y + (int)(Math.Sin(radians) * radius));

            dc.DrawEllipse(
                dotBrush,
                null,
                dotCenter,
                dotRadius * 2,
                dotRadius * 2);
        }

        static double ConstrainAngle(double x)
        {
            x %= 360;
            if (x < 0)
                x += 360;

            return x;
        }

        public static void OnUpdate(int deltaTimeMs)
        {
            currentAngle += deltaTimeMs * 0.1;
            currentAngle = ConstrainAngle(currentAngle);
            Debug.WriteLine("Current Angle: " + currentAngle);
        }
        public static void Main()
        {
        }
    }
}
