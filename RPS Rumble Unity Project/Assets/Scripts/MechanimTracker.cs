using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanimTracker : StateMachineBehaviour
{
    public string property;
    public int ID;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger(property, ID);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger(property, -1);
    }
}
