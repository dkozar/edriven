using System;
using eDriven.Core.Caching;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// Member wrapper cache<br/>
    /// A member wrapper creation is the result of reflecting the property/field of a class<br/>
    /// Since this should be done no more than once during the application lifetime, we use the cache<br/>
    /// This cache has additional methods forgetting/setting values having 2 parameters: type and property
    /// </summary>
    public class MemberCache : Cache<Type, Cache<string, MemberWrapper>>
    {
        /// <summary>
        /// Gets the member wrapper based on type and property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public MemberWrapper Get(Type type, string property)
        {
            var t = Get(type);
            if (null == t)
                return null;

            var p = t.Get(property);
            return p;
        }

        /// <summary>
        /// Puts the member wrapper based on type and property
        /// </summary>
        /// <param name="type"></param>
        /// <param name="property"></param>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        public void Put(Type type, string property, MemberWrapper wrapper)
        {
            var t = Get(type);
            if (null == t)
            {
                t = new Cache<string, MemberWrapper>();
                Put(type, new Cache<string, MemberWrapper>());
            }

            t.Put(property, wrapper);
            /*Debug.Log(string.Format(@"Put: {0} [cache size: {1}]
type: {2}, property: {3}", wrapper, Count, type, property));*/
        }
    }
}