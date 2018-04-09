using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour, IDamageable
{
	public float startingHealth = 100.0f;
	public float currentHealth = 0.0f;
	public Slider healthSlider;
    public string typeOfCore = string.Empty;
    public float distanceFromMe = 0;

	Transform myTransform;
    Animator anim;

	void Awake()
	{
		myTransform = transform;
        anim = GetComponent<Animator>();		
	}

	void Start () 
	{
        switch (typeOfCore)
        {
            case "hero":
                GameMasterObject.heroCore = this.transform;
                break;
            case "Hero":
                GameMasterObject.heroCore = this.transform;
                break;
            case "enemy":
                GameMasterObject.enemyCore = this.transform;
                break;
            case "Enemy":
                GameMasterObject.enemyCore = this.transform;
                break;
            case "tower":
                break;                
            default:
                break;
        }

		if(healthSlider != null)
		{
			healthSlider.maxValue = startingHealth;
			healthSlider.value = currentHealth;
		}

        currentHealth = startingHealth;
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
        myTransform.position = new Vector3(0f, 3000f, 0f);
        //GameMasterObject.towerMounts.Remove(this.transform);
		Destroy (gameObject, 1);
		return;
	}
}
