using UnityEngine;
using System.Collections;

public class FadeInOut : MonoBehaviour {

	#region internal types

	public enum FadeDirection
	{
		FadeIn = -1,
		FadeOut = 1
	}

	#endregion

	#region Fields

	[SerializeField] private SpriteRenderer _sprite;
	[SerializeField] private float _speed = 0.3f;

	private float _alpha = 1.0f;
	private int _fadeDir;

	#endregion

	#region Cycle life

	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		_alpha += _fadeDir * _speed * Time.deltaTime;
		_alpha = Mathf.Clamp01(_alpha);


		Color color = _sprite.color;
		color.a = _alpha;
		_sprite.color = color;
	}

	#endregion

	#region public

	public void StartFade(FadeDirection direction)
	{
		_fadeDir = (int)direction;
	}

	#endregion
}
