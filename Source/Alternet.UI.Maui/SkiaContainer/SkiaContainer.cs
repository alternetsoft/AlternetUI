using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Extensions;

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

/*
SkiaSharp.SKRect (float)
https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skrect?view=skiasharp-2.88

This is brush?
SKPaint https://learn.microsoft.com/en-us/dotnet/api/skiasharp.skpaint?view=skiasharp-2.88
*/

namespace Alternet.UI
{
    public class SkiaContainer : ContentView
    {
        private readonly SKCanvasView canvas = new();
        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        static SkiaContainer()
        {
            App.Handler ??= new MauiApplicationHandler();
        }

        public SkiaContainer()
        {
            canvas = new SKCanvasView();
            canvas.EnableTouchEvents = true;
            canvas.Touch += Canvas_Touch;
            canvas.SizeChanged += SkiaContainer_SizeChanged;
            Content = canvas;
            canvas.PaintSurface += Canvas_PaintSurface;

            /*
                IsEnabled
                IsVisible
                Width
                Height
                BackgroundColor
                Background
                IsFocused
                Window
                Bounds
                MinimumWidthRequest
                MinimumHeightRequest
                MaximumWidthRequest
                MaximumHeightRequest

                bool Focus()
                event EventHandler MeasureInvalidated;
                event EventHandler ChildrenReordered;
                event EventHandler? WindowChanged;

                // Window != null
                event EventHandler? Loaded
                event EventHandler? Unloaded
                bool IsLoaded
            */

            Focused += SkiaContainer_Focused;
            Unfocused += SkiaContainer_Unfocused;
            FocusChangeRequested += SkiaContainer_FocusChangeRequested;
        }

        private void SkiaContainer_FocusChangeRequested(object? sender, FocusRequestArgs e)
        {
            e.Result = true;
        }

        public SKCanvasView CanvasView => canvas;

        public Alternet.UI.Control? Control
        {
            get => control;

            set
            {
                if (control == value)
                    return;

                if (control is not null)
                {
                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = null;
                }

                control = value;

                if(control is not null)
                {
                    if (control.Handler is MauiControlHandler handler)
                        handler.Container = this;
                }

                canvas.InvalidateSurface();
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);
        }

        protected override void OnChildRemoved(Element child, int oldLogicalIndex)
        {
            base.OnChildRemoved(child, oldLogicalIndex);
        }

        protected override void OnParentChanged()
        {
            base.OnParentChanged();
        }

        private void SkiaContainer_SizeChanged(object? sender, EventArgs e)
        {
        }

        private void Canvas_Touch(object? sender, SKTouchEventArgs e)
        {
            if (e.ActionType == SKTouchAction.Pressed)
            {
                if (!IsFocused)
                    Focus();
            }

            if (control is not null)
            {
                TouchEventArgs args = MauiTouchUtils.Convert(e);
                control.RaiseTouch(args);
                e.Handled = args.Handled;
            }
        }

        private void Canvas_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            if (control is null)
                return;

            var dc = e.Surface.Canvas;

            if (graphics is null)
                graphics = new(dc);
            else
            {
                graphics.Canvas = dc;
            }

            RectD dirtyRect = dc.LocalClipBounds;

            var bounds = Bounds;

            control.Bounds = (0, 0, bounds.Width, bounds.Height);

            dc.Clear(control.BackColor);

            control.RaisePaint(new PaintEventArgs(graphics, dirtyRect));
        }

        private void SkiaContainer_Focused(object? sender, FocusEventArgs e)
        {
            Alternet.UI.App.Log("Focused");
            UI.Control.FocusedControl = control;
            control?.RaiseGotFocus();
        }

        private void SkiaContainer_Unfocused(object? sender, FocusEventArgs e)
        {
            Alternet.UI.App.Log("Unfocused");
            UI.Control.FocusedControl = null;
            control?.RaiseLostFocus();
        }
    }
}
