// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
int GetMinimum();
void SetMinimum(int value);

int GetMaximum();
void SetMaximum(int value);

int GetValue();
void SetValue(int value);

int GetSmallChange();
void SetSmallChange(int value);

int GetLargeChange();
void SetLargeChange(int value);

int GetTickFrequency();
void SetTickFrequency(int value);

SliderOrientation GetOrientation();
void SetOrientation(SliderOrientation value);

SliderTickStyle GetTickStyle();
void SetTickStyle(SliderTickStyle value);

void ClearTicks();

public:

enum class SliderEvent
{
    ValueChanged,
};

typedef void* (*SliderEventCallbackType)(Slider* obj, SliderEvent event, void* param);

static void SetEventCallback(SliderEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(SliderEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(SliderEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(SliderEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static SliderEventCallbackType eventCallback = nullptr;

public:
Slider();

protected:
virtual ~Slider();

private:
BYREF_ONLY(Slider);
