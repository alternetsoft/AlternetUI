#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Timer : public Object
    {
#include "Api/Timer.inc"
    public:

    private:
        
        void OnTick();
        void Start();
        void Stop();

        class TimerImpl : public wxTimer
        {
        public:
            TimerImpl(Timer* owner);

            virtual void Notify() override;

        private:
            Timer* _owner;
        };

        int _interval = 100;
        bool _autoReset = true;
        TimerImpl* _timer = nullptr;
    };
}
