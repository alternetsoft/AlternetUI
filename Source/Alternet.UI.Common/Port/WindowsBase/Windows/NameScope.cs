#pragma warning disable
#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/***************************************************************************\
*
*
*  Used to store mapping information for names occuring 
*  within the logical tree section.
*
*
\***************************************************************************/

using System;
using System.Collections;
using System.Collections.Specialized;

using System.ComponentModel;
using System.Collections.Generic;
using Alternet.UI.Internal;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Used to store mapping information for names occuring 
    /// within the logical tree section.
    /// </summary>
    internal class NameScope : INameScopeDictionary
    {        
        #region INameScope
        
        /// <summary>
        /// Register Name-Object Map 
        /// </summary>
        /// <param name="name">name to be registered</param>
        /// <param name="scopedElement">object mapped to name</param>
        public void RegisterName(string name, object scopedElement)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (scopedElement == null)
                throw new ArgumentNullException("scopedElement");

            if (name == String.Empty)
                throw new ArgumentException(SR.Get(SRID.NameScopeNameNotEmptyString));

            if (!NameValidationHelper.IsValidIdentifierName(name))
            {
                throw new ArgumentException(SR.Get(SRID.NameScopeInvalidIdentifierName, name));
            }

            if (_nameMap == null)
            {
                _nameMap = new HybridDictionary();
                _nameMap[name] = scopedElement;
            }
            else
            {
                object nameContext = _nameMap[name];
                // first time adding the Name, set it
                if (nameContext == null)
                {
                    _nameMap[name] = scopedElement;
                }
                else if (scopedElement != nameContext)
                {
                    throw new ArgumentException(SR.Get(SRID.NameScopeDuplicateNamesNotAllowed, name));
                }   
            }

            if( TraceNameScope.IsEnabled )
            {
                TraceNameScope.TraceActivityItem( TraceNameScope.RegisterName,
                                                  this, 
                                                  name,
                                                  scopedElement );
            }
        }

        /// <summary>
        /// Unregister Name-Object Map 
        /// </summary>
        /// <param name="name">name to be registered</param>
        public void UnregisterName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (name == String.Empty)
                throw new ArgumentException(SR.Get(SRID.NameScopeNameNotEmptyString));

            if (_nameMap != null && _nameMap[name] != null)
            {
                _nameMap.Remove(name);
            }
            else
            {
                throw new ArgumentException(SR.Get(SRID.NameScopeNameNotFound, name));
            }

            if( TraceNameScope.IsEnabled )
            {
                TraceNameScope.TraceActivityItem( TraceNameScope.UnregisterName,
                                                  this, name );
            }
        }

        /// <summary>
        /// Find - Find the corresponding object given a Name
        /// </summary>
        /// <param name="name">Name for which context needs to be retrieved</param>
        /// <returns>corresponding Context if found, else null</returns>
        public object FindName(string name)
        {
            if (_nameMap == null || name == null || name == String.Empty)
                return null;

            return _nameMap[name];
        }

        #endregion INameScope

        #region InternalMethods
        
        internal static INameScope NameScopeFromObject(object obj)
        {
            INameScope nameScope = obj as INameScope;
            if (nameScope == null)
            {
                DependencyObject objAsDO = obj as DependencyObject;
                if (objAsDO != null)
                {
                    nameScope = GetNameScope(objAsDO);
                }
            }

            return nameScope;
        }

        #endregion InternalMethods

        #region DependencyProperties        

        /// <summary>
        /// NameScope property. This is an attached property. 
        /// This property allows the dynamic attachment of NameScopes
        /// </summary>
        public static readonly DependencyProperty NameScopeProperty 
                                                  = DependencyProperty.RegisterAttached( 
                                                  "NameScope", 
                                                  typeof(INameScope), 
                                                  typeof(NameScope));

        /// <summary>
        /// Helper for setting NameScope property on a DependencyObject. 
        /// </summary>
        /// <param name="dependencyObject">Dependency Object  to set NameScope property on.</param>
        /// <param name="value">NameScope property value.</param>
        public static void SetNameScope(DependencyObject dependencyObject, INameScope value)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            dependencyObject.SetValue(NameScopeProperty, value);
        }

        /// <summary>
        /// Helper for reading NameScope property from a DependencyObject.
        /// </summary>
        /// <param name="dependencyObject">DependencyObject to read NameScope property from.</param>
        /// <returns>NameScope property value.</returns>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static INameScope GetNameScope(DependencyObject dependencyObject)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException("dependencyObject");
            }

            return ((INameScope)dependencyObject.GetValue(NameScopeProperty));
        }

        #endregion DependencyProperties


        #region Data
        
        // This is a HybridDictionary of Name-Object maps
        private HybridDictionary _nameMap;

        #endregion Data

        IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return new Enumerator(this._nameMap);
        }

        #region IEnumerable methods
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region IEnumerable<KeyValuePair<string, object> methods
        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region ICollection<KeyValuePair<string, object> methods
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count
        {
            get
            {
                if (_nameMap == null)
                {
                    return 0;
                }
                return _nameMap.Count;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _nameMap = null;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            if (_nameMap == null)
            {
                array = null;
                return;
            }

            foreach (DictionaryEntry entry in _nameMap)
            {
                array[arrayIndex++] = new KeyValuePair<string, object>((string)entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Remove(KeyValuePair<string, object> item)
        {
            if (!Contains(item))
            {
                return false;
            }

            if (item.Value != this[item.Key])
            {
                return false;
            }
            return Remove(item.Key);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add(KeyValuePair<string, object> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentException(SR.Get(SRID.ReferenceIsNull, "item.Key"), "item");
            }
            if (item.Value == null)
            {
                throw new ArgumentException(SR.Get(SRID.ReferenceIsNull, "item.Value"), "item");
            }

            Add(item.Key, item.Value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Contains(KeyValuePair<string, object> item)
        {
            if (item.Key == null)
            {
                throw new ArgumentException(SR.Get(SRID.ReferenceIsNull, "item.Key"), "item");
            }
            return ContainsKey(item.Key);
        }
        #endregion

        #region IDictionary<string, object> methods
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object this[string key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                return FindName(key);
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                RegisterName(key, value);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            RegisterName(key, value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool ContainsKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            object value = FindName(key);
            return (value != null);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Remove(string key)
        {
            if (!ContainsKey(key))
            {
                return false;
            }
            UnregisterName(key);
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool TryGetValue(string key, out object value)
        {
            if (!ContainsKey(key))
            {
                value = null;
                return false;
            }
            value = FindName(key);
            return true;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                if (_nameMap == null)
                {
                    return null;
                }

                var list = new List<string>();
                foreach (string key in _nameMap.Keys)
                {
                    list.Add(key);
                }
                return list;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<object> Values
        {
            get
            {
                if (_nameMap == null)
                {
                    return null;
                }

                var list = new List<object>();
                foreach (object value in _nameMap.Values)
                {
                    list.Add(value);
                }
                return list;
            }
        }
        #endregion

        #region class Enumerator
        class Enumerator : IEnumerator<KeyValuePair<string, object>>
        {
            IDictionaryEnumerator _enumerator;
            
            public Enumerator(HybridDictionary nameMap)
            {
                _enumerator = null;

                if (nameMap != null)
                {
                    _enumerator = nameMap.GetEnumerator();
                }
            }

            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            public KeyValuePair<string, object> Current
            {
                get
                {
                    if (_enumerator == null)
                    {
                        return default(KeyValuePair<string, object>);
                    }
                    return new KeyValuePair<string, object>((string)_enumerator.Key, _enumerator.Value);
                }
            }

            public bool MoveNext()
            {
                if (_enumerator == null)
                {
                    return false;
                }
                return _enumerator.MoveNext();
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (_enumerator != null)
                {
                    _enumerator.Reset();
                }
            }
        }
        #endregion
    }
}
