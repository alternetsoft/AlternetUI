// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


bool GetIsEditable();
void SetIsEditable(bool value);


int GetSelectedIndex();
void SetSelectedIndex(int value);


string GetText();
void SetText(const string& value);

void InsertItem(int index, const string& value);
void RemoveItemAt(int index);
void ClearItems();
public:
enum class ComboBoxEvent
{
    SelectedItemChanged,
    TextChanged,
};
typedef void* (*ComboBoxEventCallbackType)(ComboBox* obj, ComboBoxEvent event, void* param);
static void SetEventCallback(ComboBoxEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(ComboBoxEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static ComboBoxEventCallbackType eventCallback = nullptr;
public:
ComboBox();
protected:
virtual ~ComboBox();
private:
BYREF_ONLY(ComboBox);
