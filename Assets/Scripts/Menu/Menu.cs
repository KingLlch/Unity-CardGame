using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject SettingsPanel;

    public AudioMixer MainAudioMixer;
    public AudioSource AudioSourceVoice;
    public AudioSource AudioSourceVolume;

    private AudioClip[] voiceClips;
    private AudioClip[] effectsClips;

    private void Start()
    {
        MainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(0.3f) * 20);
        MainAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(0.3f) * 20);
        MainAudioMixer.SetFloat("VoiceVolume", Mathf.Log10(0.3f) * 20);

        voiceClips = Resources.LoadAll<AudioClip>("Sounds/Cards/Deployment/");
        effectsClips = Resources.LoadAll<AudioClip>("Sounds/Cards/StartOrder/");
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

    public void StartGame()
    {
        SceneManager.LoadScene("DeckBuild");
    }

    public void Settings()
    {
        SettingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
