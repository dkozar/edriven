using UnityEngine;

public class MvcDemo : MonoBehaviour
{

    public GUISkin Skin;
    
    // ReSharper disable UnusedMember.Local
    // ReSharper disable InconsistentNaming
    void OnGUI()
// ReSharper restore UnusedMember.Local
// ReSharper restore InconsistentNaming
    {
        GUI.skin = Skin;

        GUI.Label(new Rect(10, 10, Screen.width, 40), "MVC Demo. Press Return or P to toggle pause. Press R to reset.");        
    }
}
