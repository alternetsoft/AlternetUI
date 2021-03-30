public:
void Show();
void AddChildControl(Control* value);
string GetTitle();
void SetTitle(const string& value);
public:
Window();
virtual ~Window();
private:
BYREF_ONLY(Window);