using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    private int playerNum;
    private Slider slider;
    private Animator animator;

    void Awake()
    {
        player = GameManager.Instance.players[playerNum];
        slider = GetComponent<Slider>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.health != slider.value)
        {
            if (player.health < slider.value)
            {
                animator.SetTrigger("Take Damage");
            }

            slider.value = player.health;
        }
    }
}
