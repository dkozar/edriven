#region License

/*
 
Copyright (c) 2010-2014 Danko Kozar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 
*/

#endregion License

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace eDriven.Core.Mapper
{
    /// <summary>
    /// A class used for lazy mapping string-to-something
    /// </summary>
    public class PowerMapper : MonoBehaviour, IPowerMapper
    {
#if DEBUG
        // ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;
        // ReSharper restore UnassignedField.Global
#endif

        /// <summary>
        /// Mapper type
        /// </summary>
        public static Type MapperType;

        private static IPowerMapper _defaultMapper;
        private static Dictionary<string, IPowerMapper> _mappers;

        /// <summary>
        /// Truw if this is the default mapper
        /// </summary>
        public bool Default;

        /// <summary>
        /// Mapper ID
        /// </summary>
        public string Id;

        /// <summary>
        /// Constructor
        /// </summary>
        protected PowerMapper()
        {
            // constructor is protected
        }

        [Obfuscation(Exclude = true)]
        void Start()
        {
            if (!InitializedTypes.Contains(MapperType))
                Initialize();

            if (!Default && string.IsNullOrEmpty(Id))
                throw new Exception("Font mapper error: Id not set for a non-default font mapper");

            if (!_mappers.ContainsKey(Id))
                _mappers.Add(Id, this);
        }

        private static readonly List<Type> InitializedTypes = new List<Type>();

        private static void Initialize()
        {
#if DEBUG
            if (DebugMode)
            {
                Debug.Log(string.Format("Initializing Mapper [{0}]", MapperType));
            }
#endif

            _mappers = new Dictionary<string, IPowerMapper>();

            Object[] mappers = FindObjectsOfType(MapperType);

            foreach (Object o in mappers)
            {
                PowerMapper mapper = (PowerMapper)o;

                if (mapper.Default)
                {
                    if (null != _defaultMapper)
                        Debug.LogWarning("Duplicated default mapper [{0}]");

                    _defaultMapper = mapper;
                }

                if (!string.IsNullOrEmpty(mapper.Id))
                {

                    if (_mappers.ContainsKey(mapper.Id))
                        Debug.LogWarning("Duplicated [{0}] mapper for: " + mapper.Id);

                    _mappers.Add(mapper.Id, mapper);
                }
            }

            InitializedTypes.Add(MapperType);
        }

        public bool IsMapping(string id)
        {
            // TODO: ispisati redoslijed učitavanja fontova, napraviti kao default font mapper!

            if (!InitializedTypes.Contains(MapperType))
                Initialize();

            return _mappers.ContainsKey(id);
        }

        public IPowerMapper Get(string id)
        {
            if (!InitializedTypes.Contains(MapperType))
                Initialize();

            if (!_mappers.ContainsKey(id))
                return null;

            return _mappers[id];
        }

        public bool HasDefault()
        {
            return null == _defaultMapper;
        }

        public IPowerMapper GetDefault()
        {
            if (!InitializedTypes.Contains(MapperType))
                Initialize();

            if (null == _defaultMapper)
                throw new Exception("Default mapper [{0}] not defined");

            return _defaultMapper;
        }

        public IPowerMapper GetWithFallback(string id)
        {
            if (IsMapping(id))
                return Get(id);

            return GetDefault();
        }

        protected static Object InstanceLookup()
        {
            Object[] retValue = FindObjectsOfType(MapperType);
            if (retValue == null || retValue.Length == 0)
                throw new ApplicationException("FontMapper2 object doesn't exist on the scene!");
            if (retValue.Length > 1)
                throw new ApplicationException("More than one FontMapper2 object exists on the scene!");
            return retValue[0];
        }
    }
}