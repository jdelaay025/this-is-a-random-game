using UnityEngine;
using System.Collections;

public class Thruster : KeybindableComponent
{

    
    public Rigidbody shipRigidBody;
    float thrusterStrength = 10f;
    
    void Start () 
	{
        shipRigidBody = this.transform.root.GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        if (shipRigidBody.isKinematic == true)
        {
            SetParticles(false);
            return;
        }

        if (Input.GetKey(keyCode))
        {
            Vector3 theforce = -this.transform.forward * thrusterStrength;

            shipRigidBody.AddForceAtPosition(theforce, this.transform.position);
            SetParticles(true);
        }
        else
        {
            SetParticles(false);
        }               
    }
    
    void SetParticles(bool enabled)
    {
        ParticleSystem.EmissionModule em = GetComponentInChildren<ParticleSystem>().emission;
        em.enabled = enabled;        
    }
}
