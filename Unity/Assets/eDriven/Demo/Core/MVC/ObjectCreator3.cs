using eDriven.Core.Events;
using eDriven.Core.Util;
using UnityEngine;
using Event=eDriven.Core.Events.Event;
using Random = System.Random;

public class ObjectCreator3 : MonoBehaviour
{

    #region Properties

    // ReSharper disable InconsistentNaming
    // ReSharper disable UnassignedField.Global
    public GameObject Prefab;
    //public bool sendMessageOnEnter = false;
    // ReSharper restore InconsistentNaming

    #endregion

    #region Members

    private Timer _timer;

    #endregion

    // Use this for initialization
    // ReSharper disable UnusedMember.Local
    void Start()
    {
        //Timer.DebugOn = true;

        // ReSharper restore UnusedMember.Local
        _timer = new Timer(1) {TickOnStart = true};
        _timer.Tick += OnTimerTick;
        _timer.StopHandler += OnTimerStop;
        
		if (!GameModel.Instance.Paused)		
			_timer.Start();

        GameModel.Instance.AddEventListener(GameModel.PAUSE_CHANGED, OnPauseChanged);
        GameModel.Instance.AddEventListener(GameModel.RESET, OnReset);
    }

    private void OnPauseChanged(Event e)
    {
        bool paused = (bool)((ValueEvent) e).Value;
        Debug.Log("OnPauseChanged. Paused: " + paused);
        if (paused)
            _timer.Stop();
        else 
            _timer.Start();
    }

    private void OnTimerTick(Event e)
    {
        Debug.Log("OnTimerTick");
        SpawnObject();
    }

    private static void OnTimerStop(Event e)
    {
        Debug.Log("OnTimerStop");
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

    private static void OnReset(Event e)
    {
        Debug.Log("OnReset");
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Cube");

        // destroy all Cubes
        foreach (GameObject o in gameObjects)
        {
            Destroy(o);
        }
    }
}
