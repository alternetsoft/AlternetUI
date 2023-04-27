#pragma once

#include <vector>
#include <set>
#include <algorithm>
#include <iostream>
#include <cmath>
#include <functional>
#include <assert.h>

#include <string>
#include <functional>
#include <memory>
#include <map>
#include <stack>
#include <utility>
#include <locale>
#include <tuple>
#include <codecvt>

#include "OptionalInclude.h"

#ifdef __WXMSW__
#include <sstream>
#endif

#include "TypedEnumFlags.h"

#include <wx/wxprec.h>
#ifndef WX_PRECOMP
#include <wx/wx.h>
#endif

#include <wx/dcbuffer.h>
#include <wx/filename.h>
#include <wx/stdpaths.h>
#include <wx/notebook.h>
#include <wx/spinctrl.h>
#include <wx/listctrl.h>
#include <wx/treectrl.h>
#include <wx/fontenum.h>
#include <wx/graphics.h>
#include <wx/evtloop.h>
#include <wx/clipbrd.h>
#include <wx/mstream.h>
#include <wx/tokenzr.h>
#include <wx/dnd.h>
#include <wx/print.h>
#include <wx/printdlg.h>
#include <wx/taskbar.h>
#include <wx/colordlg.h>
#include <wx/clrpicker.h>
#include <wx/dateevt.h>
#include <wx/datectrl.h>
#include <wx/datetimectrl.h>
#include <wx/popupwin.h>
#include <wx/webview.h>

#define DELELTE_COPY_CONSTRUCTOR(TypeName) \
  TypeName(const TypeName&) = delete

#define DELETE_ASSIGNMENT_OPERATOR(TypeName) TypeName& operator=(const TypeName&) = delete

#define BYREF_ONLY(TypeName) \
  DELELTE_COPY_CONSTRUCTOR(TypeName);                 \
  DELETE_ASSIGNMENT_OPERATOR(TypeName)

#define verify(condition) if (!condition) throw std::exception();

#define verifyNonNull(value) if (value == nullptr) throw std::exception();
