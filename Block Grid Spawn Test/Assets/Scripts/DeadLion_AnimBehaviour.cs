using UnityEngine;
using System.Collections;

public class DeadLion_AnimBehaviour : StateMachineBehaviour 
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("Hit beginning Anim");
        //animator.SetBool("Dead", true);
    }
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
	{
        //Debug.Log("Hit End Anim");
        animator.SetBool("Dead", true);
    }
}
