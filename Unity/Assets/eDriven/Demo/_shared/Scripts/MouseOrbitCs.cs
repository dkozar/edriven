using UnityEngine;

public class MouseOrbitCs : MonoBehaviour
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