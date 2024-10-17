using System;
using System.Drawing;
using System.Diagnostics;

namespace ScriptSpace
{
    public class ScriptClass
    {
        static bool initialized;
        static SolidBrush arenaBackgroundBrush;
        static SolidBrush dotBrush;

        static double currentAngle;
        static int radius;
        static int dotRadius;
        static Point center;

        static double DegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return radians;
        }

        static void InitializeIfNeeded(Rectangle bounds)
        {
            if (initialized)
                return;

            arenaBackgroundBrush = new SolidBrush(Color.DarkBlue);
            dotBrush = new SolidBrush(Color.White);

            int maxSide = Math.Max(bounds.Width, bounds.Height);
            radius = (maxSide - (maxSide / 3)) / 2;
            dotRadius = maxSide / 20;

            center = new Point(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2);

            initialized = true;
        }

        public static void OnPaint(Graphics g, Rectangle bounds)
        {
            InitializeIfNeeded(bounds);

            g.SmoothingMode =
                System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.Clear(SystemColors.Control);

            var arenaBounds = bounds;
            arenaBounds.Inflate(-2, -2);
            g.FillEllipse(arenaBackgroundBrush, arenaBounds);

            var radians = DegreesToRadians(currentAngle);

            var dotCenter = new Point(
                center.X + (int)(Math.Cos(radians) * radius),
                center.Y + (int)(Math.Sin(radians) * radius));

            g.FillEllipse(
                dotBrush,
                dotCenter.X - dotRadius,
                dotCenter.Y - dotRadius,
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
