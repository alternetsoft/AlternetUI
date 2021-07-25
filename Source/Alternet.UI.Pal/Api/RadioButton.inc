// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetText();
void SetText(const string& value);


bool GetIsChecked();
void SetIsChecked(bool value);

public:
enum class RadioButtonEvent
{
    CheckedChanged,
};
typedef void* (*RadioButtonEventCallbackType)(RadioButton* obj, RadioButtonEvent event, void* param);
static void SetEventCallback(RadioButtonEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(RadioButtonEvent event, void* parameter = nullptr) { if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
private:
inline static RadioButtonEventCallbackType eventCallback = nullptr;
public:
RadioButton();
protected:
virtual ~RadioButton();
private:
BYREF_ONLY(RadioButton);
