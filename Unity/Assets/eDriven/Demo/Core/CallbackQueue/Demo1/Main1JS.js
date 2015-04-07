import eDriven.Networking.Callback;
import eDriven.Core.Test.CallbackQueue;
import System;

class Main1JS extends MonoBehaviour {
    
	public var NumberOfRequests:int = 10;
    public var MaxDelay:float = 10;

    private var _queue:TestQueue = new TestQueue();
    private var _random:System.Random = new System.Random();
	
    function OnGUI() {
        GUI.depth = 0;
        if (GUI.Button(Rect(10, 10, 100, 50), "Load"))
        {
            Debug.Log("*** STARTING ***");

            for (var i:int = 0; i < NumberOfRequests; i++)
            {
                var request:TestRequest = new TestRequest();
                request.Id = (i + 1).ToString();

                var seconds:double = _random.NextDouble() * MaxDelay;
                Debug.Log(String.Format("Delaying request [{0}] for {1} seconds", request.Id, seconds));
                request.EndTime = DateTime.Now.AddSeconds(seconds);

                _queue.Send(request, MyHandler);
            }
        }
    }
	
	private function MyHandler(r:TestRequest):void
	{
		Debug.Log(String.Format("Request [{0}] finished at [{1}]", r.Id, r.EndTime));
		if (_queue.Active.Count == 0) 
			Debug.Log("*** ALL FINISHED ***");
	}
    
}