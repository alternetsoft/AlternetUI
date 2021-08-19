// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:
void FillRectangle(const RectangleF& rectangle, Brush* brush);
void DrawRectangle(const RectangleF& rectangle, Pen* pen);
void FillEllipse(const RectangleF& bounds, Brush* brush);
void DrawEllipse(const RectangleF& bounds, Pen* pen);
void DrawText(const string& text, const PointF& origin, Font* font, Brush* brush);
void DrawImage(Image* image, const PointF& origin);
SizeF MeasureText(const string& text, Font* font);
void PushTransform(const SizeF& translation);
void Pop();
private:
DrawingContext() {}
protected:
virtual ~DrawingContext();
private:
BYREF_ONLY(DrawingContext);
