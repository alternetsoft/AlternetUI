// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

Control* GetParent();


SizeF GetSize();
void SetSize(const SizeF& value);


PointF GetLocation();
void SetLocation(const PointF& value);


RectangleF GetBounds();
void SetBounds(const RectangleF& value);


bool GetVisible();
void SetVisible(bool value);


bool GetIsMouseOver();


Color GetBackgroundColor();
void SetBackgroundColor(const Color& value);

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
    MouseClick,
};
typedef void(*ControlEventCallbackType)(Control* obj, ControlEvent event);
static void SetEventCallback(ControlEventCallbackType value) { eventCallback = value; }
protected:
void RaiseEvent(ControlEvent event) { if (eventCallback != nullptr) eventCallback(this, event); }
private:
inline static ControlEventCallbackType eventCallback = nullptr;
protected:
Control();
protected:
virtual ~Control();
private:
BYREF_ONLY(Control);
