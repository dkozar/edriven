using eDriven.Core.Events;
using eDriven.Tests.Core.EventDispatcher;
using UnityEngine;

public class EventDispatcherScript2 : EventDispatcherComponent
{
    // ReSharper disable InconsistentNaming
    public const string OBJECT_CLICKED = "objectClicked";
// ReSharper restore InconsistentNaming

    #region Shorthands

    /// <summary>
    /// The event that fires when the key is down, if processKeys is enabled
    ///</summary>
    public MulticastDelegate ObjectClicked;

// ReSharper disable UnusedMember.Local
    void Start()
// ReSharper restore UnusedMember.Local
    {
        // keys
        ObjectClicked = new MulticastDelegate(this, OBJECT_CLICKED);
    }

    #endregion

// ReSharper disable UnusedMember.Local
    void OnMouseDown()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log("EventDispatcherScript2: dispatching event");
        
        // custom event
        CustomEvent customEvent = new CustomEvent(OBJECT_CLICKED);
        customEvent.Position = transform.position;
        DispatchEvent(customEvent);
    }
}