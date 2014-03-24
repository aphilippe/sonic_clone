using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectDestroyer : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.tag == "Destroyable")
		{
			Destroy(other.gameObject);
		}
		else if(other.gameObject.tag == "Player")
		{
			Event.EventManager.sharedManager.Raise(new Event.WinGameEvent());
		}

	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.tag == "Destroyable")
		{
			Destroy(coll.gameObject);
		}
		else if(coll.gameObject.tag == "Player")
		{
			Event.EventManager.sharedManager.Raise(new Event.WinGameEvent());
		}
	}
}
