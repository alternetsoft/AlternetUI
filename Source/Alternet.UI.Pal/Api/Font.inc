// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
public:

string GetName();


float GetSizeInPoints();


string GetDescription();

static void* OpenFamiliesArray();
static int GetFamiliesItemCount(void* array);
static string GetFamiliesItemAt(void* array, int index);
static void CloseFamiliesArray(void* array);
void Initialize(GenericFontFamily genericFamily, optional<string> familyName, float emSizeInPoints);
void InitializeWithDefaultFont();
static bool IsFamilyValid(const string& fontFamily);
static string GetGenericFamilyName(GenericFontFamily genericFamily);
bool IsEqualTo(Font* other);
string Serialize();
public:
Font();
protected:
virtual ~Font();
private:
BYREF_ONLY(Font);
