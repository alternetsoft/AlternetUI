#include "AuiDockArt.h"

namespace Alternet::UI
{
	AuiDockArt::AuiDockArt()
	{

	}

	AuiDockArt::~AuiDockArt()
	{
	}

	Color AuiDockArt::GetColor(void* handle, int id)
	{
		return ((wxAuiDockArt*)handle)->GetColour(id);
	}

	int AuiDockArt::GetMetric(void* handle, int id)
	{
		return ((wxAuiDockArt*)handle)->GetMetric(id);
	}

	void AuiDockArt::SetColor(void* handle, int id, const Color& color)
	{
		((wxAuiDockArt*)handle)->SetColour(id, color);
	}

	void AuiDockArt::SetMetric(void* handle, int id, int value)
	{
		((wxAuiDockArt*)handle)->SetMetric(id, value);
	}
}
