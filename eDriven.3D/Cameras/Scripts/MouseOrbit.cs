#region License

/*
 
Copyright (c) 2010-2014 Danko Kozar

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
 
*/

#endregion License

using UnityEngine;

namespace eDriven.Cameras.Scripts
{
    public class MouseOrbit : MonoBehaviour
    {
        public Transform Target;
        private const float Distance = 10.0f;

        private const float XSpeed = 250.0f;
        private const float YSpeed = 120.0f;

        private const float YMinLimit = -20;
        private const float YMaxLimit = 80;

        private float _x;
        private float _y;

// ReSharper disable UnusedMember.Local
        void Start () {
// ReSharper restore UnusedMember.Local
            Vector3 angles = transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;

            // Make the rigid body not change rotation
            if (rigidbody)
                rigidbody.freezeRotation = true;
        }

// ReSharper disable UnusedMember.Local
        void LateUpdate () {
// ReSharper restore UnusedMember.Local
            if (null != Target)
            {
                _x += Input.GetAxis("Mouse X") * XSpeed * 0.02f;
                _y -= Input.GetAxis("Mouse Y") * YSpeed * 0.02f;
     		
                _y = ClampAngle(_y, YMinLimit, YMaxLimit);
     		       
                Quaternion rotation = Quaternion.Euler(_y, _x, 0);
                Vector3 position = rotation * new Vector3(0.0f, 0.0f, -Distance) + Target.position;
            
                transform.rotation = rotation;
                transform.position = position;
            }
        }

        private static float ClampAngle (float angle, float min, float max) {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp (angle, min, max);
        }
    }
}