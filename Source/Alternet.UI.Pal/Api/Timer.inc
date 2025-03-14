// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
bool GetEnabled();
void SetEnabled(bool value);

int GetInterval();
void SetInterval(int value);

bool GetAutoReset();
void SetAutoReset(bool value);

void Restart();

public:

enum class TimerEvent
{
    Tick,
};

typedef void* (*TimerEventCallbackType)(Timer* obj, TimerEvent event, void* param);

static void SetEventCallback(TimerEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(TimerEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(TimerEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(TimerEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static TimerEventCallbackType eventCallback = nullptr;

public:
Timer();

protected:
virtual ~Timer();

private:
BYREF_ONLY(Timer);
