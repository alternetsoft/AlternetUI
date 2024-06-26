#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Purpose:  Helper functions that require elevation but are safe to use.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Alternet.UI.Port
{
    internal static partial class SafeSecurityHelper
    {
#if PRESENTATION_CORE
        ///<summary>
        /// Given a rectangle with coords in local screen coordinates.
        /// Return the rectangle in global screen coordinates.
        ///</summary>
        internal static void TransformLocalRectToScreen(HandleRef hwnd, ref NativeMethods.RECT rcWindowCoords)
        {
            int retval = MS.Internal.WindowsBase.NativeMethodsSetLastError.MapWindowPoints(hwnd , new HandleRef(null, IntPtr.Zero), ref rcWindowCoords, 2);
            int win32Err = Marshal.GetLastWin32Error();
            if(retval == 0 && win32Err != 0)
            {
                throw new System.ComponentModel.Win32Exception(win32Err);
            }
        }
#endif

        /// <summary>
        ///     Given an assembly, returns the partial name of the assembly.
        /// </summary>
        internal static string GetAssemblyPartialName(Assembly assembly)
        {
            AssemblyName name = new AssemblyName(assembly.FullName);
            string partialName = name.Name;
            return (partialName != null) ? partialName : string.Empty;
        }

#if PRESENTATIONFRAMEWORK

        /// <summary>
        ///     Get the full assembly name by combining the partial name passed in
        ///     with everything else from proto assembly.
        /// </summary>
        internal static string GetFullAssemblyNameFromPartialName(
                                    Assembly protoAssembly,
                                    string partialName)
        {
            AssemblyName name = new AssemblyName(protoAssembly.FullName);
            name.Name = partialName;
            return name.FullName;
        }

        internal static Point ClientToScreen(UIElement relativeTo, Point point)
        {
            GeneralTransform transform;
            PresentationSource source = PresentationSource.CriticalFromVisual(relativeTo);

            if (source == null)
            {
                return new Point(double.NaN, double.NaN);
            }
            transform = relativeTo.TransformToAncestor(source.RootVisual);
            Point ptRoot;
            transform.TryTransform(point, out ptRoot);
            Point ptClient = PointUtil.RootToClient(ptRoot, source);
            Point ptScreen = PointUtil.ClientToScreen(ptClient, source);

            return ptScreen;
        }
#endif // PRESENTATIONFRAMEWORK

        // Cache of Assembly -> AssemblyName, because calling new AssemblyName() is expensive.
        // If the assembly is static, the key is the assembly; if it's dynamic, the key is a WeakRefKey
        // pointing to the assembly, so we don't root collectible assemblies.
        //
        // This cache is bound (gated) by the number of assemblies in the appdomain.
        // We use a callback on GC to purge out collected assemblies, so we don't grow indefinitely.
        //
        static Dictionary<object, AssemblyName> _assemblies; // get key via GetKeyForAssembly
        static object syncObject = new object();
        static bool _isGCCallbackPending;

        // PERF: Cache delegate for CleanupCollectedAssemblies to avoid allocating it each time.
        static readonly WaitCallback _cleanupCollectedAssemblies = CleanupCollectedAssemblies;

        /// <summary>
        ///     This function iterates through the assemblies loaded in the current
        ///     AppDomain and finds one that has the same assembly name passed in.
        /// </summary>
        internal static Assembly GetLoadedAssembly(AssemblyName assemblyName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Version reqVersion = assemblyName.Version;
            CultureInfo reqCulture = assemblyName.CultureInfo;
            byte[] reqKeyToken = assemblyName.GetPublicKeyToken();

            for (int i = assemblies.Length - 1; i >= 0; i--)
            {
                AssemblyName curAsmName = GetAssemblyName(assemblies[i]);
                Version curVersion = curAsmName.Version;
                CultureInfo curCulture = curAsmName.CultureInfo;
                byte[] curKeyToken = curAsmName.GetPublicKeyToken();

                if ( (String.Compare(curAsmName.Name, assemblyName.Name, true, TypeConverterHelper.InvariantEnglishUS) == 0) &&
                     (reqVersion == null || reqVersion.Equals(curVersion)) &&
                     (reqCulture == null || reqCulture.Equals(curCulture)) &&
                     (reqKeyToken == null || IsSameKeyToken(reqKeyToken, curKeyToken) ) )
                {
                    return assemblies[i];
                }
            }
            return null;
        }

        static AssemblyName GetAssemblyName(Assembly assembly)
        {
            object key = assembly.IsDynamic ? (object)new WeakRefKey(assembly) : assembly;
            lock (syncObject)
            {
                AssemblyName result;
                if (_assemblies == null)
                {
                    _assemblies = new Dictionary<object, AssemblyName>();
                }
                else
	            {
                    if (_assemblies.TryGetValue(key, out result))
                    {
                        return result;
                    }
	            }
                //
                // We use AssemblyName ctor here because GetName demands FileIOPermission
                // and does load more than just the required information.
                // Essentially we use AssemblyName just to help parsing the name, version, culture
                // and public key token from the assembly's name.
                //
                result = new AssemblyName(assembly.FullName);
                _assemblies.Add(key, result);
                if (assembly.IsDynamic && !_isGCCallbackPending)
                {
                    // Make sure we clean up the cache if/when this dynamic assembly is GCed
                    GCNotificationToken.RegisterCallback(_cleanupCollectedAssemblies, null);
                    _isGCCallbackPending  = true;
                }
                return result;
            }
        }

        // After a GC, clean up the weakrefs to any collected dynamic assemblies
        static void CleanupCollectedAssemblies(object state) // dummy parameter required by WaitCallback definition
        {
            bool foundLiveDynamicAssemblies = false;
            List<object> keysToRemove = null;
            lock (syncObject)
            {
                foreach (object key in _assemblies.Keys)
                {
                    WeakReference weakRef = key as WeakReference;
                    if (weakRef == null)
                    {
                        continue;
                    }
                    if (weakRef.IsAlive)
                    {
                        // There is a weak ref that is still alive, register another GC callback for next time
                        foundLiveDynamicAssemblies = true;
                    }
                    else
                    {
                        // The target has been collected, add it to our list of keys to remove
                        if (keysToRemove == null)
                        {
                            keysToRemove = new List<object>();
                        }
                        keysToRemove.Add(key);
                    }
                }
                if (keysToRemove != null)
                {
                    foreach (object key in keysToRemove)
                    {
                        _assemblies.Remove(key);
                    }
                }
                if (foundLiveDynamicAssemblies)
                {
                    GCNotificationToken.RegisterCallback(_cleanupCollectedAssemblies, null);
                }
                else
                {
                    _isGCCallbackPending = false;
                }
            }
        }

        //
        // Determine if two Public Key Tokens are the same.
        //
        internal static bool IsSameKeyToken(byte[] reqKeyToken, byte[] curKeyToken)
        {
           bool isSame = false;

           if (reqKeyToken == null && curKeyToken == null)
           {
               // Both Key Tokens are not set, treat them as same.
               isSame = true;
           }
           else if (reqKeyToken != null && curKeyToken != null)
           {
               // Both KeyTokens are set.
               if (reqKeyToken.Length == curKeyToken.Length)
               {
                   isSame = true;

                   for (int i = 0; i < reqKeyToken.Length; i++)
                   {
                      if (reqKeyToken[i] != curKeyToken[i])
                      {
                         isSame = false;
                         break;
                      }
                   }
               }
           }

           return isSame;
        }

#if PRESENTATION_CORE || PRESENTATIONFRAMEWORK
        // enum to choose between the various keys
        internal enum KeyToRead
        {
             WebBrowserDisable = 0x01 ,
             MediaAudioDisable = 0x02 ,
             MediaVideoDisable = 0x04 ,
             MediaImageDisable = 0x08 ,
             MediaAudioOrVideoDisable = KeyToRead.MediaVideoDisable | KeyToRead.MediaAudioDisable  ,
             ScriptInteropDisable = 0x10 ,
        }

        internal static bool IsFeatureDisabled(KeyToRead key)
        {
            string regValue = null;
            bool fResult = false;

            switch (key)
            {
                case KeyToRead.WebBrowserDisable:
                    regValue = RegistryKeys.value_WebBrowserDisallow;
                    break;
                case KeyToRead.MediaAudioDisable:
                    regValue = RegistryKeys.value_MediaAudioDisallow;
                    break;
                case KeyToRead.MediaVideoDisable:
                    regValue = RegistryKeys.value_MediaVideoDisallow;
                    break;
                case KeyToRead.MediaImageDisable:
                    regValue = RegistryKeys.value_MediaImageDisallow;
                    break;
                case KeyToRead.MediaAudioOrVideoDisable:
                    regValue = RegistryKeys.value_MediaAudioDisallow;
                    break;
                case KeyToRead.ScriptInteropDisable:
                    regValue = RegistryKeys.value_ScriptInteropDisallow;
                    break;
                default:// throw exception for invalid key
                throw(new System.ArgumentException(key.ToString()));

            }

            RegistryKey featureKey;
            object obj = null;
            bool keyValue = false;
            // open the key and read the value
            featureKey = Registry.LocalMachine.OpenSubKey(RegistryKeys.Alternet UI_Features);
            if (featureKey != null)
            {
                // If key exists and value is 1 return true else false
                obj = featureKey.GetValue(regValue);
                keyValue = obj is int && ((int)obj == 1);
                if (keyValue)
                {
                    fResult = true;
                }

                // special case for audio and video since they can be orred
                // this is in the condition that audio is enabled since that is
                // the path that MediaAudioVideoDisable defaults to
                // This is purely to optimize perf on the number of calls to assert
                // in the media or audio scenario.

                if ((fResult == false) && (key == KeyToRead.MediaAudioOrVideoDisable))
                {
                    regValue = RegistryKeys.value_MediaVideoDisallow;
                    // If key exists and value is 1 return true else false
                    obj = featureKey.GetValue(regValue);
                    keyValue = obj is int && ((int)obj == 1);
                    if (keyValue)
                    {
                        fResult = true;
                    }
                }
            }
            return fResult;
        }
#endif //PRESENTATIONCORE||PRESENTATIONFRAMEWORK

#if PRESENTATION_CORE

        /// <summary>
        ///     This function is a wrapper for CultureInfo.GetCultureInfoByIetfLanguageTag().
        ///     The wrapper works around a bug in that routine, which causes it to throw
        ///     a SecurityException in Partial Trust.
        /// </summary>
        static internal CultureInfo GetCultureInfoByIetfLanguageTag(string languageTag)
        {
            return CultureInfo.GetCultureInfoByIetfLanguageTag(languageTag);
        }
#endif //PRESENTATIONCORE

        internal const string IMAGE = "image";
    }

    // This cleanup token will be immediately thrown away and as a result it will
    // (a couple of GCs later) make it into the finalization queue and when finalized
    // will kick off a thread-pool job that you can use to purge a weakref cache.
    class GCNotificationToken
    {
        WaitCallback callback;
        object state;

        GCNotificationToken(WaitCallback callback, object state)
        {
            this.callback = callback;
            this.state = state;
        }

        ~GCNotificationToken()
        {
            // Schedule cleanup
            ThreadPool.QueueUserWorkItem(callback, state);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Justification = "See comment above")]
        internal static void RegisterCallback(WaitCallback callback, object state)
        {
            new GCNotificationToken(callback, state);
        }
    }
}
