# Using SkiaSharp with AlterNET UI

There are different ways to use [SkiaSharp](https://github.com/mono/SkiaSharp):

- You can write an explicit conversions for SKBitmap from/to Image and GenericImage.

For example: ``` var image = (Image) skBitmap; ```.

- You can call Image.LockSurface and GenericImage.LockSurface in order to get SKCanvas. 

For example: ``` using var canvasLock = image.LockSurface(); var canvas = canvasLock.Canvas;```.

---

Here you can find examples on using SkiaSharp with AlterNET UI.

## Example of drawing on PictureBox control

```cs
        private void Draw(Action<SKCanvas,int,int> action)
        {
            RectI rect = (0, 0, PixelFromDip(pictureBox.Width), PixelFromDip(pictureBox.Height));

            SKBitmap bitmap = new(rect.Width, rect.Height);

            SKCanvas canvas = new(bitmap);

            canvas.Clear(Color.White);

            action(canvas, rect.Width, rect.Height);

            var image = (Image)bitmap;
            pictureBox.Image = image;
        }
```

## GenericImage to SKBitmap and to PictureBox

```cs
        private void GenericToSkia()
        {
            // Creates generic image from the specified url
            GenericImage image = new(backgroundUrl1);

            // Converts created generic image to SKBitmap
            var bitmap = (SKBitmap)image;

            // Converts SKBitmap to Image and assigns it to PictureBox control
            pictureBox.Image = (Image)bitmap;
        }
```

## Paint UserControl on SKBitmap

```cs
        private void PaintOnCanvas()
        {
            RectD rectDip = (0, 0, control.Width, control.Height);
            RectI rect = rectDip.PixelFromDip();

            SKBitmap bitmap = new(rect.Width, rect.Height);

            SKCanvas canvas = new(bitmap);
            canvas.Scale((float)control.ScaleFactor);

            canvas.Clear(Color.White);

            SkiaGraphics graphics = new(canvas);

            PaintEventArgs e = new(graphics, rectDip);

            control.RaisePaint(e);

            pictureBox.Image = (Image)bitmap;
        }
```

## LockSurface on GenericImage

These methods assume to be members of a Control.

```cs
        private void LockSurfaceOnGenericImage(bool hasAlpha)
        {
            var width = 700;
            var height = 500;

            var image = GenericImage.Create(PixelFromDip(width), PixelFromDip(height), Color.Aquamarine);
            image.HasAlpha = hasAlpha;

            using (var canvasLock = image.LockSurface())
            {
                var canvas = canvasLock.Canvas;
                canvas.Scale((float)ScaleFactor);

                canvas.Clear(Color.White);

                var font = Font.Default;

                canvas.DrawText("Hello", (600, 0), font, Color.Black, Color.LightGreen);

                canvas.DrawRect(SKRect.Create(width, height), Color.Red.AsPen);

                DrawBeziersPoint(canvas);
                canvas.Flush();
            }

            pictureBox.Image = (Image)image;
        }

        private void DrawBeziersPoint(SKCanvas dc)
        {
            Pen blackPen = Color.Black.GetAsPen(3);

            PointD start = new(100, 100);
            PointD control1 = new(200, 10);
            PointD control2 = new(350, 50);
            PointD end1 = new(500, 100);
            PointD control3 = new(600, 150);
            PointD control4 = new(650, 250);
            PointD end2 = new(500, 300);

            PointD[] bezierPoints =
            {
                 start, control1, control2, end1,
                 control3, control4, end2
             };

            dc.DrawBeziers(blackPen, bezierPoints);
        }
```

## LockSurface on Bitmap and draw text

This example method assumes to be a member of a Control.

```cs
        private void DrawTextOnSkia(bool hasAlpha)
        {
            string s1 = "He|l lo";
            string s2 = "; hello ";

            var width = 700;
            var height = 500;

            bitmap ??= new Bitmap(PixelFromDip(width), PixelFromDip(height));
            bitmap.HasAlpha = hasAlpha;
            bitmap.SetDPI(GetDPI());

            using (var canvasLock = bitmap.LockSurface())
            {
                var canvas = canvasLock.Canvas;
                canvas.Scale((float)ScaleFactor);

                canvas.Clear(prm.BackColor);
                canvas.DrawRect(SKRect.Create(width, height), Color.Red.AsPen);

                PointD pt = new(10, 10);
                PointD pt2 = new(10, 150);

                var font = Font.Default;

                canvas.DrawText("Hello", (600,0), font, Color.Black, Color.LightGreen);

                canvas.DrawText(s1, pt, font, Color.Black, Color.LightGreen);

                canvas.DrawText(s2, pt2, font, Color.Red, Color.LightGreen);

                canvas.DrawPoint(pt, Color.Red);
                canvas.DrawPoint(pt2, Color.Red);

                DrawBeziersPoint(canvas);

                canvas.Flush();
            }

            pictureBox.Image = bitmap;
        }
```
