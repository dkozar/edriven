using UnityEngine;

public class TimerDemo2 : MonoBehaviour
{
    public GUISkin Skin;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
	void OnGUI()
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
    {
        GUI.skin = Skin;

        GUI.Label(new Rect(10, 10, Screen.width, 40), "Timer Demo");        
    }
}
