// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetText();
void SetText(const string& value);

public:
enum class TextBoxEvent
{
    TextChanged,
};
typedef void(*TextBoxEventCallbackType)(TextBox* obj, TextBoxEvent event);
static void SetEventCallback(TextBoxEventCallbackType value) { eventCallback = value; }
protected:
void RaiseEvent(TextBoxEvent event) { if (eventCallback != nullptr) eventCallback(this, event); }
private:
inline static TextBoxEventCallbackType eventCallback = nullptr;
public:
TextBox();
public:
virtual ~TextBox();
private:
BYREF_ONLY(TextBox);
