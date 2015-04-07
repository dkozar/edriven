import eDriven.Networking.Callback;
import eDriven.Core.Test;
import System;

class Main3JS extends MonoBehaviour {
	
	private var _wwwQueue:WwwQueue = new WwwQueue();
	private var _assetQueue:AssetBundleQueue = new AssetBundleQueue();
	
	private var _loadedBundles:Array = new Array();
	private var _objects:Array = new Array(0);
	private var _progress:float = 0;
	
	// asset bundle URL | asset name (separator is "|")
	public var AssetUrls:String[];
	
	function OnGUI() {
		GUI.depth = 0;
		if(GUI.Button(Rect(10, 10, 100, 50), "Load")) {

			// reset queues
			_wwwQueue.Reset();
			_assetQueue.Reset();

			for (var obj:Object in _objects){
				Destroy(obj);
			}
			
			for (var bundle:AssetBundle in _loadedBundles){
				bundle.Unload(true);
			}

			for (var s:String in AssetUrls)
			{
				var arr:String[] = s.Split("|"[0]);

				if (arr.Length != 2)
					throw new Exception("Error in asset string");

				var bundleUrl:String = arr[0];
				var assetName:String = arr[1];

				_wwwQueue.Send(new WWW(bundleUrl), BundleLoadedHandler);
			}
		}
	}
	
	private function BundleLoadedHandler(request:WWW):void 
	{
		Debug.Log("Bundle loaded: " + request.url);
		_bundle = request.assetBundle;
		
		var assetBundleRequest:AssetBundleRequest = _bundle.LoadAsync(GetAssetName(request.url), typeof (GameObject));

		_assetQueue.Send(assetBundleRequest, AssetLoadedHandler);
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
	
	private function GetAssetName(bundleUrl:String):String {
		for (var s:String in AssetUrls)
		{
			var arr:String[] = s.Split("|"[0]);
			if (arr[0] == bundleUrl)
			return arr[1];
		}
		return null;
	}
}