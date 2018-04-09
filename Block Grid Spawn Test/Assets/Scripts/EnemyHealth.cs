using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
	[Header("Enemy Attributes")]
	public float startingHealth = 100f;
	public float currentHealth;
	public float StartingArmor = 100f;
	public float currentArmor;

	[Header("Enemy Components")]
	public Slider healthSlider;

	Animator anim;

	void Awake () 
	{
		anim = GetComponent<Animator> ();
	}

	void Start () 
	{
		currentHealth = startingHealth;
		currentArmor = StartingArmor;

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
			ControlPointScript.enemyHere = false;
			return;
		}
		currentHealth -= damage;
	}
    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
        {
            Death();
            ControlPointScript.enemyHere = false;
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
		transform.position = new Vector3(0f, 3000f, 0f);
		ControlPointScript.enemyHere = false;
		this.gameObject.SetActive (false);

		return;
	}

	void Respawn()
	{
		this.gameObject.SetActive (true);
		currentHealth = startingHealth;
		currentArmor = StartingArmor;
		healthSlider.value = currentHealth;
	}
}
