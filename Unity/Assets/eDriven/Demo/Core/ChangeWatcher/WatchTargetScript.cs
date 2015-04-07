using UnityEngine;

public class WatchTargetScript : MonoBehaviour
{
	public bool Clicked;
	
// ReSharper disable UnusedMember.Local
    void OnMouseDown()
// ReSharper restore UnusedMember.Local
    {
        Debug.Log("WatchTargetScript.OnMouseDown");

        Clicked = true; // Note: setting a flag for 'observable'
    }
}