using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	#region Internal types

	[System.Serializable]
	public class Gravity
	{
		public Vector2 direction;
		public float force;
	}

	#endregion

	#region Fields

	// Serialized field
	[SerializeField] private float _maxSpeed = 1.0f;
	[SerializeField] private Transform _groundCheck;
	[SerializeField] private LayerMask _groundMask;
	[SerializeField] private Gravity _gravity = new Gravity{ direction = new Vector2(0.0f, -1.0f), force = 1.0f };
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _acceleration;
	[SerializeField] private float _deceleration;
	[SerializeField] private float _breakForce;
	[SerializeField] private float _airAcceleration;
	[SerializeField] private Vector2 _hitJumpVector = new Vector2(-1.0f, .5f) * 400f;


	// Utils fields
	private bool _grounded;
	private float _groundRadius = 0.025f;
	private bool _jump;
	private float _currentSpeed = 0.0f;
	private int _direction = 1;

	private float _checkGroundInActive = 0.0f;

	// Other components
	private Rigidbody2D _rigidbody;
	private Transform _transform;
	private PlayerState _state;

	#endregion

	#region Properties

	public float MaxSpeed
	{
		get { return _maxSpeed; }
	}

	public float CurrentSpeed
	{
		get { return _currentSpeed; }
	}

	public bool Grounded
	{
		get { return _grounded; }
	}

	public bool Jump
	{
		get { return _jump; }
	}

	public int Direction
	{
		get { return _direction; }
	}

	#endregion

	#region Cycle life

	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_transform = GetComponent<Transform>();
		_state = this.GetComponent<PlayerState>();
		_state.Current = PlayerState.Types.Idle;
	}

	void Update()
	{
		if((_checkGroundInActive <= 0.0f && _rigidbody.velocity.y <= 0) || _grounded)
		{
			_grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundMask);
		}
		else
		{
			_grounded = false;
			_checkGroundInActive -= Time.deltaTime;
		}
		

		if(_grounded && _state.Current != PlayerState.Types.Hit)
		{
			this.RotateToGround();
			if(Input.GetButtonDown("Jump"))
			{
				_jump = true;
			}
		}
		else
		{
			_transform.rotation = Quaternion.identity;
		}

	}

	void FixedUpdate ()
	{
		_direction = this.GetDirection();

		if((_checkGroundInActive <= 0.0f && _rigidbody.velocity.y <= 0) || _grounded)
		{
			_grounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundMask);
		}
		else
		{
			_grounded = false;
			_checkGroundInActive -= Time.deltaTime;
		}


		if(_grounded) 
		{
			if(_jump)
			{
				_state.Current = PlayerState.Types.Jump;
				_jump = false;
				_rigidbody.AddForce(_transform.up * _jumpForce);
			}
			else if(_state.Current != PlayerState.Types.Hit)
			{
				this.updateSpeedGrounded(_direction);
				Vector2 movement = new Vector2(_transform.right.x, _transform.right.y) * _currentSpeed;
				_rigidbody.velocity = movement;

				_state.Current = (_currentSpeed != 0.0f) ? PlayerState.Types.Run : PlayerState.Types.Idle;
			}
			else
			{
				_currentSpeed = 0.0f;
				_state.Current = PlayerState.Types.Idle;
				_rigidbody.velocity = Vector2.zero;
			}
		}
		else 
		{
			this.updateSpeedInTheAir(_direction);

			// Change velocity
			Vector2 velocity = _rigidbody.velocity;
			velocity.x = _currentSpeed;
			_rigidbody.velocity = velocity;

			// Apply gravity
			_rigidbody.AddForce(_gravity.direction * _gravity.force);
		}
	}

	#endregion

	#region Public

	public void Hit(GameObject gameObject)
	{
		_state.Current = PlayerState.Types.Hit;
		_rigidbody.velocity = Vector2.zero;
		Vector2 force = new Vector2(_hitJumpVector.x, _hitJumpVector.y);
		force.x *= (gameObject.transform.position.x > _transform.position.x) ? 1 : -1;
		_rigidbody.AddForce(force);
		_checkGroundInActive = 0.25f;
	}

	#endregion

	#region Private

	/// <summary>
	/// Rotates the player to the normal of the ground
	/// </summary>
	private void RotateToGround()
	{
		Ray2D ray = new Ray2D();
		ray.origin = _transform.position;
		ray.direction = -_transform.up;

		RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, float.PositiveInfinity, _groundMask);

		if(hit)
		{
			Vector3 axis = Vector3.Cross(-_transform.up, -hit.normal);

			if(axis != Vector3.zero)
			{
				Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, hit.normal);
				transform.rotation = targetRot;
			}
		}
	}

	/// <summary>
	/// Updates the speed when the player is grounded.
	/// </summary>
	/// <param name="direction">The direction</param>
	private void updateSpeedGrounded(int direction)
	{
		float currentDirection = Mathf.Sign(_currentSpeed);

		if(direction == 0 && _currentSpeed == 0.0f)
		{
			return;
		}

		// Deceleration
		if(direction == 0 && _currentSpeed != 0.0f)
		{
			_currentSpeed = Mathf.Abs(_currentSpeed) - _deceleration;
			_currentSpeed = Mathf.Max(_currentSpeed, 0.0f) * currentDirection;
		}
		// Acceleration
		else if(direction == currentDirection || (_currentSpeed == 0 && direction != 0))
		{
			_currentSpeed = Mathf.Abs(_currentSpeed) + _acceleration;
			_currentSpeed = Mathf.Min(Mathf.Abs(_currentSpeed), _maxSpeed) * direction;
		}
		// Break
		else if(direction != currentDirection)
		{
			_currentSpeed = Mathf.Abs(_currentSpeed) - _breakForce;
			_currentSpeed = _currentSpeed * currentDirection;
		}
	}

	/// <summary>
	/// Updates the speed in the air.
	/// </summary>
	/// <param name="direction">The direction.</param>
	private void updateSpeedInTheAir(int direction)
	{
		_currentSpeed = _rigidbody.velocity.x + _airAcceleration * direction;

		_currentSpeed = Mathf.Clamp(_currentSpeed, -_maxSpeed, _maxSpeed);
	}

	/// <summary>
	/// Gets the direction from the horizontal axis from input.
	/// </summary>
	/// <returns>The direction.</returns>
	private int GetDirection()
	{
		float h = Input.GetAxisRaw("Horizontal");
		return (h != 0.0f) ? (int)Mathf.Sign(h) : 0;
	}

	#endregion
}
