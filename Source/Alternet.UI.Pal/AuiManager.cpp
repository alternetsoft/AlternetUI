#include "AuiManager.h"

namespace Alternet::UI
{
	void AuiManager::Delete(void* attr) 
	{
		delete (wxAuiManager*)attr;
	}

	void* AuiManager::CreateAuiManager()
	{
		return new wxAuiManager();
	}

	AuiManager::AuiManager()
	{

	}

	AuiManager::~AuiManager()
	{
	}

}
