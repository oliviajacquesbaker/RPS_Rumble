using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndPannel : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    public void Show(int winner)
    {
        gameObject.SetActive(true);
        winnerText.text = "P" + (winner + 1) + " WINS!";
    }
}
