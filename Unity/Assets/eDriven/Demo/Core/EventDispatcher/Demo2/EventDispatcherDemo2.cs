using UnityEngine;

public class EventDispatcherDemo2 : MonoBehaviour
{
    public GUISkin Skin;

// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log(new eDriven.Core.Info());
    }

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
    void OnGUI()
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
    {
        GUI.skin = Skin;

        GUI.Label(new Rect(10, 10, Screen.width, 40), "EventDispatcherDemo2. Click a cube on the left");
    }
}