using UnityEngine;
using System.Collections;

public class EnemySplosion : MonoBehaviour {

	protected void Destroy()
	{
		Destroy(this.gameObject);
	}
}
