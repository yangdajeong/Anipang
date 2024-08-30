using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    [SerializeField] AudioMixer m_AudioMixer;
    [SerializeField] Slider m_MusicBGMSlider;
    [SerializeField] Slider m_MusicSFXSlider;
    [SerializeField] static float sfxVolume = 1;
    [SerializeField] static float bgmVolume = 1;

    private void Awake()
    {
        m_MusicBGMSlider.value = bgmVolume;
        m_MusicSFXSlider.value = sfxVolume;

        m_AudioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);

        m_MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
