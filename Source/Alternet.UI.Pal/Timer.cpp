#include "Timer.h"

namespace Alternet::UI
{
	Timer::Timer()
	{
		_timer = new TimerImpl(this);
	}

	Timer::~Timer()
	{
		Stop();
		delete _timer;
		_timer = nullptr;
	}

	bool Timer::GetEnabled()
	{
		return _timer->IsRunning();
	}

	void Timer::SetEnabled(bool value)
	{
		if (value)
			Start();
		else
			Stop();
	}

	int Timer::GetInterval()
	{
		return _interval;
	}

	void Timer::SetInterval(int value)
	{
		if (_interval == value)
			return;

		_interval = value;
		
		if (_timer->IsRunning())
			Restart();
	}
	
	void Timer::OnTick()
	{
		RaiseEvent(TimerEvent::Tick);
	}

	void Timer::Start()
	{
		if (!_timer->IsRunning())
			_timer->Start(_interval);
	}

	void Timer::Restart()
	{
		Stop();
		Start();
	}

	void Timer::Stop()
	{
		if (_timer->IsRunning())
			_timer->Stop();
	}

	// TimerImpl ------------------

	Timer::TimerImpl::TimerImpl(Timer* owner) : _owner(owner)
	{
	}

	void Timer::TimerImpl::Notify()
	{
		_owner->OnTick();
	}
}
