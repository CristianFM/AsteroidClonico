using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public void SetMasterVolume() //Vincular el Slider con el AudioMixer
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume)*20);
    }
    public void SetMusicVolume() //Vincular el Slider con el AudioMixer
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }
    public void SetsfxVolume() //Vincular el Slider con el AudioMixer
    {
        float volume = sfxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
