//#region License

///*
 
//Copyright (c) 2010-2014 Danko Kozar

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
 
//*/

//#endregion License

//using System;
//using System.Collections.Generic;
//using eDriven._3D.Events;
//using eDriven.Core.Events;
//using eDriven.Core.Managers;
//using UnityEngine;
//using MulticastDelegate=eDriven.Core.Events.MulticastDelegate;

//namespace eDriven._3D.Managers
//{
//    /// <summary>
//    /// A class that handles all cameras in the application
//    /// We don't use Camera.allCameras, because it returns only the enabled cameras
//    /// Also, we would like to have an event-driven manager, and to subscribe to it's events
//    /// </summary>
//    public class CameraManager : EventDispatcher
//    {
//#if DEBUG
//        // ReSharper disable UnassignedField.Global
//        public new static bool DebugMode;
//        // ReSharper restore UnassignedField.Global
//#endif
        
//        #region Singleton

//        private static CameraManager _instance;
        
//        private CameraManager()
//        {
//            // Constructor is protected!
//        }

//        /// <summary>
//        /// Singleton instance
//        /// </summary>
//        public static CameraManager Instance
//        {
//            get
//            {
//                if (_instance == null)
//                {
//                    _instance = new CameraManager();
//                    _instance.Initialize();
//                }

//                return _instance;
//            }
//        }

//        /// <summary>
//        /// Initialization routine
//        /// Put inside the initialization stuff, if needed
//        /// </summary>
//// ReSharper disable MemberCanBeMadeStatic.Local
//        private void Initialize()
//// ReSharper restore MemberCanBeMadeStatic.Local
//        {
//            //CameraChange = new MulticastDelegate(this, CameraChangeEvent.CAMERA_CHANGE);
//        }

//        #endregion

//        #region Events

//        //public MulticastDelegate CameraChange;

//        private MulticastDelegate _cameraChange;
//        public MulticastDelegate CameraChange
//        {
//            get
//            {
//                if (null == _cameraChange)
//                    _cameraChange = new MulticastDelegate(this, CameraChangeEvent.CAMERA_CHANGE);
//                return _cameraChange;
//            }
//            set
//            {
//                _cameraChange = value;
//            }
//        }

//        #endregion

//        #region Properties

//        /// <summary>
//        /// When disabling a camera, should I disable a game object also?
//        /// </summary>
//        public static bool DisableGameObject = true;

//        private readonly ObservableCollection<Camera> _cameras = new ObservableCollection<Camera>();
//        /// <summary>
//        /// The collection that holds all camera game objects
//        /// </summary>
//        public ObservableCollection<Camera> Cameras
//        {
//            get
//            {
//                return _cameras;
//            }
//        }

//        private Camera _currentCamera;
//        /// <summary>
//        /// Gets and sets current camera
//        /// </summary>
//        public Camera CurrentCamera
//        {
//            get { return _currentCamera; }
//            internal set
//            {
//                if (value != _currentCamera)
//                {
//                    Debug.Log(string.Format(@"Switching to camera [{0}]", value.name));

//                    CameraChangeEvent cce = new CameraChangeEvent(CameraChangeEvent.CAMERA_CHANGE);
//                    cce.PreviousCamera = _currentCamera;
//                    cce.NextCamera = value;
//                    DispatchEvent(cce);

//                    if (!cce.Canceled) // allow cancelation :)
//                    {
//                        Camera previousCamera = _currentCamera;

//                        _currentCamera = value;

//                        // enable current camera and disable others
//                        HandleCameras(_currentCamera);

//                        if (null != previousCamera)
//                        {
//                            if (null != _offHandler) // overal off handler
//                                _offHandler(previousCamera);

//                            if (_offHandlers.ContainsKey(previousCamera.name))
//                                _offHandlers[previousCamera.name](previousCamera);
//                        }

//                        if (null != _onHandler) // overal on handler
//                            _onHandler(_currentCamera);

//                        if (_onHandlers.ContainsKey(_currentCamera.name))
//                            _onHandlers[_currentCamera.name](_currentCamera);
//                    }

//                    else
//                    {
//                        Debug.Log(string.Format(@"Switching to camera [{0}] canceled.", value.name));
//                    }
//                }

