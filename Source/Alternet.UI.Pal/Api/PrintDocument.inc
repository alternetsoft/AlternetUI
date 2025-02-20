// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

public:
bool GetOriginAtMargins();
void SetOriginAtMargins(bool value);

string GetDocumentName();
void SetDocumentName(const string& value);

PrinterSettings* GetPrinterSettings();

PageSettings* GetPageSettings();

DrawingContext* GetPrintPage_DrawingContext();

bool GetPrintPage_HasMorePages();
void SetPrintPage_HasMorePages(bool value);

RectD GetPrintPage_MarginBounds();

RectD GetPrintPage_PhysicalPageBounds();

RectD GetPrintPage_PageBounds();

RectD GetPrintPage_PrintablePageBounds();

int GetPrintPage_PageNumber();

void Print();

public:

enum class PrintDocumentEvent
{
    PrintPage,
    BeginPrint,
    EndPrint,
};

typedef void* (*PrintDocumentEventCallbackType)(PrintDocument* obj, PrintDocumentEvent event, void* param);

static void SetEventCallback(PrintDocumentEventCallbackType value) { eventCallback = value; }

protected:

bool RaiseEvent(PrintDocumentEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter) != nullptr;
else
   return false;
}

bool RaiseStaticEvent(PrintDocumentEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return false;
if (eventCallback != nullptr)
   return eventCallback(nullptr, event, parameter) != nullptr;
else
   return false;
}

void* RaiseEventWithPointerResult(PrintDocumentEvent event, void* parameter = nullptr)
{
if (EventsSuspended()) return nullptr;
if (eventCallback != nullptr)
   return eventCallback(this, event, parameter);
else
   return nullptr;
}

private:

inline static PrintDocumentEventCallbackType eventCallback = nullptr;

public:
PrintDocument();

protected:
virtual ~PrintDocument();

private:
BYREF_ONLY(PrintDocument);
