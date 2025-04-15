using System.Collections;
using TMPro;
using UnityEngine;

public static class CountdownManager
{
    public static IEnumerator CountdownTimer(TMP_Text countdownText)
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        countdownText.text = "1";
    }
}
