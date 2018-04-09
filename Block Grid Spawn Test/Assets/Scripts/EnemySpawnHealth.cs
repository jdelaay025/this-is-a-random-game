using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemySpawnHealth : MonoBehaviour, IDamageable
{
	[Header("Enemy Attributes")]
	public float startingHealth = 100f;
	public float currentHealth;

	[Header("Enemy Components")]
	public Slider healthSlider;

    EnemySpawnPoint point;
	Animator anim;

	void Awake () 
	{
		anim = GetComponent<Animator> ();
        point = GetComponent<EnemySpawnPoint>();
    }

    void Start () 
	{
		currentHealth = startingHealth;

		if (healthSlider != null) 
		{
			healthSlider.value = currentHealth;
			healthSlider.maxValue = startingHealth;
		}
	}

	void Update()
	{
		if(healthSlider != null)
		{
			healthSlider.value = currentHealth;
		}

		if(currentHealth <= 0)
		{
			Death ();
			return;
		}
	}

	public void TakeDamage(float damage, Vector3 pos)
	{
		if(currentHealth <= 0)
		{
			Death ();
			return;
		}
		currentHealth -= damage;
	}

	void Death()
	{
		currentHealth = 0;
		if(healthSlider != null)
		{
			healthSlider.value = 0;	
		}

		if(anim != null)
		{
			anim.SetBool ("Death", true);
		}

        this.gameObject.SetActive(false);

        point.dead = true;
        //Respawn();

		return;
	}

	void Respawn()
	{
		this.gameObject.SetActive (true);
		currentHealth = startingHealth;
        if(healthSlider != null)
		    healthSlider.value = currentHealth;
	}
}
