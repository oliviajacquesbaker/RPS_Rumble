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
    }

    //in case of like an item or ability that increases health
    void changeHealthValue(float val, int dir)
    {
        if (dir >= 0)
        {
            slider.value += val;
            animator.SetTrigger("Take Damage");
        }
        else
        {
            slider.value -= val;
        }
    }
}
