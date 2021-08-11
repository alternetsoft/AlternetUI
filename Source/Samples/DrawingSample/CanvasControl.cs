using Alternet.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace DrawingSample
{
    public class CanvasControl : Control
    {
        public readonly static IReadOnlyList<Layer> Layers = new Layer[] { new RectanglesLayer(), new TextLinesLayer() };

        public CanvasControl()
        {
            UserPaint = true;
        }

        readonly List<Layer> visibleLayers = new List<Layer>();

        public void HideLayer(Layer value)
        {
            visibleLayers.Remove(value);
            Update();
        }

        public void ShowLayer(Layer value)
        {
            if (visibleLayers.Contains(value))
                throw new InvalidOperationException();
            visibleLayers.Add(value);
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.DrawingContext.FillRectangle(e.Bounds, Color.White);

            foreach (var layer in visibleLayers)
                layer.Draw(e.DrawingContext, e.Bounds);
        }
    }
}