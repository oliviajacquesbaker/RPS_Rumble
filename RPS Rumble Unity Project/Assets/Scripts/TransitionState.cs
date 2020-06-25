using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionState : StateMachineBehaviour
{
    public string property;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(property, -1);
    }
}
