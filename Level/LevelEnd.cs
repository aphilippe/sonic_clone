using UnityEngine;
using System.Collections;
using Event;

public class LevelEnd : MonoBehaviour {

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Player")
		{
			_animator.SetTrigger("Spin");
		}
	}

	void OnAnimationEnd()
	{
		EventManager.sharedManager.Raise(new WinGameEvent());
	}
}
