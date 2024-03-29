// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:

int GetEventSelection();


int GetEventOldSelection();


int64_t GetCreateStyle();
void SetCreateStyle(int64_t value);

static void* CreateEx(int64_t styles);
void SetArtProvider(void* art);
void* GetArtProvider();
void SetUniformBitmapSize(int width, int height);
void SetTabCtrlHeight(int height);
bool AddPage(void* page, const string& caption, bool select, ImageSet* bitmap);
bool InsertPage(uint64_t pageIdx, void* page, const string& caption, bool select, ImageSet* bitmap);
bool DeletePage(uint64_t page);
bool RemovePage(uint64_t page);
uint64_t GetPageCount();
void* GetPage(uint64_t pageIdx);
int FindPage(void* page);
bool SetPageText(uint64_t page, const string& text);
string GetPageText(uint64_t pageIdx);
bool SetPageToolTip(uint64_t page, const string& text);
string GetPageToolTip(uint64_t pageIdx);
bool SetPageBitmap(uint64_t page, ImageSet* bitmap);
int64_t SetSelection(uint64_t newPage);
int64_t GetSelection();
int64_t ChangeSelection(uint64_t newPage);
void AdvanceSelection(bool forward);
void SetMeasuringFont(Font* font);
void SetNormalFont(Font* font);
void SetSelectedFont(Font* font);
void Split(uint64_t page, int direction);
int GetTabCtrlHeight();
int GetHeightForPageHeight(int pageHeight);
bool ShowWindowMenu();
bool DeleteAllPages();
public:
enum class AuiNotebookEvent
{
    PageClose,
    PageClosed,
    PageChanged,
    PageChanging,
    PageButton,
    BeginDrag,
    EndDrag,
    DragMotion,
    AllowTabDrop,
    DragDone,
    TabMiddleMouseDown,
    TabMiddleMouseUp,
    TabRightMouseDown,
    TabRightMouseUp,
    BgDclickMouse,
};
typedef void* (*AuiNotebookEventCallbackType)(AuiNotebook* obj, AuiNotebookEvent event, void* param);
static void SetEventCallback(AuiNotebookEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(AuiNotebookEvent event, void* parameter = nullptr) { if (EventsSuspended()) return false; if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
void* RaiseEventWithPointerResult(AuiNotebookEvent event, void* parameter = nullptr) { if (EventsSuspended()) return nullptr; if (eventCallback != nullptr) return eventCallback(this, event, parameter); else return nullptr; }
private:
inline static AuiNotebookEventCallbackType eventCallback = nullptr;
public:
AuiNotebook();
protected:
virtual ~AuiNotebook();
private:
BYREF_ONLY(AuiNotebook);
