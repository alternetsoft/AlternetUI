// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
string GetEventArgString();

string GetName();
void SetName(const string& value);

Keyboard* GetKeyboard();

Mouse* GetMouse();

Clipboard* GetClipboard();

string GetDisplayName();
void SetDisplayName(const string& value);

string GetAppClassName();
void SetAppClassName(const string& value);

string GetVendorName();
void SetVendorName(const string& value);

string GetVendorDisplayName();
void SetVendorDisplayName(const string& value);

bool GetInUixmlPreviewerMode();
void SetInUixmlPreviewerMode(bool value);

bool GetInvokeRequired();

static void SetGtkCss(bool inject, const string& css);
PropertyUpdateResult SetAppearance(ApplicationAppearance appearance);
static void GetEventIdentifiers(int* eventIdentifiers, int eventIdentifiersCount);
static void ThrowError(int value);
static void SetSystemOptionInt(const string& name, int value);
void Run(Window* window);
void* GetTopWindow();
void ExitMainLoop();
static void WakeUpIdle();
void Exit();
static void SuppressDiagnostics(int flags);
void BeginInvoke(PInvokeCallbackActionType action);
void ProcessPendingEvents();
bool HasPendingEvents();
void* GetDisplayMode();
bool GetExitOnFrameDelete();
int GetLayoutDirection();
bool GetUseBestVisual();
bool IsActive();
bool SafeYield(void* window, bool onlyIfNeeded);
bool SafeYieldFor(void* window, int64_t eventsToProcess);
bool SetDisplayMode(void* videoMode);
void SetExitOnFrameDelete(bool flag);
bool SetNativeTheme(const string& theme);
void SetTopWindow(void* window);
void SetUseBestVisual(bool flag, bool forceTrueColor);

public:

enum class ApplicationEvent
{
    Idle,
    LogMessage,
    QueryEndSession,
    EndSession,
    ActivateApp,
    Hibernate,
    DialupConnected,
    DialupDisconnected,
    ExceptionInMainLoop,
    UnhandledException,
    FatalException,
    AssertFailure,
};

typedef void* (*ApplicationEventCallbackType)(Application* obj, ApplicationEvent event, void* param);

static void SetEventCallback(ApplicationEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(ApplicationEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(ApplicationEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(ApplicationEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static ApplicationEventCallbackType eventCallback = nullptr;

public:
Application();

protected:
virtual ~Application();

private:
BYREF_ONLY(Application);
