using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragonMonsterHealth : MonoBehaviour, IDamageable
{
    #region Global Variable Declaration

    [Header("Enemy Attributes")]
	public float startingHealth = 100f;
	public float currentHealth;
	public float StartingArmor = 100f;
	public float currentArmor;

	[Header("Enemy Components")]
	public Slider healthSlider;

	Animator anim;
    public bool dead = false;
    public GameObject model;

    Renderer rend;
    Color initialColor;
    bool damaged = false;

    #endregion

    void Awake () 
	{
		anim = GetComponent<Animator> ();
        rend = model.GetComponent<Renderer>();
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

        initialColor = rend.material.GetColor("_ColorTint");

    }
    void Update()
	{

		if(healthSlider != null)
		{
			healthSlider.value = currentHealth;
		}

		if(currentHealth <= 0 && !dead)
		{
			Death ();            			
		}

        if (damaged)
        {
            rend.material.SetColor("_ColorTint", Color.red);
            damaged = false;
        }
        else if(!damaged)
        {
            rend.material.SetColor("_ColorTint", initialColor);
        }

        dead = anim.GetBool("Dead");

        if (dead)
        {
            Relocate();
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
        damaged = true;
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
        damaged = true;

    }
    void Death()
	{
        dead = true;
		currentHealth = 0;
		if(healthSlider != null)
		{
			healthSlider.value = 0;	
		}

		if(anim != null)
		{
			anim.SetTrigger ("Death");
		}

        ControlPointScript.enemyHere = false;
        GameMasterObject.dragonsMonsters.Remove(this.transform);
        //Relocate();

        return;
	}
    void Relocate()
    {
        transform.position = new Vector3(0f, 3000f, 0f);
        GameMasterObject.dragonsMonsters.Remove(this.transform);
        this.gameObject.SetActive(false);

    }
    void Respawn()
	{
		this.gameObject.SetActive (true);
		currentHealth = startingHealth;
		currentArmor = StartingArmor;
		healthSlider.value = currentHealth;
	}
}
