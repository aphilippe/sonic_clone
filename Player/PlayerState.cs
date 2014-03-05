using UnityEngine;
using System.Collections;
using System;

public delegate void PlayerStateChangeEventHandler(object source, PlayerStateChangeEventArgs e);
public class PlayerStateChangeEventArgs : EventArgs
{
	private PlayerState.Types _newState;
	private PlayerState.Types _oldState;

	public PlayerStateChangeEventArgs(PlayerState.Types newState, PlayerState.Types oldState)
	{
		_newState = newState;
		_oldState = oldState;
	}

	public PlayerState.Types NewState 
	{
		get{ return _newState; }
	}

	public PlayerState.Types OldState
	{
		get { return _oldState; }
	}
}

public class PlayerState : MonoBehaviour {

	#region inernal types

	public enum Types
	{
		Idle,
		Run,
		Jump,
		Ball,
		Hit
	}

	#endregion

	#region Fields

	private Types _current;
	private float _invincibleTimeRemaining;

	#endregion

	#region Events

	public event PlayerStateChangeEventHandler PlayerStateChanged;

	#endregion

	#region Properties

	public Types Current
	{
		get { return _current; }
		set 
		{ 
			if(_current != value)
			{
				PlayerStateChangeEventArgs eventArgs = new PlayerStateChangeEventArgs(value, _current);
				PlayerStateChanged(this, eventArgs);

				_current = value;

				switch(value)
				{
				case Types.Hit:
					_invincibleTimeRemaining = 5;
					break;
				}
			}
		}
	}

	#endregion

	#region Cycle life

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(_invincibleTimeRemaining > 0)
		{
			_invincibleTimeRemaining -= Time.deltaTime;
		}
	}

	#endregion

	#region Public
	
	public bool IsAttacking()
	{
		return _current == Types.Ball || _current == Types.Jump;
	}

	public bool IsInvincible()
	{
		return _invincibleTimeRemaining > 0;
	}

	#endregion
}
