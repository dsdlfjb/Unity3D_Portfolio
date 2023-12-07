using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManger : MonoBehaviour
{
    public static AudioManger Instance;

    public AudioMixer _mixer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) return;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(string sfxName, AudioClip clip)
    {
        GameObject Sound = new GameObject(sfxName + "Sound");
        AudioSource audiosource = Sound.AddComponent<AudioSource>();

        audiosource.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];
        audiosource.clip = clip;
        audiosource.Play();

        Destroy(Sound, clip.length);
    }

    public void SFX(float val)
    {
        _mixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }


}