#region License

/*
 
Copyright (c) 2012 Danko Kozar

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
    /// If events needed, use SystemEventManager Singleton
    /// </summary>
    /// <remarks>Conceived and coded by Danko Kozar</remarks>
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
             * 1) Instantiate SystemManagerInvoker
             * */
            Framework.CreateComponent(typeof(SystemManagerInvoker), true); // exclusive, i.e. allow single instance only

            /**
             * 2) Instantiate signals
             * */
            FixedUpdateSignal = new Signal();
            UpdateSignal = new Signal();
            LateUpdateSignal = new Signal();
            RenderSignal = new Signal();
            PreRenderSignal = new Signal();
            ResizeSignal = new Signal();
            DisposingSignal = new Signal();
            LevelLoadedSignal = new Signal();
            SceneChangeSignal = new Signal();

            MouseDownSignal = new Signal();
            MouseUpSignal = new Signal();

            MiddleMouseDownSignal = new Signal();
            MiddleMouseUpSignal = new Signal();

            RightMouseDownSignal = new Signal();
            RightMouseUpSignal = new Signal();

            MouseMoveSignal = new Signal();
            MouseDragSignal = new Signal();
            MouseWheelSignal = new Signal();

            KeyDownSignal = new Signal();
            KeyUpSignal = new Signal();

            /**
             * 3) Instantiate processors
             * */
            _keyboardProcessor = new KeyboardProcessor(this);
            _mouseProcessor = new MouseProcessor(this);
            _mousePositionProcessor = new MousePositionProcessor(this);
            _screenSizeProcessor = new ScreenSizeProcessor(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// If true, system manager is enabled
        /// </summary>
        public static bool Enabled = true; // TRUE by default
        
        /// <summary>
        /// Publicly available screen size
        /// </summary>
        public Point ScreenSize = new Point();

        /// <summary>
        /// Publicly available mouse position
        /// </summary>
        public Point MousePosition = new Point();

        #endregion
        
        #region Members

        private KeyboardProcessor _keyboardProcessor;
        private MouseProcessor _mouseProcessor;
        private MousePositionProcessor _mousePositionProcessor;
        private ScreenSizeProcessor _screenSizeProcessor;

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
        /// Fixed update signal
        /// </summary>
        public Signal LevelLoadedSignal { get; private set; }

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

        #endregion

        #region Methods

        /// <summary>
        /// Runs on SystemManagerInvoker.OnGUI()
        /// </summary>
        public void ProcessInput()
        {
            if (!Enabled)
                return;

            if (null != OnGuiDepth)
            {
                GUI.depth = (int)OnGuiDepth;
            }

            /**
             * 1) Process input
             * */

            Event e = Event.current;

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

        /// <summary>
        /// Emits the level loaded signal
        /// </summary>
        public void ProcessLevelLoaded()
        {
            LevelLoadedSignal.Emit();
        }

        /// <summary>
        /// Emits the disposing signal and disposes the system manager
        /// </summary>
        public void Dispose()
        {
            DisposingSignal.Emit();

            //Framework.Reset();

            // trigger invoker initialization in later time
            _instance = null;
        }

        #endregion
    }
}