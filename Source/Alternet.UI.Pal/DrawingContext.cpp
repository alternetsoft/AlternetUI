#include "DrawingContext.h"
#include "GenericImage.h"
#include <algorithm>

#ifdef __WXOSX__
#include <CoreText/CoreText.h>
#endif

namespace Alternet::UI
{
	DrawingContext::DrawingContext(wxGraphicsContext* graphicsContext, wxDC* dc)
	{
		_graphicsContext = graphicsContext;
		_dc = dc;
	}

	DrawingContext::DrawingContext(wxDC* dc, bool useDirectDraw) : _dc(dc)
	{
		assert(_dc);

#ifdef  __WXMSW__

		useDirectDraw = false;

		if (useDirectDraw)
		{
			wxGraphicsRenderer* renderer = wxGraphicsRenderer::GetDirect2DRenderer();
			_graphicsContext = renderer->CreateContextFromUnknownDC(*_dc);
		}
		else
		{
			_graphicsContext = wxGraphicsContext::CreateFromUnknownDC(*_dc);
		}
#else
		_graphicsContext = wxGraphicsContext::CreateFromUnknownDC(*_dc);
#endif
	}

	DrawingContext::~DrawingContext()
	{
		wxDELETE(_graphicsContext);

		if (!_doNotDeleteDC)
			wxDELETE(_dc);
	}

	SizeI DrawingContext::GetSize()
	{
		return _dc->GetSize();
	}

	SizeI DrawingContext::GetPPI()
	{
		return _dc->GetPPI();
	}

	DrawingContext* DrawingContext::CreateMemoryDC(float scaleFactor)
	{
		auto bitmap = wxBitmap(10, 10);
		bitmap.SetScaleFactor(scaleFactor);
		auto memoryDC = new wxMemoryDC(bitmap);
		auto ppi = memoryDC->GetPPI();
		return new DrawingContext(memoryDC);
	}

	DrawingContext* DrawingContext::CreateMemoryDCFromImage(Image* image)
	{
		wxBitmap bitmap = image->GetBitmap();
		auto memoryDC = new wxMemoryDC(bitmap);
		return new DrawingContext(memoryDC);
	}

	SizeI DrawingContext::GetDpi()
	{
		return _dc->GetPPI();
	}

	void* DrawingContext::GetWxWidgetDC()
	{
		return _dc;
	}

	void DrawingContext::ImageFromDrawingContext(Image* image,
		int width, int height, DrawingContext* dc)
	{
		auto wxdc = dc->GetDC();
		image->_bitmap = wxBitmap(width, height, *wxdc);
	}

	void DrawingContext::ImageFromGenericImageDC(Image* image, void* source, DrawingContext* dc)
	{
		auto wxdc = dc->GetDC();
		image->_bitmap = wxBitmap(((GenericImage*)source)->_image, *wxdc);
	}

	bool DrawingContext::GetIsOk()
	{
		return _dc->IsOk();
	}

	void* DrawingContext::GetHandle()
	{
		return _dc->GetHandle();
	}

	InterpolationMode DrawingContext::GetInterpolationMode()
	{
		return _interpolationMode;
	}

	void DrawingContext::SetInterpolationMode(InterpolationMode value)
	{
		_interpolationMode = value;
	}

	void DrawingContext::SetDoNotDeleteDC(bool value)
	{
		_doNotDeleteDC = value;
	}

	wxGraphicsContext* DrawingContext::GetGraphicsContext()
	{
		return _graphicsContext;
	}

	wxDC* DrawingContext::GetDC()
	{
		return _dc;
	}

	/*static*/ wxWindow* DrawingContext::GetWindow(wxDC* dc)
	{
		auto window = dc->GetWindow();
		if (window == nullptr)
			return ParkingWindow::GetWindow();
		else
			return window;
	}

	void DrawingContext::SetClippingRect(const Rect& rect)
	{
		auto bounds = fromDip(rect, _dc->GetWindow());
		_graphicsContext->Clip(bounds.x, bounds.y, bounds.width, bounds.height);
	}

	Rect DrawingContext::GetClippingBox()
	{
		wxRect rect;
		auto result = _dc->GetClippingBox(rect);
		if (result)
			return rect;
		else
			return Rect();
	}

	void DrawingContext::Save()
	{
		_graphicsContext->PushState();
	}

	void DrawingContext::Restore()
	{
		_graphicsContext->PopState();
	}

	/*static*/ DrawingContext* DrawingContext::FromImage(Image* image)
	{
		auto bitmap = image->GetBitmap();
		auto dc = new wxMemoryDC(bitmap);
		image->SetBitmap(bitmap); // wxMemoryDC unshared bitmap, so need to reassign it back.
		return new DrawingContext(dc);
	}

	/*static*/ DrawingContext* DrawingContext::FromScreen()
	{
		return new DrawingContext(new wxScreenDC());
	}

