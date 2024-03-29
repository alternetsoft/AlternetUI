#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class ProgressBar : public Control
    {
#include "Api/ProgressBar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

    protected:
        virtual void OnWxWindowCreated() override;

    private:
        wxGauge* GetGauge();

        DelayedValue<ProgressBar, int> _maximum;
        int _minimum = 0;

        void ApplyValue();

        int RetrieveMaximum();
        void ApplyMaximum(const int& value);

        class IndeterminedModeTimer : public wxTimer
        {
        public:
            IndeterminedModeTimer(ProgressBar* owner);

            virtual void Notify() override;

        private:
            ProgressBar* _owner;
        };

        IndeterminedModeTimer* _indeterminedModeTimer = nullptr;

        bool _isIndeterminate = false;

        ProgressBarOrientation _orientation = ProgressBarOrientation::Horizontal;

        long GetStyle();

        void ApplyIndeterminate();
        int _value = 0;
    };
}
