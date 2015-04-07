import eDriven.Networking.Callback;
import eDriven.Core.Test;
import System;

class Main2JS extends MonoBehaviour {
    
	public var BundleUrl:String = "http://dankokozar.com/unity/CallbackQueue/village.unity3d";
	public var AssetName:String = "MaloNaselje";
	
	private var _wwwQueue:WwwQueue = new WwwQueue();
	private var _assetLoaderQueue:AssetBundleQueue = new AssetBundleQueue();
	
	private var _bundle:AssetBundle;
    private var _object:Object;
	private var _progress:float = 0;
	
	function Start(){
		_wwwQueue.FinishedChecker = MyWwwChecker;
		_assetLoaderQueue.FinishedChecker = MyAssetLoaderChecker;
	}
	
	private function MyWwwChecker(request:WWW):boolean
	{
		_progress = Math.Floor(request.progress * 100);
		return request.isDone;
	};
	
	private function MyAssetLoaderChecker(request:AssetBundleRequest):boolean
	{
		_progress = Math.Floor(request.progress * 100);
		return request.isDone;
	};
	
    function OnGUI() {
        GUI.depth = 0;
		if(GUI.Button(Rect(10, 10, 100, 50), _progress == 0 ? "Load" : "Loading: " + _progress + "%")) {

			// reset queues
			_wwwQueue.Reset();
			_assetLoaderQueue.Reset();

			// destroy old object
			if (null != _object)
                Destroy(_object);

			// unload old bundle
            if (null != _bundle)
                _bundle.Unload(true);
				
			// load bundle
            _wwwQueue.Send(new WWW(BundleUrl), BundleLoadedHandler);
        }
    }
    
    private function BundleLoadedHandler(request:WWW):void 
    {
        Debug.Log("Bundle loaded: " + request.url);
        _bundle = request.assetBundle;
        
        var assetBundleRequest:AssetBundleRequest = _bundle.LoadAsync(AssetName, typeof (GameObject));
        _assetLoaderQueue.Send(assetBundleRequest, AssetLoadedHandler);
    }

    private function AssetLoadedHandler(request:AssetBundleRequest):void 
    {
        Debug.Log("Asset loaded: " + request.asset.name);
        _object = Instantiate(request.asset);
		
		var camera:GameObject = GameObject.Find("Main Camera");
		if (null != camera){
			var comp:MouseOrbit  = camera.AddComponent(typeof(MouseOrbit));
			comp.target = _object.transform;
		}
    }
}