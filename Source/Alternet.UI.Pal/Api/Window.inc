// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetTitle();
void SetTitle(const string& value);

public:
enum class WindowEvent
{
    Closing,
};
typedef void* (*WindowEventCallbackType)(Window* obj, WindowEvent event, void* param);
static void SetEventCallback(WindowEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(WindowEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static WindowEventCallbackType eventCallback = nullptr;
public:
Window();
protected:
virtual ~Window();
private:
BYREF_ONLY(Window);
