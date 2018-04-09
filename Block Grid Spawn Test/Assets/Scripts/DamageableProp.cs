using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageableProp : MonoBehaviour, IDamageable 
{
	public float startingHealth = 100f;
	public float currentHealth = 0f;
	public Slider barrelHealthSlider;
	public GameObject explosionPrefab;

	Transform myTransform;
	float damage = 0f;

	void Awake()
	{
		myTransform = transform;	
	}

	void Start () 
	{		
		currentHealth = startingHealth;
		if(barrelHealthSlider != null)
		{
			barrelHealthSlider.maxValue = startingHealth;
			barrelHealthSlider.value = currentHealth;
		}
	}

	void Update () 
	{
		if(barrelHealthSlider != null)
		{
			barrelHealthSlider.value = currentHealth;
		}
		if(currentHealth <= 0)
		{
			DestroyProp ();
		}
	}

	public void TakeDamage(float damage, Vector3 pos)
	{
		if(currentHealth > 0)
		{
			currentHealth -= damage;	
		}
		else if(currentHealth <= 0)
		{
			DestroyProp ();
		}			
	}

	public void DestroyProp()
	{
		if (barrelHealthSlider != null) 
		{
			barrelHealthSlider.value = 0;
		}

		if(explosionPrefab != null)
		{
			Instantiate (explosionPrefab, myTransform.position, myTransform.rotation);
		}

		this.gameObject.SetActive (false);
	}
}
