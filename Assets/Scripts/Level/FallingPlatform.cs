using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour {

	[SerializeField] private float _fallingSpeed;
	[SerializeField] private float _timerBeforeFall;

	private bool _isFalling;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(_isFalling)
		{
			_timerBeforeFall -= Time.deltaTime;
			if(_timerBeforeFall <= 0)
			{
				this.rigidbody2D.velocity = Vector2.up *  -_fallingSpeed;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			_isFalling = true;		
		}
	}
}
