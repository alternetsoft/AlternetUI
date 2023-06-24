using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class ArrangedElement : IArrangedElement
    {
        private readonly Control control;
        private readonly IArrangedElement? parent;
        private IList<IArrangedElement>? controls;
        private Rect? explicitBounds;

        public ArrangedElement(IArrangedElement? parent, Control control)
        {
            this.control = control;
            this.parent = parent;
        }

        public void PerformLayoutDefault()
        {
            if (controls != null)
                DefaultLayout.Instance.Layout(this, controls);
        }

        public bool Visible
        {
            get => control.Visible;
            set => control.Visible = value;
        }

        public bool AutoSize
        {
            get;
            set;
        }

        public Rect Bounds
        {
            get
            {
                return control.Bounds;
            }

            set
            {
                if (Bounds == value)
                    return;
                explicitBounds = Bounds;
                control.Bounds = value;
            }
        }

        public Rect ExplicitBounds
        {
            get
            {
                if (explicitBounds == null)
                    return Bounds;
                return (Rect)explicitBounds;
            }
        }

        public Thickness Padding
        {
            get => control.Padding;
            set => control.Padding = value;
        }

        public Thickness Margin
        {
            get => control.Margin;
            set => control.Margin = value;
        }

        public Size MinimumSize
        {
            get;
            set;
        }

        public AnchorStyles Anchor
        {
            get;
            set;
        }

        public DockStyle Dock
        {
            get;
            set;
        }

        public Rect DisplayRectangle => Bounds;

        public IArrangedElement Parent
        {
            get
            {
                return parent;
            }
        }

        public double DistanceRight
        {
            get;
            set;
        }

        public double DistanceBottom
        {
            get;
            set;
        }

        public IList<IArrangedElement> Controls
        {
            get
            {
                controls ??= new List<IArrangedElement>();
                return controls;
            }
        } 

        public AutoSizeMode AutoSizeMode
        {
            get;
            set;
        }

        public Size GetPreferredSize(Size proposedSize)
        {
            return control.GetPreferredSize(proposedSize);
        }

        public void SetBounds(double x, double y, double width, double height, BoundsSpecified specified)
        {
            var bounds = control.Bounds;
            Rect result = new (x, y, width, height);

            if ((specified & BoundsSpecified.X) == 0)
            {
                result.X = bounds.X;
            }

            if ((specified & BoundsSpecified.Y) == 0)
            {
                result.Y = bounds.Y;
            }

            if ((specified & BoundsSpecified.Width) == 0)
            {
                result.Width = bounds.Width;
            }

            if ((specified & BoundsSpecified.Height) == 0)
            {
                result.Height = bounds.Height;
            }

            if (result.X != bounds.X || result.Y != bounds.Y || result.Width != bounds.Width || result.Height != bounds.Height)
            {
                Bounds = result;
            }
        }
    }
}
