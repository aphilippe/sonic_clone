using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

	#region Fields

	[SerializeField] private float _destructiveForce = 175.0f;
	[SerializeField] private GameObject _splosion;

	#endregion

	#region Life cycle
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#endregion

	#region Collisions

	void OnCollisionEnter2D(Collision2D col)
	{

		if(col.gameObject.tag == "Player")
		{
			PlayerState playerState = col.gameObject.GetComponent<PlayerState> () as PlayerState;

			// If player is in attack mode (ball or jump)
			if(playerState.IsAttacking())
			{
				// player comming from above -> bump
				if(col.relativeVelocity.y > 0)
				{
					Rigidbody2D rigidbody = col.gameObject.rigidbody2D;
					rigidbody.AddForce(Vector2.up * _destructiveForce);
				}
				else
				{
					Rigidbody2D rigidbody = col.gameObject.rigidbody2D;
					rigidbody.velocity = -col.relativeVelocity;
				}

				this.Destruct();
			}
		}
	}

	#endregion

	#region Private

	private void Destruct()
	{
		Destroy(this.gameObject);
		Instantiate(_splosion, this.transform.position, this.transform.rotation);
	}

	#endregion
}
