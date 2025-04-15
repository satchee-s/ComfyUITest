using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTimerManager : MonoBehaviour
{
    [SerializeField] float currTimer;
    float maxTimer = 90f;

    private void Start()
    {
        ResetCountdown();
    }

    public void ResetCountdown()
    {
        currTimer = maxTimer;
    }

    private void Update()
    {
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            ResetCountdown();
        }

        Countdown();
    }

    void Countdown()
    {
        currTimer -= Time.deltaTime;

        Debug.Log(currTimer);

        if (currTimer <= 0)
        {
            currTimer = 0;
            UIManagerPhotobooth.Instance.ActivateIdleScreen();
        }
    }
}
