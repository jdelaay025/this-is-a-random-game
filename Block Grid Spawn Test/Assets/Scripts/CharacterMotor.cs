using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour 
{
    #region Global Variable Declaration

    [Header("Movement Speeds")]
    public float runSpeed = 3.7f;
    public float sprintSpeed = 7.5f;
    public float aimSpeed = 0.8f;
    public float speedMultiplier = 22f;
    public float rotateSpeed = 10f;
    public float turnSpeed = 10f;

    [Header("Sprint Duration")]
    public float currentCharge = 0f;

    InputHandler ih;
    StateManager states;
    Rigidbody rb;

    Vector3 lookPosition;
    Vector3 storeDirection;

    float horizontal;
    float vertical;

    Vector3 lookDirection;

    PhysicMaterial zFriction;
    PhysicMaterial mFriction;
    Collider col;

    #endregion

    void Start () 
	{
		
	}
	
	void Update () 
	{

	}

    void HandleMovement(Vector3 h, Vector3 v, bool onGround)
    {
        if (onGround)
        {
            rb.AddForce((v + h).normalized * speed());
        }
    }

    float speed()
    {
        float speed = 0f;

        if (states.aiming && !states.reloading && !states.sprint)
        {
            speed = aimSpeed;
        }
        else
        {
            if (states.sprint)
            {
                speed = sprintSpeed;
            }
            else
            {
                speed = runSpeed;
            }
        }

        speed *= speedMultiplier;

        return speed;

    }
}
