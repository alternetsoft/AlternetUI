// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>
public:
static void* CreateBoxSizer(int orient);
static void* AddSpacer(void* handle, int size);
static SizeI CalcMin(void* handle);
static int GetOrientation(void* handle);
static void SetOrientation(void* handle, int orient);
static void RepositionChildren(void* handle, const SizeI& minSize);
public:
BoxSizer();
protected:
virtual ~BoxSizer();
private:
BYREF_ONLY(BoxSizer);
