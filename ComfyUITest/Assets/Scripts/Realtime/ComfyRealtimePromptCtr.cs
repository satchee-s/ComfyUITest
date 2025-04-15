using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Linq;

public class ComfyRealtimePromptCtr : MonoBehaviour
{
    string promptJson;
    [SerializeField] GenderManager genderManager;

    private void Start()
    {
        LoadPromptsJson();
    }

    private void LoadPromptsJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "SingleAIImage.json");

        // For standalone platforms (Windows, macOS, etc.)
        if (File.Exists(filePath))
        {
            promptJson = File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("prompt.json file not found at: " + filePath);
        }
    }

    public void QueuePrompt()
    {
        Debug.Log(genderManager.GetIsMale());
        StartCoroutine(QueuePromptCoroutine());
        UIManagerPhotobooth.Instance.ToggleCameraImage(false);
        UIManagerPhotobooth.Instance.TogglePreviewImageSingle(true);
    }

    private IEnumerator QueuePromptCoroutine()
    {
        string url = "http://127.0.0.1:8188/prompt";

        if (string.IsNullOrEmpty(promptJson))
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
            GetComponent<ComfyRealtimeWebsocket>().promptID = data.prompt_id;
        }
    }

    string CheckIfInputFieldIsEmpty()
    {
        string tempPrompt = "GTA";
        if (UIManagerPhotobooth.Instance.GetPromptInputFieldText() != string.Empty)
        {
            if (genderManager.GetIsMale())
            {
                tempPrompt = ($"{UIManagerPhotobooth.Instance.GetPromptInputFieldText()}, Male");
            }
            else
            {
                tempPrompt = ($"{UIManagerPhotobooth.Instance.GetPromptInputFieldText()}, Female");
            }
        }
        Debug.Log(tempPrompt);
        return tempPrompt;
    }

    private string GeneratePromptJson()
    {
        string guid = Guid.NewGuid().ToString();

        string promptJsonWithGuid = $@"
            {{
                ""id"": ""{guid}"",
                ""prompt"": {promptJson}
            }}";

        return promptJsonWithGuid;
    }
}
