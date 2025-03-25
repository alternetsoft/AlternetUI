#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Validator.h"

#include <wx/valtext.h>

namespace Alternet::UI
{
    class ValidatorText : public Validator
    {
#include "Api/ValidatorText.inc"
    public:
        static wxTextValidator* ToTextValidator(void* handle);
    private:
    
    };
}
