using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    //movement variables
    [System.NonSerialized]
    public float movement;
    [System.NonSerialized]
    public bool crouching;
    private bool m_jumping;
    public bool jumping
    {
        get
        {
            return m_jumping;
        }
        set
        {
            m_jumping = value;
            jumpTrigger.set(value);
        }
    }
    private Latch jumpTrigger;
    private Latch punchTrigger;
    private Latch abilityTrigger;

    //movement controll variables
    private bool isGrounded;
    private new Rigidbody2D rigidbody;
    private Animator animator;

    //player data
    private int activeCharacterIndex;
    public float health { get; private set; }
    [System.NonSerialized]
    public float[] abilityCooldownStates = new float[] { 0, 0, 0 };

    //initial data
    public float initialHealth = 100;
    public float flyFallSpeed = .5f;
    public float jumpSpeed = 2f;
    public float[] abilityCooldowns = new float[] { 3, 3, 3 };

    public void punch()
    {
        punchTrigger.set();
    }

    public void useAbility()
    {
        abilityTrigger.set();
    }

    private void selectRandomCharacter() {
        int index = (int)(Random.value * 3);
        selectCharacter(index);
    }

    public void selectCharacter(int index)
    {
        index = Mathf.Clamp(index, 0, 3);
        activeCharacterIndex = index;
    }

    void updateAnimations()
    {
        //load the selected morph
        animator.SetInteger("SubcharID", activeCharacterIndex);

        //set movement state
        animator.SetBool("Jump", jumpTrigger.isSet);
        animator.SetBool("Crouch", crouching);
        animator.SetFloat("Movement", movement);

        //trigger a punch/ability
        if (punchTrigger.isSet) {
            animator.SetTrigger("Punch");
        }

        if (abilityTrigger.isSet && abilityCooldownStates[activeCharacterIndex] <= 0)
        {
            animator.SetTrigger("Ablity");
            abilityTrigger.reset();
            abilityCooldownStates[activeCharacterIndex] = abilityCooldowns[activeCharacterIndex];
        }
    }

    private void updateMovement()
    {
        //horizontal movement handled by animator
        
        //jumping
        if (jumpTrigger.isSet && isGrounded)
        {
            Vector2 velocity = rigidbody.velocity;
            velocity.y = jumpSpeed;
            rigidbody.velocity = velocity;
            jumpTrigger.reset();
        }

        //flying
        if (jumping && activeCharacterIndex == 1)
        {
            Vector2 velocity = rigidbody.velocity;
            velocity.y = Mathf.Max(velocity.y, -flyFallSpeed);
            rigidbody.velocity = velocity;
        }
    }

    private void checkGround()
    {
        isGrounded = Physics2D.Raycast((Vector2)transform.position + (Vector2.up * .01f), Vector2.down, .02f);
    }

    private void updateCooldowns()
    {
        for (int i = 0; i < abilityCooldownStates.Length; i++)
        {
            abilityCooldownStates[i] -= Time.fixedDeltaTime;
            abilityCooldownStates[i] = Mathf.Clamp(abilityCooldownStates[i], 0, abilityCooldowns[i]);
        }
    }

    public void rockAbility()
    {

    }

    public void paperAbility()
    {

    }

    public void scissorsAbility()
    {
        
    }

    private void OnAnimatorMove()
    {
        transform.Translate(-animator.deltaPosition, Space.World);
        Vector2 velocity = rigidbody.velocity;
        velocity.x = animator.deltaPosition.x / Time.fixedDeltaTime;
        rigidbody.velocity = velocity;
    }

    private void Awake()
    {
        //initialize variables
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = initialHealth;

        //initialize state
        selectRandomCharacter();
        checkGround();
        updateAnimations();
    }

    void FixedUpdate()
    {
        //update everything
        checkGround();
        updateAnimations();
        updateMovement();
        updateCooldowns();
    }
}
