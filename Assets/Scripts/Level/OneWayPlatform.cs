using UnityEngine;
using System.Collections;

public class OneWayPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionExit2D(Collision2D coll) {
		if(coll.gameObject.tag == "Player")
		{
			GetComponent<PolygonCollider2D>().isTrigger = true;
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.gameObject.tag == "Player")
		{
			PlayerState state = other.gameObject.GetComponent<PlayerState>();
			if(state.Current != PlayerState.Types.Jump)
			{
				GetComponent<PolygonCollider2D>().isTrigger = false;
			}
		}
	}
}
