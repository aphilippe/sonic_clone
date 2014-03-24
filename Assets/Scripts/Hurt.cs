using UnityEngine;
using System.Collections;

public class Hurt : MonoBehaviour {

	#region Fields

	private bool _isDestructible;

	#endregion

	#region Life cycle

	void Start () 
	{
		_isDestructible = GetComponent<Destructible>() != null;
	}
	

	void Update () 
	{
		
	}

	#endregion

	#region Collisions

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			PlayerState playerState = col.gameObject.GetComponent<PlayerState>();
			if(!_isDestructible || !playerState.IsAttacking())
			{
				this.hitPlayer(col.gameObject);
			}
		}
	}

	#endregion

	#region Private

	private void hitPlayer(GameObject player)
	{
		PlayerController playerController = player.GetComponent<PlayerController>();
		playerController.Hit(this.gameObject);
	}

	#endregion
}
