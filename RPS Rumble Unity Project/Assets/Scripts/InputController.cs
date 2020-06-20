using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class InputController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField]
    private string jumpKey;
    [SerializeField]
    private string attackKey;
    [SerializeField]
    private string abilityKey;
    [SerializeField]
    private string crouchKey;
   
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

   

    void Update()
    {
        playerController.jumping = Input.GetKey(jumpKey);
        playerController.crouching = Input.GetKey(crouchKey);
        playerController.movement = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(attackKey))
        {
            playerController.punch();
        }
        if (Input.GetKeyDown(abilityKey))
        {
            playerController.useAbility();
        }
    }
}
