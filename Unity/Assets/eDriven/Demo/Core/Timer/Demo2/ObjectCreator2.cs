
using eDriven.Core.Events;
using eDriven.Core.Util;
using UnityEngine;
using Event=eDriven.Core.Events.Event;
using Random = System.Random;

public class ObjectCreator2 : MonoBehaviour
{
    public GameObject Prefab;
    private Timer _timer;

    // Use this for initialization
    // ReSharper disable UnusedMember.Local
    void Start()
    {
        // ReSharper restore UnusedMember.Local
        _timer = new Timer(1) {TickOnStart = true, RepeatCount = 10};
        //_timer.AddEventListener(Timer.EVENT_TICK, OnTimerTick);
        _timer.Tick += OnTimerTick;
        _timer.StopHandler += OnTimerStop;
        _timer.Start();
    }

    private void OnTimerTick(Event e)
    {
        Debug.Log("OnTimerTick");
        SpawnObject();
    }

    private void OnTimerStop(Event e)
    {
        Debug.Log("OnTimerStop");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cube");

		// destroy all Cubes
        foreach (GameObject o in gameObjects)
        {
            Destroy(o);
        }
		
		// start timer again
        _timer.Start();
    }

    private void SpawnObject()
    {
        // position
        float dx = Randomizer.RandomizeAround(0, 0.5f);
        float dy = Randomizer.RandomizeAround(0, 0.5f);
        float dz = Randomizer.RandomizeAround(0, 0.5f);
        Vector3 position = transform.position + new Vector3(dx, dy, dz);

        // instantiation
        GameObject go = (GameObject)Instantiate(Prefab, position, Quaternion.identity);

        // re-parenting
        go.transform.parent = transform;
    }
}
