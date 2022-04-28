using System;
using System.Collections.Generic;

#pragma warning disable CS1591 // Enable me later

namespace Alternet.UI
{
    internal class UixmlPortLocator : IUixmlPortDependencyResolver
    {
        private readonly IUixmlPortDependencyResolver? _parentScope;
        public static IUixmlPortDependencyResolver Current { get; set; }
        public static UixmlPortLocator CurrentMutable { get; set; }
        private readonly Dictionary<Type, Func<object?>> _registry = new Dictionary<Type, Func<object?>>();

        static UixmlPortLocator()
        {
            Current = CurrentMutable = new UixmlPortLocator();
        }

        public UixmlPortLocator()
        {
            
        }

        public UixmlPortLocator(IUixmlPortDependencyResolver parentScope)
        {
            _parentScope = parentScope;
        }

        public object? GetService(Type t)
        {
            return _registry.TryGetValue(t, out var rv) ? rv() : _parentScope?.GetService(t);
        }

        internal class RegistrationHelper<TService>
        {
            private readonly UixmlPortLocator _locator;

            public RegistrationHelper(UixmlPortLocator locator)
            {
                _locator = locator;
            }

            public UixmlPortLocator ToConstant<TImpl>(TImpl constant) where TImpl : TService
            {
                _locator._registry[typeof (TService)] = () => constant;
                return _locator;
            }

            public UixmlPortLocator ToFunc<TImlp>(Func<TImlp> func) where TImlp : TService
            {
                _locator._registry[typeof (TService)] = () => func();
                return _locator;
            }

            public UixmlPortLocator ToLazy<TImlp>(Func<TImlp> func) where TImlp : TService
            {
                var constructed = false;
                TImlp? instance = default;
                _locator._registry[typeof (TService)] = () =>
                {
                    if (!constructed)
                    {
                        instance = func();
                        constructed = true;
                    }

                    return instance;
                };
                return _locator;
            }
            
            public UixmlPortLocator ToSingleton<TImpl>() where TImpl : class, TService, new()
            {
                TImpl? instance = null;
                return ToFunc(() => instance ?? (instance = new TImpl()));
            }

            public UixmlPortLocator ToTransient<TImpl>() where TImpl : class, TService, new() => ToFunc(() => new TImpl());
        }

        public RegistrationHelper<T> Bind<T>() => new RegistrationHelper<T>(this);


        public UixmlPortLocator BindToSelf<T>(T constant)
            => Bind<T>().ToConstant(constant);

        public UixmlPortLocator BindToSelfSingleton<T>() where T : class, new() => Bind<T>().ToSingleton<T>();

        class ResolverDisposable : IDisposable
        {
            private readonly IUixmlPortDependencyResolver _resolver;
            private readonly UixmlPortLocator _mutable;

            public ResolverDisposable(IUixmlPortDependencyResolver resolver, UixmlPortLocator mutable)
            {
                _resolver = resolver;
                _mutable = mutable;
            }

            public void Dispose()
            {
                Current = _resolver;
                CurrentMutable = _mutable;
            }
        }


        public static IDisposable EnterScope()
        {
            var d = new ResolverDisposable(Current, CurrentMutable);
            Current = CurrentMutable =  new UixmlPortLocator(Current);
            return d;
        }
    }

    internal interface IUixmlPortDependencyResolver
    {
        object? GetService(Type t);
    }

    internal static class LocatorExtensions
    {
        public static T? GetService<T>(this IUixmlPortDependencyResolver resolver)
        {
            return (T?) resolver.GetService(typeof (T));
        }

        public static object GetRequiredService(this IUixmlPortDependencyResolver resolver, Type t)
        {
            return resolver.GetService(t) ?? throw new InvalidOperationException($"Unable to locate '{t}'.");
        }

        public static T GetRequiredService<T>(this IUixmlPortDependencyResolver resolver)
        {
            return (T?)resolver.GetService(typeof(T)) ?? throw new InvalidOperationException($"Unable to locate '{typeof(T)}'.");
        }
    }
}

