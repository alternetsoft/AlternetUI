// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetText();
void SetText(const string& value);


bool GetEditControlOnly();
void SetEditControlOnly(bool value);

public:
enum class TextBoxEvent
{
    TextChanged,
};
typedef void* (*TextBoxEventCallbackType)(TextBox* obj, TextBoxEvent event, void* param);
static void SetEventCallback(TextBoxEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(TextBoxEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static TextBoxEventCallbackType eventCallback = nullptr;
public:
TextBox();
protected:
virtual ~TextBox();
private:
BYREF_ONLY(TextBox);
