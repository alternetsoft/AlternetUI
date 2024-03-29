// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:

int64_t GetCreateStyle();
void SetCreateStyle(int64_t value);


int GetEventToolId();


bool GetEventIsDropDownClicked();


PointI GetEventClickPoint();


RectI GetEventItemRect();

void DoOnCaptureLost();
void DoOnLeftUp(int x, int y);
void DoOnLeftDown(int x, int y);
static void* CreateEx(int64_t styles);
void SetArtProvider(void* art);
void* GetArtProvider();
int GetToolKind(int toolId);
void* AddTool(int toolId, const string& label, ImageSet* bitmapBundle, const string& shortHelpString, int itemKind);
void* AddTool2(int toolId, const string& label, ImageSet* bitmapBundle, ImageSet* disabledBitmapBundle, int itemKind, const string& shortHelpString, const string& longHelpString, void* clientData);
void* AddTool3(int toolId, ImageSet* bitmapBundle, ImageSet* disabledBitmapBundle, bool toggle, void* clientData, const string& shortHelpString, const string& longHelpString);
void* AddLabel(int toolId, const string& label, int width);
void* AddControl(int toolId, void* control, const string& label);
void* AddSeparator(int toolId);
void* AddSpacer(int toolId, int pixels);
void* AddStretchSpacer(int toolId, int proportion);
bool Realize();
void* FindControl(int windowId);
void* FindToolByPosition(int x, int y);
void* FindToolByIndex(int idx);
void* FindTool(int toolId);
void Clear();
bool DestroyTool(int toolId);
bool DestroyToolByIndex(int idx);
bool DeleteTool(int toolId);
bool DeleteByIndex(int toolId);
int GetToolIndex(int toolId);
bool GetToolFits(int toolId);
RectD GetToolRect(int toolId);
bool GetToolFitsByIndex(int toolId);
bool GetToolBarFits();
void SetToolBitmapSizeInPixels(const SizeI& size);
SizeI GetToolBitmapSizeInPixels();
bool GetOverflowVisible();
void SetOverflowVisible(bool visible);
bool GetGripperVisible();
void SetGripperVisible(bool visible);
void ToggleTool(int toolId, bool state);
bool GetToolToggled(int toolId);
void SetMargins(int left, int right, int top, int bottom);
void EnableTool(int toolId, bool state);
bool GetToolEnabled(int toolId);
void SetToolDropDown(int toolId, bool dropdown);
bool GetToolDropDown(int toolId);
void SetToolBorderPadding(int padding);
int GetToolBorderPadding();
void SetToolTextOrientation(int orientation);
int GetToolTextOrientation();
void SetToolPacking(int packing);
int GetToolPacking();
void SetToolProportion(int toolId, int proportion);
int GetToolProportion(int toolId);
void SetToolSeparation(int separation);
int GetToolSeparation();
void SetToolSticky(int toolId, bool sticky);
bool GetToolSticky(int toolId);
string GetToolLabel(int toolId);
void SetToolLabel(int toolId, const string& label);
void SetToolBitmap(int toolId, ImageSet* bitmapBundle);
string GetToolShortHelp(int toolId);
void SetToolShortHelp(int toolId, const string& helpString);
string GetToolLongHelp(int toolId);
void SetToolLongHelp(int toolId, const string& helpString);
uint64_t GetToolCount();
void SetToolMinSize(int tool_id, int width, int height);
SizeI GetToolMinSize(int tool_id);
void SetAlignment(int tool_id, int l);
int GetAlignment(int tool_id);
public:
enum class AuiToolBarEvent
{
    ToolDropDown,
    BeginDrag,
    ToolMiddleClick,
    OverflowClick,
    ToolRightClick,
    ToolCommand,
};
typedef void* (*AuiToolBarEventCallbackType)(AuiToolBar* obj, AuiToolBarEvent event, void* param);
static void SetEventCallback(AuiToolBarEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(AuiToolBarEvent event, void* parameter = nullptr) { if (EventsSuspended()) return false; if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
void* RaiseEventWithPointerResult(AuiToolBarEvent event, void* parameter = nullptr) { if (EventsSuspended()) return nullptr; if (eventCallback != nullptr) return eventCallback(this, event, parameter); else return nullptr; }
private:
inline static AuiToolBarEventCallbackType eventCallback = nullptr;
public:
AuiToolBar();
protected:
virtual ~AuiToolBar();
private:
BYREF_ONLY(AuiToolBar);
