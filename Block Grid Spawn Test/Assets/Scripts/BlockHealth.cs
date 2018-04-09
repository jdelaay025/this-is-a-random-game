using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockHealth : MonoBehaviour, IDamageable
{
    #region Global Variable Declaration

    public float startingHealth = 100.0f;
	public float currentHealth = 0.0f;
	public Slider healthSlider;

	Transform myTransform;

    #endregion

    void Awake()
	{
		myTransform = transform;
		currentHealth = startingHealth;
	}

	void Start () 
	{
		if(healthSlider != null)
		{
			healthSlider.maxValue = startingHealth;
			healthSlider.value = currentHealth;
		}
	}

	void Update () 
	{
		if(healthSlider != null)
		{
			healthSlider.value = currentHealth;
		}
		if(currentHealth <= 0)
		{
			Death ();
		}
	}

	public void TakeDamage(float damage, Vector3 pos)
	{
		if (currentHealth > 0) 
		{
			currentHealth -= damage;
		}
		else if (currentHealth <= 0) 
		{
			Death ();
		}
	}

	public void Death()
	{
		Destroy (gameObject);
		return;
	}
}
