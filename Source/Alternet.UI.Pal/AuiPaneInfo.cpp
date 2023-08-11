#include "AuiPaneInfo.h"

namespace Alternet::UI
{
	AuiPaneInfo::AuiPaneInfo()
	{

	}

	AuiPaneInfo::~AuiPaneInfo()
	{
	}

	wxAuiPaneInfo& AuiPaneInfo::PaneInfo(void* paneInfo)
	{
		AuiPaneInfo* paneInfoContainer = (AuiPaneInfo*)paneInfo;
		return paneInfoContainer->_paneInfo;
	}

	void AuiPaneInfo::Delete(void* handle) 
	{
		delete (AuiPaneInfo*)handle;
	}
	
	void* AuiPaneInfo::CreateAuiPaneInfo()
	{
		return new AuiPaneInfo();
	}

	void AuiPaneInfo::AuiPaneInfo::SafeSet(void* handle, void* source)
	{
	}

	bool AuiPaneInfo::IsOk(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsFixed(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsResizable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsShown(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsFloating(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsDocked(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsToolbar(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsTopDockable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsBottomDockable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsLeftDockable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsRightDockable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsDockable(void* handle)
	{	
		return false;
	}

	bool AuiPaneInfo::IsFloatable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsMovable(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsDestroyOnClose(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::IsMaximized(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::HasCaption(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::HasGripper(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasBorder(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasCloseButton(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasMaximizeButton(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasMinimizeButton(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasPinButton(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::HasGripperTop(void* handle)
	{
		return false;
	}
	
	bool AuiPaneInfo::IsValid(void* handle)
	{
		return false;
	}

	bool AuiPaneInfo::HasFlag(void* handle, int flag)
	{
		return false;
	}

	void AuiPaneInfo::Window(void* handle, void* window)
	{
	}
	
	void AuiPaneInfo::Name(void* handle, const string& value)
	{
	}
	
	void AuiPaneInfo::Caption(void* handle, const string& value)
	{
	}
	
	void AuiPaneInfo::Icon(void* handle, void* bitmapBundle)
	{
	}
	
	void AuiPaneInfo::Left(void* handle)
	{
	}
	
	void AuiPaneInfo::Right(void* handle)
	{
	}
	
	void AuiPaneInfo::Top(void* handle)
	{
	}
	
	void AuiPaneInfo::Bottom(void* handle)
	{
	}
	
	void AuiPaneInfo::Center(void* handle)
	{
	}
	
	void AuiPaneInfo::Direction(void* handle, int direction)
	{
	}
	
	void AuiPaneInfo::Layer(void* handle, int layer)
	{
	}
	
	void AuiPaneInfo::Row(void* handle, int row)
	{
	}
	
	void AuiPaneInfo::Position(void* handle, int pos)
	{
	}
	
	void AuiPaneInfo::BestSize(void* handle, int x, int y)
	{
	}
	
	void AuiPaneInfo::MinSize(void* handle, int x, int y)
	{
	}
	
	void AuiPaneInfo::MaxSize(void* handle, int x, int y)
	{
	}
	
	void AuiPaneInfo::FloatingPosition(void* handle, int x, int y)
	{
	}
	
	void AuiPaneInfo::FloatingSize(void* handle, int x, int y)
	{
	}
	
	void AuiPaneInfo::Fixed(void* handle)
	{
	}
	
	void AuiPaneInfo::Resizable(void* handle, bool resizable)
	{
	}
	
	void AuiPaneInfo::Dock(void* handle)
	{
	}
	
	void AuiPaneInfo::Float(void* handle)
	{
	}
	
	void AuiPaneInfo::Hide(void* handle)
	{
	}
	
	void AuiPaneInfo::Show(void* handle, bool show)
	{
	}
	
	void AuiPaneInfo::CaptionVisible(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::Maximize(void* handle)
	{
	}

	void AuiPaneInfo::Restore(void* handle)
	{
	}

	void AuiPaneInfo::PaneBorder(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::Gripper(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::GripperTop(void* handle, bool attop)
	{
	}

	void AuiPaneInfo::CloseButton(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::MaximizeButton(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::MinimizeButton(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::PinButton(void* handle, bool visible)
	{
	}

	void AuiPaneInfo::DestroyOnClose(void* handle, bool b)
	{
	}

	void AuiPaneInfo::TopDockable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::BottomDockable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::LeftDockable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::RightDockable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::Floatable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::Movable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::DockFixed(void* handle, bool b)
	{
	}

	void AuiPaneInfo::Dockable(void* handle, bool b)
	{
	}

	void AuiPaneInfo::DefaultPane(void* handle)
	{
	}

	void AuiPaneInfo::CenterPane(void* handle)
	{
	}

	void AuiPaneInfo::ToolbarPane(void* handle)
	{
	}

	void AuiPaneInfo::SetFlag(void* handle, int flag, bool option_state)
	{
	}

}
