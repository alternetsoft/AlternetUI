#include "AuiManager.h"

namespace Alternet::UI
{
	wxAuiManager* AuiManager::Manager(void* handle)
	{
		return (wxAuiManager*)handle;
	}

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

	void* AuiManager::CreateAuiManager2(void* managedWnd, uint32_t flags)
	{
		return new wxAuiManager((wxWindow*)managedWnd, flags);
	}

	void AuiManager::UnInit(void* handle)
	{
	}

	void AuiManager::SetFlags(void* handle, uint32_t flags)
	{
	}

	uint32_t AuiManager::GetFlags(void* handle)
	{
		return 0;
	}

	bool AuiManager::AlwaysUsesLiveResize()
	{
		return false;
	}

	bool AuiManager::HasLiveResize(void* handle)
	{
		return false;
	}

	void AuiManager::SetManagedWindow(void* handle, void* managedWnd)
	{
	}

	void* AuiManager::GetManagedWindow(void* handle)
	{
		return nullptr;
	}

	void* AuiManager::GetManager(void* window)
	{
		return nullptr;
	}

	void AuiManager::SetArtProvider(void* handle, void* artProvider)
	{
	}

	void* AuiManager::GetArtProvider(void* handle)
	{
		return nullptr;
	}

	bool AuiManager::DetachPane(void* handle, void* window)
	{
		return false;
	}

	void AuiManager::Update(void* handle)
	{
	}

	string AuiManager::SavePerspective(void* handle)
	{
		return wxStr(wxEmptyString);
	}

	bool AuiManager::LoadPerspective(void* handle, const string& perspective, bool update)
	{
		return false;
	}

	void AuiManager::SetDockSizeConstraint(void* handle, double widthPct,
		double heightPct)
	{
	}

	void AuiManager::RestoreMaximizedPane(void* handle)
	{
	}

	void* AuiManager::GetPane(void* handle, void* window)
	{
		return nullptr;
	}

	void* AuiManager::GetPaneByName(void* handle, const string& name)
	{
		return nullptr;
	}

	bool AuiManager::AddPane(void* handle, void* window, void* paneInfo)
	{
		return false;
	}

	bool AuiManager::AddPane2(void* handle, void* window, void* paneInfo,
		double dropPosX, double dropPosY)
	{
		return false;
	}

	bool AuiManager::AddPane3(void* handle, void* window, int direction,
		const string& caption)
	{
		return false;
	}

	bool AuiManager::InsertPane(void* handle, void* window, void* insertLocPaneInfo,
		int insertLevel)
	{
		return false;
	}

	string AuiManager::SavePaneInfo(void* handle, void* paneInfo)
	{
		return wxStr(wxEmptyString);
	}

	void AuiManager::LoadPaneInfo(void* handle, const string& panePart, void* paneInfo)
	{
	}

	Size AuiManager::GetDockSizeConstraint(void* handle)
	{
		return Size();
	}

	void AuiManager::ClosePane(void* handle, void* paneInfo)
	{
	}

	void AuiManager::MaximizePane(void* handle, void* paneInfo)
	{
	}

	void AuiManager::RestorePane(void* handle, void* paneInfo)
	{
	}

	void* AuiManager::CreateFloatingFrame(void* handle, void* parentWindow,
		void* paneInfo)
	{
		return nullptr;
	}

	bool AuiManager::CanDockPanel(void* handle, void* paneInfo)
	{
		return false;
	}
}
