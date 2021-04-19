// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:
void FillRectangle(const RectangleF& rectangle, const Color& color);
void DrawRectangle(const RectangleF& rectangle, const Color& color);
void DrawText(const string& text, const PointF& origin, const Color& color);
SizeF MeasureText(const string& text);
void PushTransform(const SizeF& translation);
void Pop();
private:
DrawingContext() {}
protected:
virtual ~DrawingContext();
private:
BYREF_ONLY(DrawingContext);
