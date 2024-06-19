﻿using System;
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

namespace Alternet.UI
{
    public partial class SkiaContainer : SKCanvasView
    {
        public static readonly BindableProperty SampleProperty = BindableProperty.Create(
            nameof(SampleProp),
            typeof(float),
            typeof(SkiaContainer),
            0.0f,
            propertyChanged: OnSamplePropChanged);

        private SkiaGraphics? graphics;
        private Alternet.UI.Control? control;

        static SkiaContainer()
        {
            App.Handler ??= new MauiApplicationHandler();
        }

        public SkiaContainer()
        {
            /*Orientation = Microsoft.Maui.ScrollOrientation.Both;*/

            /*VerticalScrollBarVisibility = ScrollBarVisibility.Always;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Always;*/

            /*SetScrolledPosition(350, 500);

            ScrollToRequested += SkiaContainer_ScrollToRequested;
            Scrolled += SkiaContainer_Scrolled;*/

            EnableTouchEvents = true;
            Touch += Canvas_Touch;
            SizeChanged += SkiaContainer_SizeChanged;

            PaintSurface += Canvas_PaintSurface;

            Focused += SkiaContainer_Focused;
            Unfocused += SkiaContainer_Unfocused;
        }

        public float SampleProp
        {
            get => (float)GetValue(SampleProperty);
            set => SetValue(SampleProperty, value);
        }

        /*public SKCanvasView CanvasView => canvas;*/

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

                InvalidateSurface();
            }
        }

        public virtual void Invalidate()
        {
            InvalidateSurface();
        }

        public virtual void Log(object? s)
        {
            Alternet.UI.App.Log(s);
        }

        /*protected override void LayoutChildren(double x, double y, double width, double height)
        {
            base.LayoutChildren(x, y, width, height);
        }*/

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

        protected override void OnHandlerChanging(HandlerChangingEventArgs args)
        {
            base.OnHandlerChanging(args);

#if WINDOWS
            var platformView = args.OldHandler?.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;
            if (platformView is null)
                return;
            platformView.PointerEntered -= PlatformView_PointerEntered;
            platformView.PointerExited -= PlatformView_PointerExited;
            platformView.PointerPressed -= PlatformView_PointerPressed;
            platformView.PointerWheelChanged -= PlatformView_PointerWheelChanged;
            platformView.PointerReleased -= PlatformView_PointerReleased;
            platformView.KeyDown -= PlatformView_KeyDown;
            platformView.KeyUp -= PlatformView_KeyUp;
            platformView.CharacterReceived -= PlatformView_CharacterReceived;
            platformView.GotFocus -= PlatformView_GotFocus;
            platformView.LostFocus -= PlatformView_LostFocus;
#endif
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();

#if WINDOWS
            var platformView = Handler?.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;
            if (platformView is null)
                return;

            platformView.AllowFocusOnInteraction = true;
            platformView.IsTabStop = true;

            platformView.PointerEntered += PlatformView_PointerEntered;
            platformView.PointerExited += PlatformView_PointerExited;
            platformView.PointerPressed += PlatformView_PointerPressed;
            platformView.PointerWheelChanged += PlatformView_PointerWheelChanged;
            platformView.PointerReleased += PlatformView_PointerReleased;
            platformView.KeyDown += PlatformView_KeyDown;
            platformView.KeyUp += PlatformView_KeyUp;
            platformView.CharacterReceived += PlatformView_CharacterReceived;
            platformView.GotFocus += PlatformView_GotFocus;
            platformView.LostFocus += PlatformView_LostFocus;
#endif
        }

#if WINDOWS
#endif

        private static void OnSamplePropChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((SkiaContainer)bindable).InvalidateSurface();
        }

        private void SkiaContainer_Scrolled(object? sender, ScrolledEventArgs e)
        {
            /*Log("SkiaContainer_Scrolled");*/
        }

        private void SkiaContainer_ScrollToRequested(object? sender, ScrollToRequestedEventArgs e)
        {
            /*Log("SkiaContainer_ScrollToRequested");*/
        }

        private void SkiaContainer_SizeChanged(object? sender, EventArgs e)
        {
        }

        private void Canvas_Touch(object? sender, SKTouchEventArgs e)
        {
#if WINDOWS
            var platformView = Handler?.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;
            if (platformView is null)
                return;
            platformView.Focus(Microsoft.UI.Xaml.FocusState.Pointer);
#endif

            if (control is null)
                return;

            TouchEventArgs args = MauiTouchUtils.Convert(e);
            control?.RaiseTouch(args);
            e.Handled = args.Handled;
        }

        private void Canvas_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
        {
            var dc = e.Surface.Canvas;

            /*dc.DrawRect(SKRect.Create(50, 50), Alternet.Drawing.Color.Red.AsFillPaint);*/

            if (control is null)
                return;

            dc.Scale((float)control.ScaleFactor);

            if (graphics is null)
                graphics = new(dc);
            else
            {
                graphics.Canvas = dc;
            }

            var dirtyRect = dc.LocalClipBounds;

            var bounds = Bounds;

            control.Bounds = (0, 0, Math.Min(bounds.Width, dirtyRect.Width), Math.Min(bounds.Height, dirtyRect.Height));

            dc.Clear(control.BackColor);

            control.RaisePaint(new PaintEventArgs(graphics, control.Bounds));

            dc.Flush();
        }

        private void SkiaContainer_Focused(object? sender, FocusEventArgs e)
        {
            /*Log("Focused");*/
            UI.Control.FocusedControl = control;
            control?.RaiseGotFocus();
        }

        private void SkiaContainer_Unfocused(object? sender, FocusEventArgs e)
        {
            /*Log("Unfocused");*/
            UI.Control.FocusedControl = null;
            control?.RaiseLostFocus();
        }
    }
}
