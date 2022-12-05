# Rendering Graphics

> [!NOTE]
> Warning: AlterNET UI is still in beta, and the API is subject to change in the next beta releases.

## Overview

AlterNET UI includes a set of resolution-independent graphics features that use native rendering on every supported platform.

It supports rendering graphic primitives such as text, images, and graphic shapes with different fonts, pens, and brushes.

The following code example illustrates how graphics can be drawn in a UI element:

[!code-csharp[](../../tutorials/drawing-context/examples/DrawingContextTutorial/DrawingControl-Step4.cs)]
![Window with Red Circular Pattern](../../tutorials/drawing-context/images/circular-pattern.png)


Refer to our [blog post](https://www.alternet-ui.com/blog/drawing-context-tutorial) to see it in action.


## Drawing Context Features

Our [Drawing Sample](https://github.com/alternetsoft/alternet-ui-examples/tree/main/DrawingSample) illustrates the features AlterNET UI provides for rendering graphics.
Below is a list of the features that <xref:Alternet.Drawing.DrawingContext> provides, grouped by category.
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

<xref:Alternet.Drawing.DrawingContext> allows to draw text with the specified <xref:Alternet.Drawing.Font>, bounds, and <xref:Alternet.Drawing.TextWrapping>, <xref:Alternet.Drawing.TextTrimming>,
<xref:Alternet.Drawing.TextHorizontalAlignment> and <xref:Alternet.Drawing.TextVerticalAlignment>:

![](images/drawing-sample-text.png)

Here is an example of how to draw a wrapped, trimmed, and aligned text string:

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

You can draw geometry with different stroke and fill styles provided by the <xref:Alternet.Drawing.Brush> and <xref:Alternet.Drawing.Pen> objects:

![](images/drawing-sample-brushes-pens.png)

Below are the parts of the API responsible for different pen stroke styles:
- Solid lines: create an object of the <xref:Alternet.Drawing.Pen> class with a constructor that takes a
  <xref:Alternet.Drawing.Color> and line thickness value.
- Dashed lines: create an object of the <xref:Alternet.Drawing.Pen> class with a constructor that takes a
  <xref:Alternet.Drawing.PenDashStyle>, or set the <xref:Alternet.Drawing.Pen.DashStyle%2A> property.
- <xref:Alternet.Drawing.LineCap> and <xref:Alternet.Drawing.LineJoin> enumerations provide different line cap and line
  join styles.

The following classes allow you to fill geometry with different fill styles:
- Solid fill: use <xref:Alternet.Drawing.SolidBrush>
- Gradient fill: use <xref:Alternet.Drawing.RadialGradientBrush> and <xref:Alternet.Drawing.LinearGradientBrush>
- Pattern fill: use <xref:Alternet.Drawing.HatchBrush>


### GraphicsPath

<xref:Alternet.Drawing.GraphicsPath> class provides a way to stroke and fill geometric shapes defined with a series of connected lines and curves:

![](images/drawing-sample-path.png)

Here are the types of segments supported by the <xref:Alternet.Drawing.GraphicsPath>:

- Lines: <xref:Alternet.Drawing.GraphicsPath.AddLine%2A>, <xref:Alternet.Drawing.GraphicsPath.AddLines%2A>, <xref:Alternet.Drawing.GraphicsPath.AddLineTo%2A>
- Curves: <xref:Alternet.Drawing.GraphicsPath.AddBezier%2A>, <xref:Alternet.Drawing.GraphicsPath.AddBezierTo%2A>, <xref:Alternet.Drawing.GraphicsPath.AddArc%2A>
- Geometric shapes: <xref:Alternet.Drawing.GraphicsPath.AddEllipse%2A>, <xref:Alternet.Drawing.GraphicsPath.AddRectangle%2A>, <xref:Alternet.Drawing.GraphicsPath.AddRoundedRectangle%2A>

### Transforms

<xref:Alternet.Drawing.TransformMatrix> provides a way to set geometric transform to a <xref:Alternet.Drawing.DrawingContext>:

![](images/drawing-sample-transforms.png)

The transforms can include translation, rotation, and scale (see the
<xref:Alternet.Drawing.TransformMatrix.CreateTranslation%2A>, <xref:Alternet.Drawing.TransformMatrix.CreateRotation%2A>
and <xref:Alternet.Drawing.TransformMatrix.CreateScale%2A> methods). Use the
<xref:Alternet.Drawing.DrawingContext.Transform%2A> property of <xref:Alternet.Drawing.DrawingContext> to set the
current transform. The transforms can be applied sequentially with a stack-like approach, using the
<xref:Alternet.Drawing.DrawingContext.PushTransform%2A> and <xref:Alternet.Drawing.DrawingContext.Pop%2A> methods.

### Clip Regions

<xref:Alternet.Drawing.Region> class provides a way to set clip region to a <xref:Alternet.Drawing.DrawingContext>:

![](images/drawing-sample-clip.png)

Use the <xref:Alternet.Drawing.DrawingContext.Clip%2A> property of <xref:Alternet.Drawing.DrawingContext> to set the
current clip region.

### Drawing Images

<xref:Alternet.Drawing.Image> class encapsulate a graphical image.
<xref:Alternet.Drawing.DrawingContext.DrawImage%2A> method overloads provide several ways of drawing images with a
specified <xref:Alternet.Drawing.DrawingContext.InterpolationMode>:

![](images/drawing-sample-images.png)
