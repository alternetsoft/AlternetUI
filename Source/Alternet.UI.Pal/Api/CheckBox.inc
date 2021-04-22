// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetText();
void SetText(const string& value);


bool GetIsChecked();
void SetIsChecked(bool value);

public:
enum class CheckBoxEvent
{
    CheckedChanged,
};
typedef void* (*CheckBoxEventCallbackType)(CheckBox* obj, CheckBoxEvent event, void* param);
static void SetEventCallback(CheckBoxEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(CheckBoxEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static CheckBoxEventCallbackType eventCallback = nullptr;
public:
CheckBox();
protected:
virtual ~CheckBox();
private:
BYREF_ONLY(CheckBox);
