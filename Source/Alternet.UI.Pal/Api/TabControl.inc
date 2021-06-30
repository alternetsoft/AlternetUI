// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

int GetPageCount();

void InsertPage(int index, Control* page, const string& title);
void RemovePage(int index, Control* page);
SizeF GetTotalPreferredSizeFromPageSize(const SizeF& pageSize);
public:
TabControl();
protected:
virtual ~TabControl();
private:
BYREF_ONLY(TabControl);
