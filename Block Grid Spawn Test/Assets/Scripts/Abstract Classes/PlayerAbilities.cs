using UnityEngine;
using System.Collections;

public abstract class PlayerAbilities : MonoBehaviour
{
	protected float attackRate;
	protected float attackCharge;
	protected Transform myTransform;
	protected Animator anim;
	protected AudioSource sounds;

	protected virtual void Awake () 
	{
		myTransform = transform;
		anim = GetComponentInChildren<Animator> ();
		sounds = GetComponent<AudioSource> ();
	}

	public abstract void UseSkillOne (float damage);
	public abstract void UseSkillTwo (float damage);
	public abstract void UseSkillThree (float damage);
	public abstract void UseUltimate (float damage);
	public abstract void UsePassiveOne ();
	public abstract void UsePassiveTwo ();
}
