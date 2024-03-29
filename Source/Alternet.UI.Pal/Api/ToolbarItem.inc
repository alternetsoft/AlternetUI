// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:

string GetManagedCommandId();
void SetManagedCommandId(const string& value);


string GetText();
void SetText(const string& value);


bool GetChecked();
void SetChecked(bool value);


Menu* GetDropDownMenu();
void SetDropDownMenu(Menu* value);


bool GetIsCheckable();
void SetIsCheckable(bool value);


ImageSet* GetDisabledImage();
void SetDisabledImage(ImageSet* value);


ImageSet* GetImage();
void SetImage(ImageSet* value);

public:
enum class ToolbarItemEvent
{
    Click,
};
typedef void* (*ToolbarItemEventCallbackType)(ToolbarItem* obj, ToolbarItemEvent event, void* param);
static void SetEventCallback(ToolbarItemEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(ToolbarItemEvent event, void* parameter = nullptr) { if (EventsSuspended()) return false; if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
void* RaiseEventWithPointerResult(ToolbarItemEvent event, void* parameter = nullptr) { if (EventsSuspended()) return nullptr; if (eventCallback != nullptr) return eventCallback(this, event, parameter); else return nullptr; }
private:
inline static ToolbarItemEventCallbackType eventCallback = nullptr;
public:
ToolbarItem();
protected:
virtual ~ToolbarItem();
private:
BYREF_ONLY(ToolbarItem);
