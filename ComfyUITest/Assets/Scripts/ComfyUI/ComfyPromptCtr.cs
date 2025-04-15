using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

[System.Serializable]
public class ResponseData
{
    public string prompt_id;
}

public class ComfyPromptCtr : MonoBehaviour
{
    [SerializeField] GenderManager genderManager;

    string singlePromptJson;

    private void Start()
    {
        LoadPromptsJson();
    }

    private void LoadPromptsJson()
    {
        string filePathSingle = Path.Combine(Application.streamingAssetsPath, "SingleAIImage.json");

        // For standalone platforms (Windows, macOS, etc.)
        if (File.Exists(filePathSingle))
        {
            singlePromptJson = File.ReadAllText(filePathSingle);
        }
        else
        {
            Debug.LogError("prompt.json file not found at: " + filePathSingle);
        }
    }

    public void QueuePrompt()
    {
        if (ModeSwitchManager.Instance.GetState())
        {
            UIManagerPhotobooth.Instance.PrepareRenderedPhoto();
        }
        StartCoroutine(QueuePromptCoroutine());
    }

    private IEnumerator QueuePromptCoroutine()
    {
        string url = "http://127.0.0.1:8188/prompt";

        if (string.IsNullOrEmpty(singlePromptJson))
        {
            Debug.LogError("Prompt JSON is empty or not loaded!");
            yield break;
        }

        string promptText = GeneratePromptJson();
        promptText = promptText.Replace("Pprompt", CheckIfInputFieldIsEmpty());

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(promptText);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("Prompt queued successfully." + request.downloadHandler.text);

            ResponseData data = JsonUtility.FromJson<ResponseData>(request.downloadHandler.text);
            GetComponent<ComfyWebsocket>().promptID = data.prompt_id;
        }
    }

    string CheckIfInputFieldIsEmpty()
    {
        string tempPrompt = genderManager.GetIsMale() ? "GTA, Male" : "GTA, Female";
        if (UIManagerPhotobooth.Instance.GetPromptInputFieldText() != string.Empty)
        {
            Debug.Log(genderManager.GetIsMale());
            if (genderManager.GetIsMale())
            {
                tempPrompt = ($"{UIManagerPhotobooth.Instance.GetPromptInputFieldText()}, Male");
            }
            else
            {
                tempPrompt = ($"{UIManagerPhotobooth.Instance.GetPromptInputFieldText()}, Female");
            }
        }
        return tempPrompt;
    }

    private string GeneratePromptJson()
    {
        string guid = Guid.NewGuid().ToString();

        string promptJsonWithGuid = $@"
            {{
                ""id"": ""{guid}"",
                ""prompt"": {singlePromptJson}
            }}";

        return promptJsonWithGuid;
    }
}
