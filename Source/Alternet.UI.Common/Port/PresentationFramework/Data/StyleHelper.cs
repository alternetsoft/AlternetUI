#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/***************************************************************************\
*
*
*  Style and templating data structures and helper methods.
*
*
\***************************************************************************/

// Disabling 1634 and 1691:
// In order to avoid generating warnings about unknown message numbers and
// unknown pragmas when compiling C# source code with the C# compiler,
// you need to disable warnings 1634 and 1691. (Presharp Documentation)
#pragma warning disable 1634, 1691

namespace Alternet.UI.Port
{
    internal static class StyleHelper
    {
        internal static void RegisterAlternateExpressionStorage()
        {
            DependencyObject.RegisterForAlternativeExpressionStorage(
                                new AlternativeExpressionStorageCallback(GetExpressionCore),
                                out _getExpression);
        }

        private static Expression GetExpressionCore(
            DependencyObject d,
            DependencyProperty dp,
            PropertyMetadata metadata)
        {
            FrameworkElement fe;
            //FrameworkContentElement fce;
            Helper.DowncastToFEorFCE(d, out fe, /*out fce,*/ false);

            if (fe != null)
            {
                return fe.GetExpressionCore(dp, metadata);
            }

            //if (fce != null)
            //{
            //    return fce.GetExpressionCore(dp, metadata);
            //}

            return null;
        }

        //
        //  This method
        //  1. Is a wrapper for property engine's GetExpression method
        //
        internal static Expression GetExpression(
            DependencyObject d,
            DependencyProperty dp)
        {
            FrameworkElement fe;
            //FrameworkContentElement fce;
            Helper.DowncastToFEorFCE(d, out fe, /*out fce,*/ false);

            // temporarily mark the element as "initialized", so that we always get
            // the desired expression (see GetInstanceValue).
            bool isInitialized = (fe != null) ? fe.IsInitialized : /*(fce != null) ? fce.IsInitialized :*/ true;
            if (!isInitialized)
            {
                if (fe != null)
                    fe.WriteInternalFlag(InternalFlags.IsInitialized, true);
                //else if (fce != null)
                //    fce.WriteInternalFlag(InternalFlags.IsInitialized, true);
            }

            // get the desired expression
            Expression result = _getExpression(d, dp, dp.GetMetadata(d.DependencyObjectType));

            // restore the initialized flag
            if (!isInitialized)
            {
                if (fe != null)
                    fe.WriteInternalFlag(InternalFlags.IsInitialized, false);
                //else if (fce != null)
                //    fce.WriteInternalFlag(InternalFlags.IsInitialized, false);
            }

            return result;
        }

        //  The property engine's API for the "alternative Expression storage"
        //  feature are set in the static ctor.
        private static AlternativeExpressionStorageCallback _getExpression;

    }
}


