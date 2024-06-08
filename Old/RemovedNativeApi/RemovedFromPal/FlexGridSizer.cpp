#include "FlexGridSizer.h"

namespace Alternet::UI
{
	void FlexGridSizer::AddGrowableCol(void* handle, int idx, int proportion)
	{
		((wxFlexGridSizer*)handle)->AddGrowableCol(idx, proportion);
	}

	void FlexGridSizer::AddGrowableRow(void* handle, int idx, int proportion)
	{
		((wxFlexGridSizer*)handle)->AddGrowableRow(idx, proportion);
	}

	int FlexGridSizer::GetFlexibleDirection(void* handle)
	{
		return ((wxFlexGridSizer*)handle)->GetFlexibleDirection();
	}

	int FlexGridSizer::GetNonFlexibleGrowMode(void* handle)
	{
		return ((wxFlexGridSizer*)handle)->GetNonFlexibleGrowMode();
	}

	bool FlexGridSizer::IsColGrowable(void* handle, int idx)
	{
		return ((wxFlexGridSizer*)handle)->IsColGrowable(idx);
	}

	bool FlexGridSizer::IsRowGrowable(void* handle, int idx)
	{
		return ((wxFlexGridSizer*)handle)->IsRowGrowable(idx);
	}

	void FlexGridSizer::RemoveGrowableCol(void* handle, int idx)
	{
		((wxFlexGridSizer*)handle)->RemoveGrowableCol(idx);
	}

	void FlexGridSizer::RemoveGrowableRow(void* handle, int idx)
	{
		((wxFlexGridSizer*)handle)->RemoveGrowableRow(idx);
	}

	void FlexGridSizer::SetFlexibleDirection(void* handle, int direction)
	{
		((wxFlexGridSizer*)handle)->SetFlexibleDirection(direction);
	}

	void FlexGridSizer::SetNonFlexibleGrowMode(void* handle, int mode)
	{
		((wxFlexGridSizer*)handle)->SetNonFlexibleGrowMode((wxFlexSizerGrowMode)mode);
	}

	void* FlexGridSizer::GetRowHeights(void* handle)
	{
		// return ((wxFlexGridSizer*)handle)->GetRowHeights();
		return nullptr;
	}

	void* FlexGridSizer::GetColWidths(void* handle)
	{
		// return ((wxFlexGridSizer*)handle)->GetColWidths();
		return nullptr;
	}

	void FlexGridSizer::RepositionChildren(void* handle, const Int32Size& minSize)
	{
		((wxFlexGridSizer*)handle)->RepositionChildren(minSize);
	}

	Int32Size FlexGridSizer::CalcMin(void* handle)
	{
		return ((wxFlexGridSizer*)handle)->CalcMin();
	}

	void* FlexGridSizer::CreateFlexGridSizer(int cols, int vgap, int hgap)
	{
		return new wxFlexGridSizer(cols, vgap, hgap);
	}

	void* FlexGridSizer::CreateFlexGridSizer2(int rows, int cols, int vgap, int hgap)
	{
		return new wxFlexGridSizer(rows, cols, vgap, hgap);
	}

	FlexGridSizer::FlexGridSizer()
	{
	}

	FlexGridSizer::~FlexGridSizer()
	{
	}
}
