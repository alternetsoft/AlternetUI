// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

#pragma once

#include "PrintPreviewDialog.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API PrintPreviewDialog* PrintPreviewDialog_Create_()
{
    return MarshalExceptions<PrintPreviewDialog*>([&](){
            return new PrintPreviewDialog();
        });
}

