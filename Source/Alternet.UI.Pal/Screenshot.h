#pragma once
#include "Common.h"


namespace Alternet::UI
{
    int SaveScreenshot(wxWindow* xWindow, string fileName)
    {
        if (!wxStr(fileName).MakeLower().EndsWith(".bmp"))
            throwEx(u"Only .bmp file format is supported for screenshots.");

#ifndef __WXMSW__
        throwEx(u"Only Widows OS is supported for screenshots.");
#else
        // Source: https://docs.microsoft.com/en-us/windows/win32/gdi/capturing-an-image?redirectedfrom=MSDN

        bool hasChildWindows = false;
        wxWindowList& children = xWindow->GetChildren();

        for (wxWindowList::compatibility_iterator node = children.GetFirst();
            node; node = node->GetNext())
        {
            wxWindow* current = (wxWindow*)node->GetData();

            hasChildWindows = true;
        }

        HWND hWnd = xWindow->GetHWND();
        HDC hdcScreen;
        HDC hdcWindow;
        HDC hdcMemDC = NULL;
        HBITMAP hbmScreen = NULL;
        BITMAP bmpScreen;
        DWORD dwBytesWritten = 0;
        DWORD dwSizeofDIB = 0;
        HANDLE hFile = NULL;
        char* lpbitmap = NULL;
        HANDLE hDIB = NULL;
        DWORD dwBmpSize = 0;

        // Retrieve the handle to a display device context for the client 
        // area of the window. 
        hdcScreen = GetDC(NULL);
        hdcWindow = GetDC(hWnd);

        // Create a compatible DC, which is used in a BitBlt from the window DC.
        hdcMemDC = CreateCompatibleDC(hdcWindow);

        if (!hdcMemDC)
            throwEx(u"CreateCompatibleDC has failed");

        // Get the client area for size calculation.
        RECT rcClient;
        GetClientRect(hWnd, &rcClient);

        // This is the best stretch mode.
        SetStretchBltMode(hdcWindow, HALFTONE);

        auto rectWidth = rcClient.right - rcClient.left;
        auto rectHeight = rcClient.bottom - rcClient.top;

        /*
        if (hasChildWindows) 
        {
            // The source DC is the entire screen, and the destination DC is the current window (HWND).
            if (!StretchBlt(hdcWindow,
                0, 0,
                rcClient.right, rcClient.bottom,
                hdcScreen,
                0, 0,
                GetSystemMetrics(SM_CXSCREEN),
                GetSystemMetrics(SM_CYSCREEN),
                SRCCOPY))
            {
                throwEx(u"StretchBlt has failed");
            }
        }
        else 
        {
        }
        */
        
        // Create a compatible bitmap from the Window DC.
        hbmScreen = CreateCompatibleBitmap(hdcWindow, rectWidth, rectHeight);

        if (!hbmScreen)
        {
            throwEx(u"CreateCompatibleBitmap Failed");
        }

        // Select the compatible bitmap into the compatible memory DC.
        SelectObject(hdcMemDC, hbmScreen);

        if (hasChildWindows) 
        {
            // Bit block transfer into our compatible memory DC.
            if (!BitBlt(hdcMemDC,
                0, 0,
                rectWidth, rectHeight,
                hdcWindow,
                rcClient.left, rcClient.top,
                SRCCOPY))
            {
                throwEx(u"BitBlt has failed");
            }
        }
        else 
        {
            RECT rcBmp;
            SetRect(&rcBmp, 0, 0, rectWidth, rectHeight);
            HBRUSH backgroundBrush = CreateSolidBrush(GetSysColor(COLOR_BTNFACE));
            HGDIOBJ oldBrush = SelectObject(hdcMemDC, backgroundBrush);
            FillRect(hdcMemDC, &rcBmp, backgroundBrush);
            SelectObject(hdcMemDC, oldBrush);
            DeleteObject(backgroundBrush);
        }

        // Get the BITMAP from the HBITMAP.
        GetObject(hbmScreen, sizeof(BITMAP), &bmpScreen);

        BITMAPFILEHEADER   bmfHeader;
        BITMAPINFOHEADER   bi;

        bi.biSize = sizeof(BITMAPINFOHEADER);
        bi.biWidth = bmpScreen.bmWidth;
        bi.biHeight = bmpScreen.bmHeight;
        bi.biPlanes = 1;
        bi.biBitCount = 32;
        bi.biCompression = BI_RGB;
        bi.biSizeImage = 0;
        bi.biXPelsPerMeter = 0;
        bi.biYPelsPerMeter = 0;
        bi.biClrUsed = 0;
        bi.biClrImportant = 0;

        dwBmpSize = ((bmpScreen.bmWidth * bi.biBitCount + 31) / 32) * 4 * bmpScreen.bmHeight;

        // Starting with 32-bit Windows, GlobalAlloc and LocalAlloc are implemented as wrapper functions that 
        // call HeapAlloc using a handle to the process's default heap. Therefore, GlobalAlloc and LocalAlloc 
        // have greater overhead than HeapAlloc.
        hDIB = GlobalAlloc(GHND, dwBmpSize);
        if (hDIB == NULL)
            throwEx(u"GlobalAlloc has failed");

        lpbitmap = (char*)GlobalLock(hDIB);

        // Gets the "bits" from the bitmap, and copies them into a buffer 
        // that's pointed to by lpbitmap.
        GetDIBits(hdcWindow, hbmScreen, 0,
            (UINT)bmpScreen.bmHeight,
            lpbitmap,
            (BITMAPINFO*)&bi, DIB_RGB_COLORS);

        // A file is created, this is where we will save the screen capture.
        hFile = CreateFile((LPCWSTR)fileName.c_str(),
            GENERIC_WRITE,
            0,
            NULL,
            CREATE_ALWAYS,
            FILE_ATTRIBUTE_NORMAL, NULL);

        // Add the size of the headers to the size of the bitmap to get the total file size.
        dwSizeofDIB = dwBmpSize + sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

        // Offset to where the actual bitmap bits start.
        bmfHeader.bfOffBits = (DWORD)sizeof(BITMAPFILEHEADER) + (DWORD)sizeof(BITMAPINFOHEADER);

        // Size of the file.
        bmfHeader.bfSize = dwSizeofDIB;

        // bfType must always be BM for Bitmaps.
        bmfHeader.bfType = 0x4D42; // BM.

        WriteFile(hFile, (LPSTR)&bmfHeader, sizeof(BITMAPFILEHEADER), &dwBytesWritten, NULL);
        WriteFile(hFile, (LPSTR)&bi, sizeof(BITMAPINFOHEADER), &dwBytesWritten, NULL);
        WriteFile(hFile, (LPSTR)lpbitmap, dwBmpSize, &dwBytesWritten, NULL);

        // Unlock and Free the DIB from the heap.
        GlobalUnlock(hDIB);
        GlobalFree(hDIB);

        // Close the handle for the file that was created.
        CloseHandle(hFile);

        // Clean up.
        if (hbmScreen != NULL)
            DeleteObject(hbmScreen);
        
        if (hdcMemDC != NULL)
            DeleteObject(hdcMemDC);
        
        ReleaseDC(NULL, hdcScreen);
        ReleaseDC(hWnd, hdcWindow);

        return 0;
#endif
    }
}
