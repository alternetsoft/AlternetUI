// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:
void FillRectangle(const RectangleF& rectangle, const Color& color);
void DrawRectangle(const RectangleF& rectangle, const Color& color);
void DrawText(const string& text, const PointF& origin, Font* font, const Color& color);
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
