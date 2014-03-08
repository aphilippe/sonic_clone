using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FadeInOut fade = GetComponent<FadeInOut>();
		fade.StartFade(FadeInOut.FadeDirection.FadeIn);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
