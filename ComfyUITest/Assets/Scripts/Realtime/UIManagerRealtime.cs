using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerRealtime : MonoBehaviour
{
    public static UIManagerRealtime Instance;

    [SerializeField] GameObject startButton;
    [SerializeField] GameObject stopButton;

    [SerializeField] Image previewImage;

    [SerializeField] TMP_Text promptInput;

    [SerializeField] ComfyRealtimePromptCtr comfyPromptCtr;
    [SerializeField] ComfyRealtimeImageCtr comfyImageCtr;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void ToggleStartButton(bool state)
    {
        startButton.SetActive(state);
    }

    public void ToggleStopButton(bool state)
    {
        stopButton.SetActive(state);
    }

    public void OnStartButtonClick()
    {
        ToggleStartButton(false);
        ToggleStopButton(true);
        comfyPromptCtr.QueuePrompt();
    }

    public void OnStopButtonClick()
    {
        ToggleStartButton(true);
        ToggleStopButton(false);
    }

    public void SetPreviewImage(Sprite sprite)
    {
        previewImage.sprite = sprite;
    }

    public string GetPromptInput()
    {
        return promptInput.text;
    }
}