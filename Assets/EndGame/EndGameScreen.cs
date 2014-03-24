using UnityEngine;
using System.Collections;

public class EndGameScreen : MonoBehaviour {

	[SerializeField] private Rect _buttonRect = new Rect(38, 50, 25, 10);

	void OnGUI()
	{
		Rect rect = new Rect(Screen.width * _buttonRect.x / 100.0f,
		                     Screen.height * _buttonRect.y / 100.0f,
		                     Screen.width * _buttonRect.width / 100.0f,
		                     Screen.height * _buttonRect.height / 100.0f);
		if (GUI.Button(rect, "Restart"))
		{
			Application.LoadLevel("Scene");
		}
	}
}
