#include "AuiToolBarArt.h"

namespace Alternet::UI
{
	AuiToolBarArt::AuiToolBarArt()
	{

	}

	AuiToolBarArt::~AuiToolBarArt()
	{
	}
	
	void AuiToolBarArt::SetFlags(void* handle, uint32_t flags)
	{
		((wxAuiToolBarArt*)handle)->SetFlags(flags);
	}

	uint32_t AuiToolBarArt::GetFlags(void* handle)
	{
		return ((wxAuiToolBarArt*)handle)->GetFlags();
	}

	void AuiToolBarArt::SetTextOrientation(void* handle, int orientation)
	{
		((wxAuiToolBarArt*)handle)->SetTextOrientation(orientation);
	}

	int AuiToolBarArt::GetTextOrientation(void* handle)
	{
		return ((wxAuiToolBarArt*)handle)->GetTextOrientation();
	}

	int AuiToolBarArt::GetElementSize(void* handle, int elementId)
	{
		return ((wxAuiToolBarArt*)handle)->GetElementSize(elementId);
	}

	void AuiToolBarArt::SetElementSize(void* handle, int elementId, int size)
	{
		((wxAuiToolBarArt*)handle)->SetElementSize(elementId, size);
	}

	void AuiToolBarArt::UpdateColorsFromSystem(void* handle)
	{
		((wxAuiToolBarArt*)handle)->UpdateColoursFromSystem();
	}
}
