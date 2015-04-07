using UnityEngine;

public class ChangeWatcherScript : MonoBehaviour
{
    private GameObject _go;
    private WatchTargetScript _script;

    // ReSharper disable UnusedMember.Local
    void Start()
    // ReSharper restore UnusedMember.Local
    {
        // find cube
        _go = GameObject.Find("WatchTarget");

        // reference dispatcher
        _script = _go.GetComponent<WatchTargetScript>();
    }
	
// ReSharper disable UnusedMember.Local
	void Update()
// ReSharper restore UnusedMember.Local
	{
        if (null != _script && _script.Clicked)
	    {
	        _script.Clicked = false; // Note: resetting a flag on another object!

            Debug.Log(string.Format(@"Position: {0}", _go.transform.position));

            #region Action

            // make action on this
	        iTween.PunchPosition(gameObject, new Vector3(0, 1f, 0), 2);
	        iTween.ColorTo(gameObject, Color.green, 1);

	        // make action on Target (originator)
	        iTween.PunchPosition(_go, new Vector3(0, -1f, 0), 2);
	        iTween.ColorTo(_go, Color.red, 1);

	        // play audio
	        AudioSource audioSource = GetComponent<AudioSource>();
	        audioSource.Play();

	        #endregion

	    }
	}
}