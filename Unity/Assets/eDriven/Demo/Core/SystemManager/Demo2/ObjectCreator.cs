using eDriven.Core.Events;
using eDriven.Core.Managers;
using eDriven.Core.Util;
using UnityEngine;
using Event = UnityEngine.Event;

public class ObjectCreator : MonoBehaviour 
{
    public GameObject Prefab;

	// Use this for initialization
// ReSharper disable UnusedMember.Local
	void Start () {
// ReSharper restore UnusedMember.Local

        SystemManager.Instance.KeyUpSignal.Connect(KeyUpSlot);
	}
    
// ReSharper disable MemberCanBeMadeStatic.Local
    private void KeyUpSlot(object[] parameters)
// ReSharper restore MemberCanBeMadeStatic.Local
    {
        if (!enabled) return;

        Event @event = (Event)parameters[0];
        Debug.Log("KeyUpSlot: " + @event.keyCode);
        if (@event.keyCode == KeyCode.Return)
            SpawnObject();
        else if (@event.keyCode == KeyCode.R)
            Reset();
    }

    private void SpawnObject()
    {
        // position
        float dx = Randomizer.RandomizeAround(0, 0.5f);
        float dy = Randomizer.RandomizeAround(0, 0.5f);
        float dz = Randomizer.RandomizeAround(0, 0.5f);
        Vector3 position = transform.position + new Vector3(dx, dy, dz);

        // instantiation
        GameObject go = (GameObject) Instantiate(Prefab, position, Quaternion.identity);

        // re-parenting
        go.transform.parent = transform;
    }

    private static void Reset()
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