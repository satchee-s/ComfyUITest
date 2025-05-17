using UnityEngine;
using TMPro;
using System.IO;
using System.Text.RegularExpressions;

public class FormHandler : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField phoneInput;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] GameObject m_genderPanel;
    [SerializeField] GameObject m_registrationPanel;
    [SerializeField] GameObject keyboard;

    [Header("CSV Settings")]
    [SerializeField] private string fileName = "userdata.csv";

    // Hook this up to your Submit button’s OnClick()
    public void OnSubmit()
    {
        string name = nameInput.text.Trim();
        string email = emailInput.text.Trim();
        string phone = phoneInput.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            ShowError("Name cannot be empty.");
            return;
        }
        if (!IsValidEmail(email))
        {
            ShowError("Please enter a valid email address.");
            return;
        }
        if (!IsValidPhone(phone))
        {
            ShowError("Please enter a valid phone number.");
            return;
        }

        // clear any error
        messageText.text = "";

        // ensure folder exists
        string folderPath = Application.streamingAssetsPath;
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string fullPath = Path.Combine(folderPath, fileName);
        bool fileExists = File.Exists(fullPath);

        using (var writer = new StreamWriter(fullPath, append: true))
        {
            if (!fileExists)
                writer.WriteLine("Name,Email,Phone");
            writer.WriteLine($"{EscapeCsv(name)},{EscapeCsv(email)},{EscapeCsv(phone)}");
        }

        m_registrationPanel.SetActive(false);
        keyboard.SetActive(false);
        m_genderPanel.SetActive(true);
        nameInput.text = "";
        phoneInput.text = "";
        emailInput.text = "";
        Debug.Log($"Data written to: {fullPath}");
    }

    private void ShowError(string msg)
    {
        messageText.text = msg;
    }

    private bool IsValidEmail(string email)
    {
        var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern);
    }

    private bool IsValidPhone(string phone)
    {
        var pattern = @"^\+?\d{7,15}$";
        return Regex.IsMatch(phone, pattern);
    }

    private string EscapeCsv(string s)
    {
        if (s.Contains(",") || s.Contains("\""))
        {
            s = s.Replace("\"", "\"\"");
            return $"\"{s}\"";
        }
        return s;
    }
}
