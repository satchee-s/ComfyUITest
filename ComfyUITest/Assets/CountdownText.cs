using UnityEngine;
using TMPro;
using System.Collections; // If using TextMeshPro

public class CountdownText : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // Assign in inspector
    public float countdownDuration = 5f;
    [SerializeField] ComfyPromptCtr comfy;
    [SerializeField] GameObject m_spinner;
    [SerializeField] GameObject m_cameraPanel;
    [SerializeField] GameObject next;

    private void Start()
    {
        //StartCoroutine(CountdownRoutine());
    }

    public void StartAnimation()
    {
        StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        for (int i = (int)countdownDuration; i > 0; i--)
        {
            countdownText.text = i.ToString();

            // Reset scale
            countdownText.transform.localScale = Vector3.one;

            // Scale up using LeanTween
            LeanTween.scale(countdownText.gameObject, Vector3.one * 2f, 0.5f).setEaseOutBack();

            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "SMILE!";
        countdownText.transform.localScale = Vector3.one;
        LeanTween.scale(countdownText.gameObject, Vector3.one * 2f, 0.5f).setEaseOutBack();
        comfy.QueuePrompt();
        m_spinner.SetActive(true);
        m_cameraPanel.SetActive(false);
        next.SetActive(true);
        countdownText.text = "";
    }
}
