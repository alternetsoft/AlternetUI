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
		PaneInfo(handle).SafeSet(PaneInfo(source));
	}

	bool AuiPaneInfo::IsOk(void* handle)
	{
		return PaneInfo(handle).IsOk();
	}

	bool AuiPaneInfo::IsFixed(void* handle)
	{
		return PaneInfo(handle).IsFixed();
	}

	bool AuiPaneInfo::IsResizable(void* handle)
	{
		return PaneInfo(handle).IsResizable();
	}

	bool AuiPaneInfo::IsShown(void* handle)
	{
		return PaneInfo(handle).IsShown();
	}

	bool AuiPaneInfo::IsFloating(void* handle)
	{
		return PaneInfo(handle).IsFloating();
	}

	bool AuiPaneInfo::IsDocked(void* handle)
	{
		return PaneInfo(handle).IsDocked();
	}

	Int32Size AuiPaneInfo::GetBestSize(void* handle)
	{
		return PaneInfo(handle).best_size;
	}

	Int32Size AuiPaneInfo::GetMinSize(void* handle)
	{
		return PaneInfo(handle).min_size;
	}

	Int32Size AuiPaneInfo::GetMaxSize(void* handle)
	{
		return PaneInfo(handle).max_size;
	}

	bool AuiPaneInfo::IsToolbar(void* handle)
	{
		return PaneInfo(handle).IsToolbar();
	}

	bool AuiPaneInfo::IsTopDockable(void* handle)
	{
		return PaneInfo(handle).IsTopDockable();
	}

	bool AuiPaneInfo::IsBottomDockable(void* handle)
	{
		return PaneInfo(handle).IsBottomDockable();
	}

	bool AuiPaneInfo::IsLeftDockable(void* handle)
	{
		return PaneInfo(handle).IsLeftDockable();
	}

	bool AuiPaneInfo::IsRightDockable(void* handle)
	{
		return PaneInfo(handle).IsRightDockable();
	}

	bool AuiPaneInfo::IsDockable(void* handle)
	{	
		return PaneInfo(handle).IsDockable();
	}

	bool AuiPaneInfo::IsFloatable(void* handle)
	{
		return PaneInfo(handle).IsFloatable();
	}

	bool AuiPaneInfo::IsMovable(void* handle)
	{
		return PaneInfo(handle).IsMovable();
	}

	bool AuiPaneInfo::IsDestroyOnClose(void* handle)
	{
		return PaneInfo(handle).IsDestroyOnClose();
	}

	bool AuiPaneInfo::IsMaximized(void* handle)
	{
		return PaneInfo(handle).IsMaximized();
	}

	bool AuiPaneInfo::HasCaption(void* handle)
	{
		return PaneInfo(handle).HasCaption();
	}

	bool AuiPaneInfo::HasGripper(void* handle)
	{
		return PaneInfo(handle).HasGripper();
	}
	
	bool AuiPaneInfo::HasBorder(void* handle)
	{
		return PaneInfo(handle).HasBorder();
	}
	
	bool AuiPaneInfo::HasCloseButton(void* handle)
	{
		return PaneInfo(handle).HasCloseButton();
	}
	
	bool AuiPaneInfo::HasMaximizeButton(void* handle)
	{
		return PaneInfo(handle).HasMaximizeButton();
	}
	
	bool AuiPaneInfo::HasMinimizeButton(void* handle)
	{
		return PaneInfo(handle).HasMinimizeButton();
	}
	
	bool AuiPaneInfo::HasPinButton(void* handle)
	{
		return PaneInfo(handle).HasPinButton();
	}
	
	bool AuiPaneInfo::HasGripperTop(void* handle)
	{
		return PaneInfo(handle).HasGripperTop();
	}
	
	bool AuiPaneInfo::IsValid(void* handle)
	{
		return PaneInfo(handle).IsValid();
	}

	bool AuiPaneInfo::HasFlag(void* handle, int flag)
	{
		return PaneInfo(handle).HasFlag(flag);
	}

	void AuiPaneInfo::Window(void* handle, void* window)
	{
		PaneInfo(handle).Window((wxWindow*)window);
	}
	
	void AuiPaneInfo::Name(void* handle, const string& value)
	{
		PaneInfo(handle).Name(wxStr(value));
	}
	
	void AuiPaneInfo::Caption(void* handle, const string& value)
	{
		PaneInfo(handle).Caption(wxStr(value));
	}
	
	void AuiPaneInfo::Image(void* handle, ImageSet* bitmap)
	{
		PaneInfo(handle).Icon(ImageSet::BitmapBundle(bitmap));
	}
	
	void AuiPaneInfo::Left(void* handle)
	{
		PaneInfo(handle).Left();
	}
	
	void AuiPaneInfo::Right(void* handle)
	{
		PaneInfo(handle).Right();
	}
	
	void AuiPaneInfo::Top(void* handle)
	{
		PaneInfo(handle).Top();
	}
	
	void AuiPaneInfo::Bottom(void* handle)
	{
		PaneInfo(handle).Bottom();
	}
	
	void AuiPaneInfo::Center(void* handle)
	{
		PaneInfo(handle).Center();
	}
	
	void AuiPaneInfo::Direction(void* handle, int direction)
	{
		PaneInfo(handle).Direction(direction);
	}
	
	void AuiPaneInfo::Layer(void* handle, int layer)
	{
		PaneInfo(handle).Layer(layer);
	}
	
	void AuiPaneInfo::Row(void* handle, int row)
	{
		PaneInfo(handle).Row(row);
	}
	
	void AuiPaneInfo::Position(void* handle, int pos)
	{
		PaneInfo(handle).Position(pos);
	}
	
	void AuiPaneInfo::BestSize(void* handle, int x, int y)
	{
		PaneInfo(handle).BestSize(x, y);
	}
	
	void AuiPaneInfo::MinSize(void* handle, int x, int y)
	{
		PaneInfo(handle).MinSize(x, y);
	}
	
	void AuiPaneInfo::MaxSize(void* handle, int x, int y)
	{
		PaneInfo(handle).MaxSize(x, y);
	}
	
	void AuiPaneInfo::FloatingPosition(void* handle, int x, int y)
	{
		PaneInfo(handle).FloatingPosition(x, y);
	}
	
	void AuiPaneInfo::FloatingSize(void* handle, int x, int y)
	{
		PaneInfo(handle).FloatingSize(x, y);
	}
	
	void AuiPaneInfo::Fixed(void* handle)
	{
		PaneInfo(handle).Fixed();

		if (PaneInfo(handle).IsFixed())
		{
			if (true) {}
		}
	}
	
	void AuiPaneInfo::Resizable(void* handle, bool resizable)
	{
		PaneInfo(handle).Resizable(resizable);
	}
	
	void AuiPaneInfo::Dock(void* handle)
	{
		PaneInfo(handle).Dock();
	}
	
	void AuiPaneInfo::Float(void* handle)
	{
		PaneInfo(handle).Float();
	}
	
	void AuiPaneInfo::Hide(void* handle)
	{
		PaneInfo(handle).Hide();
	}
	
	void AuiPaneInfo::Show(void* handle, bool show)
	{
		PaneInfo(handle).Show(show);
	}
	
	void AuiPaneInfo::CaptionVisible(void* handle, bool visible)
	{
		PaneInfo(handle).CaptionVisible(visible);
	}

	void AuiPaneInfo::Maximize(void* handle)
	{
		PaneInfo(handle).Maximize();
	}

	void AuiPaneInfo::Restore(void* handle)
	{
		PaneInfo(handle).Restore();
	}

	void AuiPaneInfo::PaneBorder(void* handle, bool visible)
	{
		PaneInfo(handle).PaneBorder(visible);
	}

	void AuiPaneInfo::Gripper(void* handle, bool visible)
	{
		PaneInfo(handle).Gripper(visible);
	}

	void AuiPaneInfo::GripperTop(void* handle, bool attop)
	{
		PaneInfo(handle).GripperTop(attop);
	}

	void AuiPaneInfo::CloseButton(void* handle, bool visible)
	{
		PaneInfo(handle).CloseButton(visible);
	}

	void AuiPaneInfo::MaximizeButton(void* handle, bool visible)
	{
		PaneInfo(handle).MaximizeButton(visible);
	}

	void AuiPaneInfo::MinimizeButton(void* handle, bool visible)
	{
		PaneInfo(handle).MinimizeButton(visible);
	}

	void AuiPaneInfo::PinButton(void* handle, bool visible)
	{
		PaneInfo(handle).PinButton(visible);
	}

	void AuiPaneInfo::DestroyOnClose(void* handle, bool b)
	{
		PaneInfo(handle).DestroyOnClose(b);
	}

	void AuiPaneInfo::TopDockable(void* handle, bool b)
	{
		PaneInfo(handle).TopDockable(b);
	}

	void AuiPaneInfo::BottomDockable(void* handle, bool b)
	{
		PaneInfo(handle).BottomDockable(b);
	}

	void AuiPaneInfo::LeftDockable(void* handle, bool b)
	{
		PaneInfo(handle).LeftDockable(b);
	}

	void AuiPaneInfo::RightDockable(void* handle, bool b)
	{
		PaneInfo(handle).RightDockable(b);
	}

	void AuiPaneInfo::Floatable(void* handle, bool b)
	{
		PaneInfo(handle).Floatable(b);
	}

	void AuiPaneInfo::Movable(void* handle, bool b)
	{
		PaneInfo(handle).Movable(b);
	}

	void AuiPaneInfo::DockFixed(void* handle, bool b)
	{
		PaneInfo(handle).DockFixed(b);
	}

	void AuiPaneInfo::Dockable(void* handle, bool b)
	{
		PaneInfo(handle).Dockable(b);
	}

	void AuiPaneInfo::DefaultPane(void* handle)
	{
		PaneInfo(handle).DefaultPane();
	}

	void AuiPaneInfo::CenterPane(void* handle)
	{
		PaneInfo(handle).CenterPane();
	}

	void AuiPaneInfo::ToolbarPane(void* handle)
	{
		PaneInfo(handle).ToolbarPane();
	}

	void AuiPaneInfo::SetFlag(void* handle, int flag, bool option_state)
	{
		PaneInfo(handle).SetFlag(flag, option_state);
	}

}
