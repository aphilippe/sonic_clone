using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraController))]
public class CameraFollower : MonoBehaviour {

	[SerializeField]
	private Transform _followedObject;

	private Transform _transform;
	private CameraController _cameraController;

	// Use this for initialization
	void Start () {
		_transform = this.transform;
		_cameraController = GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 currentPosition = _transform.position;
		Vector2 wantedPosition = _followedObject.position;

		Vector2 deltaMovement = wantedPosition - currentPosition;

		_cameraController.move(deltaMovement);
	}
}
