using UnityEngine;

public class SystemManagerDemo2 : MonoBehaviour
{
    public GUISkin Skin;

// ReSharper disable UnusedMember.Local
// ReSharper disable InconsistentNaming
	void OnGUI()
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Local
    {
        GUI.skin = Skin;

        GUI.Label(new Rect(10, 10, Screen.width, 40), "SystemManager Demo. Press RETURN to instantiate. Press R to reset.");     
    }
}