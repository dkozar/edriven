using eDriven.Core.Util;
using eDriven.Tests.Core.EventDispatcher;
using UnityEngine;
using Event=eDriven.Core.Events.Event;

public class EventListenerScript2 : MonoBehaviour
{
    // ReSharper disable UnusedMember.Local
    void Start()
    // ReSharper restore UnusedMember.Local
    {
        //// find cube
        //GameObject go = GameObject.Find("EventDispatcher");

        //// reference dispatcher
        //EventDispatcherScript2 script = go.GetComponent<EventDispatcherScript2>();

        EventDispatcherScript2 script = ComponentUtil.ReferenceScript<EventDispatcherScript2>("EventDispatcher");

        #region Using AddEventListener method

        //// subscribe to event
        //script.AddEventListener(EventDispatcherScript.OBJECT_CLICKED, 
        //                        delegate(Event e)
        //                            {
        //                                GameObject target = ((GameObject) e.Target);
        //                                Debug.Log(string.Format(@"EventListenerScript2: event received: {0}", e));
        //                                Debug.Log(string.Format(@"Position: {0}", ((CustomEvent) e).Position));
        //                                Debug.Log(string.Format(@"target.name: {0}", target.name));

        //                                #region Action

        //                                // make action on this
        //                                iTween.PunchPosition(gameObject, new Vector3(0, 1f, 0), 2);
        //                                iTween.ColorTo(gameObject, Color.green, 1);

        //                                // make action on Target (originator)
        //                                iTween.PunchPosition(target, new Vector3(0, -1f, 0), 2);
        //                                iTween.ColorTo(target, Color.red, 1);

        //                                // play audio
        //                                AudioSource audioSource = GetComponent<AudioSource>();
        //                                audioSource.Play();

        //                                #endregion

        //                            }
        //);

        #endregion

        #region Using shorthand

        // subscribe to event
        script.ObjectClicked += delegate(Event e)
                                    {
                                        GameObject target = ((GameObject) e.Target);
                                        Debug.Log(string.Format(@"EventListenerScript2: event received: {0}", e));
                                        Debug.Log(string.Format(@"Position: {0}", ((CustomEvent) e).Position));
                                        Debug.Log(string.Format(@"target.name: {0}", target.name));

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
                                    };

        #endregion

    }
}