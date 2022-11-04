# Rendering Graphics

> [!NOTE]
> Warning: AlterNET UI is still in beta, and the API is subject to change in the next beta releases.

## Overview

AlterNET UI incudes set of resolution-independent graphics features that use native rendering on every supported platform.

It supports rendering graphic primitives such as text, images, graphic shapes with different fonts, pens and brushes.

The following code example illustrates how graphics can be drawn in a UI element:

[!code-csharp[](../../tutorials/drawing-context/examples/DrawingContextTutorial/DrawingControl-Step4.cs)]
![Window with Red Circular Pattern](../../tutorials/drawing-context/images/circular-pattern.png)


Refer to our [blog post](https://www.alternet-ui.com/blog/drawing-context-tutorial) to see it in action.


## Drawing Context Features

Out [Drawing Sample](https://github.com/alternetsoft/alternet-ui-examples/tree/main/DrawingSample) illustrates the features AlterNET UI provides for rendering graphics.
Below is a list of the features <xref:Alternet.Drawing.DrawingContext> provides grouped by category.
The screenshots are taken from the [Drawing Sample](https://github.com/alternetsoft/alternet-ui-examples/tree/main/DrawingSample).

### Geometric Shapes

<xref:Alternet.Drawing.DrawingContext> class provides means to draw a variety of geometric shapes:

![](images/drawing-sample-shapes.png)

- Lines: <xref:Alternet.Drawing.DrawingContext.DrawLine%2A>, <xref:Alternet.Drawing.DrawingContext.DrawLines%2A>
- Polygons: <xref:Alternet.Drawing.DrawingContext.DrawPolygon%2A>, <xref:Alternet.Drawing.DrawingContext.FillPolygon%2A>
- Rectangles: <xref:Alternet.Drawing.DrawingContext.DrawRectangle%2A>, <xref:Alternet.Drawing.DrawingContext.FillRectangle%2A>,
  <xref:Alternet.Drawing.DrawingContext.DrawRectangles%2A>, <xref:Alternet.Drawing.DrawingContext.FillRectangles%2A>
- Rounded rectangles: <xref:Alternet.Drawing.DrawingContext.DrawRoundedRectangle%2A>, <xref:Alternet.Drawing.DrawingContext.FillRoundedRectangle%2A>
- Circles and ellipses: <xref:Alternet.Drawing.DrawingContext.DrawCircle%2A>, <xref:Alternet.Drawing.DrawingContext.FillCircle%2A>,
  <xref:Alternet.Drawing.DrawingContext.DrawEllipse%2A>, <xref:Alternet.Drawing.DrawingContext.FillEllipse%2A>
- Curves: <xref:Alternet.Drawing.DrawingContext.DrawBezier%2A>, <xref:Alternet.Drawing.DrawingContext.DrawBeziers%2A>
- Arcs and pies: <xref:Alternet.Drawing.DrawingContext.DrawArc%2A>, <xref:Alternet.Drawing.DrawingContext.DrawPie%2A>, <xref:Alternet.Drawing.DrawingContext.FillPie%2A>

### Text

<xref:Alternet.Drawing.DrawingContext> allows to draw text with the specified <xref:Alternet.Drawing.Font>, bounds with
the specified <xref:Alternet.Drawing.TextWrapping>, <xref:Alternet.Drawing.TextTrimming>,
<xref:Alternet.Drawing.TextHorizontalAlignment> and <xref:Alternet.Drawing.TextVerticalAlignment>:

![](images/drawing-sample-text.png)

Here is an example of how to draw a wrapped, trimmed and aligned text string:

```csharp
dc.DrawText(
    "My example text",
    Control.DefaultFont,
    Brushes.Black,
    new Rect(10, 10, 100, 100),
    new TextFormat
    {
        HorizontalAlignment = TextHorizontalAlignment.Center,
        VerticalAlignment = TextVerticalAlignment.Top,
        Wrapping = TextWrapping.Word,
        Trimming = TextTrimming.Character
    });
```

### Brushes and Pens

You can draw geometry with different stroke and fill styles provided by <xref:Alternet.Drawing.Brush> and <xref:Alternet.Drawing.Pen> objects:

![](images/drawing-sample-brushes-pens.png)

Below are the parts of the API responsible for different pen stroke styles:
- Solid lines: create an object of the <xref:Alternet.Drawing.Pen> class with a constructor which takes a
  <xref:Alternet.Drawing.Color> and line thickness value.
- Dashed lines: create an object of the <xref:Alternet.Drawing.Pen> class with a constructor which takes a
  <xref:Alternet.Drawing.PenDashStyle>, or set the <xref:Alternet.Drawing.Pen.DashStyle%2A> property.

The following classes allow to fill geometry with different fill styles:
- Solid fill: use <xref:Alternet.Drawing.SolidBrush>
- Gradient fill: use <xref:Alternet.Drawing.RadialGradientBrush> and <xref:Alternet.Drawing.LinearGradientBrush>
- Pattern fill: use <xref:Alternet.Drawing.HatchBrush>