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

namespace eDriven.Core.Events
{
    ///<summary>
    /// MonoBehaviour with event dispatching possibilities
    /// You should extend this class
    ///</summary>
    [AddComponentMenu("eDriven/Core/EventDispatcherComponent")]
    [Obfuscation(Exclude = true)]
    public class EventDispatcherComponent : MonoBehaviour, IEventDispatcher, IDisposable
    {
        /// <summary>
        /// EventDispatcher instance via composition
        /// </summary>
        private EventDispatcher _dispatcher;
        
        #region Implementation of IEventDispatcher

        /// <summary>
        /// Dispatches an event
        /// </summary>
        /// <param name="e">Event to be dispatched</param>
        public void DispatchEvent(Event e)
        {
            //e.Target = gameObject; // set gameObject as Target
            _dispatcher.DispatchEvent(e);
        }

        /// <summary>
        /// Dispatches an event with an option for switching on delayed processing
        /// </summary>
        /// <param name="e">Event</param>
        /// <param name="immediate">Process immediatelly or on the next update?</param>
        public void DispatchEvent(Event e, bool immediate)
        {
            //e.Target = gameObject; // set gameObject as Target
            _dispatcher.DispatchEvent(e, immediate);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        public void AddEventListener(string eventType, EventHandler handler)
        {
            _dispatcher.AddEventListener(eventType, handler);
            HandleAddTarget(handler);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="priority">Event priority</param>
        public void AddEventListener(string eventType, EventHandler handler, int priority)
        {
            _dispatcher.AddEventListener(eventType, handler, priority);
            HandleAddTarget(handler);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        public void AddEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            _dispatcher.AddEventListener(eventType, handler, phases);
            HandleAddTarget(handler);
        }

        /// <summary>
        /// Adds the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases">Event bubbling phases that we listen to</param>
        /// <param name="priority">Event priority</param>
        public void AddEventListener(string eventType, EventHandler handler, EventPhase phases, int priority)
        {
            _dispatcher.AddEventListener(eventType, handler, phases, priority);
            HandleAddTarget(handler);
        }

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        public void RemoveEventListener(string eventType, EventHandler handler)
        {
            _dispatcher.RemoveEventListener(eventType, handler);
            HandleRemoveTarget(handler);
        }

        /// <summary>
        /// Removes the event listener
        /// </summary>
        /// <param name="eventType">Event type</param>
        /// <param name="handler">Event handler</param>
        /// <param name="phases"></param>
        public void RemoveEventListener(string eventType, EventHandler handler, EventPhase phases)
        {
            _dispatcher.RemoveEventListener(eventType, handler, phases);
            HandleRemoveTarget(handler);
        }

        /// <summary>
        /// Removes all event listeners of a certain type
        /// </summary>
        /// <param name="eventType">Event type</param>
        public void RemoveAllListeners(string eventType)
        {
            _dispatcher.RemoveAllListeners(eventType);
            _targets.Clear();
        }

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific type of event. 
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool HasEventListener(string eventType)
        {
            return _dispatcher.HasEventListener(eventType);
        }

        /// <summary>
        /// Checks whether an event listener is registered with this EventDispatcher object or any of its ancestors for the specified event type. 
        /// This method returns true if an event listener is triggered during any phase of the event flow when an event of the specified type is dispatched to this EventDispatcher object or any of its descendants.
        /// The difference between HasEventListener() and HasBubblingEventListener() is that HasEventListener() examines only the object to which it belongs, whereas HasBubblingEventListener() examines the entire event flow for the event specified by the type parameter.
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public bool HasBubblingEventListener(string eventType)
        {
            return _dispatcher.HasBubblingEventListener(eventType);
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        virtual public void Dispose()
        {
            _dispatcher.Dispose();
        }

        #endregion

        #region Gizmos

        private readonly List<GameObject> _targets = new List<GameObject>();

        private void HandleAddTarget(EventHandler handler)
        {
            //Debug.Log("HandleAddTarget: " + handler.Method.Name);

            if (handler.Target is MonoBehaviour)
            {
                GameObject go = ((MonoBehaviour) handler.Target).gameObject;
                if (!_targets.Contains(go))
                    _targets.Add(go);
            }
        }

        private void HandleRemoveTarget(EventHandler handler)
        {
            //Debug.Log("HandleRemoveTarget: " + handler.Method.Name);

            if (null != handler.Target && handler.Target is MonoBehaviour)
            {
                Debug.Log("HandleRemoveTarget");

                GameObject go = ((MonoBehaviour)handler.Target).gameObject;
                if (_targets.Contains(go))
                    _targets.Remove(go);
            }
        }

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void OnDrawGizmos()
// ReSharper restore UnusedMember.Local
        {
            Gizmos.color = new Color(63.0f / 255.0f, 1.0f, 175.0f / 255.0f, 1.0f); //Color.green;
            
            _targets.ForEach(delegate(GameObject go)
                                 {
                                     Gizmos.DrawLine(transform.position, go.transform.position);
                                     Gizmos.DrawIcon(go.transform.position, "gizmo_listener.png");
                                 });

            Gizmos.DrawIcon(transform.position, "gizmo_dispatcher.png");
        }

        #endregion

        #region Power mapper

        private static EventDispatcherComponent _defaultMapper;
        private static Dictionary<string, EventDispatcherComponent> _mappers;

        /// <summary>
        /// True for default
        /// </summary>
        public bool Default;

        /// <summary>
        /// ID
        /// </summary>
        public string Id;

        protected EventDispatcherComponent()
        {
            // constructor is protected
        }

        [ContextMenu ("Do Something")]
// ReSharper disable UnusedMember.Local
        void DoSomething () {
// ReSharper restore UnusedMember.Local
            Debug.Log ("Perform operation");
        }

// ReSharper disable UnusedMember.Local
        [Obfuscation(Exclude = true)]
        void Start()
// ReSharper restore UnusedMember.Local
        {
            if (null == _dispatcher)
                _dispatcher = new EventDispatcher(gameObject); // NOTE: Could also be lazy event dispatcher

            if (!_initialized)
                Initialize();

            if (!Default && string.IsNullOrEmpty(Id))
                throw new Exception("Mapper error: Id not set for a non-default mapper");

            if (!_mappers.ContainsKey(Id))
                _mappers.Add(Id, this);
        }

        private static bool _initialized;
        private static void Initialize()
        {
            //Debug.Log("Initializing EventDispatcherComponent");

            _mappers = new Dictionary<string, EventDispatcherComponent>();

            UnityEngine.Object[] mappers = FindObjectsOfType(typeof(EventDispatcherComponent));

            foreach (UnityEngine.Object o in mappers)
            {
                EventDispatcherComponent mapper = (EventDispatcherComponent)o;
                
                if (mapper.Default)
                {
                    if (null != _defaultMapper)
                        Debug.LogWarning("Duplicated default mapper");
                    
                    _defaultMapper = mapper;
                }
                
                if (!string.IsNullOrEmpty(mapper.Id)){

                    if (_mappers.ContainsKey(mapper.Id))
                        Debug.LogWarning("Duplicated mapper for: " + mapper.Id);

                    _mappers.Add(mapper.Id, mapper);
                }
            }

            _initialized = true;
        }

        /// <summary>
        /// Returns true if mapping with specified ID exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsMapping(string id)
        {
            if (!_initialized)
                Initialize();

            return _mappers.ContainsKey(id);
        }

        /// <summary>
        /// Gets the mapped component
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EventDispatcherComponent Get(string id)
        {
            if (!_initialized)
                Initialize();

            if (!_mappers.ContainsKey(id))
                return null;

            if (null == _mappers[id])
                throw new Exception("EventDispatcherComponent not defined for mapper: " + id);

            return _mappers[id];
        }

        /// <summary>
        /// Returns true if default mapping exists
        /// </summary>
        /// <returns></returns>
        public static bool HasDefault()
        {
            return null == _defaultMapper;
        }

        /// <summary>
        /// Gets the default mapping
        /// </summary>
        /// <returns></returns>
        public static EventDispatcherComponent GetDefault()
        {   
            if (!_initialized)
                Initialize();

            if (null == _defaultMapper)
                throw new Exception("Default EventDispatcherComponent not defined");

            return _defaultMapper;
        }

        /// <summary>
        /// Gets the mapping specified with ID, or a default one
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static EventDispatcherComponent GetWithFallback(string id)
        {
            if (IsMapping(id))
                return Get(id);
            
            return GetDefault();
        }

        #endregion

    }
}
