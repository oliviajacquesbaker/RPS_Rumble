using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
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
            if (m_jumping != value)
                jumpTrigger.set(value);
            m_jumping = value;
        }
    }
    private Latch jumpTrigger;
    private Latch punchTrigger;
    private Latch abilityTrigger;

    //movement controll variables
    private bool isGrounded;
    private new Rigidbody2D rigidbody;
    private Animator animator;
    private new CircleCollider2D collider;

    //player data
    private int activeCharacterIndex;
    public float health { get; private set; }
    [System.NonSerialized]
    public float[] abilityCooldownStates;
    [System.NonSerialized]
    public float punchCooldownState;
    private bool facing;

    //initial data
    public float arenaWidth = 10;
    public bool startingDirection = false;
    public float initialHealth = 100;
    public float punchCooldown = .5f;
    public float groundSlamDamage = 20;
    public CharacterParameterSet[] parameterSets;

    public CharacterParameterSet currentParameters
    {
        get
        {
            return parameterSets[activeCharacterIndex];
        }
    }

    [Space]
    [SerializeField]
    private LayerMask groundingLayerMask;
    [SerializeField]
    private LayerMask punchLayerMask;

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
        animator.SetBool("Jump Held", jumping);
        animator.SetBool("Crouch", crouching);
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("Move Speed", Mathf.Abs(movement));
        animator.SetBool("Move Direction", facing);

        //trigger a punch/ability
        if (punchTrigger.isSet && punchCooldownState <= 0) {
            animator.SetTrigger("Punch");
            punchCooldownState = punchCooldown;
            punchAction();
            punchTrigger.reset();
        }

        if (abilityTrigger.isSet && abilityCooldownStates[activeCharacterIndex] <= 0 && isGrounded)
        {
            abilityCooldownStates[activeCharacterIndex] = currentParameters.abilityCooldown;
            animator.SetTrigger("Ablity");
            abilityTrigger.reset();
        }
    }

    private void updateMovement()
    {
        //horizontal movement handled by animator
        if (movement != 0)
        {
            facing = movement < 0 ? true : false;
        }

        //jumping
        if (jumpTrigger.isSet && isGrounded)
        {
            Vector2 velocity = rigidbody.velocity;
            velocity.y = currentParameters.jumpSpeed;
            rigidbody.velocity = velocity;
            jumpTrigger.reset();
            animator.SetTrigger("Jump");
        }

        //flying
        if (jumping && currentParameters.canFly)
        {
            Vector2 velocity = rigidbody.velocity;
            velocity.y = Mathf.Max(velocity.y, -currentParameters.flyFallSpeed);
            rigidbody.velocity = velocity;
        }
    }

    private void checkGround()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, Vector2.down, .02f, groundingLayerMask);

        isGrounded = hit.collider;
    }

    private void updateCooldowns()
    {
        punchCooldownState -= Time.fixedDeltaTime;
        punchCooldownState = Mathf.Clamp(punchCooldownState, 0, punchCooldown);

        for (int i = 0; i < abilityCooldownStates.Length; i++)
        {
            abilityCooldownStates[i] -= Time.fixedDeltaTime;
            abilityCooldownStates[i] = Mathf.Clamp(abilityCooldownStates[i], 0, currentParameters.abilityCooldown);
        }
    }

    private void punchAction()
    {
        Vector2 punchDir = Vector2.right * (facing ? -1 : 1);
        Vector2 origin = collider.offset + (Vector2)transform.position;

        RaycastHit2D[] hits = new RaycastHit2D[5];

        hits = Physics2D.CircleCastAll(origin, .3f, punchDir, currentParameters.punchDistance + collider.radius, punchLayerMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                PlayerController targetPlayer = hits[i].collider.GetComponentInParent<PlayerController>();

                if (!targetPlayer || targetPlayer == this)
                    continue;

                float damageResistance = targetPlayer.crouching ? targetPlayer.currentParameters.crouchDamageResistance : targetPlayer.currentParameters.damageResistance;
                float damageMultiplier = 1f / (1 + damageResistance);
                float damage = damageMultiplier * currentParameters.punchDamage;

                targetPlayer.damage(damage);
                break;
            }
        }
    }

    public void damage(float hp)
    {
        health = Mathf.Max(health - hp, 0);
    }

    public void rockAbility()
    {
        PlayerController[] players = GameManager.Instance.players;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == this)
                continue;

            if (!players[i].isGrounded)
                continue;

            float damageResistance = !players[i].crouching ? players[i].currentParameters.crouchDamageResistance : players[i].currentParameters.damageResistance;
            float damageMultiplier = 1f / (1 + damageResistance);
            float damage = damageMultiplier * groundSlamDamage;

            players[i].damage(damage);
        }
    }

    public void paperAbility()
    {

    }

    public void scissorsAbility()
    {

    }

    private void OnAnimatorMove()
    {
        transform.Translate(animator.deltaPosition, Space.World);
        float x = Mathf.Clamp(transform.position.x, -arenaWidth / 2, arenaWidth / 2);
        transform.position = new Vector3(x, transform.position.y);
    }

    private void Awake()
    {
        //initialize variables
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<CircleCollider2D>();
        health = initialHealth;
        abilityCooldownStates = new float[parameterSets.Length];

        //initialize state
        selectRandomCharacter();
        checkGround();

        animator.SetBool("Move Direction", startingDirection);
        facing = startingDirection;
        updateAnimations();
    }

    void FixedUpdate()
    {
        //update everything
        checkGround();
        updateMovement();
        updateAnimations();
        updateCooldowns();
    }
}