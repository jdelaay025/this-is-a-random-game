using UnityEngine;
using System.Collections;

public class CharacterAudioManager : MonoBehaviour 
{
	#region Global Variable Declaration

	[Header("Audio Sources")]
	public AudioSource gunSounds;
	public AudioSource runFoley;
	public AudioSource footStep1;
	public AudioSource footStep2;
	public AudioSource effectsSource;

	[Header("Regular Clips and Effect Clips")]
	public AudioClip[] footStepClips;
	public AudioClipsList[] effectsList;

	[Header("Audio Control Variables")]
	public float footStepTimer;
	public float walkThreshold;
	public float runThreshold;

	StateManager states;

	float startingVolumeSprint;
	float startingVolumeJog;
	float characterMovement;

	#endregion

	void Start () 
	{
		states = GetComponent<StateManager> ();

		startingVolumeSprint = runFoley.volume;
		runFoley.volume = 0;

		startingVolumeJog = footStep1.volume;

	}

	void Update () 
	{
		characterMovement = Mathf.Abs (states.horizontal) + Mathf.Abs (states.vertical);
		characterMovement = Mathf.Clamp01 (characterMovement);

		float targetThreshold = 0f;

		if (states.sprint && !states.aiming && !states.reloading)
		{
			runFoley.volume = startingVolumeSprint * characterMovement;
			targetThreshold = runThreshold;
		} 
		else 
		{
			targetThreshold = walkThreshold;

			runFoley.volume = Mathf.Lerp (runFoley.volume, 0f, Time.deltaTime * 2);
		}

		if(characterMovement > 0 && states.onGround)
		{
			footStepTimer += Time.deltaTime;

			if(footStepTimer > targetThreshold)
			{
				PlayFootStep ();
				footStepTimer = 0;
			}
		}
		else
		{
			footStep1.Stop ();
			footStep2.Stop ();
		}
	}

	public void PlayGunSound()
	{
		gunSounds.Play ();
	}

	public void PlayFootStep()
	{
		if(!footStep1.isPlaying)
		{
			int ran = Random.Range (0, footStepClips.Length);
			footStep1.clip = footStepClips[ran];

			footStep1.Play ();
		}	
		else
		{
			if(!footStep2.isPlaying)
			{
				int ran2 = Random.Range (0, footStepClips.Length);
				footStep2.clip = footStepClips[ran2];

				footStep2.Play ();
			}
		}
	}

	public void PlayEffect(string name)
	{
		AudioClip clip = null;

		for (int i = 0; i < effectsList.Length; i++) 
		{
			if(string.Equals(effectsList[i].name, name))
			{
				clip = effectsList[i].clip;
				break;
			}
		}

		effectsSource.clip = clip;

	}
}

[System.Serializable]
public class AudioClipsList 
{
	public string name = string.Empty;
	public AudioClip clip;
}
