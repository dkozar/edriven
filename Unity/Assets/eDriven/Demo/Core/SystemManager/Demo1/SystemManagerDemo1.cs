using eDriven.Core.Managers;
using UnityEngine;

public class SystemManagerDemo1 : MonoBehaviour
{
// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        SystemManager.Instance.MouseDownSignal.Connect(print);
        SystemManager.Instance.MouseUpSignal.Connect(print);
        SystemManager.Instance.MouseMoveSignal.Connect(print);
        SystemManager.Instance.MouseDragSignal.Connect(print);
        SystemManager.Instance.KeyUpSignal.Connect(print);
        SystemManager.Instance.KeyDownSignal.Connect(print);
        SystemManager.Instance.ResizeSignal.Connect(print);
    }
}