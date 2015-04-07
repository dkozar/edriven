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

using System.Collections.Generic;
using eDriven.Core.Geom;
using eDriven.Core.Mono;
using eDriven.Core.Signals;
using UnityEngine;

namespace eDriven.Core.Managers
{
    /// <summary>
    /// This singleton processing system events:
    /// 1) Mouse and key OnGUI() UnityEngine.Events
    /// 2) Input.mousePosition on Update()
    /// 3) Screen resize
    /// It emits signals to interested components 
    /// This Singleton is a central place for emiting input signals
    /// If events needed, use SystemEventDispatcher Singleton
    /// </summary>
    public sealed class SystemManager
    {

#if DEBUG
        /// <summary>
        /// Debug mode
        /// </summary>
        public static bool DebugMode;
#endif
        
        #region Singleton

        private static SystemManager _instance;
        
        private SystemManager()
        {
            // Constructor is protected!
        }

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static SystemManager Instance
        {
            get
            {
                if (_instance == null)
                {

#if DEBUG
                    if (DebugMode)
                        Debug.Log(string.Format("Instantiating system manager instance"));
#endif

                    _instance = new SystemManager();
                    _instance.Initialize();
                }

                return _instance;
            }
        }

        #endregion

        #region Static

        /// <summary>
        /// A global GUI.depth for this framework
        /// If set, this depth is applied before rendering
        /// </summary>
        public static int? OnGuiDepth;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialization routine
        /// </summary>
        private void Initialize()
        {
#if DEBUG
            if (DebugMode)
                Debug.Log(string.Format("Initializing system manager"));
#endif
            /**
             * 1.a) Instantiate signals
             * */
            FixedUpdateSignal = new Signal();
            UpdateSignal = new Signal();
            LateUpdateSignal = new Signal();
            ResizeSignal = new Signal();
            DisposingSignal = new Signal();
            LevelInitSignal = new Signal();
            LevelLoadedSignal = new Signal();
            SceneChangeSignal = new Signal();
            GizmoSignal = new Signal();
            TouchSignal = new Signal();

            /**
             * 1.b) Instantiate signals requiring OnGUI processing
             * */

            PreRenderSignal = new StateHandlingSignal(SignalStateChangedHandler);
            RenderSignal = new StateHandlingSignal(SignalStateChangedHandler);

            MouseDownSignal = new StateHandlingSignal(SignalStateChangedHandler);
            MouseUpSignal = new StateHandlingSignal(SignalStateChangedHandler);

            MiddleMouseDownSignal = new StateHandlingSignal(SignalStateChangedHandler);
            MiddleMouseUpSignal = new StateHandlingSignal(SignalStateChangedHandler);

            RightMouseDownSignal = new StateHandlingSignal(SignalStateChangedHandler);
            RightMouseUpSignal = new StateHandlingSignal(SignalStateChangedHandler);

            MouseMoveSignal = new StateHandlingSignal(SignalStateChangedHandler);
            MouseDragSignal = new StateHandlingSignal(SignalStateChangedHandler);
            MouseWheelSignal = new StateHandlingSignal(SignalStateChangedHandler);

            KeyDownSignal = new StateHandlingSignal(SignalStateChangedHandler);
            KeyUpSignal = new StateHandlingSignal(SignalStateChangedHandler);

            /**
             * 2) Instantiate processors
             * */
            _keyboardProcessor = new KeyboardProcessor(this);
            _mouseProcessor = new MouseProcessor(this);
            _mousePositionProcessor = new MousePositionProcessor(this);
            _touchProcessor = new TouchProcessor(this);
            _screenSizeProcessor = new ScreenSizeProcessor(this);

            /**
             * 3.a) Instantiate SystemManagerInvoker
             * */
            Framework.CreateComponent<SystemManagerInvoker>(true); // exclusive, i.e. allow single instance only

            /**
             * 3.b) Instantiate SystemManagerOnGuiInvoker
             * Keeping reference to it so we could disable it when OnGUI processing not needed
             * */
            _onGuiInvoker = (SystemManagerOnGuiInvoker)Framework.CreateComponent<SystemManagerOnGuiInvoker>(true); // exclusive, i.e. allow single instance only
            _onGuiInvoker.enabled = false; // disable it by default

            
        }

