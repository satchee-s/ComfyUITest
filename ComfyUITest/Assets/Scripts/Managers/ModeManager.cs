using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    [SerializeField] ComfyPromptCtr comfyPromptCtr;
    [SerializeField] ComfyRealtimePromptCtr comfyRealtimePromptCtr;

    public void TakePhoto()
    {
        StartCoroutine(PhotoRoutine());
    }

    IEnumerator PhotoRoutine()
    {
        StartCoroutine(CountdownManager.CountdownTimer(UIManagerPhotobooth.Instance.GetCountdownTimerText()));
        UIManagerPhotobooth.Instance.PlayCountdownTimerAnimation();
        UIManagerPhotobooth.Instance.CaptureButtonClick();
        yield return new WaitForSeconds(3f);

        UIManagerPhotobooth.Instance.ToggleCountdownTimer(false);
        UIManagerPhotobooth.Instance.ToggleCameraImage(false);

        yield return new WaitForEndOfFrame();

        QueuePrompt();
    }

    public void QueuePrompt()
    {
        UIManagerPhotobooth.Instance.ClearPreviewImage();
        UIManagerPhotobooth.Instance.ToggleQRCodeImage(false);
        if(ModeSwitchManager.Instance.GetState())
        {
            UIManagerPhotobooth.Instance.ToggleQRDownloadButton(false);
            comfyPromptCtr.QueuePrompt();
            return;
        }
        comfyRealtimePromptCtr.QueuePrompt();
    }
}