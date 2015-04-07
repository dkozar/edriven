using eDriven.Core.Util;
using eDriven.Tests.Core.EventDispatcher;
using UnityEngine;
using Event=eDriven.Core.Events.Event;

public class EventListenerScript : MonoBehaviour
{
    // ReSharper disable UnusedMember.Local
    void Start()
    // ReSharper restore UnusedMember.Local
    {
        //// find cube
        //GameObject go = GameObject.Find("EventDispatcher");

        //// reference dispatcher
        //EventDispatcherScript script = go.GetComponent<EventDispatcherScript>();

        EventDispatcherScript script = ComponentUtil.ReferenceScript<EventDispatcherScript>("EventDispatcher");

        // subscribe to event
        script.Dispatcher.AddEventListener(EventDispatcherScript.OBJECT_CLICKED, 
            delegate(Event e)
            {
                GameObject target = ((GameObject) e.Target);
                Debug.Log(string.Format(@"EventListenerScript: event received: {0}", e));
                Debug.Log(string.Format(@"Position: {0}", ((CustomEvent) e).Position));

                #region Action

				// make action on this
                iTween.PunchPosition(gameObject, new Vector3(0, 1f, 0), 2);
                iTween.ColorTo(gameObject, Color.green, 1);

                // make action on Target (originator)
                iTween.PunchPosition(target, new Vector3(0, -1f, 0), 2);
                iTween.ColorTo(target, Color.red, 1);

                // play audio
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.Play();

                #endregion

            }
        );
    }
}