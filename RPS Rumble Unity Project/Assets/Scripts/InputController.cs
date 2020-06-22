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
    [SerializeField]
    private string hMove;
    [SerializeField]
    private string rockKey;
    [SerializeField]
    private string paperKey;
    [SerializeField]
    private string scissorsKey;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
   

    void Update()
    {
        try
        {
            playerController.jumping = Input.GetButton(jumpKey);
            playerController.crouching = Input.GetButton(crouchKey);
            playerController.movement = Input.GetAxis(hMove);

            if (Input.GetButtonDown(attackKey))
            {
                playerController.punch();
            }
            if (Input.GetButtonDown(abilityKey))
            {
                playerController.useAbility();
            }
            if (Input.GetButtonDown(rockKey))
            {
                playerController.selectCharacter(0);
            }
            if (Input.GetButtonDown(paperKey))
            {
                playerController.selectCharacter(1);
            }
            if (Input.GetButtonDown(scissorsKey))
            {
                playerController.selectCharacter(2);
            }
        }catch (System.ArgumentException e)
        {
            enabled = false;
            Debug.LogError("Argument exception. Message from exception: " + e.Message);
        }
    }
}
