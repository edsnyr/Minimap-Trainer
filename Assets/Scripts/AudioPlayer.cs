using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{

    public static AudioPlayer instance;

    public AudioMixer mixer;
    public AudioSource missedSound;
    public AudioSource hitSound;

    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public static UnityEvent missEvent; //played whenever the player makes an error
    public static UnityEvent hitEvent; //played when the player calls an enemy laner missing/found

    void Awake()
    {
        //ensure a single instance of the Audio Player
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if(instance != this) {
            Destroy(gameObject);
        }

        if(missEvent == null) { missEvent = new UnityEvent(); }
        if(hitEvent == null) { hitEvent = new UnityEvent(); }
        missEvent.AddListener(() => missedSound.Play());
        hitEvent.AddListener(() => hitSound.Play());
    }

    /// <summary>
    /// Initialize volume settings
    /// </summary>
    private void Start() {
        SetMasterVolume(masterSlider);
        SetMusicVolume(musicSlider);
        SetSFXVolume(sfxSlider);
    }

    public void SetMasterVolume(Slider volume) {
        mixer.SetFloat("masterVolume", Mathf.Log10(volume.value) * 20);
    }

    public void SetMusicVolume(Slider volume) {
        mixer.SetFloat("musicVolume", Mathf.Log10(volume.value) * 20);
    }

    public void SetSFXVolume(Slider volume) {
        mixer.SetFloat("sfxVolume", Mathf.Log10(volume.value) * 20);
    }
}
