using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using System.Xml.Linq;

public class DeviceCamera : MonoBehaviour
{
    [SerializeField] string fileName = "capture.jpg";
    [SerializeField] string filePath = "C:\\Users\\fajel\\Desktop\\Webcam Image";

    [SerializeField] RawImage cameraImagePreview;
    private DateTime lastWriteTime;

    private void Start()
    {
        //filePath = "C:\\Users\\Lapto\\Documents\\GitHub\\ComfyUI\\custom_nodes\\comfyui_toyxyz_test_nodes\\CaptureCam\\captured_frames";
        filePath = "C:\\Users\\fajel\\Desktop\\Webcam Image\\";
    }
    private void Update()
    {
        CheckForUpdates();
    }

    void CheckForUpdates()
    {
        string fullPath = Path.Combine(filePath, fileName);

        if (File.Exists(fullPath))
        {
            DateTime currentWriteTime = File.GetLastWriteTime(fullPath);

            if (currentWriteTime > lastWriteTime)
            {
                lastWriteTime = currentWriteTime;
                LoadImage(fullPath);
            }
        }
        else
        {
            //Debug.LogError("Image file not found: " + fullPath);
        }
    }

    void LoadImage(string path)
    {
        try
        {
            byte[] imageBytes;

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                imageBytes = new byte[stream.Length];
                stream.Read(imageBytes, 0, imageBytes.Length);
            }

            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageBytes))
            {
                if (cameraImagePreview.texture != null)
                    Destroy(cameraImagePreview.texture);

                cameraImagePreview.texture = texture;
            }
            else
            {
                Debug.LogError("Failed to load image data.");
            }
        }
        catch (IOException e)
        {
            Debug.LogError("IOException: " + e.Message);
        }
    }

    public string GetFileName()
    {
        return fileName;
    }
}
