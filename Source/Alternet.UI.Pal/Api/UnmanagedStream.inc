// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
int64_t GetLength();

bool GetIsOK();

bool GetIsSeekable();

int64_t GetPosition();
void SetPosition(int64_t value);

void* Read(void* buffer, int bufferCount, void* length);

public:
UnmanagedStream();

protected:
virtual ~UnmanagedStream();

private:
BYREF_ONLY(UnmanagedStream);
