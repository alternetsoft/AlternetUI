// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetName();


float GetSize();

static void* OpenFamiliesArray();
static int GetFamiliesItemCount(void* array);
static string GetFamiliesItemAt(void* array, int index);
static void CloseFamiliesArray(void* array);
void Initialize(GenericFontFamily genericFamily, optional<string> familyName, float emSize);
void InitializeWithDefaultFont();
static bool IsFamilyValid(const string& fontFamily);
static string GetGenericFamilyName(GenericFontFamily genericFamily);
string ToString_();
bool IsEqualTo(Font* other);
int GetHashCode_();
public:
Font();
protected:
virtual ~Font();
private:
BYREF_ONLY(Font);
