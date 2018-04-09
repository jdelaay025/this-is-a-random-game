using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeWayPoint : MonoBehaviour 
{
	public static bool placed = false;
	public AudioClip clicked;

	Image image;
	Color initialColor;
	Color fadedColor;
	float alphaValue = 3f;
	AudioSource sounds;

	void Awake () 
	{
		image = GetComponent<Image> ();
		sounds = GetComponent<AudioSource> ();
		initialColor = image.color;

	}
	void Start () 
	{
		alphaValue = 0f;
		image.color = fadedColor;
	}


	void Update () 
	{
		fadedColor = new Color(1f, 1f, 1f, alphaValue);

		if(placed)
		{
			sounds.PlayOneShot (clicked, 0.2f);
			image.color = initialColor;
			alphaValue = 1.1f;
			placed = false;
		}
		else if(!placed)
		{
			image.color = fadedColor;

			if(alphaValue > 0)
			{
				alphaValue -= Time.deltaTime * 2f;
			}
		}
	}
}
