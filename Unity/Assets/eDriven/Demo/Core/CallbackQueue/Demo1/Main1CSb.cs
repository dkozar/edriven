using System;
using eDriven.Core.Test.CallbackQueue;
using UnityEngine;
using Random=System.Random;

public class Main1CSb : MonoBehaviour
{
    public int NumberOfRequests = 10;
    public float MaxDelay = 10f;

    private readonly TestQueue _queue = new TestQueue();
    private readonly Random _random = new Random();

    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    void OnGUI()
    // ReSharper restore InconsistentNaming
    // ReSharper restore UnusedMember.Local
    {
        GUI.depth = 0;
        if (GUI.Button(new Rect(10, 10, 100, 50), "Load"))
        {
            Debug.Log("*** STARTING ***");

            for (int i = 0; i < NumberOfRequests; i++)
            {
                TestRequest request = new TestRequest();
                request.Id = (i + 1).ToString();

                double seconds = _random.NextDouble() * MaxDelay;
                Debug.Log(string.Format("Delaying request [{0}] for {1} seconds", request.Id, seconds));
                request.EndTime = DateTime.Now.AddSeconds(seconds);

                _queue.Send(request,
                    delegate(TestRequest r)
                    {
                        Debug.Log(string.Format("Request [{0}] finished at [{1}]", r.Id, r.EndTime));
                        if (_queue.Active.Count == 0)
                            Debug.Log("*** ALL FINISHED ***");
                    }
                );
            }
        }
    }
}