#pragma once

#include "CommonBase.h"

namespace Alternet::UI
{
    class DelayedValueBase
    {
    public:
        virtual void Apply() = 0;
        virtual void Receive() = 0;
    };

    template<typename TOwner, typename TValue>
    class DelayedValue : public DelayedValueBase
    {
    public:
        typedef void (TOwner::* TApplicator)(const TValue& value);
        typedef TValue (TOwner::* TRetriever)();
        typedef bool (TOwner::* TIsNotDelayedGetter)();

        DelayedValue(
            TOwner& owner,
            TValue initialValue,
            TIsNotDelayedGetter isNotDelayedGetter,
            TRetriever retriever,
            TApplicator applicator) :
            _owner(owner),
            _value(initialValue),
            _isNotDelayedGetter(isNotDelayedGetter),
            _retriever(retriever),
            _applicator(applicator)
        {
        }

        void Set(const TValue& value)
        {
            SetDelayed(value);
            if (IsNotDelayed())
                ApplyValue(value);
        }

        TValue Get()
        {
            if (IsNotDelayed())
                return RetrieveValue();
            else
                return GetDelayed();
        }

        bool IsNotDelayed()
        {
            return (_owner.*_isNotDelayedGetter)();
        }

        void Apply() override
        {
            ApplyValue(GetDelayed());
        }

        void Receive() override
        {
            SetDelayed(RetrieveValue());
        }

    private:
        BYREF_ONLY(DelayedValue);

        TValue _value;
        TOwner& _owner;

        TRetriever _retriever;
        TApplicator _applicator;
        TIsNotDelayedGetter _isNotDelayedGetter;

        void ApplyValue(TValue value)
        {
            (_owner.*_applicator)(value);
        }

        TValue RetrieveValue()
        {
            return (_owner.*_retriever)();
        }

        TValue GetDelayed()
        {
            return _value;
        }

        void SetDelayed(TValue value)
        {
            _value = value;
        }
    };

    template<typename TOwner, typename TFlags>
    class DelayedFlags : public DelayedValueBase
    {
    public:
        typedef void (TOwner::* TApplicator)(bool value);
        typedef bool (TOwner::* TRetriever)();
        typedef bool (TOwner::* TIsNotDelayedGetter)();

        typedef std::map<TFlags, std::tuple<TRetriever, TApplicator>> TApplicators;

        DelayedFlags(
            TOwner& owner,
            TFlags initialValue,
            TIsNotDelayedGetter isNotDelayedGetter,
            const TApplicators& applicators):
            _owner(owner),
            _flags(initialValue),
            _isNotDelayedGetter(isNotDelayedGetter),
            _applicators(applicators)
        {
        }

        void Set(TFlags flag, bool value)
        {
            if (IsNotDelayed())
                ApplyValue(flag, value);
            else
                SetDelayed(flag, value);
        }

        bool Get(TFlags flag)
        {
            if (IsNotDelayed())
                return RetrieveValue(flag);
            else
                return GetDelayed(flag);
        }

        bool IsNotDelayed()
        {
            return (_owner.*_isNotDelayedGetter)();
        }

        void Apply() override
        {
            for (auto& it : _applicators)
                ApplyValue(it.first, GetDelayed(it.first));
        }

        void Receive() override
        {
            for (auto& it : _applicators)
                SetDelayed(it.first, RetrieveValue(it.first));
        }

    private:
        BYREF_ONLY(DelayedFlags);

        TFlags _flags;
        TOwner& _owner;

        TApplicators _applicators;
        TIsNotDelayedGetter _isNotDelayedGetter;

        void ApplyValue(TFlags flag, bool value)
        {
            auto it = _applicators.find(flag);
            if (it == _applicators.end())
                throw std::exception();
            
            (_owner.*std::get<1>(it->second))(value);
        }

        bool RetrieveValue(TFlags flag)
        {
            auto it = _applicators.find(flag);
            if (it == _applicators.end())
                throw std::exception();

            return (_owner.*std::get<0>(it->second))();
        }

        bool GetDelayed(TFlags flag)
        {
            return (_flags & flag) == flag;
        }

        void SetDelayed(TFlags flag, bool value)
        {
            if (value)
                _flags |= flag;
            else
                _flags &= (~flag);
        }
    };

    class DelayedValues
    {
    public:
        DelayedValues(const std::vector<DelayedValueBase*>& values) : _values(values)
        {

        }

        void Add(DelayedValueBase* value)
        {
            _values.push_back(value);
        }

        void Apply()
        {
            for (auto& value : _values)
                value->Apply();
        }

        void Receive()
        {
            for (auto& value : _values)
                value->Receive();
        }

    private:
        BYREF_ONLY(DelayedValues);

        std::vector<DelayedValueBase*> _values;
    };
    
}