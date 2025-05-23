// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
bool GetAllowMouseWheel();
void SetAllowMouseWheel(bool value);

string GetEmptyTextHint();
void SetEmptyTextHint(const string& value);

bool GetHasBorder();
void SetHasBorder(bool value);

int GetItemsCount();

bool GetIsEditable();
void SetIsEditable(bool value);

int GetSelectedIndex();
void SetSelectedIndex(int value);

int GetTextSelectionStart();

int GetTextSelectionLength();

PointI GetTextMargins();

int GetOwnerDrawStyle();
void SetOwnerDrawStyle(int value);

void* GetPopupWidget();

void* GetEventDc();

RectI GetEventRect();

int GetEventItem();

int GetEventFlags();

int GetEventResultInt();
void SetEventResultInt(int value);

bool GetEventCalled();
void SetEventCalled(bool value);

void DismissPopup();
void ShowPopup();
int DefaultOnMeasureItemWidth();
int DefaultOnMeasureItem();
void DefaultOnDrawBackground();
void DefaultOnDrawItem();
void* CreateItemsInsertion();
void AddItemToInsertion(void* insertion, const string& item);
void CommitItemsInsertion(void* insertion, int index);
void InsertItem(int index, const string& value);
void RemoveItemAt(int index);
void ClearItems();
void SelectTextRange(int start, int length);
void SelectAllText();
void SetItem(int index, const string& value);

public:

enum class ComboBoxEvent
{
    SelectedItemChanged,
    MeasureItem,
    MeasureItemWidth,
    DrawItem,
    DrawItemBackground,
    AfterShowPopup,
    AfterDismissPopup,
};

typedef void* (*ComboBoxEventCallbackType)(ComboBox* obj, ComboBoxEvent event, void* param);

static void SetEventCallback(ComboBoxEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(ComboBoxEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(ComboBoxEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(ComboBoxEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static ComboBoxEventCallbackType eventCallback = nullptr;

public:
ComboBox();

protected:
virtual ~ComboBox();

private:
BYREF_ONLY(ComboBox);
