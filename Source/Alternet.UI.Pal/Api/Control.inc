// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

Control* GetParent();


SizeF GetSize();
void SetSize(const SizeF& value);


PointF GetLocation();
void SetLocation(const PointF& value);


RectangleF GetBounds();
void SetBounds(const RectangleF& value);


SizeF GetClientSize();
void SetClientSize(const SizeF& value);


virtual Thickness GetIntrinsicLayoutPadding();


virtual Thickness GetIntrinsicPreferredSizePadding();


bool GetVisible();
void SetVisible(bool value);


bool GetIsMouseOver();


Color GetBackgroundColor();
void SetBackgroundColor(const Color& value);


Color GetForegroundColor();
void SetForegroundColor(const Color& value);

void SetMouseCapture(bool value);
void AddChild(Control* control);
void RemoveChild(Control* control);
void Update();
virtual SizeF GetPreferredSize(const SizeF& availableSize);
DrawingContext* OpenPaintDrawingContext();
DrawingContext* OpenClientDrawingContext();
void BeginUpdate();
void EndUpdate();
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
    VisibleChanged,
};
typedef void* (*ControlEventCallbackType)(Control* obj, ControlEvent event, void* param);
static void SetEventCallback(ControlEventCallbackType value) { eventCallback = value; }
protected:
bool RaiseEvent(ControlEvent event, void* parameter = nullptr) { if (eventCallback != nullptr) return eventCallback(this, event, parameter) != nullptr; else return false; }
private:
inline static ControlEventCallbackType eventCallback = nullptr;
protected:
Control();
protected:
virtual ~Control();
private:
BYREF_ONLY(Control);
