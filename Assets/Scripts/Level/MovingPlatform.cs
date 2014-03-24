using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

	#region internal Types

	[SerializeField]
	enum Destination
	{
		limit_1,
		limit_2
	}

	#endregion

	#region Fieds
	[SerializeField] Transform _limit_1;
	[SerializeField] Transform _limit_2;
	[SerializeField] Destination _direction;
	[SerializeField] float _animationDuretion = 10.0f;

	Transform _transform;
	Vector2 _currentDestination;
	float _destDistance = 0.01f;
	float _startTime;
	GameObject _player = null;
	#endregion

	#region Properties

	private Destination Direction
	{
		get { return _direction; }
		set 
		{
			_direction = value;
			_currentDestination = (_direction == Destination.limit_1) ? _limit_1.position : _limit_2.position;
			_startTime = Time.time;
		}
	}

	#endregion

	// Use this for initialization
	void Start () {
		_transform = GetComponent<Transform>();
		_currentDestination = (_direction == Destination.limit_1) ? _limit_1.position : _limit_2.position;
	}
	
	// Update is called once per frame
	void Update () {

		if(Vector2.Distance(_transform.position, _currentDestination) <= _destDistance)
		{
			this.Direction = (this.Direction == Destination.limit_1) ? Destination.limit_2 : Destination.limit_1;
		}

		float distCovered = (Time.time - _startTime);
		float fracJourney = distCovered / _animationDuretion;

		float value = fracJourney;
		// This line in inspired by Mathfx : http://wiki.unity3d.com/index.php?title=Mathfx
		Vector3 newPosition = Vector2.Lerp(_transform.position, _currentDestination, value * value * (3.0f - 2.0f * value));

		if(_player != null && _player.GetComponent<PlayerController>().Grounded)
		{
			Debug.Log("PLOOOP");
			Vector3 delta = newPosition - _transform.position;
			_player.transform.Translate(delta);
		}

		_transform.position = newPosition;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			_player = other.gameObject;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			_player = null;
		}
	}
}
