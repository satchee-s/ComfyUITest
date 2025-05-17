using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager Instance;
    [SerializeField] TMP_InputField[] textBox;
    [SerializeField] TMP_InputField currenttextBox;
    //[SerializeField] TextMeshProUGUI printBox;

    private void Start()
    {
        Instance = this;

        //printBox.text = "";
        //textBox.text = "";
    }

    public void OnSelect(int index)
    {
        currenttextBox = textBox[index];
    }

    public void DeleteLetter()
    {
        if (currenttextBox == null)
            return;
        if (currenttextBox.text.Length != 0)
        {
            currenttextBox.text = currenttextBox.text.Remove(currenttextBox.text.Length - 1, 1);
        }
    }

    public void ClearTextBox()
    {
        if (currenttextBox == null)
            return;
        currenttextBox.text = string.Empty;
    }

    public void AddLetter(string letter)
    {
        if (currenttextBox == null)
            return;
        currenttextBox.text = currenttextBox.text + letter;
    }

    public void SubmitWord()
    {
        if (currenttextBox == null)
            return;
        ClearTextBox();
        currenttextBox = null;
    }
}
