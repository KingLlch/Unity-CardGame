using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject SettingsPanel;
    public GameObject HowToPlayPanel;

    public AudioMixer MainAudioMixer;
    public AudioSource AudioSourceVoice;
    public AudioSource AudioSourceVolume;

    public Scrollbar MasterSoundVolume;
    public Scrollbar EffectsSoundVolume;
    public Scrollbar VoiceSoundVolume;

    public Toggle HowToPlayToggle;

    public TMP_Dropdown LanguageDropdown;

    private AudioClip[] voiceClips;
    private AudioClip[] effectsClips;

    private void Start()
    {
        MainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(MasterSoundVolume.value) * 20);
        MainAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(EffectsSoundVolume.value) * 20);
        MainAudioMixer.SetFloat("VoiceVolume", Mathf.Log10(VoiceSoundVolume.value) * 20);

        voiceClips = Resources.LoadAll<AudioClip>("Sounds/Cards/Deployment/");
        effectsClips = Resources.LoadAll<AudioClip>("Sounds/Cards/StartOrder/");

        HowToPlayToggle.isOn = HowToPlay.Instance.IsHowToPlay;

        if (PlayerPrefs.HasKey("Language"))
            LocalizationManager.Instance.Language = PlayerPrefs.GetString("Language");
        else
            LocalizationManager.Instance.Language = "en";

        if (LanguageDropdown != null)
        {
            switch (LocalizationManager.Instance.Language)
            {
                case "en":
                    LanguageDropdown.value = 0;
                    break;

                case "ru":
                    LanguageDropdown.value = 1;
                    break;

                case "uk":
                    LanguageDropdown.value = 2;
                    break;
            }
        }
    }

    public void MasterVolumeChanged(float value)
    {
        MainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);

        PlayRandomSound(AudioSourceVoice, true);
    }

    public void EffectsVolumeChanged(float value)
    {
        MainAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20);

        PlayRandomSound(AudioSourceVolume, false);
    }

    public void VoiceVolumeChanged(float value)
    {
        MainAudioMixer.SetFloat("VoiceVolume", Mathf.Log10(value) * 20);

        PlayRandomSound(AudioSourceVoice, true);
    }

    private void PlayRandomSound(AudioSource audioSource, bool isVoice)
    {
        if (isVoice)
            audioSource.clip = voiceClips[Random.Range(0, voiceClips.Length)];

        else
            audioSource.clip = effectsClips[Random.Range(0, effectsClips.Length)];

        audioSource.Play();
    }

    public void SwitchLanguage(int value)
    {
        switch (value)
        {
            case 0:
                LocalizationManager.Instance.Language = "en";
                break;

            case 1:
                LocalizationManager.Instance.Language = "ru";
                break;

            case 2:
                LocalizationManager.Instance.Language = "uk";
                break;
        }

        PlayerPrefs.SetString("Language", LocalizationManager.Instance.Language);
        PlayerPrefs.Save();

        LocalizedText[] texts = FindObjectsOfType<LocalizedText>();
        foreach (var text in texts)
        {
            text.UpdateText();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DeckBuild");
    }

    public void Settings()
    {
        MasterSoundVolume.value = Mathf.Pow(10,GetFloatFromAudioMixer("MasterVolume") / 20);
        EffectsSoundVolume.value = Mathf.Pow(10, GetFloatFromAudioMixer("EffectsVolume") / 20);
        VoiceSoundVolume.value = Mathf.Pow(10, GetFloatFromAudioMixer("VoiceVolume") / 20);

        SettingsPanel.SetActive(true);
    }

    private float GetFloatFromAudioMixer(string nameFloat)
    {
        float value;
        MainAudioMixer.GetFloat(nameFloat, out value);
        return value;
    }

    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetHowToPlay(bool isTrue)
    {
        HowToPlay.Instance.IsHowToPlay = isTrue;
    }
}
