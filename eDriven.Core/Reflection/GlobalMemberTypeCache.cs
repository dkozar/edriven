using eDriven.Core.Util;
using UnityEngine;

namespace eDriven.Core.Reflection
{
    /// <summary>
    /// To speed things up, we use caching<br/>
    /// This is a global cache which holds the member types<br/>
    /// This way we should reflect only once per class/member in the application lifetime (assuming that member types don't change)<br/>
    /// Some operations like tweening use this cache<br/>
    /// The cache could be cleared manually anytime
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
    public class GlobalMemberTypeCache : Cache<VariableTypeCombo, MemberWrapper>
    {
#if DEBUG
        // ReSharper disable UnassignedField.Global
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;
        // ReSharper restore UnassignedField.Global
#endif

        #region Singleton

        private static GlobalMemberTypeCache _instance;

        /// <summary>
        /// Singleton class for handling focus
        /// </summary>
        private GlobalMemberTypeCache()
        {
            // Constructor is protected
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static GlobalMemberTypeCache Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating GlobalMemberTypeCache instance"));
#endif
                    _instance = new GlobalMemberTypeCache();
                    Initialize();
                }

                return _instance;
            }
        }

        #endregion

        /// <summary>
        /// Initializes the Singleton instance
        /// </summary>
        private static void Initialize()
        {

        }
    }
}
