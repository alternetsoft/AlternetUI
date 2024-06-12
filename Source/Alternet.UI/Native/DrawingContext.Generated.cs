// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class DrawingContext : NativeObject
    {
        static DrawingContext()
        {
        }
        
        private DrawingContext()
        {
        }
        
        public DrawingContext(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public bool IsOk
        {
            get
            {
                CheckDisposed();
                return NativeApi.DrawingContext_GetIsOk_(NativePointer);
            }
            
        }
        
        public System.IntPtr WxWidgetDC
        {
            get
            {
                CheckDisposed();
                return NativeApi.DrawingContext_GetWxWidgetDC_(NativePointer);
            }
            
        }
        
        public TransformMatrix Transform
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.DrawingContext_GetTransform_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<TransformMatrix>(_nnn, p => new TransformMatrix(p))!;
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DrawingContext_SetTransform_(NativePointer, value.NativePointer);
            }
        }
        
        public Region? Clip
        {
            get
            {
                CheckDisposed();
                var _nnn = NativeApi.DrawingContext_GetClip_(NativePointer);
                var _mmm = NativeObject.GetFromNativePointer<Region>(_nnn, p => new Region(p));
                ReleaseNativeObjectPointer(_nnn);
                return _mmm;
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DrawingContext_SetClip_(NativePointer, value?.NativePointer ?? IntPtr.Zero);
            }
        }
        
        public Alternet.Drawing.InterpolationMode InterpolationMode
        {
            get
            {
                CheckDisposed();
                return NativeApi.DrawingContext_GetInterpolationMode_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.DrawingContext_SetInterpolationMode_(NativePointer, value);
            }
        }
        
        public static DrawingContext CreateMemoryDC(double scaleFactor)
        {
            var _nnn = NativeApi.DrawingContext_CreateMemoryDC_(scaleFactor);
            var _mmm = NativeObject.GetFromNativePointer<DrawingContext>(_nnn, p => new DrawingContext(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public System.IntPtr GetHandle()
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetHandle_(NativePointer);
        }
        
        public void DrawRotatedText(string text, Alternet.Drawing.PointD location, Font font, Alternet.Drawing.Color foreColor, Alternet.Drawing.Color backColor, double angle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRotatedText_(NativePointer, text, location, font.NativePointer, foreColor, backColor, angle);
        }
        
        public void DrawRotatedTextI(string text, Alternet.Drawing.PointI location, Font font, Alternet.Drawing.Color foreColor, Alternet.Drawing.Color backColor, double angle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRotatedTextI_(NativePointer, text, location, font.NativePointer, foreColor, backColor, angle);
        }
        
        public Image GetAsBitmapI(Alternet.Drawing.RectI subrect)
        {
            CheckDisposed();
            var _nnn = NativeApi.DrawingContext_GetAsBitmapI_(NativePointer, subrect);
            var _mmm = NativeObject.GetFromNativePointer<Image>(_nnn, p => new Image(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public bool Blit(Alternet.Drawing.PointD destPt, Alternet.Drawing.SizeD sz, DrawingContext source, Alternet.Drawing.PointD srcPt, int rop, bool useMask, Alternet.Drawing.PointD srcPtMask)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_Blit_(NativePointer, destPt, sz, source.NativePointer, srcPt, rop, useMask, srcPtMask);
        }
        
        public bool StretchBlit(Alternet.Drawing.PointD dstPt, Alternet.Drawing.SizeD dstSize, DrawingContext source, Alternet.Drawing.PointD srcPt, Alternet.Drawing.SizeD srcSize, int rop, bool useMask, Alternet.Drawing.PointD srcMaskPt)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_StretchBlit_(NativePointer, dstPt, dstSize, source.NativePointer, srcPt, srcSize, rop, useMask, srcMaskPt);
        }
        
        public bool BlitI(Alternet.Drawing.PointI destPt, Alternet.Drawing.SizeI sz, DrawingContext source, Alternet.Drawing.PointI srcPt, int rop, bool useMask, Alternet.Drawing.PointI srcPtMask)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_BlitI_(NativePointer, destPt, sz, source.NativePointer, srcPt, rop, useMask, srcPtMask);
        }
        
        public bool StretchBlitI(Alternet.Drawing.PointI dstPt, Alternet.Drawing.SizeI dstSize, DrawingContext source, Alternet.Drawing.PointI srcPt, Alternet.Drawing.SizeI srcSize, int rop, bool useMask, Alternet.Drawing.PointI srcMaskPt)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_StretchBlitI_(NativePointer, dstPt, dstSize, source.NativePointer, srcPt, srcSize, rop, useMask, srcMaskPt);
        }
        
        public Alternet.Drawing.RectD DrawLabel(string text, Font font, Alternet.Drawing.Color foreColor, Alternet.Drawing.Color backColor, Image? image, Alternet.Drawing.RectD rect, int alignment, int indexAccel)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_DrawLabel_(NativePointer, text, font.NativePointer, foreColor, backColor, image?.NativePointer ?? IntPtr.Zero, rect, alignment, indexAccel);
        }
        
        public void DestroyClippingRegion()
        {
            CheckDisposed();
            NativeApi.DrawingContext_DestroyClippingRegion_(NativePointer);
        }
        
        public void SetClippingRegion(Alternet.Drawing.RectD rect)
        {
            CheckDisposed();
            NativeApi.DrawingContext_SetClippingRegion_(NativePointer, rect);
        }
        
        public Alternet.Drawing.RectD GetClippingBox()
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetClippingBox_(NativePointer);
        }
        
        public void DrawText(string text, Alternet.Drawing.PointD location, Font font, Alternet.Drawing.Color foreColor, Alternet.Drawing.Color backColor)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawText_(NativePointer, text, location, font.NativePointer, foreColor, backColor);
        }
        
        public Alternet.Drawing.SizeI GetDpi()
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetDpi_(NativePointer);
        }
        
        public static void ImageFromDrawingContext(Image image, int width, int height, DrawingContext dc)
        {
            NativeApi.DrawingContext_ImageFromDrawingContext_(image.NativePointer, width, height, dc.NativePointer);
        }
        
        public static void ImageFromGenericImageDC(Image image, System.IntPtr source, DrawingContext dc)
        {
            NativeApi.DrawingContext_ImageFromGenericImageDC_(image.NativePointer, source, dc.NativePointer);
        }
        
        public void GetPartialTextExtents(string text, System.Double[] widths, Font font, System.IntPtr control)
        {
            CheckDisposed();
            NativeApi.DrawingContext_GetPartialTextExtents_(NativePointer, text, widths, widths.Length, font.NativePointer, control);
        }
        
        public Alternet.Drawing.RectD GetTextExtent(string text, Font font, System.IntPtr control)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetTextExtent_(NativePointer, text, font.NativePointer, control);
        }
        
        public Alternet.Drawing.SizeD GetTextExtentSimple(string text, Font font, System.IntPtr control)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetTextExtentSimple_(NativePointer, text, font.NativePointer, control);
        }
        
        public Alternet.Drawing.SizeD MeasureText(string text, Font font, double maximumWidth, Alternet.Drawing.TextWrapping textWrapping)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_MeasureText_(NativePointer, text, font.NativePointer, maximumWidth, textWrapping);
        }
        
        public static DrawingContext FromImage(Image image)
        {
            var _nnn = NativeApi.DrawingContext_FromImage_(image.NativePointer);
            var _mmm = NativeObject.GetFromNativePointer<DrawingContext>(_nnn, p => new DrawingContext(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public static DrawingContext FromScreen()
        {
            var _nnn = NativeApi.DrawingContext_FromScreen_();
            var _mmm = NativeObject.GetFromNativePointer<DrawingContext>(_nnn, p => new DrawingContext(p))!;
            ReleaseNativeObjectPointer(_nnn);
            return _mmm;
        }
        
        public void RoundedRectangle(Pen pen, Brush brush, Alternet.Drawing.RectD rectangle, double cornerRadius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_RoundedRectangle_(NativePointer, pen.NativePointer, brush.NativePointer, rectangle, cornerRadius);
        }
        
        public void Rectangle(Pen pen, Brush brush, Alternet.Drawing.RectD rectangle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Rectangle_(NativePointer, pen.NativePointer, brush.NativePointer, rectangle);
        }
        
        public void Ellipse(Pen pen, Brush brush, Alternet.Drawing.RectD rectangle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Ellipse_(NativePointer, pen.NativePointer, brush.NativePointer, rectangle);
        }
        
        public void Path(Pen pen, Brush brush, GraphicsPath path)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Path_(NativePointer, pen.NativePointer, brush.NativePointer, path.NativePointer);
        }
        
        public void Pie(Pen pen, Brush brush, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Pie_(NativePointer, pen.NativePointer, brush.NativePointer, center, radius, startAngle, sweepAngle);
        }
        
        public void Circle(Pen pen, Brush brush, Alternet.Drawing.PointD center, double radius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Circle_(NativePointer, pen.NativePointer, brush.NativePointer, center, radius);
        }
        
        public void Polygon(Pen pen, Brush brush, Alternet.Drawing.PointD[] points, Alternet.Drawing.FillMode fillMode)
        {
            CheckDisposed();
            NativeApi.DrawingContext_Polygon_(NativePointer, pen.NativePointer, brush.NativePointer, points, points.Length, fillMode);
        }
        
        public void FillRectangle(Brush brush, Alternet.Drawing.RectD rectangle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangle_(NativePointer, brush.NativePointer, rectangle);
        }
        
        public void FillRectangleI(Brush brush, Alternet.Drawing.RectI rectangle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangleI_(NativePointer, brush.NativePointer, rectangle);
        }
        
        public void DrawRectangle(Pen pen, Alternet.Drawing.RectD rectangle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRectangle_(NativePointer, pen.NativePointer, rectangle);
        }
        
        public void FillEllipse(Brush brush, Alternet.Drawing.RectD bounds)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillEllipse_(NativePointer, brush.NativePointer, bounds);
        }
        
        public void DrawEllipse(Pen pen, Alternet.Drawing.RectD bounds)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawEllipse_(NativePointer, pen.NativePointer, bounds);
        }
        
        public void FloodFill(Brush brush, Alternet.Drawing.PointD point)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FloodFill_(NativePointer, brush.NativePointer, point);
        }
        
        public void DrawPath(Pen pen, GraphicsPath path)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawPath_(NativePointer, pen.NativePointer, path.NativePointer);
        }
        
        public void FillPath(Brush brush, GraphicsPath path)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillPath_(NativePointer, brush.NativePointer, path.NativePointer);
        }
        
        public void DrawTextAtPoint(string text, Alternet.Drawing.PointD origin, Font font, Brush brush)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawTextAtPoint_(NativePointer, text, origin, font.NativePointer, brush.NativePointer);
        }
        
        public void DrawTextAtRect(string text, Alternet.Drawing.RectD bounds, Font font, Brush brush, Alternet.Drawing.TextHorizontalAlignment horizontalAlignment, Alternet.Drawing.TextVerticalAlignment verticalAlignment, Alternet.Drawing.TextTrimming trimming, Alternet.Drawing.TextWrapping wrapping)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawTextAtRect_(NativePointer, text, bounds, font.NativePointer, brush.NativePointer, horizontalAlignment, verticalAlignment, trimming, wrapping);
        }
        
        public void DrawImageAtPoint(Image image, Alternet.Drawing.PointD origin, bool useMask)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawImageAtPoint_(NativePointer, image.NativePointer, origin, useMask);
        }
        
        public void DrawImageAtRect(Image image, Alternet.Drawing.RectD destinationRect, bool useMask)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawImageAtRect_(NativePointer, image.NativePointer, destinationRect, useMask);
        }
        
        public void DrawImagePortionAtRect(Image image, Alternet.Drawing.RectD destinationRect, Alternet.Drawing.RectD sourceRect)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawImagePortionAtRect_(NativePointer, image.NativePointer, destinationRect, sourceRect);
        }
        
        public void DrawImagePortionAtPixelRect(Image image, Alternet.Drawing.RectI destinationRect, Alternet.Drawing.RectI sourceRect)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawImagePortionAtPixelRect_(NativePointer, image.NativePointer, destinationRect, sourceRect);
        }
        
        public void Push()
        {
            CheckDisposed();
            NativeApi.DrawingContext_Push_(NativePointer);
        }
        
        public void Pop()
        {
            CheckDisposed();
            NativeApi.DrawingContext_Pop_(NativePointer);
        }
        
        public void DrawLine(Pen pen, Alternet.Drawing.PointD a, Alternet.Drawing.PointD b)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawLine_(NativePointer, pen.NativePointer, a, b);
        }
        
        public void DrawLines(Pen pen, Alternet.Drawing.PointD[] points)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawLines_(NativePointer, pen.NativePointer, points, points.Length);
        }
        
        public void DrawArc(Pen pen, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawArc_(NativePointer, pen.NativePointer, center, radius, startAngle, sweepAngle);
        }
        
        public void FillPie(Brush brush, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillPie_(NativePointer, brush.NativePointer, center, radius, startAngle, sweepAngle);
        }
        
        public void DrawPie(Pen pen, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawPie_(NativePointer, pen.NativePointer, center, radius, startAngle, sweepAngle);
        }
        
        public void DrawBezier(Pen pen, Alternet.Drawing.PointD startPoint, Alternet.Drawing.PointD controlPoint1, Alternet.Drawing.PointD controlPoint2, Alternet.Drawing.PointD endPoint)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawBezier_(NativePointer, pen.NativePointer, startPoint, controlPoint1, controlPoint2, endPoint);
        }
        
        public void DrawBeziers(Pen pen, Alternet.Drawing.PointD[] points)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawBeziers_(NativePointer, pen.NativePointer, points, points.Length);
        }
        
        public void DrawPoint(Pen pen, double x, double y)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawPoint_(NativePointer, pen.NativePointer, x, y);
        }
        
        public void DrawCircle(Pen pen, Alternet.Drawing.PointD center, double radius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawCircle_(NativePointer, pen.NativePointer, center, radius);
        }
        
        public void FillCircle(Brush brush, Alternet.Drawing.PointD center, double radius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillCircle_(NativePointer, brush.NativePointer, center, radius);
        }
        
        public void DrawRoundedRectangle(Pen pen, Alternet.Drawing.RectD rect, double cornerRadius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRoundedRectangle_(NativePointer, pen.NativePointer, rect, cornerRadius);
        }
        
        public void FillRoundedRectangle(Brush brush, Alternet.Drawing.RectD rect, double cornerRadius)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRoundedRectangle_(NativePointer, brush.NativePointer, rect, cornerRadius);
        }
        
        public void DrawPolygon(Pen pen, Alternet.Drawing.PointD[] points)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawPolygon_(NativePointer, pen.NativePointer, points, points.Length);
        }
        
        public void FillPolygon(Brush brush, Alternet.Drawing.PointD[] points, Alternet.Drawing.FillMode fillMode)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillPolygon_(NativePointer, brush.NativePointer, points, points.Length, fillMode);
        }
        
        public void DrawRectangles(Pen pen, Alternet.Drawing.RectD[] rects)
        {
            CheckDisposed();
            NativeApi.DrawingContext_DrawRectangles_(NativePointer, pen.NativePointer, rects, rects.Length);
        }
        
        public void FillRectangles(Brush brush, Alternet.Drawing.RectD[] rects)
        {
            CheckDisposed();
            NativeApi.DrawingContext_FillRectangles_(NativePointer, brush.NativePointer, rects, rects.Length);
        }
        
        public Alternet.Drawing.Color GetPixel(Alternet.Drawing.PointD p)
        {
            CheckDisposed();
            return NativeApi.DrawingContext_GetPixel_(NativePointer, p);
        }
        
        public void SetPixel(Alternet.Drawing.PointD p, Pen pen)
        {
            CheckDisposed();
            NativeApi.DrawingContext_SetPixel_(NativePointer, p, pen.NativePointer);
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DrawingContext_GetIsOk_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr DrawingContext_GetWxWidgetDC_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_GetTransform_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_SetTransform_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_GetClip_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_SetClip_(IntPtr obj, IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.InterpolationMode DrawingContext_GetInterpolationMode_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_SetInterpolationMode_(IntPtr obj, Alternet.Drawing.InterpolationMode value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_CreateMemoryDC_(double scaleFactor);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr DrawingContext_GetHandle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRotatedText_(IntPtr obj, string text, Alternet.Drawing.PointD location, IntPtr font, NativeApiTypes.Color foreColor, NativeApiTypes.Color backColor, double angle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRotatedTextI_(IntPtr obj, string text, Alternet.Drawing.PointI location, IntPtr font, NativeApiTypes.Color foreColor, NativeApiTypes.Color backColor, double angle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_GetAsBitmapI_(IntPtr obj, Alternet.Drawing.RectI subrect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DrawingContext_Blit_(IntPtr obj, Alternet.Drawing.PointD destPt, Alternet.Drawing.SizeD sz, IntPtr source, Alternet.Drawing.PointD srcPt, int rop, bool useMask, Alternet.Drawing.PointD srcPtMask);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DrawingContext_StretchBlit_(IntPtr obj, Alternet.Drawing.PointD dstPt, Alternet.Drawing.SizeD dstSize, IntPtr source, Alternet.Drawing.PointD srcPt, Alternet.Drawing.SizeD srcSize, int rop, bool useMask, Alternet.Drawing.PointD srcMaskPt);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DrawingContext_BlitI_(IntPtr obj, Alternet.Drawing.PointI destPt, Alternet.Drawing.SizeI sz, IntPtr source, Alternet.Drawing.PointI srcPt, int rop, bool useMask, Alternet.Drawing.PointI srcPtMask);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool DrawingContext_StretchBlitI_(IntPtr obj, Alternet.Drawing.PointI dstPt, Alternet.Drawing.SizeI dstSize, IntPtr source, Alternet.Drawing.PointI srcPt, Alternet.Drawing.SizeI srcSize, int rop, bool useMask, Alternet.Drawing.PointI srcMaskPt);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectD DrawingContext_DrawLabel_(IntPtr obj, string text, IntPtr font, NativeApiTypes.Color foreColor, NativeApiTypes.Color backColor, IntPtr image, Alternet.Drawing.RectD rect, int alignment, int indexAccel);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DestroyClippingRegion_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_SetClippingRegion_(IntPtr obj, Alternet.Drawing.RectD rect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectD DrawingContext_GetClippingBox_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawText_(IntPtr obj, string text, Alternet.Drawing.PointD location, IntPtr font, NativeApiTypes.Color foreColor, NativeApiTypes.Color backColor);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeI DrawingContext_GetDpi_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_ImageFromDrawingContext_(IntPtr image, int width, int height, IntPtr dc);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_ImageFromGenericImageDC_(IntPtr image, System.IntPtr source, IntPtr dc);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_GetPartialTextExtents_(IntPtr obj, string text, System.Double[] widths, int widthsCount, IntPtr font, System.IntPtr control);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.RectD DrawingContext_GetTextExtent_(IntPtr obj, string text, IntPtr font, System.IntPtr control);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeD DrawingContext_GetTextExtentSimple_(IntPtr obj, string text, IntPtr font, System.IntPtr control);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.SizeD DrawingContext_MeasureText_(IntPtr obj, string text, IntPtr font, double maximumWidth, Alternet.Drawing.TextWrapping textWrapping);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_FromImage_(IntPtr image);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr DrawingContext_FromScreen_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_RoundedRectangle_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.RectD rectangle, double cornerRadius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Rectangle_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.RectD rectangle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Ellipse_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.RectD rectangle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Path_(IntPtr obj, IntPtr pen, IntPtr brush, IntPtr path);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Pie_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Circle_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.PointD center, double radius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Polygon_(IntPtr obj, IntPtr pen, IntPtr brush, Alternet.Drawing.PointD[] points, int pointsCount, Alternet.Drawing.FillMode fillMode);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangle_(IntPtr obj, IntPtr brush, Alternet.Drawing.RectD rectangle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangleI_(IntPtr obj, IntPtr brush, Alternet.Drawing.RectI rectangle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRectangle_(IntPtr obj, IntPtr pen, Alternet.Drawing.RectD rectangle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillEllipse_(IntPtr obj, IntPtr brush, Alternet.Drawing.RectD bounds);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawEllipse_(IntPtr obj, IntPtr pen, Alternet.Drawing.RectD bounds);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FloodFill_(IntPtr obj, IntPtr brush, Alternet.Drawing.PointD point);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawPath_(IntPtr obj, IntPtr pen, IntPtr path);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillPath_(IntPtr obj, IntPtr brush, IntPtr path);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawTextAtPoint_(IntPtr obj, string text, Alternet.Drawing.PointD origin, IntPtr font, IntPtr brush);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawTextAtRect_(IntPtr obj, string text, Alternet.Drawing.RectD bounds, IntPtr font, IntPtr brush, Alternet.Drawing.TextHorizontalAlignment horizontalAlignment, Alternet.Drawing.TextVerticalAlignment verticalAlignment, Alternet.Drawing.TextTrimming trimming, Alternet.Drawing.TextWrapping wrapping);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawImageAtPoint_(IntPtr obj, IntPtr image, Alternet.Drawing.PointD origin, bool useMask);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawImageAtRect_(IntPtr obj, IntPtr image, Alternet.Drawing.RectD destinationRect, bool useMask);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawImagePortionAtRect_(IntPtr obj, IntPtr image, Alternet.Drawing.RectD destinationRect, Alternet.Drawing.RectD sourceRect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawImagePortionAtPixelRect_(IntPtr obj, IntPtr image, Alternet.Drawing.RectI destinationRect, Alternet.Drawing.RectI sourceRect);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Push_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_Pop_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawLine_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD a, Alternet.Drawing.PointD b);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawLines_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD[] points, int pointsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawArc_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillPie_(IntPtr obj, IntPtr brush, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawPie_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD center, double radius, double startAngle, double sweepAngle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawBezier_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD startPoint, Alternet.Drawing.PointD controlPoint1, Alternet.Drawing.PointD controlPoint2, Alternet.Drawing.PointD endPoint);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawBeziers_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD[] points, int pointsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawPoint_(IntPtr obj, IntPtr pen, double x, double y);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawCircle_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD center, double radius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillCircle_(IntPtr obj, IntPtr brush, Alternet.Drawing.PointD center, double radius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRoundedRectangle_(IntPtr obj, IntPtr pen, Alternet.Drawing.RectD rect, double cornerRadius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRoundedRectangle_(IntPtr obj, IntPtr brush, Alternet.Drawing.RectD rect, double cornerRadius);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawPolygon_(IntPtr obj, IntPtr pen, Alternet.Drawing.PointD[] points, int pointsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillPolygon_(IntPtr obj, IntPtr brush, Alternet.Drawing.PointD[] points, int pointsCount, Alternet.Drawing.FillMode fillMode);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_DrawRectangles_(IntPtr obj, IntPtr pen, Alternet.Drawing.RectD[] rects, int rectsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_FillRectangles_(IntPtr obj, IntPtr brush, Alternet.Drawing.RectD[] rects, int rectsCount);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color DrawingContext_GetPixel_(IntPtr obj, Alternet.Drawing.PointD p);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void DrawingContext_SetPixel_(IntPtr obj, Alternet.Drawing.PointD p, IntPtr pen);
            
        }
    }
}
