using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

[System.Serializable]
public class ImageData
{
    public string filename;
    public string subfolder;
    public string type;
}

[System.Serializable]
public class OutputData
{
    public ImageData[] images;
}

[System.Serializable]
public class PromptData
{
    public OutputData outputs;
}

public class ComfyImageCtr : MonoBehaviour
{
    public List<Texture2D> outputSprites = new List<Texture2D>();
    string fileName;

    public void RequestFileName(string id)
    {
        StartCoroutine(RequestFileNameRoutine(id));
    }

    IEnumerator RequestFileNameRoutine(string promptID)
    {
        string url = "http://127.0.0.1:8188/history/" + promptID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("Received: " + webRequest.downloadHandler.text);
                    List<string> filenames = ExtractFilenames(webRequest.downloadHandler.text);
                    int expectedCount = filenames.Count;
                    Debug.Log(expectedCount);
                    foreach (string _filename in filenames)
                    {
                        string imageURL = "http://127.0.0.1:8188/view?filename=" + _filename + "&type=temp&subfolder=";
                        fileName = _filename;
                        StartCoroutine(DownloadImage(imageURL));
                    }
                    yield return new WaitUntil(() => outputSprites.Count >= expectedCount);
                    SetPreviewImage();
                    break;
            }
        }
    }

    List<string> ExtractFilenames(string jsonString)
    {
        List<string> filenames = new List<string>();
        int index = 0;
        while (true)
        {
            int keyIndex = jsonString.IndexOf("\"filename\":", index);
            if (keyIndex == -1) break;
            int firstQuote = jsonString.IndexOf("\"", keyIndex + 11);
            if (firstQuote == -1) break;
            int secondQuote = jsonString.IndexOf("\"", firstQuote + 1);
            if (secondQuote == -1) break;
            string filename = jsonString.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
            filenames.Add(filename);
            index = secondQuote + 1;
        }
        return filenames;
    }

    IEnumerator DownloadImage(string imageUrl)
    {
        outputSprites.Clear();
        yield return new WaitForSeconds(0.5f);
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                outputSprites.Add(texture);
            }
            else
            {
                Debug.LogError("Image download failed: " + webRequest.error);
            }
        }
    }

    void SetPreviewImage()
    {
        UIManagerPhotobooth.Instance.ToggleLoadingSpinner(false);
        UIManagerPhotobooth.Instance.ToggleQRDownloadButton(true);
        UIManagerPhotobooth.Instance.ToggleQRCodeImage(true);
        UIManagerPhotobooth.Instance.GetPreviewImageSingle().GetComponent<RawImage>().texture = outputSprites[0];
    }

    public string GetFileName()
    {
        return fileName;
    }    
}