	void DrawingContext::DrawImageAtPoint(Image* image, const Point& origin, bool useMask)
	{
		wxBitmap bitmap = image->GetBitmap();
		auto window = _dc->GetWindow();

		auto pt = fromDip(origin, _dc->GetWindow());

		auto wxr = wxRect(pt, image->GetPixelSize());
		_graphicsContext->DrawBitmap(bitmap, wxr.x, wxr.y, wxr.width, wxr.height);
	}

	void DrawingContext::DrawBitmapAtPointI(Image* image, int x, int y, bool useMask)
	{
		wxBitmap bitmap = image->GetBitmap();
		_dc->DrawBitmap(bitmap, x, y, useMask);
	}

	void DrawingContext::DrawBitmapAtRectI(Image* image, const RectI& rect, bool useMask)
	{
		wxBitmap bitmap = image->GetBitmap();

		// Create a memory DC to hold the source bitmap
		wxMemoryDC memDC;
		memDC.SelectObject(bitmap);

		int destX = rect.X;
		int destY = rect.Y;
		int destW = rect.Width;
		int destH = rect.Height;

		int srcW = bitmap.GetWidth();
		int srcH = bitmap.GetHeight();

		// StretchBlit will scale the source bitmap into the destination rectangle
		_dc->StretchBlit(
			destX, destY, destW, destH,   // destination rectangle
			&memDC,
			0, 0, srcW, srcH,             // source rectangle
			useMask ? wxCOPY : wxCOPY,    // raster operation (mask handling can be extended)
			useMask ? true : false        // use mask if requested
		);

		memDC.SelectObject(wxNullBitmap); // release
	}

	void DrawingContext::DrawImageAtRect(Image* image, const Rect& destinationRect, bool useMask)
	{
		wxBitmap bitmap = image->GetBitmap();
		auto destRect = fromDip(destinationRect, _dc->GetWindow());

		auto oldInterpolationQuality = _graphicsContext->GetInterpolationQuality();
		_graphicsContext->SetInterpolationQuality(GetInterpolationQuality(_interpolationMode));

		_graphicsContext->DrawBitmap(
			bitmap,
			destRect.x,
			destRect.y,
			destRect.width,
			destRect.height);

		_graphicsContext->SetInterpolationQuality(oldInterpolationQuality);
	}

	void DrawingContext::SetTransformValues(
		float m11, float m12, float m21, float m22, float dx, float dy)
	{
		wxMatrix2D m(m11, m12, m21, m22);
		wxPoint2DDouble t(dx, dy);

		wxAffineMatrix2D matrix;
		matrix.Set(m, t);

		_currentTransform = matrix;

		_graphicsContext->SetTransform(_graphicsContext->CreateMatrix(_currentTransform));
	}

	/*static*/ wxInterpolationQuality DrawingContext::GetInterpolationQuality(InterpolationMode mode)
	{
		switch (mode)
		{
		case InterpolationMode::None:
			return wxINTERPOLATION_NONE;
		case InterpolationMode::LowQuality:
			return wxINTERPOLATION_FAST;
		case InterpolationMode::MediumQuality:
			return wxINTERPOLATION_GOOD;
		case InterpolationMode::HighQuality:
			return wxINTERPOLATION_BEST;
		default:
			throwExNoInfo;
		}
	}

	float DrawingContext::GetTextHeight(const NativeStringSpan& text, void* font)
	{
		auto wxf = Font::FromFontRef(font);

		wxDouble height;

		_dc->SetFont(wxf);

		auto wText = wxStr(text);

		auto size = _dc->GetTextExtent(wText);

		height = size.y;

		height = std::ceil(height);

		return height;
	}

#ifdef __WXOSX__

double GetCTFontHeight(const wxFont& font)
{
    CFStringRef cfName = CFStringCreateWithCString(NULL,
        font.GetFaceName().utf8_str(), kCFStringEncodingUTF8);
    CTFontRef ctFont = CTFontCreateWithName(cfName, font.GetPointSize(), NULL);

    double ascent  = CTFontGetAscent(ctFont);
    double descent = CTFontGetDescent(ctFont);
    double leading = CTFontGetLeading(ctFont);

    CFRelease(ctFont);
    CFRelease(cfName);

    return ascent + descent + leading;
}

#endif


	float DrawingContext::GetFontRefHeight(void* fontRef)
	{
#ifdef __WXOSX__
		auto wxf = Font::FromFontRef(fontRef);
        auto result = GetCTFontHeight(wxf);
		return result;
#else
		auto wxf = Font::FromFontRef(fontRef);
		_dc->SetFont(wxf);

		wxFontMetrics metrics = _dc->GetFontMetrics();
		int totalHeight = metrics.height;

		return totalHeight;
#endif		
	}

	Size DrawingContext::GetTextExtentSimple(const NativeStringSpan& text, void* font)
	{
		auto wxf = Font::FromFontRef(font);

		wxDouble height;
		wxDouble width;

		_dc->SetFont(wxf);

		auto wText = wxStr(text);

		auto size = _dc->GetTextExtent(wText);

		width = size.x;
		height = size.y;

		height = std::ceil(height);

		return Size(width, height);
	}
}