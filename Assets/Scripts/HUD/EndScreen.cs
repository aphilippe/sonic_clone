using UnityEngine;
using System.Collections;
using Event;


public class EndScreen : MonoBehaviour {

	private float _timer = 2f;
	private bool _timerStarted = false;

	// Use this for initialization
	void Start () {
		EventManager.sharedManager.AddListener<WinGameEvent>(OnWinGameEvent);

	}
	
	// Update is called once per frame
	void Update () {
		if(_timerStarted)
		{
			_timer -= Time.deltaTime;

			if(_timer <= 0.0f)
			{
				Application.LoadLevel("EndGameScene");
			}
		}
	}

	void OnDestroy()
	{
		EventManager.sharedManager.RemoveListener<WinGameEvent>(OnWinGameEvent);
	}

	void OnWinGameEvent(WinGameEvent e)
	{
		FadeInOut fade = GetComponent<FadeInOut>();
		fade.StartFade(FadeInOut.FadeDirection.FadeOut);
		_timerStarted = true;
	}
}
