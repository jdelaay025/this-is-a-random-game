using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DannyAbilities : PlayerAbilities 
{
	public static float classDamage = 30.0f;
	public int whichWeapon = 0;
	public List<GameObject> weapons;
	public static bool Ultimate = false;

	protected override void Awake()
	{
		base.Awake();
	}

	void Start () 
	{
		attackRate = 0.5f;
		attackCharge = 0.5f;

		for(int i = 0; i < weapons.Count; i++)
		{
			weapons [i].SetActive (false);
		}

		UseSkillOne (classDamage);

	}

	void OnEnable()
	{
		GameMasterObject.playerUse = this.gameObject;
	}

	void Update () 
	{
		PlayerMotor.whichWeapon = whichWeapon;
	}

	public override void UseSkillOne (float damage)
	{
		anim.SetInteger ("WhichWeapon", 1);
		anim.SetBool ("Ultimate Charge", false);
		Ultimate = false;
		whichWeapon = 1;
		PlayerMotor.shootRate = 1.1f;
		classDamage = 60;

		for(int i = 0; i < weapons.Count; i++)
		{
			if(weapons[i] == weapons[whichWeapon])
			{
				weapons [i].SetActive (true);
			}
			else if(weapons[i] != weapons[whichWeapon])
			{
				weapons [i].SetActive (false);
			}				
		}
	}
	public override void UseSkillTwo (float damage)
	{
		anim.SetInteger ("WhichWeapon", 0);
		anim.SetBool ("Ultimate Charge", false);
		Ultimate = false;
		whichWeapon = 0;
		PlayerMotor.shootRate = 0.3f;
		classDamage = 47;

		for(int i = 0; i < weapons.Count; i++)
		{
			if(weapons[i] == weapons[whichWeapon])
			{
				weapons [i].SetActive (true);
			}
			else if(weapons[i] != weapons[whichWeapon])
			{
				weapons [i].SetActive (false);
			}				
		}
	}
	public override void UseSkillThree (float damage)
	{
		anim.SetInteger ("WhichWeapon", 2);
		anim.SetBool ("Ultimate Charge", false);
		Ultimate = false;
		whichWeapon = 2;
		PlayerMotor.shootRate = 3f;
		classDamage = 160;

		for(int i = 0; i < weapons.Count; i++)
		{
			if(weapons[i] == weapons[whichWeapon])
			{
				weapons [i].SetActive (true);
			}
			else if(weapons[i] != weapons[whichWeapon])
			{
				weapons [i].SetActive (false);
			}				
		}
	}
	public override void UseUltimate (float damage)
	{
		anim.SetInteger ("WhichWeapon", 3);
		anim.SetBool ("Ultimate Charge", false);
		Ultimate = true;
		whichWeapon = 3;
		PlayerMotor.shootRate = 3f;
		classDamage = 600;

		for(int i = 0; i < weapons.Count; i++)
		{
			if(weapons[i] == weapons[whichWeapon])
			{
				weapons [i].SetActive (true);
			}
			else if(weapons[i] != weapons[whichWeapon])
			{
				weapons [i].SetActive (false);
			}				
		}
	}
	public override void UsePassiveOne ()
	{
		
	}
	public override void UsePassiveTwo ()
	{
		
	}
}
