// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

SizeF GetSize();
void SetSize(const SizeF& value);


PointF GetLocation();
void SetLocation(const PointF& value);


RectangleF GetBounds();
void SetBounds(const RectangleF& value);


bool GetVisible();
void SetVisible(bool value);


bool GetIsMouseOver();

void SetMouseCapture(bool value);
void AddChild(Control* control);
void RemoveChild(Control* control);
void Update();
virtual SizeF GetPreferredSize(const SizeF& availableSize);
DrawingContext* OpenPaintDrawingContext();
DrawingContext* OpenClientDrawingContext();
public:
enum class ControlEvent
{
    Paint,
    MouseEnter,
    MouseLeave,
    MouseMove,
    MouseLeftButtonDown,
    MouseLeftButtonUp,
};
typedef void(*ControlEventCallbackType)(Control* obj, ControlEvent event);
static void SetEventCallback(ControlEventCallbackType value) { eventCallback = value; }
protected:
void RaiseEvent(ControlEvent event) { if (eventCallback != nullptr) eventCallback(this, event); }
private:
inline static ControlEventCallbackType eventCallback = nullptr;
protected:
Control();
public:
virtual ~Control();
private:
BYREF_ONLY(Control);