//            }
//        }
////Object[] cameras = FindSceneObjectsOfType(typeof(Camera));

//        #endregion

//        #region Members

//        private readonly Dictionary<string, CameraHandler> _onHandlers = new Dictionary<string, CameraHandler>();
//        private readonly Dictionary<string, CameraHandler> _offHandlers = new Dictionary<string, CameraHandler>();

//        private CameraHandler _onHandler;
//        /// <summary>
//        /// Fires when switching TO camera
//        /// </summary>
//        public CameraHandler OnHandler
//        {
//            get { return _onHandler; }
//            set { _onHandler = value; }
//        }

//        private CameraHandler _offHandler;
//        /// <summary>
//        /// Fires when switched FROM camera
//        /// </summary>
//        public CameraHandler OffHandler
//        {
//            get { return _offHandler; }
//            set { _offHandler = value; }
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        /// Registers a camera
//        /// </summary>
//        /// <param name="camera"></param>
//        public void Register(Camera camera)
//        {
//            Debug.Log(string.Format(@"Registering camera [{0}]", camera.name));

//            if (_cameras.Contains(camera))
//                throw new Exception(string.Format(@"Camera [{0}] already registered", camera.name));

//            _cameras.Add(camera);

//            EnableCamera(camera, false);
//        }

//        /// <summary>
//        /// Registers a camera
//        /// </summary>
//        /// <param name="camera"></param>
//        /// <param name="onHandler"></param>
//        /// <param name="offHandler"></param>
//        public void Register(Camera camera, CameraHandler onHandler, CameraHandler offHandler)
//        {
//            Register(camera);

//            if (null != onHandler)
//            {
//                Debug.Log(string.Format(@"  ==> with onHandler"));
//                _onHandlers.Add(camera.name, onHandler);
//            }

//            if (null != offHandler)
//            {
//                Debug.Log(string.Format(@"  ==> with offHandler"));
//                _offHandlers.Add(camera.name, offHandler);
//            }
//        }

//        /// <summary>
//        /// Registers a camera
//        /// </summary>
//        /// <param name="cameraName"></param>
//        /// <param name="onHandler"></param>
//        /// <param name="offHandler"></param>
//        public void Register(string cameraName, CameraHandler onHandler, CameraHandler offHandler)
//        {
//            Register(Get(cameraName), onHandler, offHandler);
//        }

//        /// <summary>
//        /// Removes a camera
//        /// </summary>
//        /// <param name="camera">A camera to remove from manager</param>
//        public void Unregister(Camera camera)
//        {
//            Debug.Log(string.Format(@"Unregistering camera [{0}]", camera.name));

//            if (!_cameras.Contains(camera))
//                throw new Exception(string.Format(@"CameraManager does not reference a camera [""{0}""]", camera.name));

//            _cameras.Remove(camera);

//            _onHandlers.Remove(camera.name);
//            _offHandlers.Remove(camera.name);
//        }

//        /// <summary>
//        /// Removes a camera by ID
//        /// </summary>
//        /// <param name="cameraName">A camera ID to remove from manager</param>
//        public void Unregister(string cameraName)
//        {
//            Unregister(Get(cameraName));
//        }

//        /// <summary>
//        /// Checks if a camera has been registered
//        /// </summary>
//        /// <param name="camera">A camera to check</param>
//        public bool HasRegistered(Camera camera)
//        {
//            return _cameras.Contains(camera);
//        }

//        /// <summary>
//        /// Checks if a camera has been registered
//        /// </summary>
//        /// <param name="cameraName">A name of the camera to check</param>
//        public bool HasRegistered(string cameraName)
//        {
//            return _cameras.Contains(Get(cameraName));
//        }

//        /// <summary>
//        /// Gets a camera defined by name
//        /// </summary>
//        /// <param name="cameraName"></param>
//        /// <returns></returns>
//        public Camera Get(string cameraName)
//        {
//            Camera camera = _cameras.Find(delegate(Camera cam)
//                                              {
//                                                  return cam.name == cameraName;
//                                              });

//            if (null == camera)
//                throw new Exception(string.Format(@"CameraManager does not reference a camera [Name=""{0}""]", cameraName));

//            return camera;
//        }
        
