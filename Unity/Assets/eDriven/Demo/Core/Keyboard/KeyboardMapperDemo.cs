using eDriven.Core.Control.Keyboard;
using eDriven.Core.Events;
using UnityEngine;

public class KeyboardMapperDemo : MonoBehaviour
{
// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log(new eDriven.Core.Info());

        Debug.Log("Press CTRL+C or SHIFT+X");

        // CTRL + C, KEY_UP
        KeyboardMapper.Instance.Map(new KeyCombination(KeyboardEvent.KEY_UP, KeyCode.C, true, false, false), delegate(KeyboardEvent e)
        {
            Debug.Log(e.KeyCode);
        });

        // SHIFT+X, KEY_DOWN
        KeyboardMapper.Instance.Map(new KeyCombination(KeyboardEvent.KEY_DOWN, KeyCode.X, false, true, false), delegate(KeyboardEvent e)
        {
            Debug.Log(e.KeyCode);
        });
    }
}