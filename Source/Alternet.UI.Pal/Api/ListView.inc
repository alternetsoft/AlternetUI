// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


ImageList* GetSmallImageList();
void SetSmallImageList(ImageList* value);


ImageList* GetLargeImageList();
void SetLargeImageList(ImageList* value);


ListViewView GetCurrentView();
void SetCurrentView(ListViewView value);


ListViewSelectionMode GetSelectionMode();
void SetSelectionMode(ListViewSelectionMode value);

void* OpenSelectedIndicesArray();
int GetSelectedIndicesItemCount(void* array);
int GetSelectedIndicesItemAt(void* array, int index);
void CloseSelectedIndicesArray(void* array);
void InsertItemAt(int index, const string& text, int columnIndex, int imageIndex);
void RemoveItemAt(int index);
void ClearItems();
void InsertColumnAt(int index, const string& header);
void RemoveColumnAt(int index);
void ClearSelected();
void SetSelected(int index, bool value);
public:
enum class ListViewEvent
{
    SelectionChanged,
};
typedef void* (*ListViewEventCallbackType)(ListView* obj, ListViewEvent event, void* param);
static void SetEventCallback(ListViewEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(ListViewEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static ListViewEventCallbackType eventCallback = nullptr;
public:
ListView();
protected:
virtual ~ListView();
private:
BYREF_ONLY(ListView);
