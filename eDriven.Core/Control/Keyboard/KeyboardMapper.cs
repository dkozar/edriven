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
using System.Text;
using eDriven.Core.Events;
using eDriven.Core.Managers;
using UnityEngine;
using Event=eDriven.Core.Events.Event;

namespace eDriven.Core.Control.Keyboard
{
    /// <summary>
    /// Maps key gestures to event handlers<br/>
    /// Subscribes to KEY_DOWN and KEY_UP events and handles them internally<br/>
    /// Note: not to be mixed with the "regular" event system
    /// </summary>
    public class KeyboardMapper : IDisposable
    {
#if DEBUG
        /// <summary>
        /// Debug
        /// </summary>
        public static bool DebugMode;
#endif

        /// <summary>
        /// Key handler signature
        /// </summary>
        /// <param name="e"></param>
        public delegate void KeyHandler(KeyboardEvent e);

        #region Singleton

        private static KeyboardMapper _instance;

        private KeyboardMapper()
        {
            // constructor is protected!
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static KeyboardMapper Instance
        {
            get
            {
                if (_instance == null)
                {
#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating KeyboardMapper instance"));
#endif
                    _instance = new KeyboardMapper();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        private void Initialize()
        {
            /**
             * Subscribe to system event manager key events
             * */
            SystemEventDispatcher.Instance.AddEventListener(KeyboardEvent.KEY_DOWN, Instance.OnKeyEvent);
            SystemEventDispatcher.Instance.AddEventListener(KeyboardEvent.KEY_UP, Instance.OnKeyEvent);
            SystemManager.Instance.DisposingSignal.Connect(DisposingSlot, true);
        }

        #endregion

        /// <summary>
        /// Mappings
        /// </summary>
        private readonly Dictionary<KeyCombination, List<KeyHandler>> _mappings = new Dictionary<KeyCombination, List<KeyHandler>>();

        /// <summary>
        /// One shot mappings
        /// </summary>
        private readonly Dictionary<KeyCombination, List<KeyHandler>> _oneShotMappings = new Dictionary<KeyCombination, List<KeyHandler>>();

        /// <summary>
        /// Maps a key
        /// </summary>
        /// <param name="keyCombination">The combo</param>
        /// <param name="keyHandler">The handler</param>
        /// <param name="singleShot">True for remove the mapping after the single shot</param>
        public KeyboardMapper Map(KeyCombination keyCombination, KeyHandler keyHandler, bool singleShot)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("KeyboardMapper.Map: " + keyCombination);
#endif
            if (!_mappings.ContainsKey(keyCombination))
                _mappings.Add(keyCombination, new List<KeyHandler>());

            _mappings[keyCombination].Add(keyHandler);

            if (singleShot)
            {
                if (!_oneShotMappings.ContainsKey(keyCombination))
                    _oneShotMappings.Add(keyCombination, new List<KeyHandler>());

                _oneShotMappings[keyCombination].Add(keyHandler);
            }

            return this;
        }

        /// <summary>
        /// Maps a key
        /// </summary>
        /// <param name="keyCombination">The combo</param>
        /// <param name="keyHandler">The handler</param>
        public KeyboardMapper Map(KeyCombination keyCombination, KeyHandler keyHandler)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("KeyboardMapper.Map: " + keyCombination);
#endif
            if (!_mappings.ContainsKey(keyCombination))
                _mappings.Add(keyCombination, new List<KeyHandler>());

            _mappings[keyCombination].Add(keyHandler);

            return this;
        }

        /// <summary>
        /// Unmaps a key
        /// </summary>
        /// <param name="keyCombination">The combo</param>
        /// <param name="keyHandler">The handler</param>
        public KeyboardMapper Unmap(KeyCombination keyCombination, KeyHandler keyHandler)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log("KeyboardMapper.Unmap: " + keyCombination);
#endif

            if (_mappings.ContainsKey(keyCombination))
            {
                _mappings[keyCombination].Remove(keyHandler);

                if (_mappings[keyCombination].Count == 0)
                    _mappings.Remove(keyCombination);
            }

            if (_oneShotMappings.ContainsKey(keyCombination))
            {
                _oneShotMappings[keyCombination].Remove(keyHandler);

                if (_oneShotMappings[keyCombination].Count == 0)
                    _oneShotMappings.Remove(keyCombination);
            }

            return this;
        }

        /// <summary>
        /// Removes the keyboard mappings for a given key combo
        /// </summary>
        /// <param name="keyCombination">The combo to unmap</param>
        public void UnmapCombo(KeyCombination keyCombination)
        {
            _mappings.Remove(keyCombination);
            _oneShotMappings.Remove(keyCombination);
        }

        /// <summary>
        /// Removes all keyboard mappings
        /// </summary>
        public void UnmapAll()
        {
            _mappings.Clear();
            _oneShotMappings.Clear();
        }

        /// <summary>
        /// Returns true if the specified mapping exists
        /// </summary>
        /// <param name="keyCombination">The combo to check for</param>
        /// <param name="keyHandler">The handler to check for</param>
        /// <returns></returns>
        public bool Maps(KeyCombination keyCombination, KeyHandler keyHandler)
        {
            if (_mappings.ContainsKey(keyCombination))
                return _mappings[keyCombination].Contains(keyHandler);

            return false;
        }

        /// <summary>
        /// Checks if a mapping exists
        /// </summary>
        /// <returns></returns>
        public string Report()
        {
            StringBuilder sb = new StringBuilder();
            int comboCount = 0;
            foreach (KeyCombination combo in _mappings.Keys)
            {
                sb.AppendLine(string.Format("  {0} -> {1}", combo, _mappings[combo].Count));
                comboCount++;
            }
            string output = string.Format(@"Number of combos: {0}
{1}
", comboCount, sb);

            return output;
        }

        /// <summary>
        /// The keyboard event handler
        /// Fires on both key press/release
        /// </summary>
        /// <param name="e"></param>
        private void OnKeyEvent(Event e)
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format(@"OnKeyEvent [{0}] ", e));
#endif

            KeyboardEvent ke = (KeyboardEvent)e;

            KeyCombination combo = KeyCombination.FromKeyboardEvent(ke);

            if (_mappings.ContainsKey(combo))
            {
                foreach (KeyHandler keyHandler in _mappings[combo])
                {
                    keyHandler(ke);
                }

                // one shot mappings
                if (_oneShotMappings.ContainsKey(combo))
                {
                    foreach (KeyHandler handler in _oneShotMappings[combo])
                    {
#if DEBUG
                        if (DebugMode)
                        {
                            Debug.Log(string.Format("Unmapping one shot mapping for key [{0}]", combo.KeyCode));
                        }
#endif
                        Unmap(combo, handler);
                    }

                    _oneShotMappings[combo].Clear();
                }

            }
            else
            {
#if DEBUG
                if (DebugMode)
                    Debug.Log(string.Format(@"Key mappings doesn't define action for a key combination [{0}] ", combo));
#endif
            }
        }

        #region Implementation of ISlot

        private void DisposingSlot(params object[] parameters)
        {
            Dispose();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            foreach (KeyValuePair<KeyCombination, List<KeyHandler>> pair in _mappings)
            {
                pair.Value.Clear();
            }

            _mappings.Clear();

            SystemEventDispatcher.Instance.RemoveEventListener(KeyboardEvent.KEY_DOWN, Instance.OnKeyEvent);
            SystemEventDispatcher.Instance.RemoveEventListener(KeyboardEvent.KEY_UP, Instance.OnKeyEvent);

            _instance = null;
        }

        #endregion
    }
}