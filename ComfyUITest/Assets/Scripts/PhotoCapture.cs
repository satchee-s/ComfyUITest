using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [SerializeField] ComfyPromptCtr promptCtr;

    //void Start()
    //{
    //    path = "C:\\Users\\user\\Desktop\\CHUCK\\ComfyUI\\ComfyUI\\input";

    //    webCamTexture = new WebCamTexture();
    //    rawImage.texture = webCamTexture;
    //    webCamTexture.Play();
    //}

    //void Update()
    //{
    //    if (webCamTexture.isPlaying)
    //    {
    //        rawImage.texture = webCamTexture;
    //    }
    //}

    //public void TakePhoto()
    //{
    //    StartCoroutine(PhotoRoutine());
    //}

    //IEnumerator PhotoRoutine()  
    //{
    //    StartCoroutine(CountdownManager.CountdownTimer(UIManagerPhotobooth.Instance.GetCountdownTimerText()));
    //    UIManagerPhotobooth.Instance.PlayCountdownTimerAnimation();
    //    UIManagerPhotobooth.Instance.CaptureButtonClick();
    //    yield return new WaitForSeconds(3f);

    //    UIManagerPhotobooth.Instance.ToggleCountdownTimer(false);
    //    UIManagerPhotobooth.Instance.ToggleCameraImage(false);

    //    yield return new WaitForEndOfFrame();

    //    // Capture the photo
    //    Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
    //    photo.SetPixels(webCamTexture.GetPixels());
    //    photo.Apply();

    //    // Save the photo with a unique name
    //    string fileName = "photo_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
    //    string filePath = Path.Combine(path, fileName);
    //    byte[] bytes = photo.EncodeToPNG();
    //    File.WriteAllBytes(filePath, bytes);
        
    //    UIManagerPhotobooth.Instance.PrepareRenderedPhoto();

    //    promptCtr.QueuePrompt();
    //}
}
