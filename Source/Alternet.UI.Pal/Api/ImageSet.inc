// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
bool GetIsOk();

bool GetIsReadOnly();

SizeI GetDefaultSize();

void LoadFromStream(void* stream);
void AddImage(Image* image);
void Clear();
void LoadSvgFromStream(void* stream, int width, int height, const Color& color);
void LoadSvgFromString(const string& s, int width, int height, const Color& color);
void InitImage(Image* image, int width, int height);
void InitImageFor(Image* image, void* window);
SizeI GetPreferredBitmapSizeAtScale(double scale);
SizeI GetPreferredBitmapSizeFor(void* window);

public:
ImageSet();

protected:
virtual ~ImageSet();

private:
BYREF_ONLY(ImageSet);
