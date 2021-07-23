// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

ImageList* GetImageList();
void SetImageList(ImageList* value);


void* GetRootItem();


TreeViewSelectionMode GetSelectionMode();
void SetSelectionMode(TreeViewSelectionMode value);

void* OpenSelectedItemsArray();
int GetSelectedItemsItemCount(void* array);
void* GetSelectedItemsItemAt(void* array, int index);
void CloseSelectedItemsArray(void* array);
int GetItemCount(void* parentItem);
void InsertItemAt(void* parentItem, int index, const string& text, int imageIndex);
void RemoveItem(void* item);
void ClearItems(void* parentItem);
void ClearSelected();
void SetSelected(void* item, bool value);
public:
enum class TreeViewEvent
{
    SelectionChanged,
};
typedef void* (*TreeViewEventCallbackType)(TreeView* obj, TreeViewEvent event, void* param);
static void SetEventCallback(TreeViewEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(TreeViewEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static TreeViewEventCallbackType eventCallback = nullptr;
public:
TreeView();
protected:
virtual ~TreeView();
private:
BYREF_ONLY(TreeView);
