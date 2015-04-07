using UnityEngine;
using Event=eDriven.Core.Events.Event;

namespace eDriven.Tests.Core.EventDispatcher
{
    public class CustomEvent : Event
    {
        public Vector3 Position;

        public CustomEvent(string type)
            : base(type)
        {
        }

        public CustomEvent(string type, object target)
            : base(type, target)
        {
        }

        public CustomEvent(string type, bool bubbles)
            : base(type, bubbles)
        {
        }
    }
}