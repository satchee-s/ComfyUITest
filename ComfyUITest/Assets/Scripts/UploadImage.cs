using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

[System.Serializable]
public class QRCodeData
{
    public string qr_code;
    public string url;
}

public class UploadImage : MonoBehaviour
{
    [Header("Upload Settings")]
    [Tooltip("Leave blank to auto-pick the newest PNG in StreamingAssets/images/")]
    public string fileName;

    [SerializeField]
    private string uploadURL = "http://192.168.0.182:5000/upload";

    [Header("Result")]
    public string extractedPath;
    private void Start()
    {
        Screen.SetResolution(1080, 1920, true);
        uploadURL = "http://192.168.0.182:5000/upload";
        //uploadURL = "http://10.255.254.146:5000/upload";   
    }

    public void StartUpload(string path)
    {
        fileName = path;
        StartCoroutine(UploadFile());
    }

    private IEnumerator UploadFile()
    {
        // 1) Determine the folder and filename
        string imagesDir = Path.Combine(Application.streamingAssetsPath, "images");
        if (!Directory.Exists(imagesDir))
        {
            Debug.LogError($"Upload failed: directory not found: {imagesDir}");
            yield break;
        }

        // auto-pick latest if none provided
        if (string.IsNullOrEmpty(fileName))
        {
            var pngs = Directory.GetFiles(imagesDir, "*.png");
            if (pngs.Length == 0)
            {
                Debug.LogError("Upload failed: no PNGs found in " + imagesDir);
                yield break;
            }
            // pick by last write time descending
            fileName = pngs
                .OrderByDescending(f => File.GetLastWriteTimeUtc(f))
                .First();
            fileName = Path.GetFileName(fileName);
        }

        string fullPath = Path.Combine(imagesDir, fileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogError($"Upload failed: file not found: {fullPath}");
            yield break;
        }

        // 2) Read the bytes
        byte[] fileBytes = File.ReadAllBytes(fullPath);

        // 3) Create form & upload
        WWWForm form = new WWWForm();
        form.AddBinaryData("file", fileBytes, fileName, "image/png");

        using (UnityWebRequest www = UnityWebRequest.Post(uploadURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Upload error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                extractedPath = JsonUtility.FromJson<QRCodeData>(jsonResponse).qr_code;
                Debug.Log("File uploaded successfully. Server QR path: " + extractedPath);
                QRCodeManager.Instance.LoadImageAsTexture(extractedPath);
            }
        }
    }
}
