// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


ListBoxSelectionMode GetSelectionMode();
void SetSelectionMode(ListBoxSelectionMode value);


int GetSelectedIndicesCount();

void InsertItem(int index, const string& value);
void RemoveItemAt(int index);
void ClearItems();
int GetSelectedIndexAt(int index);
void ClearSelection();
void SetSelected(int index, bool value);
public:
enum class ListBoxEvent
{
    SelectionChanged,
};
typedef void* (*ListBoxEventCallbackType)(ListBox* obj, ListBoxEvent event, void* param);
static void SetEventCallback(ListBoxEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(ListBoxEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static ListBoxEventCallbackType eventCallback = nullptr;
public:
ListBox();
protected:
virtual ~ListBox();
private:
BYREF_ONLY(ListBox);
