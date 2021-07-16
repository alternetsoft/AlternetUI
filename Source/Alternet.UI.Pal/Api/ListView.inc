// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


ListViewView GetCurrentView();
void SetCurrentView(ListViewView value);

void InsertItemAt(int index, const string& value);
void RemoveItemAt(int index);
void ClearItems();
void InsertColumnAt(int index, const string& header);
void RemoveColumnAt(int index);
public:
ListView();
protected:
virtual ~ListView();
private:
BYREF_ONLY(ListView);
