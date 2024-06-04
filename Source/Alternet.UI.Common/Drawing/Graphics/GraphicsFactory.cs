using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides access to the graphics factory.
    /// </summary>
    public static class GraphicsFactory
    {
        private static IGraphicsFactoryHandler? handler;

        public static SKFilterQuality DefaultScaleQuality = SKFilterQuality.High;

        public static bool DefaultAntialias = true;

        public static IGraphicsFactoryHandler Handler
        {
            get => handler ??= App.Handler.CreateGraphicsFactoryHandler();

            set => handler = value;
        }

        public static void SetPaintDefaults(SKPaint paint)
        {
            paint.IsAntialias = GraphicsFactory.DefaultAntialias;
        }

        public static SKPaint CreateFillPaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.Fill;
            return result;
        }

        public static SKPaint CreateStrokePaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.Stroke;
            return result;
        }

        public static SKPaint CreateStrokeAndFillPaint(SKColor color)
        {
            var result = new SKPaint();
            SetPaintDefaults(result);
            result.Color = color;
            result.Style = SKPaintStyle.StrokeAndFill;
            return result;
        }

        public static SKPaint CreateStrokePaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.Stroke;
            return result;
        }

        public static SKPaint CreateFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.Fill;
            return result;
        }

        public static SKPaint CreateStrokeAndFillPaint(SKFont font)
        {
            var result = new SKPaint(font);
            SetPaintDefaults(result);
            result.Style = SKPaintStyle.StrokeAndFill;
            return result;
        }
    }
}
