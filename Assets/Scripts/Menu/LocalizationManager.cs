using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    private static LocalizationManager _instance;

    public static LocalizationManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LocalizationManager>();
            }

            return _instance;
        }
    }

    public string Language = "en";
    public TMP_Dropdown selectLanguage;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("Language"))
            Language = PlayerPrefs.GetString("Language");
        else
            Language = "en";

        if (selectLanguage != null)
        {
            switch (Language)
            {
                case "en":
                    selectLanguage.value = 0;
                    break;

                case "ru":
                    selectLanguage.value = 1;
                    break;

                case "uk":
                    selectLanguage.value = 2;
                    break;
            }
        }
    }

    public void SwitchLanguage(int value)
    {
        switch (value)
        {
            case 0:
                Language = "en";
                break;

            case 1:
                Language = "ru";
                break;

            case 2:
                Language = "uk";
                break;
        }

        PlayerPrefs.SetString("Language", Language);
        PlayerPrefs.Save();

        LocalizedText[] texts = FindObjectsOfType<LocalizedText>();
        foreach (var text in texts)
        {
            text.UpdateText();
        }
    }
}

