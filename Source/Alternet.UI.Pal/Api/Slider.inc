// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
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

public:
enum class SliderEvent
{
    ValueChanged,
};
typedef void* (*SliderEventCallbackType)(Slider* obj, SliderEvent event, void* param);
static void SetEventCallback(SliderEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(SliderEvent event) { if (eventCallback != nullptr) return eventCallback(this, event, nullptr) != nullptr; else return false; }
private:
inline static SliderEventCallbackType eventCallback = nullptr;
public:
Slider();
protected:
virtual ~Slider();
private:
BYREF_ONLY(Slider);
