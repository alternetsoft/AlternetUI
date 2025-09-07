#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/***************************************************************************\
*
*
*  ServiceProvider class that is passed into MarkupExtensions.ProvideValue method
*  that understands the ParserContext.
*
*
\***************************************************************************/
using System;

namespace Alternet.UI.Port
{
    /// <summary>
    ///  Base class for all Xaml markup extensions.
    /// </summary>

    internal class ProvideValueServiceProvider
        : IServiceProvider, IProvideValueTarget, IXamlTypeResolver, IUriContext, IFreezeFreezables
    {
        // Construction
        
        /* yezo: Xaml
        internal ProvideValueServiceProvider(ParserContext context)
        {
            _context = context;
        }*/

        internal ProvideValueServiceProvider()
        {
        }

        // Set the TargetObject/Property (for use by IProvideValueTarget).
        
        internal void SetData(object targetObject, object targetProperty)
        {
            _targetObject = targetObject;
            _targetProperty = targetProperty;
        }

        // Clear the TargetObject/Property (after a call to ProvideValue)
        
        internal void ClearData()
        {
            _targetObject = _targetProperty = null;
        }


        // IXamlTypeResolver implementation
        
        Type IXamlTypeResolver.Resolve(string qualifiedTypeName) // E.g. foo:Class
        {
            /* yezo: Xaml
            return _context.XamlTypeMapper.GetTypeFromBaseString(qualifiedTypeName, _context, true);
            */
            return null;
        }

        // IProvideValueTarget implementation
        
        object IProvideValueTarget.TargetObject
        {
            get { return _targetObject; }
        }
        object IProvideValueTarget.TargetProperty
        {
            get { return _targetProperty; }
        }

        // IUriContext implementation
        
        Uri IUriContext.BaseUri
        {
            get
            { /* yezo: Xaml return _context.BaseUri; */return null; }
            set { throw new NotSupportedException(SR.Get(SRID.ParserProvideValueCantSetUri)); }
        }

        bool IFreezeFreezables.FreezeFreezables
        {
            get
            {
                /* yezo: Xaml 
                return _context.FreezeFreezables;
                */
                return false;
            }
        }

        bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
        {
            /* yezo: Xaml 
            return _context.TryCacheFreezable(value, freezable);
            */
            return false;
        }

        Freezable IFreezeFreezables.TryGetFreezable(string value)
        {
            /* yezo: Xaml 
            return _context.TryGetFreezable(value);
            */
            return null;
        }


        // IServiceProvider implementation (this is the way to get to the
        // above interface implementations).
        
        public object GetService(Type service)
        {
            // IProvideValueTarget is the only implementation that
            // doesn't need the ParserContext
            
            if( service == typeof(IProvideValueTarget))
            {
                return this as IProvideValueTarget;
            }

            /* yezo: Xaml 
            if( _context != null )
            {
                if( service == typeof(IXamlTypeResolver))
                {
                    return this as IXamlTypeResolver;
                }

                else if( service == typeof(IUriContext))
                {
                    return this as IUriContext;
                }

                else if (service == typeof(IFreezeFreezables))
                {
                    return this as IFreezeFreezables;
                }
            }*/


            return null;
        }


        // Data

        /* yezo: Xaml private ParserContext _context = null; */
        private object _targetObject = null;
        private object _targetProperty = null;
}
}

