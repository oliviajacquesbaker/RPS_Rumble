using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Countdown : MonoBehaviour
{
    private TextMeshProUGUI text;

    [System.NonSerialized]
    public bool isComplete = false;
    public bool isTriggered = false;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Trigger()
    {
        StartCoroutine(countRoutine());
        isTriggered = true;
    }

    IEnumerator countRoutine()
    {
        text.text = "3";
        yield return new WaitForSecondsRealtime(1);
        text.text = "2";
        yield return new WaitForSecondsRealtime(1);
        text.text = "1";
        yield return new WaitForSecondsRealtime(1);
        text.text = "Fight!";
        yield return new WaitForSecondsRealtime(1);
        text.text = "";
        isComplete = true;
    }
}
