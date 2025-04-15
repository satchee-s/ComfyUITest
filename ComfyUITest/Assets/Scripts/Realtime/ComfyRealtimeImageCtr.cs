using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using System.Linq;

public class ComfyRealtimeImageCtr : MonoBehaviour
{
    [SerializeField] string imageFolderPath = "C:\\Users\\user\\Desktop\\CHUCK\\ComfyUI\\ComfyUI\\temp";

    [SerializeField] ComfyRealtimePromptCtr comfyPromptCtr;

    public void LoadLatestImage()
    {
        if (!string.IsNullOrEmpty(GetLatestImagePath()))
        {
            StartCoroutine(LoadImageFromFile(GetLatestImagePath()));
        }
        else
        {
            Debug.LogError("No image found in the specified folder.");
        }
    }

    private string GetLatestImagePath()
    {
        if (!Directory.Exists(imageFolderPath))
        {
            Debug.LogError("Directory does not exist: " + imageFolderPath);
            return null;
        }

        DirectoryInfo dir = new DirectoryInfo(imageFolderPath);
        FileInfo latestFile = dir.GetFiles("*.jpg")
                                 .OrderByDescending(f => f.LastWriteTime)
                                 .FirstOrDefault();

        return latestFile != null ? latestFile.FullName : null;
    }

    IEnumerator LoadImageFromFile(string filePath)
    {
        byte[] imageData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(imageData))
        {
            //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            UIManagerPhotobooth.Instance.GetPreviewImageSingle().GetComponent<RawImage>().texture = texture;
            if (!ModeSwitchManager.Instance.GetState())
            {
                comfyPromptCtr.QueuePrompt();
            }
        }
        else
        {
            Debug.LogError("Failed to load image from file.");
        }
        yield return null;
    }
}