using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Menu : MonoBehaviour
{
    public GameObject SettingsPanel;

    public AudioMixer MainAudioMixer;
    public AudioSource AudioSourceVoice;
    public AudioSource AudioSourceVolume;

    public Scrollbar MasterSoundVolume;
    public Scrollbar EffectsSoundVolume;
    public Scrollbar VoiceSoundVolume;

    private AudioClip[] voiceClips;
    private AudioClip[] effectsClips;

    private void Start()
    {
        MainAudioMixer.SetFloat("MasterVolume", Mathf.Log10(MasterSoundVolume.value) * 20);
        MainAudioMixer.SetFloat("EffectsVolume", Mathf.Log10(EffectsSoundVolume.value) * 20);
        MainAudioMixer.SetFloat("VoiceVolume", Mathf.Log10(VoiceSoundVolume.value) * 20);

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
}
