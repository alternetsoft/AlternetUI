// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:
void FillRectangle(const RectangleF& rectangle, const Color& color);
void DrawRectangle(const RectangleF& rectangle, const Color& color);
void DrawText(const string& text, const PointF& origin, const Color& color);
SizeF MeasureText(const string& text);
private:
DrawingContext() {}
public:
virtual ~DrawingContext();
private:
BYREF_ONLY(DrawingContext);
