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
		Manager(handle)->UnInit();
	}

	void AuiManager::SetFlags(void* handle, uint32_t flags)
	{
		Manager(handle)->SetFlags(flags);
	}

	uint32_t AuiManager::GetFlags(void* handle)
	{
		return Manager(handle)->GetFlags();
	}

	bool AuiManager::AlwaysUsesLiveResize()
	{
		return wxAuiManager::AlwaysUsesLiveResize();
	}

	bool AuiManager::HasLiveResize(void* handle)
	{
		return Manager(handle)->HasLiveResize();
	}

	void AuiManager::SetManagedWindow(void* handle, void* managedWnd)
	{
		Manager(handle)->SetManagedWindow((wxWindow*)managedWnd);
	}

	void* AuiManager::GetManagedWindow(void* handle)
	{
		return Manager(handle)->GetManagedWindow();
	}

	void* AuiManager::GetManager(void* window)
	{
		return wxAuiManager::GetManager((wxWindow*)window);
	}

	void AuiManager::SetArtProvider(void* handle, void* artProvider)
	{
		Manager(handle)->SetArtProvider((wxAuiDockArt*)artProvider);
	}

	void* AuiManager::GetArtProvider(void* handle)
	{
		return Manager(handle)->GetArtProvider();
	}

	bool AuiManager::DetachPane(void* handle, void* window)
	{
		return Manager(handle)->DetachPane((wxWindow*)window);
	}

	void AuiManager::Update(void* handle)
	{
		Manager(handle)->Update();
	}

	string AuiManager::SavePerspective(void* handle)
	{
		return wxStr(Manager(handle)->SavePerspective());
	}

	bool AuiManager::LoadPerspective(void* handle, const string& perspective, bool update)
	{
		return Manager(handle)->LoadPerspective(wxStr(perspective), update);
	}

	void AuiManager::SetDockSizeConstraint(void* handle, double widthPct,
		double heightPct)
	{
		Manager(handle)->SetDockSizeConstraint(widthPct,heightPct);
	}

	void AuiManager::RestoreMaximizedPane(void* handle)
	{
		Manager(handle)->RestoreMaximizedPane();
	}

	Size AuiManager::GetDockSizeConstraint(void* handle)
	{
		double widthPct;
		double heightPct;
		Manager(handle)->GetDockSizeConstraint(&widthPct, &heightPct);
		return Size(widthPct, heightPct);
	}

	wxAuiPaneInfo& AuiManager::PaneInfo(void* paneInfo)
	{
		return AuiPaneInfo::PaneInfo(paneInfo);
	}

	void* AuiManager::FromPaneInfo(wxAuiPaneInfo& paneInfo)
	{
		AuiPaneInfo* result = new AuiPaneInfo();
		result->_paneInfo = paneInfo;
		return result;
	}

	void AuiManager::ClosePane(void* handle, void* paneInfo)
	{
		Manager(handle)->ClosePane(PaneInfo(paneInfo));
	}

	void AuiManager::MaximizePane(void* handle, void* paneInfo)
	{
		Manager(handle)->MaximizePane(PaneInfo(paneInfo));
	}

	void AuiManager::RestorePane(void* handle, void* paneInfo)
	{
		Manager(handle)->RestorePane(PaneInfo(paneInfo));
	}

	void* AuiManager::CreateFloatingFrame(void* handle, void* parentWindow,
		void* paneInfo)
	{
		return Manager(handle)->CreateFloatingFrame((wxWindow*)parentWindow,
			PaneInfo(paneInfo));
	}

	bool AuiManager::CanDockPanel(void* handle, void* paneInfo)
	{
		return Manager(handle)->CanDockPanel(PaneInfo(paneInfo));
	}

	void* AuiManager::GetPane(void* handle, void* window)
	{
		wxAuiPaneInfo& paneInfo = Manager(handle)->GetPane((wxWindow*)window);
		return FromPaneInfo(paneInfo);
	}

	void* AuiManager::GetPaneByName(void* handle, const string& name)
	{
		wxAuiPaneInfo& paneInfo = Manager(handle)->GetPane(wxStr(name));
		return FromPaneInfo(paneInfo);
	}

	bool AuiManager::AddPane(void* handle, void* window, void* paneInfo)
	{
		return Manager(handle)->AddPane((wxWindow*)window, PaneInfo(paneInfo));
	}

	bool AuiManager::AddPane2(void* handle, void* window, void* paneInfo,
		double dropPosX, double dropPosY)
	{
		return Manager(handle)->AddPane((wxWindow*)window, PaneInfo(paneInfo),
			wxPoint(dropPosX, dropPosY));
	}

	bool AuiManager::AddPane3(void* handle, void* window, int direction,
		const string& caption)
	{
		return Manager(handle)->AddPane((wxWindow*) window, direction,
			wxStr(caption));
	}

	bool AuiManager::InsertPane(void* handle, void* window, void* insertLocPaneInfo,
		int insertLevel)
	{
		return Manager(handle)->InsertPane((wxWindow*) window, PaneInfo(insertLocPaneInfo),
			insertLevel);
	}

	string AuiManager::SavePaneInfo(void* handle, void* paneInfo)
	{
		return wxStr(Manager(handle)->SavePaneInfo(PaneInfo(paneInfo)));
	}

	void AuiManager::LoadPaneInfo(void* handle, const string& panePart, void* paneInfo)
	{
		Manager(handle)->LoadPaneInfo(wxStr(panePart), PaneInfo(paneInfo));
	}
}
