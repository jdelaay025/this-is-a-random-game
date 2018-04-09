using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class CreepHealth : MonoBehaviour, IDamageable 
{
    public Slider healthSlider;
    public float startingHealth = 20f;
    public float curHealth = 0f;
    public Vector3 spawnPoint;

    public Color initialColor;

    Transform myTransform;
    Animator anim;
    Renderer rend;
    bool damaged = false;
    Creep creepAi;

    void Awake()
    {
        myTransform = transform;
        anim = GetComponent<Animator>();
        rend = GetComponentInChildren<Renderer>();
        creepAi = GetComponent<Creep>();
    }

    void Start () 
	{
        if (healthSlider != null)
        {
            healthSlider.maxValue = startingHealth;
            healthSlider.value = curHealth;            
        }

        if (rend != null)
        {
            initialColor = rend.material.color;
        }

        curHealth = startingHealth;
    }

    void Update () 
	{
        if (healthSlider != null)
        {
            healthSlider.value = curHealth;
        }

        if (damaged)
        {
            rend.material.color = Color.red;
            damaged = false;
        }
        else if (!damaged)
        {
            rend.material.color = initialColor;
        }

        if (curHealth <= 0)
        {
            Death();
        }
    }

    public void TakeDamage(float damage, Vector3 pos)
    {
        if (curHealth <= 0)
        {
            if (healthSlider != null)
            {
                healthSlider.value = 0;
            }

            Death();
            return;
        }

        curHealth -= damage;
        damaged = true;
    }

    void Death()
    {
        curHealth = 0;

        // creepAi.currentState = creepAi.targetWayPoint;
        //creepAi.chaseTarget = null;

        //myTransform.position = new Vector3(0f, 3000f, 0f);
        //Respawn();
        this.gameObject.SetActive(false);
    }

    public void Respawn()
    {
        curHealth = startingHealth;       
    }
}
