using UnityEngine;

/// <summary>
/// MouseLook moded by Danko Kozar
/// </summary>
/// <remarks>Moded by Danko Kozar</remarks>
[AddComponentMenu("Camera-Control/Mouse Look 2")]
public class MouseLook2 : MonoBehaviour {

	public enum RotationAxes
	{
	    MouseXAndY = 0, MouseX = 1, MouseY = 2
	}
    
    public enum MouseLookMode
    {
        Always, LeftMouseButton, RightMouseButton, MiddleMouseButton
    }

	public RotationAxes Axes = RotationAxes.MouseXAndY;

    public MouseLookMode Mode = MouseLookMode.Always;

	public float SensitivityX = 15F;
	public float SensitivityY = 15F;

	public float MinimumX = -360F;
	public float MaximumX = 360F;

	public float MinimumY = -60F;
	public float MaximumY = 60F;

	float _rotationX;
	float _rotationY;
	
	Quaternion _originalRotation;

// ReSharper disable UnusedMember.Local
	void Update ()
// ReSharper restore UnusedMember.Local
	{
	    bool doProcess = false;
	    switch (Mode)
	    {
	        case MouseLookMode.Always:
	            doProcess = true;
	            break;
            case MouseLookMode.LeftMouseButton:
                if (Input.GetMouseButton(0))
                    doProcess = true;
                break;
            case MouseLookMode.RightMouseButton:
                if (Input.GetMouseButton(1))
                    doProcess = true;
	            break;
	        case MouseLookMode.MiddleMouseButton:
                if (Input.GetMouseButton(2))
                    doProcess = true;
                break;
	    }

        if (!doProcess)
            return;

		if (Axes == RotationAxes.MouseXAndY)
		{
			// Read the mouse input axis
			_rotationX += Input.GetAxis("Mouse X") * SensitivityX;
			_rotationY += Input.GetAxis("Mouse Y") * SensitivityY;

			_rotationX = ClampAngle (_rotationX, MinimumX, MaximumX);
			_rotationY = ClampAngle (_rotationY, MinimumY, MaximumY);
			
			Quaternion xQuaternion = Quaternion.AngleAxis (_rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis (_rotationY, Vector3.left);
			
			transform.localRotation = _originalRotation * xQuaternion * yQuaternion;
		}
		else if (Axes == RotationAxes.MouseX)
		{
			_rotationX += Input.GetAxis("Mouse X") * SensitivityX;
			_rotationX = ClampAngle (_rotationX, MinimumX, MaximumX);

			Quaternion xQuaternion = Quaternion.AngleAxis (_rotationX, Vector3.up);
			transform.localRotation = _originalRotation * xQuaternion;
		}
		else
		{
			_rotationY += Input.GetAxis("Mouse Y") * SensitivityY;
			_rotationY = ClampAngle (_rotationY, MinimumY, MaximumY);

			Quaternion yQuaternion = Quaternion.AngleAxis (_rotationY, Vector3.left);
			transform.localRotation = _originalRotation * yQuaternion;
		}
	}
	
// ReSharper disable UnusedMember.Local
	void Start ()
// ReSharper restore UnusedMember.Local
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		_originalRotation = transform.localRotation;
	}
	
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}