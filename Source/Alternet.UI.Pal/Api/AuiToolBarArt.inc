// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:
static void SetFlags(void* handle, uint32_t flags);
static uint32_t GetFlags(void* handle);
static void SetTextOrientation(void* handle, int orientation);
static int GetTextOrientation(void* handle);
static int GetElementSize(void* handle, int elementId);
static void SetElementSize(void* handle, int elementId, int size);
static void UpdateColorsFromSystem(void* handle);
public:
AuiToolBarArt();
protected:
virtual ~AuiToolBarArt();
private:
BYREF_ONLY(AuiToolBarArt);
