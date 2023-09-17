// DefaultLayout.cs
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2006 Jonathan Pobst
//
// Authors:
// Jonathan Pobst (monkey@jpobst.com)
// Stefan Noack (noackstefan@googlemail.com)
// Modified by Alternet

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class DefaultLayout
    {
        internal static readonly DefaultLayout Instance = new DefaultLayout();

        private DefaultLayout()
        {
        }

        public void InitLayout(Control control, BoundsSpecified specified)
        {
            Control? parent = control.Parent;
            if (parent == null)
                return;

            Rect space = GetDisplayRectangle(parent);

            Rect bounds = control.Bounds;
            if ((specified & (BoundsSpecified.Width | BoundsSpecified.X))
                != BoundsSpecified.None)
            {
                LayoutPanel.SetDistanceRight(
                    control,
                    space.Right - bounds.X - bounds.Width);
            }

            if ((specified & (BoundsSpecified.Height | BoundsSpecified.Y))
                != BoundsSpecified.None)
            {
                LayoutPanel.SetDistanceBottom(
                    control,
                    space.Bottom - bounds.Y - bounds.Height);
            }
        }

        private static Rect GetDisplayRectangle(Control control)
        {
            var size = control.ClientSize;
            Rect result = new(0, 0, size.Width - 1, size.Height - 1);
            result.Offset(control.Padding.Left, control.Padding.Top);
            result.Inflate(-control.Padding.Left, -control.Padding.Top);
            result.Inflate(-control.Padding.Right, -control.Padding.Bottom);
            return result;
        }

        private static void LayoutDockedChildren(Control parent)
        {
            Rect space = GetDisplayRectangle(parent);

            // Deal with docking; go through in reverse, MS docs say that
            // lowest Z-order is closest to edge
            for (int i = parent.Children.Count - 1; i >= 0; i--)
            {
                Control child = parent.Children[i];
                Size child_size = child.Bounds.Size;

                DockStyle dock = LayoutPanel.GetDock(child);
                bool autoSize = LayoutPanel.GetAutoSize(child);

                if (!child.Visible || dock == DockStyle.None)
                    continue;

                switch (dock)
                {
                    case DockStyle.None:
                        break;

                    case DockStyle.Left:
                        if (autoSize)
                        {
                            child_size =
                                child.GetPreferredSize(
                                    new Size(child_size.Width, space.Height));
                        }

                        child.SetBounds(
                            space.Left,
                            space.Y,
                            child_size.Width,
                            space.Height,
                            BoundsSpecified.All);
                        space.X += child_size.Width;
                        space.Width -= child_size.Width;
                        break;

                    case DockStyle.Top:
                        if (autoSize)
                        {
                            child_size =
                                child.GetPreferredSize(
                                    new Size(space.Width, child_size.Height));
                        }

                        child.SetBounds(
                            space.Left,
                            space.Y,
                            space.Width,
                            child_size.Height,
                            BoundsSpecified.All);
                        space.Y += child_size.Height;
                        space.Height -= child_size.Height;
                        break;

                    case DockStyle.Right:
                        if (autoSize)
                        {
                            child_size =
                                child.GetPreferredSize(
                                    new Size(child_size.Width, space.Height));
                        }

                        child.SetBounds(
                            space.Right - child_size.Width,
                            space.Y,
                            child_size.Width,
                            space.Height,
                            BoundsSpecified.All);
                        space.Width -= child_size.Width;
                        break;

                    case DockStyle.Bottom:
                        if (autoSize)
                        {
                            child_size =
                                child.GetPreferredSize(
                                    new Size(space.Width, child_size.Height));
                        }

                        child.SetBounds(
                            space.Left,
                            space.Bottom - child_size.Height,
                            space.Width,
                            child_size.Height,
                            BoundsSpecified.All);
                        space.Height -= child_size.Height;
                        break;

                    case DockStyle.Fill:
                        child.SetBounds(
                            space.Left,
                            space.Top,
                            space.Width,
                            space.Height,
                            BoundsSpecified.All);
                        break;
                }
            }
        }

        /*private static void LayoutAnchoredChildren(Control parent)
        {
            Rect space = GetDisplayRectangle(parent);

            foreach (Control child in parent.Children)
            {
                bool autoSize = LayoutPanel.GetAutoSize(child);
                DockStyle dock = LayoutPanel.GetDock(child);

                if (!child.Visible || dock != DockStyle.None)
                    continue;

                AnchorStyles anchor = child.Anchor;
                Rect bounds = child.Bounds;
                var left = bounds.Left;
                var top = bounds.Top;
                var width = bounds.Width;
                var height = bounds.Height;

                double childDistanceRight = LayoutPanel.GetDistanceRight(child);
                double childDistanceBottom = LayoutPanel.GetDistanceBottom(child);

                if ((anchor & AnchorStyles.Right) != 0)
                {
                    if ((anchor & AnchorStyles.Left) != 0)
                        width = space.Right - childDistanceRight - left;
                    else
                        left = space.Right - childDistanceRight - width;
                }
                else if ((anchor & AnchorStyles.Left) == 0)
                {
                    // left+=diff_width/2 will introduce rounding errors
                    // (diff_width removed from svn after r51780)
                    // This calculates from scratch every time:
                    left += (space.Width - (left + width + childDistanceRight)) / 2;
                    childDistanceRight = space.Width - (left + width);
                }

                if ((anchor & AnchorStyles.Bottom) != 0)
                {
                    if ((anchor & AnchorStyles.Top) != 0)
                        height = space.Bottom - childDistanceBottom - top;
                    else
                        top = space.Bottom - childDistanceBottom - height;
                }
                else if ((anchor & AnchorStyles.Top) == 0)
                {
                    // top += diff_height/2 will introduce rounding
                    // errors (diff_height removed from after r51780)
                    // This calculates from scratch every time:
                    top += (space.Height - (top + height + childDistanceBottom)) / 2;
                    childDistanceBottom = space.Height - (top + height);
                }

                // Sanity
                if (width < 0)
                    width = 0;
                if (height < 0)
                    height = 0;

                if (autoSize)
                {
                    Size proposed_size = Size.Empty;
                    if ((anchor & AnchorStyles.LeftRight) == AnchorStyles.LeftRight)
                        proposed_size.Width = width;
                    if ((anchor & AnchorStyles.TopBottom) == AnchorStyles.TopBottom)
                        proposed_size.Height = height;

                    Size preferred_size =
                        GetPreferredControlSize(child, proposed_size);

                    if ((anchor & AnchorStyles.LeftRight) != AnchorStyles.Right)
                        childDistanceRight += width - preferred_size.Width;
                    else
                        left += width - preferred_size.Width;
                    if ((anchor & AnchorStyles.TopBottom) != AnchorStyles.Bottom)
                        childDistanceBottom += height - preferred_size.Height;
                    else
                        top += height - preferred_size.Height;

                    child.SetBounds(left, top, preferred_size.Width, preferred_size.Height, BoundsSpecified.None);
                }
                else
                {
                    child.SetBounds(left, top, width, height, BoundsSpecified.None);
                }
            }
        }*/

        public static void Layout(Control container)
        {
            LayoutDockedChildren(container);
            //LayoutAnchoredChildren(container, controls);
            //return container.AutoSize;
        }

        /*private static Size GetPreferredControlSize(Control child, Size proposed)
        {
            var preferredsize = child.GetPreferredSize(proposed);
            double width, height;

            AutoSizeMode autoSizeMode = LayoutPanel.GetAutoSizeMode(child);

            if (autoSizeMode == AutoSizeMode.GrowAndShrink)
            {
                width = preferredsize.Width;
                height = preferredsize.Height;
            }
            else
            {
                width = child.ExplicitBounds.Width;
                height = child.ExplicitBounds.Height;
                if (preferredsize.Width > width)
                    width = preferredsize.Width;
                if (preferredsize.Height > height)
                    height = preferredsize.Height;
            }

            return new Size(width, height);
        }*/

        /*internal Size GetPreferredSize(Control container, Size proposedConstraints)
        {
            var parent = container;
            var controls = parent.Controls;
            Size retsize = Size.Empty;

            // Add up the requested sizes for Docked controls
            for (int i = controls.Count - 1; i >= 0; i--)
            {
                Control child = controls[i];

                if (!child.Visible || child.Dock == DockStyle.None)
                    continue;

                if (child.Dock == DockStyle.Left || child.Dock == DockStyle.Right)
                {
                    Size sz = child.AutoSize ? child.GetPreferredSize(new Size(0, proposedConstraints.Height)) : child.Bounds.Size;
                    retsize.Width += sz.Width;
                }
                else if (child.Dock == DockStyle.Top || child.Dock == DockStyle.Bottom)
                {
                    Size sz = child.AutoSize ? child.GetPreferredSize(new Size(proposedConstraints.Width, 0)) : child.Bounds.Size;
                    retsize.Height += sz.Height;
                }
                else if (child.Dock == DockStyle.Fill && child.AutoSize)
                {
                    Size sz = child.GetPreferredSize(proposedConstraints);
                    retsize += sz;
                }
            }

            // See if any non-Docked control is positioned lower or more
            // right than our size
            foreach (Control child in parent.Children)
            {

                if (!child.Visible || child.Dock != DockStyle.None)
                    continue;

                // If its anchored to the bottom or right, that doesn't really count
                if ((child.Anchor & AnchorStyles.BottomTop) == AnchorStyles.Bottom ||
                    (child.Anchor & AnchorStyles.RightLeft) == AnchorStyles.Right)
                    continue;

                Rect child_bounds = child.Bounds;
                if (child.AutoSize)
                {

                    Size proposed_child_size = Size.Empty;
                    if ((child.Anchor & AnchorStyles.LeftRight) == AnchorStyles.LeftRight)
                    {
                        proposed_child_size.Width =
                            proposedConstraints.Width -
                            child.DistanceRight -
                            (child_bounds.Left - parent.DisplayRectangle.Left);
                    }

                    if ((child.Anchor & AnchorStyles.TopBottom) == AnchorStyles.TopBottom)
                    {
                        proposed_child_size.Height = proposedConstraints.Height - child.DistanceBottom - (child_bounds.Top - parent.DisplayRectangle.Top);
                    }

                    Size preferred_size = GetPreferredControlSize(child, proposed_child_size);
                    child_bounds = new Rect(child_bounds.Location, preferred_size);
                }

                // This is the non-sense Microsoft uses (Padding vs DisplayRectangle)
                retsize.Width =
                    Math.Max(
                        retsize.Width,
                        child_bounds.Right - parent.Padding.Left + child.Margin.Right);
                retsize.Height = Math.Max(
                    retsize.Height,
                    child_bounds.Bottom - parent.Padding.Top + child.Margin.Bottom);

                // retsize.Width = Math.Max (retsize.Width, child_bounds.Right - parent.DisplayRectangle.Left + child.Margin.Right);
                // retsize.Height = Math.Max (retsize.Height, child_bounds.Bottom - parent.DisplayRectangle.Top + child.Margin.Bottom);
            }

            return retsize;
        }*/
    }
}
