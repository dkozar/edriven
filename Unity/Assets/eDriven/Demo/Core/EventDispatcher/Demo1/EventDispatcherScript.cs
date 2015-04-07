using eDriven.Core.Events;
using eDriven.Tests.Core.EventDispatcher;
using UnityEngine;

public class EventDispatcherScript : MonoBehaviour
{
    public EventDispatcher Dispatcher = new EventDispatcher();

// ReSharper disable InconsistentNaming
    public const string OBJECT_CLICKED = "objectClicked";
// ReSharper restore InconsistentNaming

// ReSharper disable UnusedMember.Local
    void OnMouseDown()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log("EventDispatcherScript: dispatching event");
        
        //Dispatcher.DispatchEvent(new Event(OBJECT_CLICKED, (object)gameObject)); // setting this.gameObject as Target

        // custom event
        CustomEvent customEvent = new CustomEvent(OBJECT_CLICKED, (object) gameObject);
        customEvent.Position = transform.position;
        Dispatcher.DispatchEvent(customEvent);
    }
}