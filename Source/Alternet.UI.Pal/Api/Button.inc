// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetText();
void SetText(const string& value);

public:
enum class ButtonEvent
{
    Click,
};
typedef void(*ButtonEventCallbackType)(Button* obj, ButtonEvent event);
static void SetEventCallback(ButtonEventCallbackType value) { eventCallback = value; }
protected:
void RaiseEvent(ButtonEvent event) { if (eventCallback != nullptr) eventCallback(this, event); }
private:
inline static ButtonEventCallbackType eventCallback = nullptr;
public:
Button();
protected:
virtual ~Button();
private:
BYREF_ONLY(Button);
