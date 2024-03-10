// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:

void* GetEventDcHandle();


DrawingContext* GetEventDc();


RectI GetEventRect();


int GetEventItem();


int GetEventHeight();
void SetEventHeight(int value);


bool GetHasBorder();
void SetHasBorder(bool value);


int GetItemsCount();
void SetItemsCount(int value);


ListBoxSelectionMode GetSelectionMode();
void SetSelectionMode(ListBoxSelectionMode value);

int GetVisibleEnd();
int GetVisibleBegin();
int GetRowHeight(int line);
bool IsSelected(int line);
static void* CreateEx(int64_t styles);
void ClearItems();
void ClearSelected();
void SetSelected(int index, bool value);
int GetFirstSelected();
int GetNextSelected();
int GetSelectedCount();
int GetSelection();
void EnsureVisible(int itemIndex);
int ItemHitTest(const PointD& position);
void SetSelection(int selection);
void SetSelectionBackground(const Color& color);
bool IsCurrent(int current);
bool DoSetCurrent(int current);
public:
enum class VListBoxEvent
{
    SelectionChanged,
    MeasureItem,
    DrawItem,
    DrawItemBackground,
    ControlRecreated,
};
typedef void* (*VListBoxEventCallbackType)(VListBox* obj, VListBoxEvent event, void* param);
static void SetEventCallback(VListBoxEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(VListBoxEvent event, void* parameter = nullptr) { if (EventsSuspended()) return false; if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
void* RaiseEventWithPointerResult(VListBoxEvent event, void* parameter = nullptr) { if (EventsSuspended()) return nullptr; if (eventCallback != nullptr) return eventCallback(this, event, parameter); else return nullptr; }
private:
inline static VListBoxEventCallbackType eventCallback = nullptr;
public:
VListBox();
protected:
virtual ~VListBox();
private:
BYREF_ONLY(VListBox);