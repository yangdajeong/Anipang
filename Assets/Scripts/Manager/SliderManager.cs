using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
   // private static SliderManager instance;

    [SerializeField]  AudioMixer m_AudioMixer;
    [SerializeField]  Slider m_MusicBGMSlider;
    [SerializeField]  Slider m_MusicSFXSlider;
    [SerializeField] static float sfxVolume = 1;
    [SerializeField] static float bgmVolume = 1;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }

    //    //m_MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
    //    //m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    //}

    private void Awake()
    {
        //m_AudioMixer.SetFloat("BGM", 0);
        //m_AudioMixer.SetFloat("SFX", 0);

        m_MusicBGMSlider.value = bgmVolume;
        m_MusicSFXSlider.value = sfxVolume;

        m_AudioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume) * 20);
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(bgmVolume) * 20);

        m_MusicBGMSlider.onValueChanged.AddListener(SetBGMVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    //public void SetSFXVolume(float volume)
    //{
    //    if (volume > 0f)
    //    {
    //        sfxVolume = Mathf.Log10(volume) * 20;
    //    }
    //    else
    //    {
    //        sfxVolume = -80; // 최소 음량 (예: -80dB)
    //    }
    //    m_AudioMixer.SetFloat("SFX", sfxVolume);
    //    Debug.Log("SFX Volume: " + sfxVolume);
    //}

    //public void SetBGMVolume(float volume)
    //{
    //    if (volume > 0f)
    //    {
    //        bgmVolume = Mathf.Log10(volume) * 20;
    //    }
    //    else
    //    {
    //        bgmVolume = -80; // 최소 음량 (예: -80dB)
    //    }
    //    m_AudioMixer.SetFloat("BGM", bgmVolume);
    //    Debug.Log("BGM Volume: " + bgmVolume);
    //}
}
