using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlPointScript : MonoBehaviour 
{
	[Header("UI Slider Info")]
	public Slider playerCapSlider;
	public Slider enemyCapSlider;
	public Text sliderValue;
	public Text heroesSliderValue;
	public Text enemySliderValue;
	public float maxValue = 100f;
	public float currentPlayerValue = 0f;
	public float currentEnemyValue = 0f;
	public float countMultiplier = 9f;
	public float countPitchMultiplier = 0f;

	public ParticleSystem capParticle;
	public float chargeLevel = 0f;

	Color fadedColor;
	Color initialColor = new Color(1f, 1f, 1f, 1f);

//	Transform myTransform;

	public static bool playerHere = false;
	public static bool enemyHere = false;
	float alphaValue = 3f;
	Renderer rend;
	bool isCapped = false;
	float inactiveTimer = 5f;
	float respawnTimer = 5f;
	int nextPoint = 0;
	AudioSource sounds;
	float chargeSound = 0f;
	float chargePitch = 0f;

	void Awake () 
	{
//		myTransform = transform;
		rend = GetComponentInChildren<Renderer> ();
		sounds = GetComponent<AudioSource> ();

		alphaValue = 0f;

		inactiveTimer = 0.25f;
	}

	void Start()
	{
		rend.material.SetColor("_TintColor", Color.yellow);
		sliderValue.color = initialColor;

		GameMasterObject.CapturePoint = transform;

		sounds.volume = 0f;

		countPitchMultiplier = countMultiplier * 0.17f;
	}

	void Update () 
	{
		float rotateSpeed = 0;

		if (enemyHere && !playerHere) 
		{
			if (currentEnemyValue <= 0)
				rotateSpeed = 0.25f;
			else if (currentEnemyValue <= 10 && currentEnemyValue > 0)
				rotateSpeed = 1;
			else if (currentEnemyValue <= 25 && currentEnemyValue > 10)
				rotateSpeed = 1.5f;
			else if (currentEnemyValue <= 50 && currentEnemyValue > 25)
				rotateSpeed = 2.75f;
			else if (currentEnemyValue <= 75 && currentEnemyValue > 50)
				rotateSpeed = 3.5f;
			else if (currentEnemyValue <= 85 && currentEnemyValue > 75)
				rotateSpeed = 4.5f;
			else if (currentEnemyValue > 85)
				rotateSpeed = 5.5f;
		} 
		else if (!enemyHere && playerHere) 
		{
			if (currentPlayerValue <= 0)
				rotateSpeed = 0.25f;
			else if (currentPlayerValue <= 10 && currentPlayerValue > 0)
				rotateSpeed = 1;
			else if (currentPlayerValue <= 25 && currentPlayerValue > 10)
				rotateSpeed = 1.5f;
			else if (currentPlayerValue <= 50 && currentPlayerValue > 25)
				rotateSpeed = 2.75f;
			else if (currentPlayerValue <= 75 && currentPlayerValue > 50)
				rotateSpeed = 3.5f;
			else if (currentPlayerValue <= 85 && currentPlayerValue > 75)
				rotateSpeed = 4.5f;
			else if (currentPlayerValue > 85)
				rotateSpeed = 5.5f;
		} 
		else if (!enemyHere && !playerHere) 
		{
			rotateSpeed = 0.25f;
		}



		transform.rotation *= Quaternion.Euler (0f, 90f * Time.deltaTime * rotateSpeed, 0f);
		fadedColor = new Color(1f, 1f, 1f, alphaValue);

//		Debug.Log (((currentPlayerValue * countPitchMultiplier) * 0.07f));
//		Debug.Log ();

		sounds.volume = chargeSound;
		sounds.pitch = chargePitch;

		if(isCapped)
		{
			if(inactiveTimer > 0)
			{
				inactiveTimer -= Time.deltaTime;
			}
			else if(inactiveTimer <= 0)
			{
				GameMasterObject.changePostion = true;
				SetToInactive ();
			}
		}

		#region Find Player and Enemy Sliders

		if(playerCapSlider == null)
		{
			playerCapSlider = GameMasterObject.heroesSlider;

			if(playerCapSlider != null)
			{
				playerCapSlider.maxValue = maxValue;
				playerCapSlider.value = 0f;
			}
		}
		if(enemyCapSlider == null)
		{
			enemyCapSlider = GameMasterObject.enemiesSlider;

			if(enemyCapSlider != null)
			{
				enemyCapSlider.maxValue = maxValue;
				enemyCapSlider.value = 0f;
			}
		}

		#endregion

		if (playerCapSlider != null && enemyCapSlider != null) 
		{
			playerCapSlider.value = currentPlayerValue;
			enemyCapSlider.value = currentEnemyValue;

			if (playerHere) 
			{
				if (currentPlayerValue < 100f) 
				{
					currentPlayerValue += Time.deltaTime * countMultiplier;
					sliderValue.text = ((int)currentPlayerValue).ToString();

					sliderValue.color = initialColor;
					alphaValue = 1f;

					chargePitch = ((currentPlayerValue * countPitchMultiplier) * 0.07f);
					chargePitch = Mathf.Clamp (chargePitch, 0.7f, 5f);

					chargeSound = chargeLevel;
				} 
				else if (currentPlayerValue >= 100f) 
				{
					rend.material.SetColor ("_TintColor", Color.blue);
					currentEnemyValue = 0f;

					sliderValue.color = fadedColor;

					if(alphaValue > 0)
					{
						alphaValue -= Time.deltaTime;
					}

					if(!isCapped)
					{
						
						GameMasterObject.heroPoints++;
						if(capParticle != null)
						{
							Instantiate (capParticle, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
						}

						isCapped = true;
					}
				}
			} 
			else if (enemyHere) 
			{
				if (currentEnemyValue < 100f) 
				{
					currentEnemyValue += Time.deltaTime * countMultiplier;
					sliderValue.text = ((int)currentEnemyValue).ToString ();

					sliderValue.color = initialColor;
					alphaValue = 1f;

					chargePitch = ((currentEnemyValue * countPitchMultiplier) * 0.07f);
					chargePitch = Mathf.Clamp (chargePitch, 0.7f, 5f);

					chargeSound = chargeLevel;
				} 
				else if (currentEnemyValue >= 100f) 
				{
					rend.material.SetColor ("_TintColor", Color.red);
					currentPlayerValue = 0f;

					sliderValue.color = fadedColor;

					if(alphaValue > 0)
					{
						alphaValue -= Time.deltaTime;
					}

					if(!isCapped)
					{
						GameMasterObject.enemyPoints++;
						if(capParticle != null)
						{
							Instantiate (capParticle, transform.position + new Vector3(0f, 1.2f, 0f), transform.rotation);
						}

						isCapped = true;
					}
				}
			}
			else if(!playerHere && !enemyHere)
			{
				sliderValue.color = fadedColor;

				if(alphaValue > 0)
				{
					alphaValue -= Time.deltaTime;
				}

				if(chargeSound > 0)
				{
					chargeSound -= Time.deltaTime * 2f;	
				}

//				chargePitch = 0f;
			}
		}

		if(heroesSliderValue != null)
		{
			heroesSliderValue.text = ((int)currentPlayerValue).ToString();
		}

		if(enemySliderValue != null)
		{
			enemySliderValue.text = ((int)currentEnemyValue).ToString();
		}
	}

	void OnTriggerEnter(Collider other)
	{
//		Debug.Log (other.gameObject.tag);

		if(other.gameObject.tag == "Enemy" && !playerHere)
		{
			enemyHere = true;
		}
		else if(other.gameObject.tag == "Player" && !enemyHere)
		{
			playerHere = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Enemy" && !playerHere)
		{
			enemyHere = true;
		}
		else if(other.gameObject.tag == "Player" && !enemyHere)
		{
			playerHere = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "Enemy")
		{
			enemyHere = false;
		}
		else if(other.gameObject.tag == "Player")
		{
			playerHere = false;
		}
	}

	void SetToInactive()
	{	
		gameObject.SetActive(false);
	}

	public void ChangePositions()
	{
//		Debug.Log ("Change Position");
		nextPoint++;

		if(nextPoint < GameMasterObject.capPointPositions.Count)
		{			
			transform.position = GameMasterObject.capPointPositions [nextPoint].position;
			this.gameObject.SetActive (true);
			GameMasterObject.changePostion = false;
		}
		else if(nextPoint >= GameMasterObject.capPointPositions.Count)
		{
			GameMasterObject.allPointsCapped = true;
		}
	}

	void OnDisable()
	{
		isCapped = false;
		ResetValues ();
	}

	public void ResetValues()
	{
		currentEnemyValue = 0f;
		currentPlayerValue = 0f;
		enemyHere = false;
		playerHere = false;
		rend.material.SetColor ("_TintColor", Color.yellow);
		inactiveTimer = 0.25f;
	}

//	public void Respawn()
//	{
//		
//	}
}
