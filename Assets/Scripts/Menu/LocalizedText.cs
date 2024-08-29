using TMPro;
using UnityEngine;

public class LocalizedText : MonoBehaviour
{
    public string en;
    public string ru;
    public string uk;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if(LocalizationManager.Instance.Language == "en")
            text.text = en;
        else if (LocalizationManager.Instance.Language == "ru")
            text.text = ru;
        else if (LocalizationManager.Instance.Language == "uk")
            text.text = uk;
    }

    public void UpdateText()
    {
        if (LocalizationManager.Instance.Language == "en")
            text.text = en;
        else if (LocalizationManager.Instance.Language == "ru")
            text.text = ru;
        else if (LocalizationManager.Instance.Language == "uk")
            text.text = uk;
    }
}

