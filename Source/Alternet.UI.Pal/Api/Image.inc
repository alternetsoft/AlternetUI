// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:

double GetScaleFactor();
void SetScaleFactor(double value);


SizeI GetDipSize();


double GetScaledHeight();


SizeI GetScaledSize();


double GetScaledWidth();


SizeI GetPixelSize();


bool GetIsOk();


bool GetHasAlpha();
void SetHasAlpha(bool value);


int GetPixelWidth();


int GetPixelHeight();


int GetDepth();

bool InitializeFromDipSize(int width, int height, double scale, int depth);
bool InitializeFromScreen();
bool LoadFromStream(void* stream);
bool LoadSvgFromStream(void* stream, int width, int height, const Color& color);
bool LoadSvgFromString(const string& s, int width, int height, const Color& color);
void Initialize(const SizeI& size, int depth);
void InitializeFromImage(Image* source, const SizeI& size);
void CopyFrom(Image* otherImage);
bool SaveToStream(void* stream, const string& format);
bool SaveToFile(const string& fileName);
void* ConvertToGenericImage();
void LoadFromGenericImage(void* image, int depth);
bool GrayScale();
void ResetAlpha();
bool LoadFile(const string& name, int type);
bool SaveFile(const string& name, int type);
bool SaveStream(void* stream, int type);
bool LoadStream(void* stream, int type);
Image* GetSubBitmap(const RectI& rect);
Image* ConvertToDisabled(uint8_t brightness);
void Rescale(const SizeI& sizeNeeded);
static int GetDefaultBitmapType();
public:
Image();
protected:
virtual ~Image();
private:
BYREF_ONLY(Image);
