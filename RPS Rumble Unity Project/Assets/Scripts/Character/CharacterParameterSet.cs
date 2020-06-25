using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Parameter Set", menuName = "Game/Character Parameter Set")]
public class CharacterParameterSet : ScriptableObject
{
    [Header("Movement")]
    public float jumpSpeed = 2f;
    public bool canFly = false;
    public float flyFallSpeed = .5f;

    [Header("Abilities")]
    public float abilityCooldown = 3;

    [Header("Combat")]
    public float punchDamage = 5;
    public float punchDistance = .5f;
    public float damageResistance = 0;
    public float crouchDamageResistance = 0;

    private void OnValidate()
    {
        jumpSpeed = Mathf.Max(jumpSpeed, 0);
        flyFallSpeed = Mathf.Max(flyFallSpeed, 0);

        abilityCooldown = Mathf.Max(abilityCooldown, 0);

        punchDamage = Mathf.Max(punchDamage, 0);
        damageResistance = Mathf.Max(damageResistance, 0);
        crouchDamageResistance = Mathf.Max(crouchDamageResistance, 0);
    }
}
