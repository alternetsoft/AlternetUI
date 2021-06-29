// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


ListBoxSelectionMode GetSelectionMode();
void SetSelectionMode(ListBoxSelectionMode value);

void InsertItem(int index, const string& value);
void RemoveItemAt(int index);
void ClearItems();
public:
ListBox();
protected:
virtual ~ListBox();
private:
BYREF_ONLY(ListBox);
