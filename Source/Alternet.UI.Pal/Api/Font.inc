// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:
void* OpenFamiliesArray();
int GetFamiliesItemCount(void* array);
string GetFamiliesItemAt(void* array, int index);
void CloseFamiliesArray(void* array);
void Initialize(GenericFontFamily genericFamily, optional<string> familyName, float emSize);
void InitializeWithDefaultFont();
static bool IsFamilyValid(const string& fontFamily);
static string GetGenericFamilyName(GenericFontFamily genericFamily);
public:
Font();
protected:
virtual ~Font();
private:
BYREF_ONLY(Font);
