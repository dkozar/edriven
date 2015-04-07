using eDriven.Core.Events;
using UnityEngine;
using Event=eDriven.Core.Events.Event;

public class GameStateView : MonoBehaviour {

    public GUISkin Skin;

	// Use this for initialization
// ReSharper disable UnusedMember.Local
	void Start () {
// ReSharper restore UnusedMember.Local
        GameModel.Instance.AddEventListener(GameModel.NUMBER_OF_LIVES_CHANGED, OnNumberOfLivesChanged);
        GameModel.Instance.AddEventListener(GameModel.SCENE_CHANGED, OnSceneChanged);
        GameModel.Instance.AddEventListener(GameModel.PAUSE_CHANGED, OnPauseChanged);
	}

// ReSharper disable MemberCanBeMadeStatic.Local
    private void OnPauseChanged(Event e)
// ReSharper restore MemberCanBeMadeStatic.Local
    {
        bool paused = (bool)((ValueEvent)e).Value;
        Debug.Log("OnPauseChanged. Paused: " + paused);
		audio.Play();
    }

// ReSharper disable MemberCanBeMadeStatic.Local
    private void OnNumberOfLivesChanged(Event e)
// ReSharper restore MemberCanBeMadeStatic.Local
    {
        Debug.Log("OnNumberOfLivesChanged");
    }

// ReSharper disable MemberCanBeMadeStatic.Local
    private void OnSceneChanged(Event e)
// ReSharper restore MemberCanBeMadeStatic.Local
    {
        Debug.Log("OnSceneChanged");
    }

    // Update is called once per frame
// ReSharper disable UnusedMember.Local
	void Update () {
// ReSharper restore UnusedMember.Local
	    
	}

    public float LabelWidth = 200;
    public float LabelHeight = 80;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Local
    void OnGUI()
// ReSharper restore UnusedMember.Local
// ReSharper restore InconsistentNaming
    {
        GUI.skin = Skin;

        if (GameModel.Instance.Paused)
        {
            float sw = Screen.width;
            float sh = Screen.height;
            Rect r = new Rect((sw - LabelWidth) / 2, (sh - LabelHeight) / 2, LabelWidth, LabelHeight);
            GUI.Box(r, "Paused");
        }
    }
}
