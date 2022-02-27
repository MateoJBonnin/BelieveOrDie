using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Options : MonoBehaviour
{
    public AudioMixer mixer;

    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;
    public Slider musicSlider;

    public TextMeshProUGUI masterLabel;
    public TextMeshProUGUI sfxLabel;
    public TextMeshProUGUI voiceLabel;
    public TextMeshProUGUI musicLabel;

    string format = "{0}%";
    private void Start()
    {
        masterSlider.onValueChanged.AddListener(OnMasterChange);
        sfxSlider.onValueChanged.AddListener(OnSfxChange);
        voiceSlider.onValueChanged.AddListener(OnVoiceChange);
        musicSlider.onValueChanged.AddListener(OnMusicChange);

        masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("Sfx", 1);
        voiceSlider.value = PlayerPrefs.GetFloat("voice", 1);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
    }


    private void OnMasterChange(float value)
    {
        SetVolume("Master", value);
        masterLabel.text = string.Format(format, (value * 100).ToString("0"));
    }

    private void OnSfxChange(float value)
    {
        SetVolume("Sfx", value);
        sfxLabel.text = string.Format(format, (value * 100).ToString("0"));
    }

    private void OnVoiceChange(float value)
    {
        SetVolume("voice", value);
        voiceLabel.text = string.Format(format, (value * 100).ToString("0"));
    }

    private void OnMusicChange(float value)
    {
        SetVolume("Music", value);
        musicLabel.text = string.Format(format, (value * 100).ToString("0"));
    }

    void SetVolume(string mixer, float _value)
    {
        this.mixer.SetFloat(mixer, Mathf.Log10(_value) * 20);
        PlayerPrefs.SetFloat(mixer, _value);
    }
    
}
