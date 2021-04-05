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

void AddChild(Control* control);
void RemoveChild(Control* control);
SizeF GetPreferredSize(const SizeF& availableSize);
protected:
Control();
public:
virtual ~Control();
private:
BYREF_ONLY(Control);
