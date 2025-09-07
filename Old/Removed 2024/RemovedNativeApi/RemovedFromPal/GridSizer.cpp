#include "GridSizer.h"

namespace Alternet::UI
{
	int GridSizer::GetCols(void* handle)
	{
		return ((wxGridSizer*)handle)->GetCols();
	}

	int GridSizer::GetRows(void* handle)
	{
		return ((wxGridSizer*)handle)->GetRows();
	}

	int GridSizer::GetEffectiveColsCount(void* handle)
	{
		return ((wxGridSizer*)handle)->GetEffectiveColsCount();
	}

	int GridSizer::GetEffectiveRowsCount(void* handle)
	{
		return ((wxGridSizer*)handle)->GetEffectiveRowsCount();
	}

	int GridSizer::GetHGap(void* handle)
	{
		return ((wxGridSizer*)handle)->GetHGap();
	}

	int GridSizer::GetVGap(void* handle)
	{
		return ((wxGridSizer*)handle)->GetVGap();
	}

	void GridSizer::SetCols(void* handle, int cols)
	{
		((wxGridSizer*)handle)->SetCols(cols);
	}

	void GridSizer::SetHGap(void* handle, int gap)
	{
		((wxGridSizer*)handle)->SetHGap(gap);
	}

	void GridSizer::SetRows(void* handle, int rows)
	{
		((wxGridSizer*)handle)->SetRows(rows);
	}

	void GridSizer::SetVGap(void* handle, int gap)
	{
		((wxGridSizer*)handle)->SetVGap(gap);
	}

	Int32Size GridSizer::CalcMin(void* handle)
	{
		return ((wxGridSizer*)handle)->CalcMin();
	}

	void GridSizer::RepositionChildren(void* handle, const Int32Size& minSize)
	{
		((wxGridSizer*)handle)->RepositionChildren(minSize);
	}

	void* GridSizer::CreateGridSizer(int cols, int vgap, int hgap)
	{
		return new wxGridSizer(cols, vgap, hgap);
	}

	void* GridSizer::CreateGridSizer2(int rows, int cols, int vgap, int hgap)
	{
		return new wxGridSizer(rows, cols, vgap, hgap);
	}

	GridSizer::GridSizer()
	{
	}

	GridSizer::~GridSizer()
	{
	}
}
