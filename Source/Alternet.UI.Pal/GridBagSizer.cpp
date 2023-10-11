#include "GridBagSizer.h"

namespace Alternet::UI
{
	void* GridBagSizer::CreateGridBagSizer(int vgap, int hgap)
	{
		return new wxGridBagSizer(vgap, hgap);
	}

	Int32Size GridBagSizer::CalcMin(void* handle)
	{
		return ((wxGridBagSizer*)handle)->CalcMin();
	}

	void* GridBagSizer::FindItemAtPoint(void* handle, const Int32Point& pt)
	{
		return ((wxGridBagSizer*)handle)->FindItemAtPoint(pt);
	}

	void* GridBagSizer::FindItemAtPosition(void* handle, const Int32Point& pos)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		return ((wxGridBagSizer*)handle)->FindItemAtPosition(_pos);
	}

	void* GridBagSizer::FindItemWithData(void* handle, void* userData)
	{
		return ((wxGridBagSizer*)handle)->FindItemWithData((wxObject*)userData);
	}

	Int32Size GridBagSizer::GetCellSize(void* handle, int row, int col)
	{
		return ((wxGridBagSizer*)handle)->GetCellSize(row, col);
	}

	Int32Size GridBagSizer::GetEmptyCellSize(void* handle)
	{
		return ((wxGridBagSizer*)handle)->GetEmptyCellSize();
	}

	void GridBagSizer::RepositionChildren(void* handle, const Int32Size& minSize)
	{
		((wxGridBagSizer*)handle)->RepositionChildren(minSize);
	}

	void GridBagSizer::SetEmptyCellSize(void* handle, const Int32Size& sz)
	{
		((wxGridBagSizer*)handle)->SetEmptyCellSize(sz);
	}

	void* GridBagSizer::Add(void* handle, void* window, const Int32Point& pos,
		const Int32Size& span, int flag, int border, void* userData)
	{
/*
	wxSizerItem* Add( wxWindow *window,
					  const wxGBPosition& pos,
					  const wxGBSpan& span = wxDefaultSpan,
					  int flag = 0,
					  int border = 0,
					  wxObject* userData = NULL );
*/
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);

		return ((wxGridBagSizer*)handle)->Add((wxWindow*) window, _pos,
			_span, flag, border, (wxObject*) userData);
	}

	void* GridBagSizer::Add2(void* handle, void* sizer, const Int32Point& pos,
		const Int32Size& span, int flag, int border, void* userData)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->Add((wxSizer*) sizer, _pos,
			_span, flag, border, (wxObject*) userData);
	}

	void* GridBagSizer::Add3(void* handle, void* item)
	{
		return ((wxGridBagSizer*)handle)->Add((wxGBSizerItem*)item);
	}

	void* GridBagSizer::Add4(void* handle, int width, int height,
		const Int32Point& pos, const Int32Size& span, int flag, int border, void* userData)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->Add(width, height,
			_pos, _span, flag, border, (wxObject*) userData);
	}

	bool GridBagSizer::CheckForIntersection(void* handle, void* item, void* excludeItem)
	{
/*
	bool CheckForIntersection(wxGBSizerItem* item, wxGBSizerItem* excludeItem = NULL);
	bool CheckForIntersection(const wxGBPosition& pos, const wxGBSpan& span,
	wxGBSizerItem* excludeItem = NULL);
*/

		return ((wxGridBagSizer*)handle)->CheckForIntersection((wxGBSizerItem*)item,
			(wxGBSizerItem*)excludeItem);
	}

	bool GridBagSizer::CheckForIntersection2(void* handle, const Int32Point& pos,
		const Int32Size& span, void* excludeItem)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->CheckForIntersection(_pos,
			_span, (wxGBSizerItem*)excludeItem);
	}

	void* GridBagSizer::FindItem(void* handle, void* window)
	{
		return ((wxGridBagSizer*)handle)->FindItem((wxWindow*)window);
	}

	void* GridBagSizer::FindItem2(void* handle, void* sizer)
	{
		return ((wxGridBagSizer*)handle)->FindItem((wxSizer*)sizer);
	}

	Int32Point GridBagSizer::GetItemPosition(void* handle, void* window)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemPosition((wxWindow*)window);
		return Int32Point(result.GetCol(),result.GetRow());
	}

	Int32Point GridBagSizer::GetItemPosition2(void* handle, void* sizer)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemPosition((wxSizer*)sizer);
		return Int32Point(result.GetCol(), result.GetRow());
	}

	Int32Point GridBagSizer::GetItemPosition3(void* handle, int index)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemPosition(index);
		return Int32Point(result.GetCol(), result.GetRow());
	}

	Int32Point GridBagSizer::GetItemSpan(void* handle, void* window)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemSpan((wxWindow*)window);
		return Int32Point(result.GetColspan(), result.GetRowspan());
	}

	Int32Point GridBagSizer::GetItemSpan2(void* handle, void* sizer)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemSpan((wxSizer*)sizer);
		return Int32Point(result.GetColspan(), result.GetRowspan());
	}

	Int32Point GridBagSizer::GetItemSpan3(void* handle, int index)
	{
		auto result = ((wxGridBagSizer*)handle)->GetItemSpan(index);
		return Int32Point(result.GetColspan(), result.GetRowspan());
	}

	bool GridBagSizer::SetItemPosition(void* handle, void* window, const Int32Point& pos)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		return ((wxGridBagSizer*)handle)->SetItemPosition((wxWindow*) window, _pos);
	}

	bool GridBagSizer::SetItemPosition2(void* handle, void* sizer, const Int32Point& pos)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		return ((wxGridBagSizer*)handle)->SetItemPosition((wxSizer*)sizer, _pos);
	}

	bool GridBagSizer::SetItemPosition3(void* handle, int index, const Int32Point& pos)
	{
		wxGBPosition _pos = wxGBPosition(pos.X, pos.Y);
		return ((wxGridBagSizer*)handle)->SetItemPosition(index, _pos);
	}

	bool GridBagSizer::SetItemSpan(void* handle, void* window, const Int32Size& span)
	{
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->SetItemSpan((wxWindow*)window, _span);
	}

	bool GridBagSizer::SetItemSpan2(void* handle, void* sizer, const Int32Size& span)
	{
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->SetItemSpan((wxSizer*)sizer, _span);
	}

	bool GridBagSizer::SetItemSpan3(void* handle, int index, const Int32Size& span)
	{
		wxGBSpan _span = wxGBSpan(span.Width, span.Height);
		return ((wxGridBagSizer*)handle)->SetItemSpan(index, _span);
	}

	GridBagSizer::GridBagSizer()
	{
	}

	GridBagSizer::~GridBagSizer()
	{
	}
}