//        /// <summary>
//        /// Switches to camera
//        /// </summary>
//        /// <returns></returns>
//        public Camera SwitchTo(Camera camera)
//        {
//            //Debug.Log(string.Format(@"Switching to camera [{0}]", camera.name));

//            Camera previousCamera = _currentCamera;

//            if (!_cameras.Contains(camera))
//                throw new Exception(string.Format(@"Cannot switch to camera: CameraManager does not reference a camera [""{0}""]", camera.name));

//            CurrentCamera = camera;

//            return previousCamera;
//        }

//        /// <summary>
//        /// Switches to camera defined with name 
//        /// </summary>
//        /// <param name="cameraName"></param>
//        /// <returns></returns>
//        public Camera SwitchTo(string cameraName)
//        {
//            return SwitchTo(Get(cameraName));
//        }

//        /// <summary>
//        /// Switches to camera defined with name with the posibillity do delay action to the next update
//        /// </summary>
//        /// <param name="cameraName"></param>
//        /// <param name="waitForUpdate">Should I wait for the next update to do it?</param>
//        /// <returns></returns>
//        /// <remarks>
//        /// Waiting for update addresses the following exception:
//        /// Destroying object immediately is not permitted during physics trigger and contact callbacks. You must use Destroy instead.
//        /// UnityEngine.Object:DestroyImmediate(Object, Boolean)
//        /// UnityEngine.Object:DestroyImmediate(Object, Boolean)
//        /// UnityEngine.Object:DestroyImmediate(Object)
//        /// GlowEffect:OnDisable() (at Assets\Visual\Effects\Glow\GlowEffect.cs:126)
//        /// </remarks>
//        public void SwitchTo(string cameraName, bool waitForUpdate)
//        {
//            if (waitForUpdate)
//            {
//                // save camera name
//                _cameraName = cameraName;

//                // do it on next update
//                //SystemManager.Instance.Update += OnNextUpdate;
//                SystemManager.Instance.UpdateSignal.Connect(UpdateSlot, 0, true); // auto disconnect
//            }
//            else
//            {
//                // do it immediatelly
//                SwitchTo(Get(cameraName));
//            }
//        }

//        private string _cameraName;

//        //private void OnNextUpdate(Core.Events.Event e)
//        //{
//        //    //SystemManager.Instance.Update -= OnNextUpdate;
//        //    SystemManager.Instance.UpdateSignal.Disconnect(this);
//        //    SwitchTo(GetEaser(_cameraName));
//        //}

//        #endregion

//        #region Helper

//        /// <summary>
//        /// Enable current camera
//        /// </summary>
//// ReSharper disable SuggestBaseTypeForParameter
//        private static void EnableCamera(Camera camera, bool enable)
//// ReSharper restore SuggestBaseTypeForParameter
//        {
//            try
//            {
//                // game object
//                camera.gameObject.activeInHierarchy = enable;

//                camera.enabled = enable;

//                /**
//                * Enable audio listener
//                * Note: There should always be one enabled audio listener on the scene. 
//                * Else, you get warnings.
//                * */
//                AudioListener audioListener = camera.GetComponent<AudioListener>();
//                if (null != audioListener)
//                    audioListener.enabled = enable;
//            }

//            catch(Exception ex)
//            {
//                Debug.Log("ERROR: " + ex);
//            }
//        }
        
//        /// <summary>
//        /// Disables all cameras except one
//        /// </summary>
//        /// <param name="toEnable">A camera to exclude</param>
//// ReSharper disable SuggestBaseTypeForParameter
//        private void HandleCameras(Camera toEnable)
//// ReSharper restore SuggestBaseTypeForParameter
//        {
//            foreach (Camera camera in _cameras)
//            {
//                EnableCamera(camera, toEnable == camera);
//            }
//        }

//        #endregion

//        #region Events

//        /// <summary>
//        /// Camera handler definition
//        /// </summary>
//        /// <param name="camera"></param>
//        public delegate void CameraHandler(Camera camera);

//        #endregion

//        #region Implementation of ISlot

//        public void UpdateSlot(params object[] parameters)
//        {
//            //SystemManager.Instance.UpdateSignal.Disconnect(this);
//            SwitchTo(Get(_cameraName));
//        }

//        #endregion
//    }
//}