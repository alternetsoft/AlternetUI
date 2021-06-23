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
#include <optional>

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

#define DELELTE_COPY_CONSTRUCTOR(TypeName) \
  TypeName(const TypeName&) = delete

#define DELETE_ASSIGNMENT_OPERATOR(TypeName) TypeName& operator=(const TypeName&) = delete

#define BYREF_ONLY(TypeName) \
  DELELTE_COPY_CONSTRUCTOR(TypeName);                 \
  DELETE_ASSIGNMENT_OPERATOR(TypeName)

#define verify(condition) if (!condition) throw std::exception();

#define verifyNonNull(value) if (value == nullptr) throw std::exception();