        /// <summary>
        /// The list of smart signals having subscribers
        /// Since we need to process OnGUI for a certain signals, we are keeping those in the list
        /// When the list is empty - no need for OnGUI processing - so we are turning _onGuiInvoker OFF for the performance reasons
        /// </summary>
        private readonly List<Signal> _activeSignals = new List<Signal>();
        private void SignalStateChangedHandler(Signal signal, bool connected)
        {
            //Debug.Log(string.Format("SignalStateChangedHandler: {0} [{1}]", signal, connected));
            if (connected && !_activeSignals.Contains(signal))
            {
                _activeSignals.Add(signal);
                //Debug.Log(string.Format("SignalStateChangedHandler: added {0} [{1}]", signal, connected));
            }
            else if (!connected && _activeSignals.Contains(signal))
            {
                _activeSignals.Remove(signal);
                //Debug.Log(string.Format("SignalStateChangedHandler: removed {0} [{1}]", signal, connected));
            }

            HandleOnGuiInvokerEnabledState();
        }

        #endregion

        #region Properties

        private static bool _enabled = true; // TRUE by default

        /// <summary>
        /// If true, system manager is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (value != _enabled)
                {
                    _enabled = value;
                    // process the Enable change
                    // if system manager not enabled, disable it so we're not stealing performance using OnGUI calls
                    HandleOnGuiInvokerEnabledState();
                }
            }
        }

        private void HandleOnGuiInvokerEnabledState()
        {
            if (null != _onGuiInvoker)
                _onGuiInvoker.enabled = _enabled && _activeSignals.Count > 0;
        }

        private Point _screenSize = new Point();
        /// <summary>
        /// Publicly available screen size
        /// </summary>
        public Point ScreenSize
        {
            get { return _screenSize; }
            internal set { _screenSize = value; }
        }

        /// <summary>
        /// Publicly available mouse position
        /// </summary>
        public Point MousePosition = new Point();

        #endregion
        
        #region Members

        private KeyboardProcessor _keyboardProcessor;
        private MouseProcessor _mouseProcessor;
        private MousePositionProcessor _mousePositionProcessor;
        private TouchProcessor _touchProcessor;
        private ScreenSizeProcessor _screenSizeProcessor;

        /// <summary>
        /// A separate invoker instance
        /// This one handles the OnGUI calls separately
        /// The reason for the separation is because the OnGUI calls are currently very expensive
        /// Not every implementation has to use OnGUI, so this component could be switched OFF if no OnGUI functionality needed
        /// </summary>
        private SystemManagerOnGuiInvoker _onGuiInvoker;

        /// <summary>
        /// True if the Control key is pressed
        /// </summary>
        public bool ControlKeyPressed { get; internal set; }

        /// <summary>
        /// True if the Alt key is pressed
        /// </summary>
        public bool AltKeyPressed { get; internal set; }

        /// <summary>
        /// True if the Shift key is pressed
        /// </summary>
        public bool ShiftKeyPressed { get; internal set; }

        #endregion

        #region Signals

        /// <summary>
        /// Fixed update signal
        /// </summary>
        public Signal FixedUpdateSignal { get; private set; }

        /// <summary>
        /// Update update signal
        /// </summary>
        public Signal UpdateSignal { get; private set; }

        /// <summary>
        /// Late update signal
        /// </summary>
        public Signal LateUpdateSignal { get; private set; }

        /// <summary>
        /// Pre render update signal
        /// </summary>
        public Signal PreRenderSignal { get; private set; }

        /// <summary>
        /// Render signal
        /// </summary>
        public Signal RenderSignal { get; private set; }

        /// <summary>
        /// Resize signal
        /// </summary>
        public Signal ResizeSignal { get; private set; }

        /// <summary>
        /// Disposing signal
        /// </summary>
        public Signal DisposingSignal { get; private set; }

        /// <summary>
        /// Level loaded signal
        /// </summary>
        public Signal LevelInitSignal { get; private set; }

        /// <summary>
        /// Level loaded signal
        /// </summary>
        public Signal LevelLoadedSignal { get; private set; }

        /// <summary>
        /// Gizmos signal
        /// </summary>
        public Signal GizmoSignal { get; private set; }

        /// <summary>
        /// Scene change signal
        /// </summary>
        public Signal SceneChangeSignal { get; private set; }

        /// <summary>
        /// Mouse down signal
        /// </summary>
        public Signal MouseDownSignal { get; private set; }

        /// <summary>
        /// Mouse up signal
        /// </summary>
        public Signal MouseUpSignal { get; private set; }

        /// <summary>
        /// Middle mouse down signal
        /// </summary>
        public Signal MiddleMouseDownSignal { get; private set; }

        /// <summary>
        /// Middle mouse up signal
        /// </summary>
        public Signal MiddleMouseUpSignal { get; private set; }

        /// <summary>
        /// Right mouse down signal
        /// </summary>
        public Signal RightMouseDownSignal { get; private set; }

        /// <summary>
        /// Right mouse up signal
        /// </summary>
        public Signal RightMouseUpSignal { get; private set; }

        /// <summary>
        /// Mouse move signal
        /// </summary>
        public Signal MouseMoveSignal { get; private set; }

        /// <summary>
        /// Mouse drag signal
        /// </summary>
        public Signal MouseDragSignal { get; private set; }

        /// <summary>
        /// Mouse wheel signal
        /// </summary>
        public Signal MouseWheelSignal { get; private set; }

        /// <summary>
        /// Key down signal
        /// </summary>
        public Signal KeyDownSignal { get; private set; }

        /// <summary>
        /// Key up signal
        /// </summary>
        public Signal KeyUpSignal { get; private set; }

        /// <summary>
        /// Touch
        /// </summary>
        public Signal TouchSignal { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Process Awake
        /// </summary>
        public void ProcessAwake()
        {
            if (LevelInitSignal.Connected)
                LevelInitSignal.Emit();
        }

        /// <summary>
        /// Runs on SystemManagerInvoker.OnGUI()
        /// </summary>
        public void ProcessInput()
        {
            if (!_enabled/* || !_onGuiEnabled*/)
                return;

            if (null != OnGuiDepth)
            {
                GUI.depth = (int)OnGuiDepth;
            }

            /**
             * 1) Process input
             * */

            Event e = Event.current;

            //Debug.Log("e.isMouse: " + e.isMouse);

            if (e.type == EventType.Layout)
                return; // no native layout

            if (e.isKey)
                _keyboardProcessor.Process(e);

            else if (e.isMouse)
                _mouseProcessor.Process(e);

            else if (e.type == EventType.ScrollWheel)
                _mouseProcessor.ProcessWheel(e);

            /**
             * PreRenderSignal:
             * This is the last chance to influence the rendering result
             * */
            else if (e.type == EventType.Repaint)
                PreRenderSignal.Emit(e);

            /**
             * 2) Actual rendering
             * */
            RenderSignal.Emit(e);
        }

        /// <summary>
        /// Runs on SystemManagerInvoker.Update()
        /// </summary>
        public void ProcessUpdate()
        {
            if (!Enabled)
                return;

            _screenSizeProcessor.Process(null);

            _mousePositionProcessor.Process(null);

            _touchProcessor.Process(null);

            UpdateSignal.Emit();
        }

        /// <summary>
        /// Runs on SystemManagerInvoker.FixedUpdate()
        /// </summary>
        public void ProcessFixedUpdate()
        {
            FixedUpdateSignal.Emit();
        }

        /// <summary>
        /// Runs on SystemManagerInvoker.LateUpdate()
        /// </summary>
        public void ProcessLateUpdate()
        {
            LateUpdateSignal.Emit();
        }

        public void ProcessOnEnable()
        {
            //Debug.Log("SM ProcessOnEnable");
        }

        public void ProcessOnDisable()
        {
            //Debug.Log("SM ProcessOnDisable");
        }

        private static int _levelId = -1;

        /// <summary>
        /// Emits the level loaded signal
        /// </summary>
        public void ProcessLevelLoaded()
        {
            /**
             * Note: OnLevelWasLoaded fires twice when used with objects using DontDestroyOnLoad()
             * eDriven Framework game object is one of such objects, so we have to make a check here
             * */
            if (_levelId == Application.loadedLevel)
                return;

            _levelId = Application.loadedLevel;

            if (LevelLoadedSignal.Connected)
                LevelLoadedSignal.Emit();
        }

        /// <summary>
        /// Emits the gizmos signal
        /// </summary>
        public void ProcessOnDrawGizmos()
        {
            if (GizmoSignal.Connected)
                GizmoSignal.Emit();
        }

        /// <summary>
        /// Emits the disposing signal and disposes the system manager
        /// </summary>
        public void Dispose()
        {
            DisposingSignal.Emit();
            
            // trigger invoker initialization in later time
            _instance = null;
        }

        #endregion
    }
}