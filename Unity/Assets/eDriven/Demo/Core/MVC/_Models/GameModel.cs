using eDriven.Core.Events;

public class GameModel : EventDispatcher
{
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
	public const string NUMBER_OF_LIVES_CHANGED = "numberOfLivesChanged";
	public const string SCENE_CHANGED = "sceneChanged";
	public const string PAUSE_CHANGED = "pauseChanged";
	public const string RESET = "reset";
// ReSharper restore InconsistentNaming
// ReSharper restore UnusedMember.Global

	#region Singleton

	private static GameModel _instance;

	private GameModel()
	{
		// constructor is protected!
	}

	/// <summary>
	/// Singleton instance
	/// </summary>
	public static GameModel Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameModel();
				_instance.Initialize();
			}

			return _instance;
		}
	}

	/// <summary>
	/// Initialization routine
	/// Put inside the initialization stuff, if needed
	/// </summary>
	private void Initialize()
	{
		// initialization here
	}

	#endregion

	private bool _paused = true;
	/// <summary>
	/// The example of event dispatching property
	/// </summary>
	public bool Paused
	{
		get
		{
			return _paused;
		}
		set
		{
			if (value != _paused)
			{
				_paused = value;
				ValueEvent ve = new ValueEvent(PAUSE_CHANGED);
				ve.Value = _paused;
				DispatchEvent(ve);
			}
		}
	}

	/// <summary>
	/// The example of event dispatching method
	/// </summary>
	public void Reset()
	{
		ValueEvent ve = new ValueEvent(RESET);
		DispatchEvent(ve);
	}

	#region Test properties

	private float _lives = 3;
	public float Lives
	{
		get
		{
			return _lives;
		}
		set
		{
			if (value != _lives)
			{
				_lives = value;
				ValueEvent ve = new ValueEvent(NUMBER_OF_LIVES_CHANGED);
				ve.Value = _lives;
				DispatchEvent(ve);
			}
		}
	}

	private string _scene;
	public string Scene
	{
		get
		{
			return _scene;
		}
		set
		{
			if (value != _scene)
			{
				_scene = value;
				ValueEvent ve = new ValueEvent(SCENE_CHANGED);
				ve.Value = _scene;
				DispatchEvent(ve);
			}
		}
	}

	#endregion

}