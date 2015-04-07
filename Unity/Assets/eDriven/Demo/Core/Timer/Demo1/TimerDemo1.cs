using eDriven.Core.Util;
using UnityEngine;

public class TimerDemo1 : MonoBehaviour
{
    private Timer _timer;

// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log(new eDriven.Core.Info());
                               
        /**
         * Timer test
         * */
        _timer = new Timer(1);
        _timer.TickOnStart = true;
        _timer.Tick += delegate
                           {
                               Debug.Log("Timer tick");
                           };
        _timer.Start();
    }
}