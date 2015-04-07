using System;
using eDriven.Networking.Callback;
using UnityEngine;

public class Main2CSb : MonoBehaviour
{
	public string BundleUrl = "http://dankokozar.com/unity/CallbackQueue/village.unity3d";
	public string AssetName = "MaloNaselje";
	
    private readonly WwwQueue _bundleQueue = new WwwQueue();
    private readonly AssetBundleQueue _assetBundleQueue = new AssetBundleQueue();

    private AssetBundle _bundle;
    private GameObject _object;
    private float _progress;

// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        _bundleQueue.FinishedChecker = delegate(WWW request)
        {
            _progress = (float)Math.Floor(request.progress * 100);
            return request.isDone;
        };
    }
    
// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
    void OnGUI()
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
    { // classic OnGUI
        GUI.depth = 0;
        if (GUI.Button(new Rect(10, 10, 100, 50), _progress == 0 ? "Load" : "Loading: " + _progress + "%"))
        {
            // reset queues
            _bundleQueue.Reset();
            _assetBundleQueue.Reset();

            // destroy old object
            if (null != _object)
                Destroy(_object);

            // unload old bundle
            if (null != _bundle)
                _bundle.Unload(true);

            // load bundle
            _bundleQueue.Send(new WWW(BundleUrl), 
                delegate(WWW request)
                {
                    Debug.Log("Bundle loaded: " + request.url);
                    _bundle = request.assetBundle;

                    AssetBundleRequest assetBundleRequest = _bundle.LoadAsync(AssetName, typeof(GameObject));
                    _assetBundleQueue.Send(assetBundleRequest, 
                        delegate(AssetBundleRequest request2)
                        {
                            Debug.Log("Asset loaded: " + request2.asset.name);
                            _object = (GameObject)Instantiate(request2.asset);

                            // add mouse orbit
                            GameObject cameraGo = GameObject.Find("Main Camera");
                            if (null != cameraGo)
                            {
                                MouseOrbitCs mouseOrbit = cameraGo.AddComponent<MouseOrbitCs>();
                                mouseOrbit.Target = _object.transform;
                            }
                        }
                    );
                }
            );
        }
    }
}