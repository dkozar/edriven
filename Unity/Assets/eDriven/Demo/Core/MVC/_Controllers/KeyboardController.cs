using eDriven.Core.Managers;
using UnityEngine;
using Event = UnityEngine.Event;

public class KeyboardController : MonoBehaviour {

    public GUISkin Skin;

    // Use this for initialization
// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        SystemManager.Instance.KeyUpSignal.Connect(KeyUpSlot);
    }

// ReSharper disable MemberCanBeMadeStatic.Local
    private void KeyUpSlot(params object[] parameters)
// ReSharper restore MemberCanBeMadeStatic.Local
    {
        Event @event = (Event)parameters[0];
        Debug.Log("KeyUpSlot: " + @event.keyCode);

        switch (@event.keyCode)
        {
            case KeyCode.Return:
            case KeyCode.P:
                GameModel.Instance.Paused = !GameModel.Instance.Paused;
                break;
            case KeyCode.R:
                GameModel.Instance.Reset();
                break;
        }
    }
}
