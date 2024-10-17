//---------------------------------------------------------------------------

#include <clx.h>
#pragma hdrstop

#include "MainFrm.h"
#include "EditFrm.h"
#include "AboutFrm.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.xfm"
TMainForm *MainForm;
AnsiString sUntitled = "Untitled";

//---------------------------------------------------------------------------
__fastcall TMainForm::TMainForm(TComponent* Owner)
        : TForm(Owner)
{
}

void __fastcall TMainForm::CreateMDIChild(AnsiString Name)
{
  TEditForm *Child;

  /* create a new MDI child window */
  Child = new TEditForm(Application);
  Child->Caption = Name;
  if (FileExists(Name))
  {
        Child->MemText->Lines->LoadFromFile(Name);
  }
}

//---------------------------------------------------------------------------
void __fastcall TMainForm::mnuNewClick(TObject *Sender)
{
  CreateMDIChild(sUntitled + IntToStr(MDIChildCount + 1));
}//---------------------------------------------------------------------------
void __fastcall TMainForm::mnuAboutClick(TObject *Sender)
{
  ShowAboutBox();
}//---------------------------------------------------------------------------
void __fastcall TMainForm::mnuExitClick(TObject *Sender)
{
  Close();
}//---------------------------------------------------------------------------
void __fastcall TMainForm::actnNewExecute(TObject *Sender)
{
  CreateMDIChild(sUntitled + IntToStr(MDIChildCount + 1));
}//---------------------------------------------------------------------------
void __fastcall TMainForm::actnOpenExecute(TObject *Sender)
{
  if (OpenDialog->Execute())
  {
    if (!ActiveMDIChild)
        CreateMDIChild(String(OpenDialog->FileName));
    else
        ActiveMDIChild->Caption = OpenDialog->FileName;

    if (ActiveMDIChild->ClassNameIs("TEditForm"))
      ((TEditForm *)ActiveMDIChild)->MemText->Lines->LoadFromFile(String(OpenDialog->FileName));
  }
}//---------------------------------------------------------------------------
void __fastcall TMainForm::actnSaveExecute(TObject *Sender)
{
  if (ActiveMDIChild)
  {
      SaveDialog->FileName = ActiveMDIChild->Caption;
      if (SaveDialog->Execute())
      {
          if (ActiveMDIChild->ClassNameIs("TEditForm"))
              ((TEditForm *)ActiveMDIChild)->MemText->Lines->SaveToFile(String(SaveDialog->FileName));

          ActiveMDIChild->Caption = SaveDialog->FileName;
      }
  }
}//---------------------------------------------------------------------------
void __fastcall TMainForm::actnExitExecute(TObject *Sender)
{
   Close();
}//---------------------------------------------------------------------------
void __fastcall TMainForm::actnWinCloseExecute(TObject *Sender)
{
  ActiveMDIChild->Close();
}//---------------------------------------------------------------------------
void __fastcall TMainForm::FormCreate(TObject *Sender)
{
  actnNewExecute(NULL);
}//---------------------------------------------------------------------------
