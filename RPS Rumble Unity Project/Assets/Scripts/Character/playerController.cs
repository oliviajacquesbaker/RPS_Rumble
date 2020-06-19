using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class playerController : MonoBehaviour
{
    [System.NonSerialized]
    public float movement;
    [System.NonSerialized]
    public bool crouching;
    private Latch jumpTrigger;
    private Latch punchTrigger;
    private Latch abilityTrigger;

    private bool isGrounded;

    private int activeCharacterIndex;
    public SubCharacter[] subChars;
    private SubCharacter activeCharacter => subChars[activeCharacterIndex];

    private Animator animator;
    private new Rigidbody2D rigidbody;

    public void jump()
    {
        jumpTrigger.set();
    }

    public void punch()
    {
        punchTrigger.set();
    }

    public void useAbility()
    {
        abilityTrigger.set();
    }

    private void selectRandomCharacter() {
        int index = (int)(Random.value * subChars.Length);
        selectCharacter(index);
    }

    public void selectCharacter(int index)
    {
        index = Mathf.Clamp(index, 0, subChars.Length);
        activeCharacterIndex = index;
    }

    void updateAnimations()
    {
        animator.SetInteger("SubcharID", activeCharacterIndex);

        if (punchTrigger.isSet)
            animator.SetTrigger("Punch");
        if (abilityTrigger.isSet)
            animator.SetTrigger("Ablity");

        animator.SetBool("Jump", jumpTrigger.isSet);
        animator.SetBool("Crouch", crouching);
        animator.SetFloat("Movement", movement);
    }

    private void OnAnimatorMove()
    {
        Vector2 velocity = rigidbody.velocity;
        velocity.x = animator.deltaPosition.x / Time.fixedDeltaTime;
        rigidbody.velocity = velocity;
    }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        selectRandomCharacter();
        updateAnimations();
    }

    void FixedUpdate()
    {
        updateAnimations();
        activeCharacter.updateCharacter(this);
    }
}
