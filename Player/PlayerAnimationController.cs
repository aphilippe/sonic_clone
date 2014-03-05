using UnityEngine;
using System.Collections;

// TODO: Update animator and animation with the player controller status
[RequireComponent(typeof(PlayerController)), RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour {

	#region Fields


	// Utils fields
	private int _previousDirection = 1;

	// Other components
	private Animator _animator;
	private PlayerController _controller;
	private Transform _transform;
	private PlayerState _playerState;
	#endregion

	#region Life Cycle
	void Start () 
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<PlayerController>();
		_transform = GetComponent<Transform>();

		_playerState = GetComponent<PlayerState>();
		_playerState.PlayerStateChanged += new PlayerStateChangeEventHandler(OnPlayerStateChanged);
	}

	void LateUpdate ()
	{
		_animator.SetFloat("Speed", Mathf.Abs(_controller.CurrentSpeed / _controller.MaxSpeed));

		if(!_playerState.IsInvincible() && animation.isPlaying)
		{
			animation.Stop();
			GetComponent<SpriteRenderer>().enabled = true;
		}

		this.UpdateDirection();
	}
	#endregion

	#region Events handle

	private void OnPlayerStateChanged(object source, PlayerStateChangeEventArgs e)
	{
		Debug.Log(e.OldState + " -> " + e.NewState);
		switch(e.NewState)
		{
		case PlayerState.Types.Jump:
			_animator.SetTrigger("Jump");
			break;
		case PlayerState.Types.Idle:
			_animator.SetTrigger("Idle");
			break;
		case PlayerState.Types.Run:
			_animator.SetTrigger("Run");
			break;
		case PlayerState.Types.Hit:
			_animator.SetTrigger("Hit");
			animation.Play("Blink");
			break;
		}
	}

	#endregion

	#region Private

	private void UpdateDirection()
	{
		int currentDirection = _controller.Direction;
		if(currentDirection == 0)
		{
			return;
		}

		if(currentDirection != _previousDirection)
		{
			Vector3 scale = _transform.localScale;
			scale.x *= -1;
			_transform.localScale = scale;
		}
		_previousDirection = currentDirection;
	}

	#endregion
}
