// IArrangedElement.cs
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
// Copyright (c) 2018 Filip Navara
// Modified by Alternet

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal interface IArrangedElement : IArrangedElementLite
    {
        IList<IArrangedElement> Controls { get; }

        bool AutoSize { get; set; }

        Rect ExplicitBounds { get;}

        Size MinimumSize { get; set; }

        AnchorStyles Anchor { get; set; }

        DockStyle Dock { get; set; }

        Rect DisplayRectangle { get; }

        double DistanceRight { get; set; }

        double DistanceBottom { get; set; }

        IArrangedElement? Parent { get; }

        AutoSizeMode AutoSizeMode { get; set; }

        void SetBounds(double x, double y, double width, double height, BoundsSpecified specified);

        void PerformLayout();
    }

    internal interface IArrangedElementLite
    {
        bool Visible { get; set; }

        Rect Bounds { get; set; }

        Size ClientSize { get; }

        Thickness Padding { get; set; }

        Thickness Margin { get; set; }

        Size GetPreferredSize(Size proposedSize);
    }
}
