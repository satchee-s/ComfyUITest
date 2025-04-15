using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UIManagerPhotobooth : MonoBehaviour
{
    public static UIManagerPhotobooth Instance;

    [Header("Raw Images")]
    [SerializeField] RawImage cameraImage;

    [Header("Video Players")]
    [SerializeField] VideoPlayer previewVideoPlayer;

    [Header("Buttons")]
    [SerializeField] Button captureButton, redoButton;

    [Header("Animators")]
    [SerializeField] Animator loadingSpinner;
    [SerializeField] Animator countDownTimer;

    [Header("Transition Animators")]
    [SerializeField] Animator keyboardCanvasTransitionAnimator;
    [SerializeField] Animator displayCanvasTransitionAnimator;

    [Header("Input Fields")]
    [SerializeField] TMP_InputField promptInputField;

    [Header("GameObjects")]
    [SerializeField] GameObject previewImageMultiple;
    [SerializeField] GameObject previewImageSingle;
    [SerializeField] GameObject captureModeSwitch;
    [SerializeField] GameObject qrDownloadButton;
    [SerializeField] GameObject qrCodeImage;
    [SerializeField] GameObject genderPanel;
    [SerializeField] GameObject keyboardPanel;
    [SerializeField] GameObject idleKeyboardPanel;
    [SerializeField] GameObject idlePreviewPanel;

    [Header("VideoClips")]
    [SerializeField] VideoClip idlePreviewBackgroundClip;
    [SerializeField] VideoClip activePreviewBackgroundClip;

    [Header("ComfyUI")]
    [SerializeField] ComfyPromptCtr promptCtr;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }

    #region Toggles
    public void ToggleCameraImage(bool state)
    {
        cameraImage.gameObject.SetActive(state);
    }

    public void TogglePreviewImageMultiple(bool state)
    {
        previewImageMultiple.SetActive(state);
    }
    public void TogglePreviewImageSingle(bool state)
    {
        previewImageSingle.SetActive(state);
    }

    public void ToggleCaptureButton(bool state)
    {
        captureButton.gameObject.SetActive(state);
    }

    public void ToggleRedoButton(bool state)
    {
        redoButton.gameObject.SetActive(state);
    }

    public void ToggleLoadingSpinner(bool state)
    {
        loadingSpinner.gameObject.SetActive(state);
    }

    public void ToggleCaptureModeSwitch(bool state)
    {
        captureModeSwitch.SetActive(state);
    }

    public void ToggleCountdownTimer(bool state)
    {
        countDownTimer.gameObject.SetActive(state);
    }

    public void TogglePromptInputField(bool state)
    {
        promptInputField.gameObject.SetActive(state);
    }

    public void ToggleQRDownloadButton(bool state)
    {
        qrDownloadButton.SetActive(state);
    }

    public void ToggleQRCodeImage(bool state)
    {
        qrCodeImage.SetActive(state);
    }

    public void ToggleGenderPanel(bool state)
    {
        genderPanel.SetActive(state);
    }

    public void ToggleKeyboardPanel(bool state)
    {
        keyboardPanel.SetActive(state);
    }

    public void ToggleIdleKeyboardPanel(bool state)
    {
        idleKeyboardPanel.SetActive(state);
    }

    public void ToggleIdlePreviewPanel(bool state)
    {
        idlePreviewPanel.SetActive(state);
    }

    #endregion

    #region Transition Animation
    public void PlayFadeInTransitionAnimation()
    {
        displayCanvasTransitionAnimator.Play("FadeIn");
        keyboardCanvasTransitionAnimator.Play("FadeIn");
    }

    public void PlayFadeOutTransitionAnimation()
    {
        displayCanvasTransitionAnimator.Play("FadeOut");
        keyboardCanvasTransitionAnimator.Play("FadeOut");
    }
    #endregion

    #region Clear
    public void ClearPreviewImage()
    {
        previewImageSingle.GetComponent<RawImage>().texture = null;
    }

    public void ClearPromptInputField()
    {
        promptInputField.text = string.Empty;
    }
    #endregion

    #region Button Click
    public void GenderButtonClick()
    {
        StartCoroutine(GenderButtonClickCoroutine());
    }

    IEnumerator GenderButtonClickCoroutine()
    {
        PlayFadeOutTransitionAnimation();
        yield return new WaitForSeconds(0.3f);
        ToggleGenderPanel(false);
        ToggleCameraImage(true);
        ToggleKeyboardPanel(true);
        SwitchPreviewBackground(activePreviewBackgroundClip);
        PlayFadeInTransitionAnimation();
    }

    public void HomeButtonClick()
    {
        StartCoroutine(HomeButtonClickCoroutine());
    }

    IEnumerator HomeButtonClickCoroutine()
    {
        PlayFadeOutTransitionAnimation();
        yield return new WaitForSeconds(0.3f);
        ToggleCameraImage(false);
        ToggleKeyboardPanel(false);
        ActivateIdleScreen();
        PlayFadeInTransitionAnimation();
    }

    public void CaptureButtonClick()
    {
        ToggleCountdownTimer(true);
        countDownTimer.Play("Countdown");

        ToggleCaptureModeSwitch(false);
        TogglePromptInputField(false);
    }

    public void TouchScreenClick()
    {
        StartCoroutine(TouchScreenClickCoroutine());
    }

    IEnumerator TouchScreenClickCoroutine()
    {
        PlayFadeOutTransitionAnimation();
        yield return new WaitForSeconds(0.3f);
        DeactivateIdleScreen();
        PlayFadeInTransitionAnimation();
    }
    #endregion

    public void PrepareRenderedPhoto()
    {
        TogglePreviewImageSingle(true);
        ToggleLoadingSpinner(true);
    }

    public IEnumerator SwitchOnCoroutine(bool state)
    {
        PlayFadeOutTransitionAnimation();
        yield return new WaitForSeconds(0.3f);
        TogglePreviewImageSingle(!state);
        ToggleCameraImage(state);
        ClearPreviewImage();
        PlayFadeInTransitionAnimation();
    }

    public IEnumerator SwitchOffCoroutine(bool state)
    {
        PlayFadeOutTransitionAnimation();
        yield return new WaitForSeconds(0.3f);
        TogglePreviewImageSingle(!state);
        ToggleCameraImage(state);
        ToggleQRDownloadButton(state);
        ToggleQRCodeImage(state);
        ClearPreviewImage();
        PlayFadeInTransitionAnimation();
    }

    #region Getters

    public GameObject GetPreviewImageMultiple()
    {
        return previewImageMultiple;
    }

    public GameObject GetPreviewImageSingle()
    {
        return previewImageSingle;
    }

    public Animator GetCountdownTimer()
    {
        return countDownTimer;
    }

    public TMP_Text GetCountdownTimerText()
    {
        return countDownTimer.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public string GetPromptInputFieldText()
    {
        return promptInputField.text;
    }
    #endregion


    public void PlayCountdownTimerAnimation()
    {
        ToggleCountdownTimer(true);
        countDownTimer.Play("Countdown");
    }

    public void SwitchPreviewBackground(VideoClip clip)
    {
        previewVideoPlayer.clip = clip;
    }

    #region Idle Screen
    public void ActivateIdleScreen()
    {
        ToggleIdleKeyboardPanel(true);
        ToggleIdlePreviewPanel(true);

        ToggleCameraImage(false);
        ToggleGenderPanel(false);
        ToggleKeyboardPanel(false);
    }

    public void DeactivateIdleScreen()
    {
        ToggleIdleKeyboardPanel(false);
        ToggleIdlePreviewPanel(false);

        ToggleGenderPanel(true);
    }
    #endregion
}
