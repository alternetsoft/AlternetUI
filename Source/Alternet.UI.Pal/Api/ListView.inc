// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetItemsCount();


ImageList* GetSmallImageList();
void SetSmallImageList(ImageList* value);


ImageList* GetLargeImageList();
void SetLargeImageList(ImageList* value);


ListViewView GetCurrentView();
void SetCurrentView(ListViewView value);

void InsertItemAt(int index, const string& text, int columnIndex, int imageIndex);
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
