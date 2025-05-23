// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
void* GetRealHandle();

bool GetSizingGripVisible();
void SetSizingGripVisible(bool value);


public:

enum class StatusBarEvent
{
    ControlRecreated,
};

typedef void* (*StatusBarEventCallbackType)(StatusBar* obj, StatusBarEvent event, void* param);

static void SetEventCallback(StatusBarEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(StatusBarEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(StatusBarEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(StatusBarEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static StatusBarEventCallbackType eventCallback = nullptr;

public:
StatusBar();

protected:
virtual ~StatusBar();

private:
BYREF_ONLY(StatusBar);
